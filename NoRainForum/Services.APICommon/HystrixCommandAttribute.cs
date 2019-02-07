using AspectCore.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.APICommon
{
    public class HystrixCommandAttribute : AbstractInterceptorAttribute
    {
        public string FallBackMethod { get; set; }
        public HystrixCommandAttribute(string method)
        {
            this.FallBackMethod = method;
        }
        /// <summary>
        /// 重试次数，为0则不重试
        /// </summary>
        public int RetryCount { get; set; } = 0;
        /// <summary>
        /// 重试间隔时间
        /// </summary>
        public int RetryTime { get; set; } = 1000;
        /// <summary>
        /// 是否采用熔断
        /// </summary>
        public bool EnableCircuitBreaker { get; set; } = false;
        /// <summary>
        /// 熔断前允许错误次数
        /// </summary>
        public int CircuitBreakerCount { get; set; } = 3;
        /// <summary>
        /// 熔断时间
        /// </summary>
        public int CircuitBreakerTime { get; set; } = 2000;
        /// <summary>
        /// 超过多少秒认为超时，0表示不检测超时
        /// </summary>
        public int TimeOut { get; set; } = 3000;
        /// <summary>
        /// 是否启用缓存，0表示不启用
        /// </summary>
        public int CacheTTLMilliseconds { get; set; } = 0;

        private static ConcurrentDictionary<MethodInfo, Policy> policies = new
ConcurrentDictionary<MethodInfo, Policy>();

        private static readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Policy policy;
            policies.TryGetValue(context.ServiceMethod, out policy);
            lock (policies)
            {
                if (policy == null)
                {
                    policy = Policy.NoOpAsync();
                    if (EnableCircuitBreaker)
                    {
                        policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(CircuitBreakerCount, TimeSpan.FromSeconds(CircuitBreakerTime)));
                    }
                    if (TimeOut > 0)
                    {
                        policy.WrapAsync(Policy.TimeoutAsync(TimeOut, Polly.Timeout.TimeoutStrategy.Pessimistic));
                    }
                    if (RetryCount > 0)
                    {
                        policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(RetryTime)));
                    }
                    Policy fallback = Policy.Handle<Exception>().FallbackAsync(async (ctx, t) =>
                    {
                        AspectContext aspectContext = (AspectContext)ctx["aspectContext"];
                        var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(this.FallBackMethod);
                        Object fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
                        aspectContext.ReturnValue = fallBackResult;

                    }, async (ex, t) => { });

                    policy = fallback.WrapAsync(policy);

                    policies.TryAdd(context.ServiceMethod, policy);
                }
            }

            Context pollyCtx = new Context();
            pollyCtx["aspectContext"] = context;
            if (CacheTTLMilliseconds > 0)
            {
                //用类名+方法名+参数的下划线连接起来作为缓存key
                string cacheKey = "HystrixMethodCacheManager_Key_" +
                            context.ServiceMethod.DeclaringType + "." +
                            context.ServiceMethod + string.Join("_", context.Parameters);
                //尝试去缓存中获取。如果找到了，则直接用缓存中的值做返回值
                if (memoryCache.TryGetValue(cacheKey, out var cacheValue))
                {
                    context.ReturnValue = cacheValue;
                }
                else
                {
                    //如果缓存中没有，则执行实际被拦截的方法
                    await policy.ExecuteAsync(ctx => next(context), pollyCtx);
                    //存入缓存中
                    using (var cacheEntry = memoryCache.CreateEntry(cacheKey))
                    {
                        cacheEntry.Value = context.ReturnValue;
                        cacheEntry.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMilliseconds(CacheTTLMilliseconds);
                    }
                }
            }
            else//如果没有启用缓存，就直接执行业务方法
            {
                await policy.ExecuteAsync(ctx => next(context), pollyCtx);
            }
        }
    }
}

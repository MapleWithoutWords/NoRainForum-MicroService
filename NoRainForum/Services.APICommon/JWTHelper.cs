using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.APICommon
{
    public class JWTHelper
    {
        private static string secret = "GJHKLDgjfkdlJGDKlKGJgn^&klZJGJGKLgjnkljgkLHGuj14JGK#&Y#";
        public static string CalcJWT(object payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            string token = encoder.Encode(payload, secret);
            return token;
        }
        public static bool Decrypt<T>(string token, out T obj) where T : class
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var json = decoder.Decode(token, secret, verify: true);
                obj = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch (TokenExpiredException)
            {
                obj = null;
                return false;
            }
            catch (SignatureVerificationException)
            {
                obj = null;
                return false;
            }
            catch (Exception)
            {
                obj = null;
                return false;
            }
        }
        public static string GetToken(HttpContext HttpContext, string key)
        {
            StringValues vs;
            if (!HttpContext.Request.Headers.TryGetValue(key, out vs))
            {
                return null;
            }
            return vs.First();
        }
    }
}

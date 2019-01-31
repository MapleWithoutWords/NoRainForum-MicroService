using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common.IServiceCommon
{
    public interface IBaseService<T>:ISupport
    {
        Task<T> GetByIdAsync(long id);
        Task<List<T>> GetPageDataAsync(int pageIndex=1,int pageDataCount=10);
        Task MarkDeleteAsync(long id);
        Task<long> TotalCountAsync();
    }
}

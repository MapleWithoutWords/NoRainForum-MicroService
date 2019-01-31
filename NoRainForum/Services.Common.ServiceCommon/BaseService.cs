using Microsoft.EntityFrameworkCore;
using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common.ServiceCommon
{
    public class BaseService<T> where T : BaseEntity
    {
        private DbContext context;
        public BaseService(DbContext context)
        {
            this.context = context;
        }
        public IQueryable<T> GetAll()
        {
            return context.Set<T>().Where(e => !e.IsDeleted);
        }
        public async Task MarkDeleteAsync(long id)
        {
            T entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
            }
            await context.SaveChangesAsync();
        }
        public async Task<T> GetByIdAsync(long id)
        {
            return await GetAll().SingleOrDefaultAsync(e=>e.Id==id);
        }
        public async Task<long> TotalCountAsync()
        {
            return await GetAll().LongCountAsync();
        }
    }
}

using Services.Common.IServiceCommon;
using Services.Post.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Post.IService
{
    public interface IIdNameService:IBaseService<IdNameDTO>
    {
        Task<long> AddNewAsync(AddIdNameDTO dto);
        Task UpdateAsync(UpdateIdNameDTO dto);
        Task<IdNameDTO> GetByNameAsync(string name);
        Task<List<IdNameDTO>> GetAll();
    }
}

using Services.Admin.DTO;
using Services.Common.IServiceCommon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Admin.IService
{
    public interface IAdminUserService : IBaseService<ListAdminUserDTO>
    {
        Task<long> AddNewAsync(AddAdminUserDTO dto);
        Task<bool> LoginAsync(string phoneNum,string password);
        Task<bool> IsLockAsync(long id);
        Task LockAdminUserAsync(long id);
        Task UnLockAdminUserAsync(long id);
        Task EditorPasswordAsync(long id, string newPassword);
        Task UpdateAsync(UpdateAdminUserDTO dto);
        Task<ListAdminUserDTO> GetByPhoneNumAsync(string phoneNum);
        Task ChangePasswordAsync(long id,string newPassword);
    }
}

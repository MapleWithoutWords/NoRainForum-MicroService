using Services.Common.IServiceCommon;
using Services.User.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.User.IService
{
    public interface IUserService : IBaseService<ListUserDTO>
    {
        Task<long> AddNewAsync(AddUserDTO dto);
        Task UpdateAsync(UpdateUserDTO dto);
        Task<ListUserDTO> GetByEmailAsync(string email);
        Task<ListUserDTO> GetByNickNameAsync(string nickName);
        Task<bool> IsLockAsync(long id);
        Task LockUserAsync(long id);
        Task UnLockUserAsync(long id);
        Task<bool> LoginAsync(string email, string password);
        Task EditorPasswordAsync(long id, string newPassword);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using ARMDesktopUI.Library.Models;

namespace ARMDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();

        Task<Dictionary<string, string>> GetAllRoles();

        Task AddRoleToUser(string userId, string roleName);

        Task RemoveRoleFromUser(string userId, string roleName);
    }
}
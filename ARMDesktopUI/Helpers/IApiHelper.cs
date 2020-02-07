using System.Threading.Tasks;
using ARMDesktopUI.Models;

namespace ARMDesktopUI.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string userName, string password);
    }
}
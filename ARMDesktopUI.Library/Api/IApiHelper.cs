using System.Net.Http;
using System.Threading.Tasks;
using ARMDesktopUI.Library.Models;

namespace ARMDesktopUI.Library.Api
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string userName, string password);

        Task GetLoggedInUserInfo(string token);

        void LogOffUser();
    }
}
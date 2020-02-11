using System.Collections.Generic;
using System.Web.Http;
using ARMDataManager.Library.DataAccess;
using ARMDataManager.Library.Models;
using Microsoft.AspNet.Identity;

namespace ARMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // Return the user id of the currently authorized person
        public List<UserModel> GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            var data = new UserData();

            return data.GetUserById(userId);
        }
    }
}
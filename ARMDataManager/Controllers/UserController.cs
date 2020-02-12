using System.Collections.Generic;
using System.Linq;
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
        public UserModel GetById()
        {
            // get the user id from the context
            string userId = RequestContext.Principal.Identity.GetUserId();
            var data = new UserData();

            return data
                .GetUserById(userId)
                .First();
        }
    }
}
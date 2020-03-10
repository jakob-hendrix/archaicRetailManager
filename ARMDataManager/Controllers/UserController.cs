using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ARMDataManager.Library.DataAccess;
using ARMDataManager.Library.Models;
using ARMDataManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ARMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // Return the user id of the currently authorized person
        [HttpGet]
        public UserModel GetById()
        {
            // get the user id from the context
            string userId = RequestContext.Principal.Identity.GetUserId();
            var data = new UserData();

            return data
                .GetUserById(userId)
                .First();
        }

        [HttpGet]
        [Route("api/Users/Admin/GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            var output = new List<ApplicationUserModel>();

            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    var userModel = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };

                    foreach (var role in user.Roles)
                    {
                        userModel.Roles.Add(
                            role.RoleId,
                            roles.First(x => x.Id == role.RoleId).Name);
                    }

                    output.Add(userModel);
                }
            }

            return output;
        }
    }
}
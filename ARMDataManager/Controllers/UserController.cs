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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/Users/Admin/GetAllUsers")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/Users/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/Users/Admin/AddRole")]
        public void AddRole(UserRolePairModel userRolePair)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.AddToRole(userRolePair.UserId, userRolePair.RoleName);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/Users/Admin/RemoveRole")]
        public void RemoveRole(UserRolePairModel userRolePair)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.RemoveFromRole(userRolePair.UserId, userRolePair.RoleName);
            }
        }
    }
}
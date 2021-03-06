﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ARMDesktopUI.Library.Models;

namespace ARMDesktopUI.Library.Api
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public UserEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<UserModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Users/Admin/GetAllUsers"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                var result = await response.Content.ReadAsAsync<List<UserModel>>();
                return result;
            }
        }

        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Users/Admin/GetAllRoles"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                return result;
            }
        }

        public async Task AddRoleToUser(string userId, string roleName)
        {
            var data = new { userId, roleName };
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Users/Admin/AddRole", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task RemoveRoleFromUser(string userId, string roleName)
        {
            var data = new { userId, roleName };
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Users/Admin/RemoveRole", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
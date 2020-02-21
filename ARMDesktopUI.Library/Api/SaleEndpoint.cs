using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ARMDesktopUI.Library.Models;

namespace ARMDesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private IApiHelper _apiHelper;

        public SaleEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                // TODO: need to return anything?
            }
        }
    }
}
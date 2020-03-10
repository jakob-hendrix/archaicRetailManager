using System.Collections.Generic;
using System.Web.Http;
using ARMDataManager.Library.DataAccess;
using ARMDataManager.Library.Models;
using Microsoft.AspNet.Identity;

namespace ARMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            var data = new SaleData();
            var userId = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
        }

        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
        public List<SaleReportModel> GetSalesReport()
        {
            if (RequestContext.Principal.IsInRole("Admin"))
            {
                // example: do admin stuff
            }
            else if (RequestContext.Principal.IsInRole("Manager"))
            {
                // example: do manager stuff
            }

            var data = new SaleData();
            return data.GetSaleReport();
        }
    }
}
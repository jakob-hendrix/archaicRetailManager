using System.Collections.Generic;
using System.Web.Http;
using ARMDataManager.Library.DataAccess;
using ARMDataManager.Library.Models;
using Microsoft.AspNet.Identity;

namespace ARMDataManager.Controllers
{
    //[Authorize]
    public class SaleController : ApiController
    {
        [HttpPost]
        public void Post(SaleModel sale)
        {
            var data = new SaleData();
            data.SaveSale(sale, RequestContext.Principal.Identity.GetUserId());
        }

        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            var data = new SaleData();
            return data.GetSaleReport();
        }
    }
}
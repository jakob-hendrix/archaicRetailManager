using System;
using System.Web.Http;
using ARMDataManager.Library.Models;

namespace ARMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [HttpPost]
        public void Post(SaleModel sale)
        {
            Console.WriteLine();
        }

        //[HttpGet]
        //public List<ProductModel> Get()
        //{
        //    var data = new ProductData();
        //    return data.GetProducts();
        //}
    }
}
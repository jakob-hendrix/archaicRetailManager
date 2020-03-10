using System.Collections.Generic;
using System.Web.Http;
using ARMDataManager.Library.DataAccess;
using ARMDataManager.Library.Models;

namespace ARMDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        [Authorize(Roles = "Admin,Manager")]
        public List<InventoryModel> Get()
        {
            var data = new InventoryData();
            return data.GetInventory();
        }

        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            var data = new InventoryData();
            data.SaveInventoryRecord(item);
        }
    }
}
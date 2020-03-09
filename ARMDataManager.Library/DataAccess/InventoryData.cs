using System.Collections.Generic;
using ARMDataManager.Library.Internal.DataAccess;
using ARMDataManager.Library.Models;

namespace ARMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            var data = new SqlDataAccess();
            var output = data.LoadData<InventoryModel, dynamic>
            (
                "dbo.spInventory_GetAll",
                new { },
                "ARMData"
            );
            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            var data = new SqlDataAccess();
            data.SaveData
            (
                "dbo.spInventory_Insert",
                item,
                "ARMData"
            );
        }
    }
}
using System.Collections.Generic;
using ARMDataManager.Library.Internal.DataAccess;
using ARMDataManager.Library.Models;

namespace ARMDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var parameters = new { };

            var output =
                sql.LoadData<ProductModel, dynamic>
                (
                    "dbo.spProduct_GetAll",
                    parameters,
                    "TRMData"
                );

            return output;
        }
    }
}
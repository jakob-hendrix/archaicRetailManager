using System.Collections.Generic;
using System.Linq;
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
                    "ARMData"
                );

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var parameters = new { Id = productId };

            var output =
                sql.LoadData<ProductModel, dynamic>
                (
                    "dbo.spProduct_GetById",
                    parameters,
                    "ARMData"
                ).FirstOrDefault();

            return output;
        }
    }
}
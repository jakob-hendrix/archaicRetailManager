using System;
using System.Collections.Generic;
using ARMDataManager.Library.Models;

namespace ARMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel sale)
        {
            // TODO: make this SOLID / DRY / better

            // Start filling in the sale detail models we will persist
            var details = new List<SaleDetailDbModel>();
            var products = new ProductData();

            foreach (var item in sale.SaleDetails)
            {
                var detail = new SaleDetailDbModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // Get product information
                var productInfo = products.GetProductById(item.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database.");
                }

                details.Add(detail);
            }

            // Fill in the available information
            // Create the SaleModel
            // Save the SaleModel
            // Get the id from the sale model
            // Finish filling in the sale detail models
            // Save the sale detail models
        }

        //public List<ProductModel> GetProducts()

        //{
        //    SqlDataAccess sql = new SqlDataAccess();

        //    var parameters = new { };

        //    var output =
        //        sql.LoadData<ProductModel, dynamic>
        //        (
        //            "dbo.spProduct_GetAll",
        //            parameters,
        //            "ARMData"
        //        );

        //    return output;
        //}
    }
}
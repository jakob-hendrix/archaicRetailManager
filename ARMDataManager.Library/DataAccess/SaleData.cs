using System;
using System.Collections.Generic;
using System.Linq;
using ARMDataManager.Library.Helpers;
using ARMDataManager.Library.Internal.DataAccess;
using ARMDataManager.Library.Models;

namespace ARMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            // TODO: make this SOLID / DRY / better

            // Start filling in the saleInfo detail models we will persist
            var details = new List<SaleDetailDbModel>();
            var products = new ProductData();
            decimal taxRate = (decimal)ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDbModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // Get product information
                var productInfo = products.GetProductById(detail.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);
            }

            // Create the SaleModel
            SaleDbModel sale = new SaleDbModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                TaxTotal = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.TaxTotal;

            // Save the SaleModel
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData<SaleDbModel>("dbo.spSale_Insert", sale, "ARMData");

            // Get the id for the new sale
            sale.Id = sql.LoadData<int, dynamic>
            (
                "spSale_Lookup",
                new { sale.CashierId, sale.SaleDate },
                "ARMData"
            ).FirstOrDefault();

            // Finish filling in the saleInfo detail and save the saleInfo detail models
            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                sql.SaveData("dbo.spSaleDetail_Insert", item, "ARMData");
            }
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
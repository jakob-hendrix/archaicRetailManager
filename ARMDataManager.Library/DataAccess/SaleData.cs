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
            using (var sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("ARMData");

                    sql.SaveDataInTransaction<SaleDbModel>("dbo.spSale_Insert", sale);

                    // Get the id for the new sale
                    sale.Id = sql
                        .LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate })
                        .FirstOrDefault();

                    // Finish filling in the saleInfo detail and save the saleInfo detail models
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch
                {
                    // TODO: notify the user
                    sql.RollbackTransaction();
                    throw;
                }
            }
        }

        public List<SaleReportModel> GetSaleReport()
        {
            using (var data = new SqlDataAccess())
            {
                var output =
                    data.LoadData<SaleReportModel, dynamic>
                    (
                        "dbo.spSale_SaleReport",
                        new { },
                        "ARMData"
                    );
                return output;
            }
        }
    }
}
using System;

namespace ARMDataManager.Library.DataAccess
{
    public class SaleReportModel
    {
        public DateTime SaleDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal Total { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
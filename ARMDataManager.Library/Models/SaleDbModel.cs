﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMDataManager.Library.Models
{
    public class SaleDbModel
    {
        public int Id { get; set; }
        public string CashierId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal Total { get; set; }
    }
}
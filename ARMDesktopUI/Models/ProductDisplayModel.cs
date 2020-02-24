﻿namespace ARMDesktopUI.Models
{
    public class ProductDisplayModel : ChangingPropertiesModel
    {
        private int _quantityInStock;
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }

        public int QuantityInStock
        {
            get => _quantityInStock;
            set
            {
                _quantityInStock = value;
                CallPropertyChanged(nameof(QuantityInStock));
            }
        }

        public bool IsTaxable { get; set; }
    }
}
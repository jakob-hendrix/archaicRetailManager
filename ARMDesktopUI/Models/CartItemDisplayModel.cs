using System.ComponentModel;
using System.Runtime.CompilerServices;
using ARMDesktopUI.Annotations;

namespace ARMDesktopUI.Models
{
    public class CartItemDisplayModel : ChangingPropertiesModel
    {
        private int _quantityInCart;
        public ProductDisplayModel Product { get; set; }

        public int QuantityInCart
        {
            get => _quantityInCart;
            set
            {
                _quantityInCart = value;
                CallPropertyChanged(nameof(QuantityInCart));
                CallPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            // TODO: display differently based on amount
            get => $"{Product.ProductName} ({QuantityInCart})";
        }
    }
}
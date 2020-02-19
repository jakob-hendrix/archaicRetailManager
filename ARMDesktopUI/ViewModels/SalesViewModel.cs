using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ARMDesktopUI.Library.Api;
using ARMDesktopUI.Library.Models;
using Caliburn.Micro;

namespace ARMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private int _itemQuantity;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAllProducts();
            Products = new BindingList<ProductModel>(productList);
        }

        public BindingList<ProductModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public int ItemQuantity
        {
            get => _itemQuantity;
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }

                return subTotal.ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                // TODO: replace with calc
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                // TODO: replace with calc
                return "$0.00";
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected
                // Make sure there is an item quantity
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            var item = new CartItemModel
            {
                Product = SelectedProduct,
                QuantityInCart = ItemQuantity
            };

            Cart.Add(item);
            NotifyOfPropertyChange(() => SubTotal);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected

                return output;
            }
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                // Make sure something is in the cart

                return output;
            }
        }

        public void CheckOut()
        {
        }
    }
}
using ARMDesktopUI.Library.Api;
using ARMDesktopUI.Library.Helpers;
using ARMDesktopUI.Library.Models;
using Caliburn.Micro;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ARMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndpoint _saleEndpoint;
        private int _itemQuantity = 1;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
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

        private BindingList<ProductModel> _products;

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

        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

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

        public string SubTotal => CalculateSubTotal().ToString("C");

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            subTotal = Cart.Sum(i => i.Product.RetailPrice * i.QuantityInCart);
            return subTotal;
        }

        public string Tax => CalculateTax().ToString("C");

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = (decimal)_configHelper.GetTaxRate() / 100;

            taxAmount =
                Cart.Where(i => i.Product.IsTaxable)
                    .Sum(i => i.Product.RetailPrice * i.QuantityInCart * taxRate);

            return taxAmount;
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }

        public bool CanAddToCart => ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;

                // TODO: fix this HACK to properly force the cart to refresh
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                var item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
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
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut => Cart.Count > 0;

        public async Task CheckOut()
        {
            // Convert our cart into a sale for the API
            var sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
        }
    }
}
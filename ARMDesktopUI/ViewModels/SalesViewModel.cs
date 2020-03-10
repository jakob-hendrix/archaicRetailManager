using System;
using System.Collections.Generic;
using ARMDesktopUI.Library.Api;
using ARMDesktopUI.Library.Helpers;
using ARMDesktopUI.Library.Models;
using Caliburn.Micro;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using ARMDesktopUI.Models;
using AutoMapper;

namespace ARMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _windowManager;
        private int _itemQuantity = 1;
        private BindingList<ProductDisplayModel> _products;
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper,
            ISaleEndpoint saleEndpoint, IMapper mapper, StatusInfoViewModel status, IWindowManager windowManager)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _status = status;
            _windowManager = windowManager;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex);
                TryClose();
            }
        }

        private void ShowErrorDialog(Exception ex)
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.Title = "System Error";

            if (ex.Message == "Unauthorized")
            {
                _status.UpdateMessage("Unauthorized Access",
                    "You do not have the correct permission for this action.\n\nPlease contact your system administrator.");
                _windowManager.ShowDialog(_status, null, settings);
            }
            else
            {
                _status.UpdateMessage("Fatal Exception", $"{ex.Message}");
                _windowManager.ShowDialog(_status, null, settings);
            }
        }

        private async Task ResetViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            // TODO: add clearing of the cart item if it doesn't do it manually

            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAllProducts();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        public BindingList<ProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public ProductDisplayModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                var item = new CartItemDisplayModel
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

        public bool CanRemoveFromCart => SelectedCartItem?.QuantityInCart > 0;

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
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

            // if successful, we should reset the page back to it's starting conditions
            await ResetViewModel();
        }
    }
}
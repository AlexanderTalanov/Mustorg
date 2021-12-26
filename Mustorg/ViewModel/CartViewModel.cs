using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BLL.Interfaces;
using BLL.Models;
using Mustorg.Util;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace Mustorg.ViewModel
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly IOrderService _orderService;
        private readonly IProfileService _profileService;
        private readonly IFileService _fileService;
        private readonly int _userId;

        public delegate void DialogHandler(int id);
        public event DialogHandler OrderCreated;
        public CartViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IDialogService dialogService, IOrderService orderService, IProfileService profileService, int userId, IFileService fileService)
        {
            _crud = dbCrud;
            _categoryService = categoryService;
            _catalogService = productCatalogService;
            _dialogService = dialogService;
            _orderService = orderService;
            _profileService = profileService;
            _userId = userId;
            _fileService = fileService;

            IsVisible = "Visible";
            CanMakeOrder = false;
            OrderVisibility = "Hidden"; 
            CouponSale = $"Скидка за купон: 0 руб.";
            MessageVisibility = "Hidden";
            CheckVisibility = "Hidden";

            UpdateOrderPage();

            PickPoint = new ObservableCollection<PickPointModel>();
            var res = _crud.GetPickPoints();
            foreach (var i in res)
            {
                PickPoint.Add(i);
            }

            Messenger.Default.Register<GenericMessage<CartModel>>(this, Update);
        }

        private string _IsVisible;
        public string IsVisible 
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }

        private void UpdateOrderPage()
        {
            total = 0;

            Cart = new ObservableCollection<CartModel>();
            var result = _orderService.GetShoppingCart(_userId);
            foreach (CartModel i in result)
            {
                ProductModel product = _crud.GetProduct(i.ProductId);
                if (product.Amount <= i.Amount) i.PlusEnabled = false;
                else i.PlusEnabled = true;
                i.ViewPrice = $"Стоимость: {i.FullPrice:0.#} руб.";
                if (i.FullSale == null) i.FullSale = 0;
                if (i.Amount == 1) i.MinusEnabled = false;
                else i.MinusEnabled = true;
                i.ViewSale = $"Скидка: {i.FullSale:0.#} руб.";
                i.ViewTotal = $"Итого: {i.FullPrice - i.FullSale:0.#} руб.";
                Cart.Add(i);
                total += i.FullPrice - i.FullSale;
            }
            if (Cart.Count != 0)
            {
                CanMakeOrder = true;
                IsVisible = "Hidden";
                foreach (var i in Cart)
                {
                    if (i.Exists == "Visible")
                    {
                        CanMakeOrder = false;
                    }
                }
            }
            else
            {
                CanMakeOrder = false;
                IsVisible = "Visible";
            }

            Coupon = new ObservableCollection<CouponModel>();
            CouponModel none = new CouponModel();
            none.CouponName = "Нет";
            none.Offer = 0;
            Coupon.Add(none);
            var resu = _profileService.GetCoupons(_userId, (decimal)total);
            foreach (var i in resu)
            {
                Coupon.Add(i);
            }

            StatusSale = $"Скидка за статус: {total * _orderService.GetStatusSale(_userId):0.##} руб.";
            Total = $"Итого: {total * (1m - _orderService.GetStatusSale(_userId)):0.##} руб.";
            CouponSale = $"Скидка за купон: 0 руб.";
            SelectedCoupon = 0;
        }


        #region Корзина
        private void Update(GenericMessage<CartModel> msg)
        {
            UpdateOrderPage();
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(args => BinClicked(args));
                return _deleteCommand;
            }
        }
        private void BinClicked(object args)
        {
            _crud.DeleteCart(Cart[(int)args].Id);
            UpdateOrderPage();
        }


        private ICommand _plusAmountCommand;
        public ICommand PlusAmountCommand
        {
            get
            {
                if (_plusAmountCommand == null)
                    _plusAmountCommand = new RelayCommand(args => PlusClicked(args));
                return _plusAmountCommand;
            }
        }
        private void PlusClicked(object args)
        {
            Cart[(int)args].Amount++;
            _crud.UpdateCart(Cart[(int)args]);
            UpdateOrderPage();
        }


        private ICommand _minusAmountCommand;
        public ICommand MinusAmountCommand
        {
            get
            {
                if (_minusAmountCommand == null)
                    _minusAmountCommand = new RelayCommand(args => MinusClicked(args));
                return _minusAmountCommand;
            }
        }
        private void MinusClicked(object args)
        {
            Cart[(int)args].Amount--;
            _crud.UpdateCart(Cart[(int)args]);
            UpdateOrderPage();
        }
        

        private ObservableCollection<CartModel> _Cart;
        public ObservableCollection<CartModel> Cart 
        {
            get
            {
                return _Cart;
            }
            set
            {
                _Cart = value;
                NotifyPropertyChanged("Cart");
            }
        }
        #endregion


        #region Заказ
        public ICommand EndMakeOrder
        {
            get
            {
                if (_endMakeOrder == null)
                    _endMakeOrder = new RelayCommand(args => CloseMakeOrder(args));
                return _endMakeOrder;
            }
        }
        private ICommand _endMakeOrder;
        private void CloseMakeOrder(object args)
        {
            OrderVisibility = "Hidden";
        }

        public ICommand MakeOrder
        {
            get
            {
                if (_makeOrder == null)
                    _makeOrder = new RelayCommand(args => OpenMakeOrder(args));
                return _makeOrder;
            }
        }
        private ICommand _makeOrder;
        private void OpenMakeOrder(object args)
        {
            OrderVisibility = "Visible";
        }

        public ICommand EndOrder
        {
            get
            {
                if (_endOrder == null)
                    _endOrder = new RelayCommand(args => MakeOrderCheck(args));
                return _endOrder;
            }
        }
        private ICommand _endOrder;
        private void MakeOrderCheck(object args)
        {
            try 
            {
                if (_orderService.MakeOrder(_userId, Coupon[SelectedCoupon].Id, PickPoint[SelectedPickPoint].Id, (decimal)total - (decimal)Coupon[SelectedCoupon].Offer, Cart) != 0)
                {
                    Message = "Успешно! Вам доступен новый купон.";
                    MessageVisibility = "Visible";
                    CheckVisibility = "Visible";
                    OrderCreated?.Invoke(0);
                }                
            }
            catch (Exception ex)
            {
                Message = "Ошибка";
                MessageVisibility = "Visible";
                _fileService.PrintExceptions(ex);
            }
            finally
            {
                UpdateOrderPage();
            }
        }

        public bool CanMakeOrder 
        {
            get 
            {
                return _CanMakeOrder;
            }
            set
            {
                _CanMakeOrder = value;
                NotifyPropertyChanged("CanMakeOrder");
            }
        }
        private bool _CanMakeOrder;

        public string OrderVisibility
        { 
            get
            {
                return _Ordervisibility;
            }
            set 
            {
                _Ordervisibility = value;
                NotifyPropertyChanged("OrderVisibility");
            }
        }
        private string _Ordervisibility;

        public int SelectedPickPoint
        {
            get
            {
                return _SelectedPickPoint;
            }
            set
            {
                _SelectedPickPoint = value;
                NotifyPropertyChanged("SelectedPickPoint");
            }
        }
        private int _SelectedPickPoint;

        public ICommand SelectedIndexChangedCommand
        {
            get
            {
                if (_selectedIndexChangedCommand == null)
                    _selectedIndexChangedCommand = new RelayCommand(args => ChangeIndex(args));
                return _selectedIndexChangedCommand;
            }
        }
        private ICommand _selectedIndexChangedCommand;
        private void ChangeIndex(object args)
        {
            SelectedCoupon = (int)args;
            CouponSale = $"Скидка за купон: {Coupon[SelectedCoupon].Offer:0.#} руб.";
            Total = $"Итого: {total * (1 - _orderService.GetStatusSale(_userId)) - Coupon[SelectedCoupon].Offer:0.##} руб.";
        }

        public int SelectedCoupon
        {
            get
            {
                return _SelectedCoupon;
            }
            set
            {
                _SelectedCoupon = value;
                NotifyPropertyChanged("SelectedCoupon");
            }
        }
        private int _SelectedCoupon;

        public ObservableCollection<CouponModel> Coupon
        {
            get
            {
                return _Coupon;
            } 
            set
            {
                _Coupon = value;
                NotifyPropertyChanged("Coupon");
            }
        }
        private ObservableCollection<CouponModel> _Coupon;

        public ObservableCollection<PickPointModel> PickPoint { get; set; }

        public string Total
        {
            get
            {
                return _Total;
            }
            set
            {
                _Total = value;
                NotifyPropertyChanged("Total");
            }
        }
        private string _Total;
        private decimal? total;

        public string CouponSale
        {
            get 
            {
                return _CouponSale;
            }
            set
            {
                _CouponSale = value;
                NotifyPropertyChanged("CouponSale");
            }
        }
        private string _CouponSale;

        public string StatusSale 
        {
            get
            {
                return _StatusSale;
            }
            set
            {
                _StatusSale = value;
                NotifyPropertyChanged("StatusSale");
            }
        }
        private string _StatusSale;
        #endregion


        #region Сообщение
        public ICommand Back
        {
            get
            {
                if (_back == null)
                    _back = new RelayCommand(args => CloseMessage(args));
                return _back;
            }
        }
        private ICommand _back;
        private void CloseMessage(object args)
        {
            OrderVisibility = "Hidden";
            MessageVisibility = "Hidden";
            CheckVisibility = "Hidden";
        }

        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                NotifyPropertyChanged("Message");
            }
        }
        private string _Message; 

        public string MessageVisibility
        {
            get
            {
                return _MessageVisibility;
            }
            set
            {
                _MessageVisibility = value;
                NotifyPropertyChanged("MessageVisibility");
            }
        }
        private string _MessageVisibility;

        private ICommand _PDF;
        public ICommand PDF
        {
            get
            {
                if (_PDF == null)
                    _PDF = new RelayCommand(args => ExportToPDF(args));
                return _PDF;
            }
        }
        private void ExportToPDF(object args)
        {
            var orders = _crud.GetAllOrders();
            int key = orders[orders.Count - 1].Id;
            _fileService.PrintCheque(key);
        }

        private string _CheckVisibility;
        public string CheckVisibility
        {
            get
            {
                return _CheckVisibility;
            }
            set
            {
                _CheckVisibility = value;
                NotifyPropertyChanged("CheckVisibility");
            }

        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

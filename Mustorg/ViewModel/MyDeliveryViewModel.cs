using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BLL.Interfaces;
using BLL.Models;
using Mustorg.Util;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace Mustorg.ViewModel
{
    public class MyDeliveryViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly IProfileService _profileService;
        private readonly IFileService _fileService;
        private readonly int _userId;
        public MyDeliveryViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IDialogService dialogService, IProfileService profileService, int userId, IFileService fileService)
        {
            _crud = dbCrud;
            _categoryService = categoryService;
            _catalogService = productCatalogService;
            _dialogService = dialogService;
            _profileService = profileService;
            _userId = userId;
            _fileService = fileService;

            string date = "12/31/2020";
            DateStart = DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture);
            DateEnd = DateTime.Now;

            Status = new ObservableCollection<StatusModel>();
            var stat = _crud.GetAllStatuses();
            SelectedStatus = 0;
            Status.Add(new StatusModel { Id = 0, Name = "Любой" });
            foreach (var i in stat)
            {
                Status.Add(i);
            }

            Orders = new ObservableCollection<OrderModel>();
            Update(0);
        }
        public void Update(int nullable)
        {
            Orders.Clear();
            var orders = _profileService.GetCustomerOrders(_userId, -1, DateStart, DateEnd);
            foreach (var i in orders)
            {
                i.ViewSum = $"{i.Sum:0.#} руб.";
                i.PickPoint = $"Пункт выдачи: {i.PickPoint}";
                i.OrderStatus = $"Статус заказа: {i.OrderStatus}";
                i.ViewDate = i.Date.ToShortDateString();
                Orders.Add(i);
            }
            if (Orders.Count == 0)
            {
                IsVisible = "Visible";
            }
            else
            {
                IsVisible = "Hidden";
            }
        }

        private ICommand _sort;
        public ICommand Sort
        {
            get
            {
                if (_sort == null)
                    _sort = new RelayCommand(args => SortOrders(args));
                return _sort;
            }
        }

        private void SortOrders(object args)
        {
            Update(0);
        }

        private ICommand _cheque;
        public ICommand Cheque
        {
            get
            {
                if (_cheque == null)
                    _cheque = new RelayCommand(args => PrintCheque(args));
                return _cheque;
            }
        }
        private void PrintCheque(object args)
        {
            _fileService.PrintCheque(Orders[(int)args].Id);
        }

        private int _SelectedStatus;
        public int SelectedStatus
        {
            get
            {
                return _SelectedStatus;
            }
            set
            {
                _SelectedStatus = value;
                NotifyPropertyChanged("SelectedValue");
            }
        }

        private DateTime _DateStart;
        public DateTime DateStart
        {
            get
            {
                return _DateStart; 
            }
            set
            {
                _DateStart = value;
                NotifyPropertyChanged("DateStart");
            }
        }

        private DateTime _DateEnd;
        public DateTime DateEnd
        {
            get
            {
                return _DateEnd;
            }
            set
            {
                _DateEnd = value;
                NotifyPropertyChanged("DateEnd");
            }
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

        public ObservableCollection<StatusModel> Status { get; set; }

        private ObservableCollection<OrderModel> _Orders;
        public ObservableCollection<OrderModel> Orders
        {
            get
            {
                return _Orders;
            }
            set
            {
                _Orders = value;
                NotifyPropertyChanged("Orders");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

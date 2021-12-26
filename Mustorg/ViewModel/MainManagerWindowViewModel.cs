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
    public class MainManagerWindowViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private readonly IManagerService _managerService;
        private readonly int _userId;

        public MainManagerWindowViewModel(IDbCrud dbCrud, IDialogService dialogService, int userId, IFileService fileService, IManagerService managerService)
        {
            _crud = dbCrud;
            _dialogService = dialogService;
            _fileService = fileService;
            _managerService = managerService;
            _userId = userId;

            Expanded = false;
            ExpandedVisibility = "Hidden";

            Text = ""; 
            string date = "12/31/2020";
            DateStart = DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture);
            DateEnd = DateTime.Now;
            Manager = _crud.GetManager(_userId);
            Manager.Activity_Name = $"Место работы: {Manager.Activity_Name}";
            if (Manager.Pick_Point_Name != null) Manager.Pick_Point_Name = $"Пункт выдачи: {Manager.Pick_Point_Name}";
            else Manager.Pick_Point_Name = $"Склад: Крутицкая улица, 29";

            Orders = new ObservableCollection<OrderModel>();
            Update(0);
        }

        public void Update(int nullable)
        {
            Orders.Clear();
            if (Manager.Activity_Id == 1)
            {
                var orders = _managerService.GetOrdersWarehouse(DateStart, DateEnd, 0, -1, Text);
                foreach (var i in orders)
                {
                    i.ManButton = "Отправить";
                    i.ViewDate = i.Date.ToShortDateString();
                    i.PickPoint = $"Пункт выдачи: {i.PickPoint}";
                    i.ViewId = $"ID заказа: {i.Id}";
                    Orders.Add(i);
                }
            }
            else
            {
                var orders = _managerService.GetOrdersWarehouse(DateStart, DateEnd, 1, Manager.Pick_Point_Id, Text);
                foreach (var i in orders)
                {
                    if (i.Order_Status_Id == 2) i.ManButton = "Принять";
                    else i.ManButton = "Выдать";
                    i.ViewDate = i.Date.ToShortDateString();
                    i.PickPoint = $"Пункт выдачи: {i.PickPoint}";
                    i.ViewId = $"ID заказа: {i.Id}";
                    Orders.Add(i);
                }
            }
        }

        private bool _Expanded;
        public bool Expanded
        {
            get
            {
                return _Expanded;
            }
            set
            {
                _Expanded = value;
                if (ExpandedVisibility == "Hidden") ExpandedVisibility = "Visible";
                else ExpandedVisibility = "Hidden";
                NotifyPropertyChanged("Expanded");
            }
        }

        private string _ExpandedVisibility;
        public string ExpandedVisibility
        {
            get
            {
                return _ExpandedVisibility;
            }
            set
            {
                _ExpandedVisibility = value;
                NotifyPropertyChanged("ExpandedVisibility");
            }
        }

        public ManagerModel Manager { get; set; }

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

        private ICommand _send;
        public ICommand Send
        {
            get
            {
                if (_send == null)
                    _send = new RelayCommand(args => ChangeOrderStatus(args));
                return _send;
            }
        }
        private void ChangeOrderStatus(object args)
        {
            if (Orders[(int)args].OrderStatus == "Обрабатывается")
            {
                Orders[(int)args].Order_Status_Id = 2;
                _crud.UpdateOrder(Orders[(int)args]);
            }
            else if (Orders[(int)args].OrderStatus == "Отправлен")
            {
                Orders[(int)args].Order_Status_Id = 1;
                _crud.UpdateOrder(Orders[(int)args]);
            }
            else if (Orders[(int)args].OrderStatus == "Доставлен")
            {
                Orders[(int)args].Order_Status_Id = 5;
                _crud.UpdateOrder(Orders[(int)args]);
            }
            Update(0);
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

        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                NotifyPropertyChanged("Text");
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


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

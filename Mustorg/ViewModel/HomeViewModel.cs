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
using GalaSoft.MvvmLight.Messaging;


namespace Mustorg.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly IProfileService _profileService;
        private readonly int _userId;

        public delegate void DialogHandler(int productId);
        public event DialogHandler Notify;

        public HomeViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IDialogService dialogService, IProfileService profileService, int userId)
        {
            _crud = dbCrud;
            _categoryService = categoryService;
            _catalogService = productCatalogService;
            _dialogService = dialogService;
            _userId = userId;
            _profileService = profileService;

            var tempProd = _catalogService.GetTopFiveProducts(_userId);
            Products = new ObservableCollection<ProductModel>();

            Update(0);
        }

        public void Update(int nullable)
        {
            Products.Clear();
            var tempProd = _catalogService.GetTopFiveProducts(_userId);
            foreach (var i in tempProd)
            {
                i.ViewPrice = $"{i.Price:0.#} руб.";
                Products.Add(i);
            }

            if (Products.Count != 0)
            {
                LabelVisible = "Hidden";
            }
            else
            {
                LabelVisible = "Visible";
            }
        }

        private string _LabelVisible;
        public string LabelVisible
        {
            get
            {
                return _LabelVisible;
            }
            set
            {
                _LabelVisible = value;
                NotifyPropertyChanged("LabelVisible");
            }
        }

        private ICommand _selectedIndexCommand;
        public ICommand SelectedIndexChangedCommand
        {
            get
            {
                if (_selectedIndexCommand == null)
                    _selectedIndexCommand = new RelayCommand(args => SelectedIndexChanged(args));
                return _selectedIndexCommand;
            }
        }
        private void SelectedIndexChanged(object args)
        {
            if ((int)args != -1)
            {
                Notify?.Invoke(Products[(int)args].Id);
            }
        }


        public ObservableCollection<ProductModel> Products
        {
            get
            {
                return _Products;
            }
            set
            {
                _Products = value;
                NotifyPropertyChanged("Products");
            }
        }
        private ObservableCollection<ProductModel> _Products;



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

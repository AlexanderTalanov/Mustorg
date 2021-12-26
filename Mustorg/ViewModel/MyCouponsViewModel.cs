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
    public class MyCouponsViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly IProfileService _profileService;
        private readonly int _userId;

        public MyCouponsViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IDialogService dialogService, IProfileService profileService, int userId)
        {
            _crud = dbCrud;
            _categoryService = categoryService;
            _catalogService = productCatalogService;
            _dialogService = dialogService;
            _profileService = profileService;
            _userId = userId;

            Update(0);
        }

        public void Update(int nullable)
        {
            Coupons = new ObservableCollection<CouponModel>();
            var result = _profileService.GetCoupons(_userId, 1000000);
            foreach (var i in result)
            {
                i.ViewText = $"Купон на скидку {i.Offer:0.#} руб. \nПри покупке от {i.Condition:0.#} руб. ";
                Coupons.Add(i);
            }
        }

        public ObservableCollection<CouponModel> Coupons
        {
            get 
            {
                return _Coupons;
            }
            set 
            {
                _Coupons = value;
                NotifyPropertyChanged("Coupons");
            }
        }
        private ObservableCollection<CouponModel> _Coupons;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

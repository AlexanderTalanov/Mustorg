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
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly IProfileService _profileService;
        private readonly int _userId;

        public ProfileViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IDialogService dialogService, IProfileService profileService, int userId, IFileService fileService)
        {
            _crud = dbCrud;
            _categoryService = categoryService;
            _catalogService = productCatalogService;
            _dialogService = dialogService;
            _profileService = profileService;
            _userId = userId;

            CouponsVM = new MyCouponsViewModel(dbCrud, categoryService, productCatalogService, dialogService, profileService, userId);
            ProfileVM = new MyProfileViewModel(dbCrud, categoryService, productCatalogService, dialogService, profileService, userId);
            OrdersVM = new MyOrdersViewModel(dbCrud, categoryService, productCatalogService, dialogService, profileService, userId, fileService);
            DeliveryVM = new MyDeliveryViewModel(dbCrud, categoryService, productCatalogService, dialogService, profileService, userId, fileService);
            StatsVM = new StatsViewModel(dbCrud, categoryService, productCatalogService, dialogService, profileService, userId);
        }

        public MyProfileViewModel ProfileVM { get; set; }
        public MyCouponsViewModel CouponsVM { get; set; }
        public MyOrdersViewModel OrdersVM { get; set; }
        public StatsViewModel StatsVM { get; set; }
        public MyDeliveryViewModel DeliveryVM { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

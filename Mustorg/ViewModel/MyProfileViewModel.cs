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
    public class MyProfileViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly IProfileService _profileService;
        private readonly int _userId;
        public MyProfileViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IDialogService dialogService, IProfileService profileService, int userId)
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
            var user = _profileService.GetCutomerProfile(_userId);
            Progress = user.Progress;
            Name = user.Name;
            Money = $"Сумма всех заказов: {user.Sum:0.#} руб.";

            switch (user.Customer_Status_Id)
            {
                case 1:
                    {
                        Color = "DarkGoldenrod";
                        Gold = 1;
                        Bronze = Base = Silver = 0.3;
                    }break;
                case 2:
                    {
                        Color = "Silver";
                        Silver = 1;
                        Bronze = Base = Gold = 0.3;
                    }
                    break;
                case 3:
                    {
                        Color = "SaddleBrown";
                        Bronze = 1;
                        Gold = Base = Silver = 0.3;
                    }
                    break;
                case 4:
                    {
                        Color = "LightSteelBlue";
                        Base = 1;
                        Bronze = Gold = Silver = 0.3;
                    }
                    break;
            }
        }

        private string _Money;
        public string Money
        {
            get
            {
                return _Money;
            }
            set
            {
                _Money = value;
                NotifyPropertyChanged("Money");
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }
        
        private double _Progress;
        public double Progress
        {
            get 
            {
                return _Progress;
            }
            set
            {
                _Progress = value;
                NotifyPropertyChanged("Progress");
            }
        }

        private double _Gold;
        public double Gold
        {
            get
            {
                return _Gold;
            }
            set
            {
                _Gold = value;
                NotifyPropertyChanged("Gold");
            }
        }

        private double _Silver;
        public double Silver
        {
            get
            {
                return _Silver;
            }
            set
            {
                _Silver = value;
                NotifyPropertyChanged("Silver");
            }
        }

        private double _Bronze;
        public double Bronze
        {
            get
            {
                return _Bronze;
            }
            set
            {
                _Bronze = value;
                NotifyPropertyChanged("Bronze");
            }
        }

        private double _Base;
        public double Base
        {
            get
            {
                return _Base;
            }
            set
            {
                _Base = value;
                NotifyPropertyChanged("Base");
            }
        }

        private string _Color;
        public string Color
        {
            get 
            {
                return _Color;
            }
            set
            {
                _Color = value;
                NotifyPropertyChanged("Color");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

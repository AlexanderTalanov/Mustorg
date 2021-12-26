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
    public class CatalogViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly int _userId;

        public CatalogViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IDialogService dialogService, int userId)
        {
            _crud = dbCrud;
            _categoryService = categoryService;
            _catalogService = productCatalogService;
            _dialogService = dialogService;
            _userId = userId;

            IsVisible = "Hidden";
            Selected = -1;

            var tempTypes = _categoryService.GetTypeModels();
            Types = new ObservableCollection<TypeModel>();
            foreach (var i in tempTypes)
            {
                Types.Add(i);
            }

            Cost = new ObservableCollection<string>();
            Cost.Add("Нет");
            Cost.Add("По возрастанию");
            Cost.Add("По убыванию");

            Brands = new ObservableCollection<BrandModel>();
            var brands = _crud.GetAllBrands();
            Brands.Add(new BrandModel { Id = -1, Name = "Любой" });
            foreach (var i in brands)
            {
                Brands.Add(i);
            }

            SelectedBrand = 0;
            Text = "";
            SelectedSort = 0;

            Update(null);
        }


        #region Фильтры
        private ICommand _selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand
        {
            get
            {
                if (_selectedItemChangedCommand == null)
                    _selectedItemChangedCommand = new RelayCommand(args => ChangeCategory(args));
                return _selectedItemChangedCommand;
            }
        }
        private void ChangeCategory(object args)
        {
            if (args != null)
            {
                SelectedCategory = args.ToString();
            }
        }

        private ICommand _noSelection;
        public ICommand NoSelection
        {
            get
            {
                if (_noSelection == null)
                    _noSelection = new RelayCommand(args => DeleteSelection(args));
                return _noSelection;
            }
        }
        private void DeleteSelection(object args)
        {
            SelectedCategory = null;
            var tempTypes = _categoryService.GetTypeModels();
            Types = new ObservableCollection<TypeModel>();
            foreach (var i in tempTypes)
            {
                Types.Add(i);
            }
        }

        private ICommand _sort;
        public ICommand Sort
        {
            get
            {
                if (_sort == null)
                    _sort = new RelayCommand(args => Update(args));
                return _sort;
            }
        }
        private void Update(object args)
        {
            var tempProd = _catalogService.GetProductWithFilters(SelectedCategory, SelectedBrand, Text, SelectedSort);
            Products = new ObservableCollection<ProductModel>();
            foreach (var i in tempProd)
            {
                i.ViewPrice = $"{i.Price:0.#} руб.";
                Products.Add(i);
            }
            SelectedIndexChanged(Selected);
        }

        private string _Path;
        public string Path
        {
            get
            {
                return _Path;
            }
            set 
            {
                _Path = value;
                NotifyPropertyChanged("Path");
            }
        }

        private string SelectedCategory;

        private int _SelectedBrand;
        public int SelectedBrand
        {
            get
            {
                return _SelectedBrand;
            }
            set
            {
                _SelectedBrand = value;
                NotifyPropertyChanged("SelectedBrand");
            }
        }

        private int _SelectedSort;
        public int SelectedSort
        {
            get
            {
                return _SelectedSort;
            }
            set
            {
                _SelectedSort = value;
                NotifyPropertyChanged("SelectedSort");
            }
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

        public ObservableCollection<string> Cost { get; set; }
        public ObservableCollection<BrandModel> Brands { get; set; }
        #endregion


        #region Выбор товара
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
                IsVisible = "Visible";
                productId = Products[(int)args].Id;

                var Product = _crud.GetAllProducts().Where(i => i.Id == productId).ToList();
                Article = " Артикул: " + Product[0].Article.ToString();
                Guarantee = " Гарантия " + Product[0].Guarantee;
                Description = "Характеристики: \n" + Product[0].Description;
                Name = Product[0].Name;
                Photo = Product[0].Photo;
                Price = $"Стоимость: {Product[0].Price:0.#} руб.";
                CanAddToCart = Product[0].Availability;

                if (Product[0].Sale != null)
                {
                    Sale = $"Скидка: {Product[0].Sale:0.#} руб.";
                }
                else
                {
                    Sale = "";
                }

                if (Product[0].Availability == true)
                {
                    Availability = " Есть в наличии";
                }
                else
                {
                    Availability = " Нет в наличии";
                }
            }
        }

        public void Update(int productId)
        {
            if (productId != -1 && productId != 0)
            {
                IsVisible = "Visible";
                this.productId = productId;

                var Product = _crud.GetAllProducts().Where(i => i.Id == this.productId).ToList();
                Article = " Артикул: " + Product[0].Article.ToString();
                Guarantee = " Гарантия " + Product[0].Guarantee;
                Description = "Характеристики: \n" + Product[0].Description;
                Name = Product[0].Name;
                Photo = Product[0].Photo;
                Price = $"Стоимость: {Product[0].Price:0.#} руб.";
                CanAddToCart = Product[0].Availability;

                if (Product[0].Sale != null)
                {
                    Sale = $"Скидка: {Product[0].Sale:0.#} руб.";
                }
                else
                {
                    Sale = "";
                }

                if (Product[0].Availability == true)
                {
                    Availability = " Есть в наличии";
                }
                else
                {
                    Availability = " Нет в наличии";
                }
            }
            else if (this.productId != -1 && productId == 0)
            {
                var Product = _crud.GetAllProducts().Where(i => i.Id == this.productId).ToList();
                if (Product.Count != 0)
                {
                    CanAddToCart = Product[0].Availability;

                    if (Product[0].Availability == true)
                    {
                        Availability = " Есть в наличии";
                    }
                    else
                    {
                        Availability = " Нет в наличии";
                    }
                }
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

        private int productId;
        public string Article
        {
            get
            {
                return _Article;
            }
            set
            {
                _Article = value;
                NotifyPropertyChanged("Article");
            }
        }
        private string _Article;
        public string Guarantee
        {
            get
            {
                return _Guarantee;
            }
            set
            {
                _Guarantee = value;
                NotifyPropertyChanged("Guarantee");
            }
        }
        private string _Guarantee;
        public string Description 
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                NotifyPropertyChanged("Description");
            }
        }
        private string _Description;
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
        private string _Name;
        public string Photo 
        { 
            get
            {
                return _Photo;
            }
            set
            {
                _Photo = value;
                NotifyPropertyChanged("Photo");
            }
        }
        private string _Photo;
        public string Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
                NotifyPropertyChanged("Price");
            }
        }
        private string _Price;
        public string Sale {
            get
            {
                return _Sale;
            }
            set
            {
                _Sale = value;
                NotifyPropertyChanged("Sale");
            }
        }
        private string _Sale;
        public string Availability {
            get
            {
                return _Availability;
            }
            set
            {
                _Availability = value;
                NotifyPropertyChanged("Availability");
            }
        }
        private string _Availability;
        public bool CanAddToCart
        {
            get 
            {
                return _CanAddToCart;
            }
            set
            {
                _CanAddToCart = value;
                NotifyPropertyChanged("CanAddToCart");
            }
        }
        private bool _CanAddToCart;
        #endregion


        #region Добавить в корзину
        public int Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                NotifyPropertyChanged("Selected");
            }
        }
        private int _Selected;

        private ICommand _addToCart;
        public ICommand AddToCart
        {
            get
            {
                if (_addToCart == null)
                    _addToCart = new RelayCommand(args => UpdateCart(args));
                return _addToCart;
            }
        }
        private void UpdateCart(object args)
        {
            CartModel cart = new CartModel();
            cart.ProductId = productId;
            cart.CustomerId = _userId; 
            _crud.CreateCart(cart);
            Messenger.Default.Send(new GenericMessage<CartModel>(null));
        }

        private ICommand _back;
        public ICommand Back
        {
            get
            {
                if (_back == null)
                    _back = new RelayCommand(args => GoBack(args));
                return _back;
            }
        }
        private void GoBack(object args)
        {
            Selected = -1;
            IsVisible = "Hidden";
        }
        #endregion


        private ObservableCollection<TypeModel> _Types;
        public ObservableCollection<TypeModel> Types
        {
            get
            {
                return _Types;
            }
            set
            {
                _Types = value;
                NotifyPropertyChanged("Types");
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

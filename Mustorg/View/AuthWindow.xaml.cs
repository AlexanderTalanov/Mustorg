using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mustorg.ViewModel;
using BLL.Interfaces;
using Mustorg.Util;


namespace Mustorg
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow(IDbCrud crudService, ICategoryService categoryService, ICatalogService productCatalogService, IOrderService orderService, IProfileService profileService, IFileService fileService, IManagerService managerService)
        {
            InitializeComponent();

            AuthViewModel viewModel = new AuthViewModel(crudService, categoryService, productCatalogService, orderService, new DialogService(), profileService, fileService, managerService);

            DataContext = viewModel;
            viewModel.Notify += CloseWindow;
            viewModel.ErrorAlert += CloseWindow;

        }
        
        void CloseWindow()
        {
            this.Close();
        }
    }
}

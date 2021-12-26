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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mustorg.ViewModel;
using BLL.Interfaces;
using Mustorg.Util;

namespace Mustorg
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(IDbCrud crudService, ICategoryService categoryService, ICatalogService productCatalogService, IOrderService orderService, IProfileService profileService, int userId, IFileService fileService)
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(crudService, categoryService, productCatalogService, orderService, new DialogService(), profileService, userId, fileService);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using BLL.Interfaces;
using View.Util;
using Ninject;


namespace Mustorg
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var kernel = new StandardKernel(new NinjectRegistration(), new ServiceModule());

            IDbCrud crudServ = kernel.Get<IDbCrud>();
            ICategoryService catServ = kernel.Get<ICategoryService>();
            ICatalogService prodCatServ = kernel.Get<ICatalogService>();
            IOrderService ordServ = kernel.Get<IOrderService>();
            IProfileService profServ = kernel.Get<IProfileService>();
            IFileService fileServ = kernel.Get<IFileService>();
            IManagerService manServ = kernel.Get<IManagerService>();

            AuthWindow authWindow = new AuthWindow(crudServ, catServ, prodCatServ, ordServ, profServ, fileServ, manServ);
            authWindow.Show();
        }
    }
}

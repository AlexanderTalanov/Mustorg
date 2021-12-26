using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mustorg.View;
using BLL.Interfaces;

namespace Mustorg.Util
{
    public class DialogService : IDialogService
    {
        public void OpenMainWindow(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IOrderService orderService, IDialogService dialogService, IProfileService profileService, int userId, IFileService fileService)
        {
            MainWindow mainWindow = new MainWindow(dbCrud, categoryService, productCatalogService, orderService, profileService, userId, fileService);
            mainWindow.Show();
        }
        public void OpenManMainWindow(IDbCrud dbCrud, int userId, IFileService fileService, IManagerService managerService)
        {
            ManagerMainWindow mainWindow = new ManagerMainWindow(dbCrud, userId, fileService, managerService);
            mainWindow.Show();
        }
    }
}

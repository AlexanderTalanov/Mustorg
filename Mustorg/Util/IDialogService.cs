using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace Mustorg.Util
{
    public interface IDialogService
    {
        void OpenMainWindow(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IOrderService orderService, IDialogService dialogService, IProfileService profileService, int UserId, IFileService fileService);
        void OpenManMainWindow(IDbCrud dbCrud, int userId, IFileService fileService, IManagerService managerService);
    }
}

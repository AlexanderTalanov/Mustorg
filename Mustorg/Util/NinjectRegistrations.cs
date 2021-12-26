using BLL.Interfaces;
using Ninject.Modules;
using BLL;
using BLL.Services;

namespace View.Util
{
    public class NinjectRegistration : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbCrud>().To<DBDataOperations>();
            Bind<ICategoryService>().To<CategoryService>();
            Bind<ICatalogService>().To<ProductCatalogService>();
            Bind<IOrderService>().To<OrderService>();
            Bind<IProfileService>().To<ProfileService>();
            Bind<IFileService>().To<FileService>();
            Bind<IManagerService>().To<ManagerService>();
        }
    }
}

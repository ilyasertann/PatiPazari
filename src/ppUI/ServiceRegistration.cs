using DataAccessLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using TravelApi.DataAccessLayer;


namespace TravelApi
{
    public static class ServiceRegistration
    {
        public static void AddCustomDependenciesConfigure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<PatiPazariDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));

            services.AddTransient<ICategoryDal, EfCategoryRepository>();
            services.AddTransient<ICategoryService, CategoryManager>();

            services.AddTransient<IUserDal, EfUserRepository>();
            services.AddTransient<IUserService, UserManager>();

            services.AddTransient<IAdvertDal, EfAdvertRepository>();
            services.AddTransient<IAdvertService, AdvertManager>();

            services.AddTransient<IBasketDal, EfBasketRepository>();
            services.AddTransient<IBasketService, BasketManager>();

            services.AddTransient<IOrderDal, EfOrderRepository>();
            services.AddTransient<IOrderService, OrderManager>();



        }

        public static void AddCustomValdiatorConfigure(this IServiceCollection services)
        {
         
        }
    }
  
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;
using Tech_Shop.Interfaces;
using Tech_Shop.Mocks;

[assembly: OwinStartupAttribute(typeof(Tech_Shop.Startup))]
namespace Tech_Shop
{
    public partial class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAllDevices, MockDevices>();
            services.AddTransient<IDeviceCategory, MockCategory>();
        }
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

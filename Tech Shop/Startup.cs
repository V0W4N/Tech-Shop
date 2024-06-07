using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using Tech_Shop.Interfaces;

[assembly: OwinStartupAttribute(typeof(Tech_Shop.Startup))]
namespace Tech_Shop
{
    public partial class Startup
    {
      
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

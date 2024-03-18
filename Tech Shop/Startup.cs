using Microsoft.Owin;
using Owin;

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

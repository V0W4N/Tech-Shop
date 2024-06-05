using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
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
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                SlidingExpiration = true
            });
            ConfigureAuth(app);
        }
    }
}

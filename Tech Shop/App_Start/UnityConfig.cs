using System.Web.Mvc;
using Unity.Mvc5;
using Tech_Shop.Services;
using Tech_Shop.Models;
using Unity;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Tech_Shop.Controllers;
using Unity.Injection;
using System;

public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();


        container.RegisterType<DbContext>();
        container.RegisterType<CartService>();
        container.RegisterType<OrderService>();
        container.RegisterType<ApplicationDbContext, ApplicationDbContext>();
        // Register all controllers
        var controllers = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => typeof(IController).IsAssignableFrom(t) && t.IsClass);

        foreach (var controllerType in controllers)
        {
            var constructor = controllerType.GetConstructors().FirstOrDefault();
            if (constructor != null)
            {
                container.RegisterType(controllerType, new InjectionConstructor());
            }
        }

        DependencyResolver.SetResolver(new UnityDependencyResolver(container));
    }
}

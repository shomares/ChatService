using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Ninject.Web.Common.OwinHost;
using ChatServicio.src.services;
using Microsoft.AspNet.SignalR;
using ChatServicio.src.services.util;

[assembly: OwinStartup(typeof(ChatServicio.Startup))]

namespace ChatServicio
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            GlobalHost.DependencyResolver = new SignalRNinjectDependencyResolver(Service.Instance.Kernel);
            app.UseStaticFiles("/views");
            app.UseStaticFiles("/Scripts");
            app.UseStaticFiles("/Scripts/app");
            app.UseStaticFiles("/Content");
            app.MapSignalR();

        }


    }
}

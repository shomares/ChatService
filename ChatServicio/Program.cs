using ChatServicio.src.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ChatServicio
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[]
          {
                new ChatService(),
                new ChatServiceWeb()
          };

            if (Environment.UserInteractive)
            {
                RunServicesInInteractiveMode(servicesToRun);
            }
            else
            {
                ServiceBase.Run(servicesToRun);
            }

        }

        private static void RunServicesInInteractiveMode(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Start the services in interactive mode...");
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var onStartMethod = typeof(ServiceBase).GetMethod("OnStart", bindingFlags);

            foreach (var service in servicesToRun)
            {
                Console.Write("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.WriteLine("started");
            }
            Console.WriteLine("Press <ENTER> to stop services...");
            Console.ReadLine();

            var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", bindingFlags);
            foreach (var service in servicesToRun)
            {
                Console.Write("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("stopped");
            }
            Console.WriteLine("All services are stopped...");
        }
    }
}

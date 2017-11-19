using ChatServicio.src.chatTcp;
using ChatServicio.src.interfaces;
using ChatServicio.src.repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServicio.src.services
{
    public class Service
    {
        private static Service _instance;

        public IKernel Kernel { get; set; }


        public IServerChat GetServer()
        {
            return Kernel.Get<IServerChat>();
        }

        public static Service Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Service();
                return _instance;
            }
        }

        private Service()
        {
            Kernel = new StandardKernel();
            Register();
        }

        private void Register()
        {
            Kernel.Bind<IRepositoryConnection>().To<MemoryRepository>();
            Kernel.Bind<IServerChat>().To<ServerChatTCP>().InSingletonScope();
        }



    }
}

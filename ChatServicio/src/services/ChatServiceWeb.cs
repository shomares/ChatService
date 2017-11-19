using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ChatServicio.src.services
{
    partial class ChatServiceWeb : ServiceBase
    {

        IDisposable _webHost;

        public ChatServiceWeb()
        {
            InitializeComponent();
            ServiceName = "ChatServiceWebSockets";

        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            var url = "http://*:8087/";
            _webHost = WebApp.Start<Startup>(url);
        }

        protected override void OnStop()
        {
            _webHost?.Dispose();
        }
    }
}

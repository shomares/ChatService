using ChatServicio.src.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServicio.src.services
{
    partial class ChatService : ServiceBase
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;
        private IServerChat _service;

        public ChatService()
        {
            InitializeComponent();
            ServiceName = "ChatServiceTCP";
        }

        protected override void OnStart(string[] args)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _task = Run(_cancellationTokenSource.Token);

        }

        private async Task Run(CancellationToken cancellationToken)
        {
            _service = Service.Instance.GetServer();
            _service.OnError += _service_OnError;
            _service.OnRecieved += _service_OnRecieved;
            _service.OnStart += _service_OnStart;
            _service.cancellationToken = cancellationToken;
            await _service.Start();
        }

        private void _service_OnStart(string data)
        {
           
        }

        private void _service_OnRecieved(string data)
        {
            
        }

        private void _service_OnError(Exception data)
        {
           
        }

        protected override void OnStop()
        {
            _cancellationTokenSource.Cancel();
           
        }
    }
}

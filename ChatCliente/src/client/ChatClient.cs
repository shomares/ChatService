using ChatCliente.Properties;
using ChatCliente.src.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatCliente.src.client
{
    public class ChatClient : IChatClient
    {
        private TcpClient client;
        private CancellationToken _token;

        #region events
        public event OnRecieved OnRecieved;
        public event OnStarted OnStart;
        public event OnException OnError;
        #endregion

        public CancellationToken cancellationToken
        {
            get
            {
                return _token;
            }

            set
            {
                _token = value;
            }
        }


        public ChatClient()
        {
            client = new TcpClient();
        }

        public void Start()
        {
            client.Connect(Settings.Default.server, Settings.Default.port);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    byte[] buffer = new byte[256];
                    client.Client.Receive(buffer, SocketFlags.None);
                    OnRecieved(ASCIIEncoding.ASCII.GetString(buffer));

                }
                catch (Exception) { }
            }
        }

        public async Task SendData(string data)
        {
            await Task.Run(() =>
            {
                client.Client.Send(ASCIIEncoding.ASCII.GetBytes(data), SocketFlags.None);
            }
            );
        }
    }
}

using ChatServicio.Properties;
using ChatServicio.src.chatWeb;
using ChatServicio.src.interfaces;
using ChatServicio.src.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServicio.src.chatTcp
{
    public class ServerChatTCP : IServerChat
    {
        #region events
        public event OnRecieved OnRecieved;
        public event OnStarted OnStart;
        public event OnException OnError;
        #endregion

        private IRepositoryConnection _repository;
        private TcpListener server;
        private CancellationToken _token;

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

        public ServerChatTCP(IRepositoryConnection _repository)
        {
            this._repository = _repository;
            server = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), Settings.Default.port);
         
            ThreadPool.SetMaxThreads(20, 20);
        }

        public  string GetLocalIPAddress()
        {

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (addr != null && !addr.Address.ToString().Equals("0.0.0.0"))
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return String.Empty;
        }

        public async Task Start()
        {
            server.Start();
            while (!_token.IsCancellationRequested)
            {
                OnStart("Initializad server...");
                Socket client = await server.AcceptSocketAsync();
                lock (this)
                {
                    _repository.AddClient(new ClientType()
                    {
                        Adresss = client.RemoteEndPoint,
                        Socket = client
                    });
                }
                //Init new thread--------------------
                ThreadPool.QueueUserWorkItem(Handler, client);
            }
            server.Stop();
        }
        private void Handler(object client)
        {
            string message = null;
            Socket cl = (Socket)client;
            ClientType clie = null;
            while (!_token.IsCancellationRequested)
            {
                if (cl.Connected)
                {
                    try
                    {
                        byte[] buffer = new byte[256];
                        clie = _repository.GetClient(cl.RemoteEndPoint);
                        cl.Receive(buffer, SocketFlags.None);
                        //Send broadcast
                        message = ASCIIEncoding.ASCII.GetString(buffer);
                        OnRecieved(message);
                        SendData(buffer);
                    }
                    catch (Exception)
                    {
                        lock (this)
                        {
                            _repository.DeleteClient(clie);
                        }
                    }
                }
                else
                {
                    lock (this)
                    {
                        _repository.DeleteClient(clie);
                    }

                }
            }
        }

        private void SendData(byte[] buffer)
        {
            ChatHub.SendAll(buffer);
            List<ClientType> toDelete = new List<ClientType>();

            foreach (var element in _repository.GetAllClient())
            {
                try
                {
                    element.Socket.Send(buffer, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    toDelete.Add(element);
                    element.Socket.Disconnect(false);
                    element.Socket.Close();
                    OnError(ex);
                }
            }
            lock (this) {
                toDelete.ForEach(s => { lock (this) { _repository.DeleteClient(s); } });
            }
           
        }


        public async Task RecieveData(string message)
        {
            await Task.Run(() =>
            {
                var buffer = ASCIIEncoding.ASCII.GetBytes(message);
                SendData(buffer);
            });

        }
    }
}

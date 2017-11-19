using ChatServicio.src.interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServicio.src.chatWeb
{
    public class ChatHub : Hub
    {
        private IServerChat _serverChat;


        public ChatHub(IServerChat _serverChat)
        {
            this._serverChat = _serverChat;
        }

        public void Send(String message)
        {
            _serverChat.RecieveData(message);
        }

        public static void SendAll(byte[] message)
        {

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.recieveData(ASCIIEncoding.ASCII.GetString(message).Replace('\0', ' '));
        }
    }
}

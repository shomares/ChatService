using ChatServicio.src.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ChatServicio.src.interfaces
{
    public interface IRepositoryConnection
    {
        void AddClient(ClientType client);
        IEnumerable<ClientType> GetAllClient();
        void DeleteClient(ClientType element);
        ClientType GetClient(EndPoint remoteEndPoint);
    }
}

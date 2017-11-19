using ChatServicio.src.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServicio.src.types;
using System.Net;

namespace ChatServicio.src.repositories
{
    public class MemoryRepository : IRepositoryConnection
    {
        private IDictionary<EndPoint, ClientType> _data;

        public MemoryRepository()
        {
            _data = new Dictionary<EndPoint, ClientType>();
        }

        public void AddClient(ClientType client)
        {
            _data.Add(client.Adresss, client);
        }

        public void DeleteClient(ClientType element)
        {
            if(element!=null)
            _data.Remove(element.Adresss);
        }

        public IEnumerable<ClientType> GetAllClient()
        {
            return _data.Values;
        }

        public ClientType GetClient(EndPoint remoteEndPoint)
        {
            return _data.ContainsKey(remoteEndPoint) ? _data[remoteEndPoint] : null;
        }
    }
}

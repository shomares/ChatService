using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServicio.src.types
{
    public class ClientType
    {
        public Socket Socket { get; set; }
        public EndPoint Adresss { get; set; }
    }
}

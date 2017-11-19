using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatCliente.src.interfaces
{

    #region events
    public delegate void OnStarted(string data);
    public delegate void OnException(Exception data);
    public delegate void OnRecieved(string data);
    #endregion
    public interface IChatClient
    {
        CancellationToken cancellationToken { get; set; }
        void Start();
        Task SendData(string data);
        event OnStarted OnStart;
        event OnException OnError;
        event OnRecieved OnRecieved;
    }
}

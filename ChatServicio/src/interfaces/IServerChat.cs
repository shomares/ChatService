using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServicio.src.interfaces
{
    #region events
    public delegate void OnStarted(string data);
    public delegate void OnException(Exception data);
    public delegate void OnRecieved(string data);
    #endregion
    public interface IServerChat
    {
        Task Start();
        Task RecieveData(string message);

        event OnStarted OnStart;
        event OnException OnError;
        event OnRecieved OnRecieved;

        CancellationToken cancellationToken { get; set; }

    }
}

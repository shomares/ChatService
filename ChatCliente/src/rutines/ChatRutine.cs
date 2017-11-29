using ChatCliente.src.client;
using ChatCliente.src.interfaces;
using ChatCliente.src.types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatCliente.src.rutines
{
    public class ChatRutine : INotifyPropertyChanged
    {
        private ObservableCollection<Message> _messagesObservation;
        private IChatClient _client;
        private bool _isActive;
        private CancellationTokenSource _cancellationTokenSource;
        private Thread th;
        public event OnException OnError;

        private Message _current;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Message> Messages
        {
            set { _messagesObservation = value; NotifyPropertyChanged("Messages"); }
            get { return _messagesObservation; }
        }
        public Message Current
        {
            set { _current = value; NotifyPropertyChanged("Current"); }
            get { return _current; }
        }

        public bool IsActive
        {
            set { _isActive = value; NotifyPropertyChanged("IsActive"); }
            get { return _isActive; }
        }


        public ChatRutine()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Messages = new ObservableCollection<Message>();
            this.IsActive = true;
            this._client = new ChatClient();
            this._client.cancellationToken = _cancellationTokenSource.Token;
            this._client.OnRecieved += _client_OnRecieved;
            this._client.OnError += _client_OnError;
            this._client.OnStart += _client_OnStart;
            Current = new Message();
        }

        private void _client_OnStart(string data)
        {
            App.Current?.Dispatcher.Invoke(() => // <--- HERE
            {
                this.IsActive = false;
            });
        }

        private void _client_OnError(Exception data)
        {
            OnError(data);
        }

        private void _client_OnRecieved(string data)
        {

            App.Current?.Dispatcher.Invoke(() => // <--- HERE
            {
                Messages.Add(new Message()
                {
                    Data = data.Replace('\0', ' '),
                    Fecha = DateTime.Now
                });
                NotifyPropertyChanged("Messages");
            });


        }

        public void Start()
        {
             th = new Thread(() =>
            {
                _client.Start();
            });
            th.Start();

        }

        public async void SendMessage()
        {
            if (!String.IsNullOrEmpty(_current.Data))
            {
                await _client.SendData(_current.Data);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            th.Abort();
        }
    }
}

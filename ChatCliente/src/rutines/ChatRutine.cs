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
        private CancellationTokenSource _cancellationTokenSource;

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



        public ChatRutine()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Messages = new ObservableCollection<Message>();
            this._client = new ChatClient();
            this._client.cancellationToken = _cancellationTokenSource.Token;
            this._client.OnRecieved += _client_OnRecieved;
            Current = new Message();
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
            Thread th = new Thread(() =>
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
        }
    }
}

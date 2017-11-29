using ChatCliente.src.rutines;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatCliente
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ChatRutine rutinas;
        public MainWindow()
        {
            InitializeComponent();
            rutinas = Resources["ChatRutine"] as ChatRutine;

            rutinas.OnError += Rutinas_OnError;
            rutinas.Start();

            
        }

        private async void Rutinas_OnError(Exception data)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                await this.ShowMessageAsync("Error", "No se conectar con el servicio");
                System.Windows.Application.Current.Shutdown();
            });
           
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                rutinas.Current.Data = ((TextBox)sender).Text;
                rutinas.SendMessage();
            }

        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            rutinas.Stop();
            System.Windows.Application.Current.Shutdown();
        }
    }
}

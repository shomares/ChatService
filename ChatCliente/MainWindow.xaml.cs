using ChatCliente.src.rutines;
using MahApps.Metro.Controls;
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
            rutinas.Start();
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                rutinas.Current.Data = ((TextBox)sender).Text;
                rutinas.SendMessage();
            }

        }
    }
}

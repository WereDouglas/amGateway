using amLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace amGateway
{
    /// <summary>
    /// Interaction logic for EditComm.xaml
    /// </summary>
    public partial class EditComm : Window
    {
        
        private Prefix _prefix;
         private Network _network;
        private ObservableCollection<Prefix> _prefixList = null;
        public EditComm(string provided,string comm)
        {
            InitializeComponent();
            providers.Content = provided;
            commPort.Text = comm;
          
        }

     

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (commPort.Text != "")
            {
                _network = new Network(null);
                _network.Update(providers.Content.ToString(), commPort.Text);
                System.Windows.MessageBox.Show("Updated");
                this.DialogResult = true;
               
            }
            else
            {
                MessageBox.Show("please input the port !");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

using amLibrary;
using GsmComm.GsmCommunication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace amGateway
{
    /// <summary>
    /// Interaction logic for MessagePage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        private Message _message;
        private ObservableCollection<Message> _messageList = null;
        string url = "http://smsintergration.cloudapp.net/access.sms/getLastHour.php?id=";
        private Network _network;
        private ObservableCollection<Network> _networkList = null;

        private BackgroundWorker bw = new BackgroundWorker();
        private GsmCommMain comm;
        public SettingPage()
        {
            InitializeComponent();
            Refresh();
            save.Visibility = Visibility.Hidden;
        }

       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all selected messages?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (Network u in dtGridNetwork.SelectedItems)
                {
                    u.Delete(u.Id.ToString());
                }
                Refresh();
            }
            else
            {
                return;
            }
        }
        private void Refresh()
        {
            dtGridNetwork.ItemsSource = null;            
            _networkList = new ObservableCollection<Network>(App.am.Networks);
            dtGridNetwork.ItemsSource = _networkList;           

        }

        private void Detect_Click(object sender, RoutedEventArgs e)
        {
            connect();
        }
        public void connect()
        {
            string cmbCOM = "COM" + comms.Text;
            comm = new GsmCommMain(cmbCOM, 9600, 150);

            try
            {

                if (comm.IsConnected())
                {
                    info.Content = comm.IdentifyDevice().Manufacturer.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().Manufacturer.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().Model.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().Revision.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().SerialNumber.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.GetCurrentOperator();
                    info.Content = info.Content + Environment.NewLine + comm.GetSignalQuality();
                    info.Content = info.Content + Environment.NewLine + comm.GetSmscAddress();
                    info.Content = info.Content + Environment.NewLine + comm.GetSubscriberNumbers();
                    save.Visibility = Visibility.Visible;


                    Console.WriteLine("comm is already open");

                }
                else
                {
                    Console.WriteLine("comm is not open");
                    comm.Open();

                    info.Content = comm.IdentifyDevice().Manufacturer.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().Manufacturer.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().Model.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().Revision.ToUpper().ToString();
                    info.Content = info.Content + Environment.NewLine + comm.IdentifyDevice().SerialNumber.ToUpper().ToString();
                    save.Visibility = Visibility.Visible;

                }

            }
            catch (Exception r)
            {

                MessageBox.Show(r.Message);
            }

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (network.Text != "")
            {
                _network = App.am.Networks.Add();
                _network.Names = network.Text;
                _network.Comm = "COM" + comms.Text;
                _network.Status = "disconnected";
                _network.Save();

                System.Windows.MessageBox.Show("Network saved ");
                Refresh();
            }
            else
            {
                MessageBox.Show("please input the network name");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as System.Windows.Controls.Button;
            Network user = button.DataContext as Network;

            if (MessageBox.Show("Are you sure you want to delete " + user.Names + " ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                user.Delete(user.Id.ToString());
                Refresh();
            }
            else
            {

                return;
            }
        }

        private void dtGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrefixButton_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as System.Windows.Controls.Button;
            Network user = button.DataContext as Network;
            AddPrefix inputDialog = new AddPrefix(user.Names);
            if (inputDialog.ShowDialog() == true)
                 Refresh();
        }

        private void EditCommButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as System.Windows.Controls.Button;
            Network user = button.DataContext as Network;
            EditComm inputDialog = new EditComm(user.Names,user.Comm);
            if (inputDialog.ShowDialog() == true)
                              Refresh();
        }


    }
}

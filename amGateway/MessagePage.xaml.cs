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
using System.Timers;
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
    public partial class MessagePage : Page
    {
        private Message _message;
        private ObservableCollection<Message> _messageList = null;
        string url = "http://smsintergration.cloudapp.net/access.sms/getLastHour.php?id=";
        private Network _network;
        private ObservableCollection<Network> _networkList = null;

        private BackgroundWorker bwMessage = new BackgroundWorker();
        private GsmCommMain comm;
        public int intervals = 2;
        public MessagePage()
        {
            
            InitializeComponent();
            intervals = Convert.ToInt32(interval.Text);
            Refresh();
            Timer timer = new Timer(1000 * 60);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            syncs(url);

        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate()
            {
                InputBlock.Text = " " + "  " + DateTime.Now.ToString();// InputBlock.Text + Environment.NewLine + "now sending messages";
               // Scroller.ScrollToBottom();
                InputBlock.ScrollToEnd();
            });

            if (!bwMessage.IsBusy)
                tasks();
            Console.WriteLine("new process after one minute:");
        }
        public void tasks()
        {

            bwMessage.RunWorkerAsync();
            bwMessage.WorkerReportsProgress = true;
            //  bwMessage.WorkerSupportsCancellation = true;
            bwMessage.DoWork += new DoWorkEventHandler(bwMessage_DoWork);
            bwMessage.ProgressChanged += new ProgressChangedEventHandler(bwMessage_ProgressChanged);
            bwMessage.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwMessage_RunWorkerCompleted);
        }
        private void bwMessage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                //this.tbProgress.Content = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                // this.tbProgress.Content = ("Error: " + e.Error.Message);
            }

            else
            {
                // this.tbProgress.Content = "All messages sent";
            }
        }
        private void bwMessage_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // this.tbProgress.Content = (e.ProgressPercentage.ToString() + "Count");
        }

        private void bwMessage_DoWork(object sender, DoWorkEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate()
            {
                InputBlock.Text = InputBlock.Text + Environment.NewLine + "Downloading messages from the server......" + "  " + DateTime.Now.ToString();
                Scroller.ScrollToBottom();
                InputBlock.ScrollToEnd();
            });
            Console.WriteLine("starting the back ground task");
            BackgroundWorker worker = sender as BackgroundWorker;

            syncs(url);
            System.Threading.Thread.Sleep(1000);

        }

        private void syncs(string url)
        {

            App.Current.Dispatcher.Invoke((Action)delegate()
            {
                InputBlock.Text = InputBlock.Text + Environment.NewLine + "Connecting to the download url" + "  " + DateTime.Now.ToString();
                Scroller.ScrollToBottom();
                InputBlock.ScrollToEnd();
            });

            double lastID = 0;
            _messageList = new ObservableCollection<Message>(App.am.Messages);

            try
            {

                lastID = _messageList.Max(h => Convert.ToDouble(h.Id));

            }
            catch
            {

                MessageBox.Show("you have no last ID please insert/reset the last ID !");
                App.Current.Dispatcher.Invoke((Action)delegate()
                {
                    InputBlock.Text = Environment.NewLine + "you have no last ID please insert/reset the last ID !";
                    Scroller.ScrollToBottom();
                    InputBlock.ScrollToEnd();
                });

            }

            if (lastID != 0)
            {

                url = url + lastID.ToString();
                using (var client = new WebClient())
                {

                    try
                    {
                        var json = client.DownloadString(url);
                        //Console.WriteLine(url);
                        WebRequest request = WebRequest.Create(url);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream dataStream = response.GetResponseStream();

                        StreamReader reader = new StreamReader(dataStream);

                        string responseFromServer = reader.ReadToEnd();

                        Console.WriteLine(responseFromServer);
                        App.Current.Dispatcher.Invoke((Action)delegate()
                        {
                            InputBlock.Text = InputBlock.Text + Environment.NewLine + responseFromServer + "  " + DateTime.Now.ToString();
                        });

                        // Console.WriteLine(response.ToString());
                        List<Message> models = JsonConvert.DeserializeObject<List<Message>>(responseFromServer);

                        for (int d = 0; d < models.Count; d++)
                        {
                            //Console.WriteLine( models[d].Numbers);


                            _message = App.am.Messages.Add();
                            _message.Numbers = models[d].Numbers;
                            _message.Id = models[d].Id;
                            _message.Messages = models[d].Messages;
                            _message.Dor = DateTime.Now.ToString();
                            _message.Sent = "F";

                            _message.Save();

                        }
                        App.Current.Dispatcher.Invoke((Action)delegate()
                        {
                            Refresh();
                        });
                    }
                    catch
                    {
                        Console.WriteLine("server is taking long to respond");
                        App.Current.Dispatcher.Invoke((Action)delegate()
                        {
                            InputBlock.Text = InputBlock.Text + Environment.NewLine + "server is taking long to respond!";
                            Scroller.ScrollToBottom();
                            InputBlock.ScrollToEnd();
                        });

                    }
                }

            }


        }
        private void Refresh()
        {
            dtGrid.ItemsSource = null;
            _messageList = new ObservableCollection<Message>(App.am.Messages);
            dtGrid.ItemsSource = _messageList.OrderByDescending(k => k.Id);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dtGrid.ItemsSource);
            view.Filter = UserFilter;
            port.Text = "ALL";
            port.Text = port.Text + Environment.NewLine + "Message No.: " + _messageList.Count().ToString();
            port.Text = port.Text + Environment.NewLine + "Unsent : " + _messageList.Where(k => k.Sent == "F").Select(t => t.Sent).Count().ToString();
            port.Text = port.Text + Environment.NewLine + "Sent : " + _messageList.Where(k => k.Sent == "T").Select(t => t.Sent).Count().ToString();

            portToday.Text = "TODAY";
            portToday.Text = portToday.Text + Environment.NewLine + "Message No.: " + _messageList.Where(k =>  Convert.ToDateTime(k.Dor).Date.ToString("d") == DateTime.Now.ToString("d")).Count().ToString();
            portToday.Text = portToday.Text + Environment.NewLine + "Unsent : " + _messageList.Where(k => k.Sent == "F" && Convert.ToDateTime(k.Dor).Date.ToString("d") == DateTime.Now.ToString("d")).Select(t => t.Sent).Count().ToString();
            portToday.Text = portToday.Text + Environment.NewLine + "Sent : " + _messageList.Where(k => k.Sent == "T" && Convert.ToDateTime(k.Dor).Date.ToString("d") == DateTime.Now.ToString("d")).Select(t => t.Sent).Count().ToString();

        }
        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dtGrid.ItemsSource).Refresh();
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
            {
                return true;
            }
            else if ((item as Message).Numbers.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ((item as Message).Numbers.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if ((item as Message).Messages.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ((item as Message).Messages.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if ((item as Message).Dor.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ((item as Message).Dor.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if ((item as Message).Sent.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ((item as Message).Sent.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all selected messages?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (Message u in dtGrid.SelectedItems)
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
       

        private void dtGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (chkSelectAll.IsChecked.Value == true)
            {
                dtGrid.SelectAll();
            }
            else
            {
                dtGrid.UnselectAll();
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (chkSelectAll.IsChecked.Value == true)
            {
                dtGrid.SelectAll();
            }
            else
            {
                dtGrid.UnselectAll();
            }
        }

        private void interval_LostFocus(object sender, RoutedEventArgs e)
        {
            intervals = Convert.ToInt32(interval.Text);
            Timer timer = new Timer(1000 * 60 * intervals);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }
    }
}

using amLibrary;
using amLibrary.Helpers;
using GsmComm.GsmCommunication;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlServerCe;
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
using System.Windows.Shapes;

namespace amGateway
{
    /// <summary>
    /// Interaction logic for GenWindow.xaml
    /// </summary>
    public partial class GenWindow : Window
    {
        private Message _message;
        private ObservableCollection<Message> _messageList = null;
        public string url = "http://smsintergration.cloudapp.net/access.sms/getLastHour.php?id=";
        private Network _network;
        private ObservableCollection<Network> _networkList = null;
        private Prefix _prefix;
        private ObservableCollection<Prefix> _prefixList = null;

        private BackgroundWorker bw = new BackgroundWorker();
        public int intervals = 5;

        public GenWindow()
        {
            InitializeComponent();
            intervals = Convert.ToInt32(interval.Text);

            string fileName = @"c:\amMessage\amMessage.sdf";

            if (File.Exists(fileName))
            {
                CreateDB();
            }
            else
            {
                CreateDirectories();
                CreateDB();
            }
            _messageList = new ObservableCollection<Message>(App.am.Messages);
            _networkList = new ObservableCollection<Network>(App.am.Networks);
            _mainFrame.NavigationService.Navigate(new Uri("MessagePage.xaml", UriKind.Relative));
            InputBlock.Text = "initializing...................";



            try
            {
                double lastID = _messageList.Max(h => Convert.ToDouble(h.Id));
                lastSMS.Content = lastID.ToString();
            }
            catch
            {

                MessageBox.Show("you have no last ID please insert/reset the last ID !");
            }

            Timer timer = new Timer(1000 * 60 * intervals);
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            Queue myQ = new Queue();
            if (Sending.IsInternetAvailable())
            {
                port2.Content = port2.Content + Environment.NewLine + " internet connection available";
            }
            else
            {
                port2.Content = port2.Content + Environment.NewLine + " no internet connection";
            }
            ConnectDevice();
          
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate()
            {
                InputBlock.Text = " " + DateTime.Now.ToString();// InputBlock.Text + Environment.NewLine + "now sending messages";
                Scroller.ScrollToBottom();
                InputBlock.ScrollToEnd();
            });

            if (!bw.IsBusy)
                tasks();
            Console.WriteLine("new process after one minute:");
            ProcessQueue();

        }
        public void tasks()
        {

            bw.RunWorkerAsync();
            bw.WorkerReportsProgress = true;
            //  bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // this.tbProgress.Content = (e.ProgressPercentage.ToString() + "Count");
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate()
            {
                InputBlock.Text = InputBlock.Text + Environment.NewLine + "starting the back ground task" + "  " + DateTime.Now.ToString();
                Scroller.ScrollToBottom();
                InputBlock.ScrollToEnd();
            });
            Console.WriteLine("starting the back ground task");
            BackgroundWorker worker = sender as BackgroundWorker;
            send();

            System.Threading.Thread.Sleep(1000);

        }

        public void ConnectDevice()
        {


            _networkList = new ObservableCollection<Network>(App.am.Networks);
            _network = new Network(null);
            foreach (Network n in _networkList)
            {

                if (Messenger.connected(n.Names, n.Comm))
                {
                    App.Current.Dispatcher.Invoke((Action)delegate()
                    {
                        _network.UpdateStatus(n.Names, "connected");
                        InputBlock.Text = InputBlock.Text + Environment.NewLine + "Connected:" + n.Names + "to:" + n.Comm;
                        Scroller.ScrollToBottom();
                        InputBlock.ScrollToEnd();
                    });
                    Console.WriteLine("modem  connected:" + n.Names + "to:" + n.Comm);

                    port2.Content = port2.Content + Environment.NewLine + "Connected:" + n.Names + "";

                }

                else
                {

                    _network.UpdateStatus(n.Names, "disconnected");
                    App.Current.Dispatcher.Invoke((Action)delegate()
                   {
                       InputBlock.Text = InputBlock.Text + Environment.NewLine + "Not connected:" + n.Names + "to:" + n.Comm;
                       Scroller.ScrollToBottom();
                       InputBlock.ScrollToEnd();
                   });
                    Console.WriteLine("not connected:" + n.Names + "to:" + n.Comm);
                    port2.Content = port2.Content + Environment.NewLine + " " + n.Names + " " + n.Status;
                }

            }


        }
        Queue<QueueClass> myQs = new Queue<QueueClass>();
        public void send()
        {
            Console.WriteLine("sending the messsages");

            App.Current.Dispatcher.Invoke((Action)delegate()
            {
                InputBlock.Text = InputBlock.Text + Environment.NewLine + "sending the messsages" + "  " + DateTime.Now.ToString();
            });
            _networkList = new ObservableCollection<Network>(App.am.Networks);
            foreach (Network n in _networkList)
            {
                if (n.Status == "connected")
                {

                    Console.WriteLine(" we are sending the messsages to " + n.Names + " Network:");

                    App.Current.Dispatcher.Invoke((Action)delegate()
                    {
                        InputBlock.Text = InputBlock.Text + Environment.NewLine + " we are sending the messsages to " + n.Names + " Network:" + "  " + DateTime.Now.ToString();
                        Scroller.ScrollToBottom();
                        InputBlock.ScrollToEnd();
                    });
                    _prefixList = new ObservableCollection<Prefix>(App.am.Prefixs);

                    foreach (Prefix p in _prefixList)
                    {
                        if (p.Provider == n.Names)
                        {

                            _messageList = new ObservableCollection<Message>(App.am.Messages);

                            if (_messageList.Where(y => y.Sent == "F").Count() > 0)
                            {
                                Console.WriteLine("sending for :" + n.Names);


                                foreach (Message u in _messageList)
                                {
                                    if (u.Sent == "F" && u.Numbers.Contains(p.Pre))
                                    {

                                        //  myQs.Enqueue(n.Comm,u.Numbers,u.Messages);

                                        App.Current.Dispatcher.Invoke((Action)delegate()
                                        {
                                            InputBlock2.Text = InputBlock2.Text + Environment.NewLine + "Number: " + u.Numbers + " : " + u.Messages + "  " + DateTime.Now.ToString();
                                            Scroller.ScrollToBottom();
                                            InputBlock2.ScrollToEnd();
                                        });
                                        try
                                        {
                                            //Sender(DBObject parent, string id, string message, string number, string network)
                                            // if (Messenger.SendUpdate(App.am, u.Id, u.Messages, u.Numbers, n.Names,n.Comm))
                                            //{
                                            if (Messenger.Sender(App.am, u.Id, u.Messages, u.Numbers, n.Names))
                                            {
                                                App.Current.Dispatcher.Invoke((Action)delegate()
                                                {
                                                    InputBlock2.Text = InputBlock2.Text + Environment.NewLine + "Message sent:... " + u.Numbers + " " + DateTime.Now.ToString();
                                                    Scroller.ScrollToBottom();
                                                    InputBlock2.ScrollToEnd();
                                                });
                                            }
                                            else
                                            {

                                                App.Current.Dispatcher.Invoke((Action)delegate()
                                                {
                                                    InputBlock2.Text = InputBlock2.Text + Environment.NewLine + " Message not sent:... " + u.Numbers + " " + DateTime.Now.ToString();
                                                    Scroller.ScrollToBottom();
                                                    InputBlock2.ScrollToEnd();
                                                });

                                            }
                                            Console.WriteLine("Sending process:-number:" + u.Numbers + " : " + u.Messages);
                                        }
                                        catch
                                        {
                                            App.Current.Dispatcher.Invoke((Action)delegate()
                                            {
                                                InputBlock.Text = InputBlock2.Text + Environment.NewLine + "there is an error somewhere....." + DateTime.Now.ToString();
                                                Scroller.ScrollToBottom();
                                                InputBlock2.ScrollToEnd();
                                            });

                                        }

                                    }
                                }


                            }
                        }
                    }
                }
            }
        }
        public bool Send(string mid, string message, string number, string comm)
        {

            return true;// Messenger.SendUpdate(App.am, mid, message, number, comm);

        }
        private static readonly System.Threading.SemaphoreSlim asyncLock = new System.Threading.SemaphoreSlim(1);
        private async void ProcessQueue()
        {
            // Lock this up so that one thread at a time can get through here.  
            // Others will do an async await until it is their turn.
            await asyncLock.WaitAsync();
            try
            {
                // Offload this to a background thread (so that the UI is not affected)
                var queueProcessingTask = Task.Run(() =>
                {
                    var processingStuck = false;
                    while (myQs.Count >= 1 && !processingStuck)
                    {

                        // Get the next item
                        var queueItem = myQs.Peek();
                        // Try to process this one. (ie DoStuff)
                        App.Current.Dispatcher.Invoke((Action)delegate()
                        {
                            InputBlock2.Text = InputBlock2.Text + Environment.NewLine + "Processing:." + queueItem.number + " " + " Processing this: " + queueItem.comm + "  " + DateTime.Now.ToString();
                            Scroller.ScrollToBottom();
                            InputBlock2.ScrollToEnd();
                        });


                        processingStuck = Send(queueItem.mid, queueItem.message, queueItem.number, queueItem.comm);
                        // If we processed successfully, then we can dequeue the item
                        if (!processingStuck)
                        {
                            myQs.Dequeue();
                            App.Current.Dispatcher.Invoke((Action)delegate()
                            {
                                InputBlock.Text = InputBlock2.Text + Environment.NewLine + "Removed from queue:." + queueItem.number + " " + " Processing this: " + queueItem.comm + "  " + DateTime.Now.ToString();
                                Scroller.ScrollToBottom();
                                InputBlock2.ScrollToEnd();
                            });
                        }
                    }
                });
                Task.WaitAll(queueProcessingTask);
            }
            finally
            {
                asyncLock.Release();
            }
        }

        private void CreateDB()
        {

            string con;

            con = string.Format(@"Data Source=c:\amMessage\amMessage.sdf;Password=access; Persist Security Info=True;");

            SqlCeConnection conn = new SqlCeConnection(con);
            conn.Open();
            SqlCeCommand cmd = conn.CreateCommand();

            if (!Helper.TableExists(conn, "message"))
            {
                cmd.CommandText = "CREATE TABLE message (id nvarchar(255)  NULL, ids nvarchar(255)  NULL, numbers nvarchar(255)  NULL,messages nvarchar(255) NULL,sent nvarchar(255) NULL,dor nvarchar(255) NULL);";
                cmd.ExecuteNonQuery();
            }
            if (!Helper.TableExists(conn, "network"))
            {
                //        cmd.CommandText = "INSERT INTO [network](id,names,comm)VALUES(@id,,@names,@comm)";
                cmd.CommandText = "CREATE TABLE network (id nvarchar(255)  NULL,comm nvarchar(255) NULL,names nvarchar(255) NULL,status nvarchar(255) NULL);";
                cmd.ExecuteNonQuery();
            }
            if (!Helper.TableExists(conn, "prefix"))
            {
                //        cmd.CommandText = "INSERT INTO [network](id,names,comm)VALUES(@id,,@names,@comm)";
                cmd.CommandText = "CREATE TABLE prefix (id nvarchar(255)  NULL,pre nvarchar(255) NULL,provider nvarchar(255) NULL);";
                cmd.ExecuteNonQuery();
            }

            if (!Helper.TableExists(conn, "id"))
            {
                //        cmd.CommandText = "INSERT INTO [network](id,names,comm)VALUES(@id,,@names,@comm)";
                cmd.CommandText = "CREATE TABLE id (id nvarchar(255)  NULL,comm nvarchar(255) NULL,names nvarchar(255) NULL);";
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        private void CreateDirectories()
        {

            // public static string conString = @"Data Source=C:\transporter\wimea.sdf;Password=wimea; Persist Security Info=True;";
            string path = @"c:\amMessage";
            if (Directory.Exists(path))
            {
                return;
            }
            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);


            string paths = @"c:\amMessage\images";
            if (Directory.Exists(paths))
            {

                return;
            }
            // Try to create the directory.
            DirectoryInfo dim = Directory.CreateDirectory(paths);
            Console.WriteLine("The directory was created successfully at {0}.",
            Directory.GetCreationTime(paths));
            string con;

            con = string.Format(@"Data Source=C:\amMessage\amMessage.sdf;Password=access; Persist Security Info=True;");
            SqlCeEngine en = new SqlCeEngine(con);
            en.CreateDatabase();


        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _mainFrame.NavigationService.Navigate(new Uri("MessagePage.xaml", UriKind.Relative));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _mainFrame.NavigationService.Navigate(new Uri("SettingPage.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConnectDevice();
        }

        private void refreshClick(object sender, RoutedEventArgs e)
        {

            port2.Content = "";
            _networkList = new ObservableCollection<Network>(App.am.Networks);
            _network = new Network(null);
            foreach (Network n in _networkList)
            {

                Messenger.disconnected(n.Names, n.Comm);
                _network.UpdateStatus(n.Names, "disconnected");
                port2.Content = Environment.NewLine + " " + n.Names + " disconnected";


            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            bw.Dispose();
            Application.Current.Shutdown();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void interval_LostFocus(object sender, RoutedEventArgs e)
        {
            intervals = Convert.ToInt32(interval.Text);
            Timer timer = new Timer(1000 * 60 * intervals);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void interval_TextChanged(object sender, TextChangedEventArgs e)
        {

        }




    }
    class QueueClass
    {
        public DBObject Obj { get; set; }
        public string mid { get; set; }
        public string message { get; set; }
        public string number { get; set; }
        public string comm { get; set; }
    }
}

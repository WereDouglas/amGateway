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
    /// Interaction logic for AddPrefix.xaml
    /// </summary>
    public partial class AddPrefix : Window
    {
        
        private Prefix _prefix;
        private ObservableCollection<Prefix> _prefixList = null;
        public AddPrefix(string provided)
        {
            InitializeComponent();
            providers.Content = provided;
            Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as System.Windows.Controls.Button;
           Prefix user = button.DataContext as Prefix;

            if (MessageBox.Show("Are you sure you want to delete " + user.Pre + " ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                user.Delete(user.Id.ToString());
                Refresh();
            }
            else
            {

                return;
            }
        }
        private void Refresh()
        {
            dtGrid.ItemsSource = null;
            _prefixList = new ObservableCollection<Prefix>(App.am.Prefixs.Where(t=>t.Provider==providers.Content.ToString()));
            dtGrid.ItemsSource = _prefixList;

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (pref.Text != "")
            {
                _prefix = App.am.Prefixs.Add();
                _prefix.Pre = pref.Text;
                _prefix.Provider = providers.Content.ToString();
                _prefix.Save();

                System.Windows.MessageBox.Show("Prefix saved");
                Refresh();
            }
            else
            {
                MessageBox.Show("please input the network name");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

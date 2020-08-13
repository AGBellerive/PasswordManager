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

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for GroupAccounts.xaml
    /// </summary>
    public partial class GroupedAccounts : Page
    {
        private static FileManager manager;
        public GroupedAccounts()
        {
            InitializeComponent();

            if (manager == null)
            {
                manager = new FileManager();
            }
        }

        private void searchEmail(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AccountListWithEmail.Text = "";
                List<Account> matched = manager.groupEmail(emailSearch.Text);

                if (matched.Count > 0 && emailSearch.Text.Length > 0) AccountListScroller.Visibility = Visibility.Visible;
                else AccountListScroller.Visibility = Visibility.Hidden;

                foreach (Account acc in matched)
                {
                    AccountListWithEmail.Text += acc.Site + "\n";
                }
            }
        }

        private void searchPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AccountListWithPassword.Text = "";
                List<Account> matched = manager.groupPassword(passwordSearch.Password);
                
                if (matched.Count > 0 && passwordSearch.Password.Length > 0) AccountListPasswordScroller.Visibility = Visibility.Visible;
                else AccountListPasswordScroller.Visibility = Visibility.Hidden;

                foreach (Account acc in matched)
                {
                    AccountListWithPassword.Text += acc.Site + "\n";
                }
            }
            
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new DisplayPassword();
        }
    }
}

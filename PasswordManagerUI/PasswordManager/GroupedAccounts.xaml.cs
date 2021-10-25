using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using log4net;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for GroupAccounts.xaml
    /// </summary>
    public partial class GroupedAccounts : Page
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static FileManager manager;

        /**
        * This constructor checks if the filemanager object is created, if it isnt
        * it creates a new object, then it hides multiple ui elements from the user
        * to then later be shwon
        */
        public GroupedAccounts()
        {
            InitializeComponent();
            LOG.Info("Group Account Initilized");
            if (manager == null)
            {
                manager = new FileManager();
            }
            AccountListWithEmail.Focus();
            AccountListScroller.Visibility = Visibility.Hidden;
            AccountListPasswordScroller.Visibility = Visibility.Hidden;
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

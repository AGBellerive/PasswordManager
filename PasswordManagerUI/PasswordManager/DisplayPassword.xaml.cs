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
    /// Interaction logic for DisplayPassword.xaml
    /// </summary>
    public partial class DisplayPassword : Page
    {
        private static FileManager manager;
        public bool isFileChanged { get; set; }

        /**
        * This constructor checks if the filemanager object is created, if it isnt
        * it creates a new object, then it hides multiple ui elements from the user
        * to then later be shwon
        */
        public DisplayPassword()
        {
            InitializeComponent();
            if (manager == null) manager = new FileManager();
            manager.readJson();
            SearchedAccountName.Focus();
            AccountListScroller.Visibility = Visibility.Hidden;
            otherLbl.Visibility = Visibility.Hidden;
            Other.Visibility = Visibility.Hidden;
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (SearchedAccountName.Text.Equals("Exit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0); 
                Account foundAccount = manager.searchAccount(SearchedAccountName.Text);
                
                if (foundAccount.Site.Equals("MULTI-FIND"))
                {
                    MultipleAccountDisplay mad = new MultipleAccountDisplay();
                    mad.load(SearchedAccountName.Text);
                    mad.AccountList.Text = "";

                    foreach (Account acc in manager.multiAccountFind)
                    {
                        mad.AccountList.Text += acc.Site +"\n";
                    }
                    Application.Current.MainWindow.Content = mad;
                }

                else if (foundAccount != null)
                {
                    AccountName.Content = foundAccount.Site;
                    UserName.Content = foundAccount.Username;
                    Email.Content = foundAccount.Email;
                    Password.Content = foundAccount.Password;

                    if (foundAccount.Other.Length > 0)
                    {
                        otherLbl.Visibility = Visibility.Visible;
                        Other.Visibility = Visibility.Visible;

                        Other.Content = foundAccount.Other;
                    }
                    else
                    {
                        otherLbl.Visibility = Visibility.Hidden;
                        Other.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void DisplayAllAccounts(object sender, RoutedEventArgs e)
        {
            AccountListScroller.Visibility = Visibility.Visible;
            AccountList.Visibility = Visibility.Visible;
            AccountList.Text = "";

            foreach (Account item in manager.allAccounts)
            {
                AccountList.Text += item.Site +"\n";
            }
            dropDown.IsExpanded = false;
        }

        private void Add_Account_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new AddAccount();
        }

        private void Change_Password_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new ChangePassword();
        }

        private void Grouped_Accounts_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new GroupedAccounts();
        }

        private void Delete_Account_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new DeleteAccount();
        }
    }
}

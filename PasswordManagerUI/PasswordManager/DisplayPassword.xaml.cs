using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for DisplayPassword.xaml
    /// </summary>
    public partial class DisplayPassword : Page
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static FileManager manager = new FileManager();

        private readonly Navigation nav;
        public bool isFileChanged { get; set; }

        /**
        * This constructor checks if the filemanager object is created, if it isnt
        * it creates a new object, then it hides multiple ui elements from the user
        * to then later be shwon
        */
        public DisplayPassword()
        {
            InitializeComponent();
            LOG.Info("Display password initilized");

            if (manager == null) manager = new FileManager();
            if (nav == null) nav = new Navigation();

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
                    LOG.Info("Changing to Multiple Account Display");
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
            LOG.Info("All accounts displaying");
            AccountListScroller.Visibility = Visibility.Visible;
            AccountList.Visibility = Visibility.Visible;
            AccountList.Text = "";

            //Temp solution. Whenever after initial setup and you try to click for all accounts, it crashes becaseu all accounts is null
            
            if (manager.allAccounts == null) return;

            foreach (Account item in manager.allAccounts)
            {
                AccountList.Text += item.Site +"\n";
            }
            dropDown.IsExpanded = false;
        }

        private void Add_Account_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Add Account Clicked");
            nav.GoToAddAccount();
        }

        private void Change_Password_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Change Password Clicked");
            nav.GoToChangePassword();
        }

        private void Grouped_Accounts_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Grouped Accounts Clicked");
            nav.GoToGroupedAccounts();
        }

        private void Delete_Account_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Delete Account Clicked");

            nav.GoToDeleteAccount();
        }
    }
}

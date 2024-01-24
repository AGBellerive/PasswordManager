using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;

namespace PasswordManager
{
    public partial class ChangePassword : Page
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static FileManager manager;
        private Account foundAccount;
        private readonly Navigation nav;

        /**
         * This constructor checks if the filemanager object is created, if it isnt
         * it creates a new object, then it hides multiple ui elements from the user
         * to then later be shown. This constructor also creates a navigation object
         * to deal with all the navigation of the application
        */
        public ChangePassword()
        {
            InitializeComponent();

            LOG.Info("Change password initilized");
            if (manager == null) manager = new FileManager();
            if (nav == null) nav = new Navigation();

            SearchedAccountName.Focus();
            UpdateBtn.Visibility = Visibility.Hidden;
            NewPassword.Visibility = Visibility.Hidden;
            newPasswordLbl.Visibility = Visibility.Hidden;
            DisplayAllAccounts();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Password Being updated");
            manager.updatePassword(NewPassword.Text,foundAccount);
            nav.GoToDisplayPassword();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                foundAccount = manager.specificSearch(SearchedAccountName.Text);

                if (foundAccount != null)
                {
                    AccountName.Content = foundAccount.Site;
                    UserName.Content = foundAccount.Username;
                    Email.Content = foundAccount.Email;
                    Password.Content = foundAccount.Password;

                    NewPassword.Visibility = Visibility.Visible;
                    UpdateBtn.Visibility = Visibility.Visible;
                    newPasswordLbl.Visibility = Visibility.Visible;
                }
            }
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Returning to display password");
            nav.GoToDisplayPassword();
        }

        private void DisplayAllAccounts()
        {
            LOG.Info("Displaying all accounts");
            AccountListScroller.Visibility = Visibility.Visible;
            AccountList.Visibility = Visibility.Visible;
            AccountList.Text = "";

            foreach (Account item in FileManager.allAccounts)
            {
                AccountList.Text += item.Site + "\n";
            }
        }
    }
}

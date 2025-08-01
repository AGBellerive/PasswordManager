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

            
            UpdateBtn.Visibility = Visibility.Hidden;
            NewPassword.Visibility = Visibility.Hidden;
            newPasswordLbl.Visibility = Visibility.Hidden;

            Loaded += (sender, e) => SearchBox.FocusInput(); // This allows the searchbox to pull focus when the page is launched
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Password Being updated");
            manager.updatePassword(NewPassword.Text,foundAccount);
            nav.GoToDisplayPassword();
        }

        private void SearchHandler(object sender, RoutedEventArgs e)
        {
            string SearchedAccountNameText = ((TextBox)e.OriginalSource).Text;
            foundAccount = manager.specificSearch(SearchedAccountNameText);
            if (foundAccount != null)
            {
                AccountName.Text = foundAccount.Site;
                UserName.Text = foundAccount.Username;
                Email.Text = foundAccount.Email;
                Password.Text = foundAccount.Password;

                NewPassword.Visibility = Visibility.Visible;
                UpdateBtn.Visibility = Visibility.Visible;
                newPasswordLbl.Visibility = Visibility.Visible;
            }
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Returning to display password");
            nav.GoToDisplayPassword();
        }

        private void PopulateLabels(Account foundAccount)
        {
            AccountName.Text = foundAccount.Site;
            UserName.Text = foundAccount.Username;
            Email.Text = foundAccount.Email;
            Password.Text = foundAccount.Password;

            NewPassword.Visibility = Visibility.Visible;
            UpdateBtn.Visibility = Visibility.Visible;
            newPasswordLbl.Visibility = Visibility.Visible;
            NewPassword.Focus();
        }

        private void AccountOnClick(object sender, RoutedEventArgs e)
        {
            Utils utils = new Utils();
            Account clickedAccount = utils.AccountOnClick(sender, e);
            PopulateLabels(clickedAccount);
            SearchBox.SearchedAccountName.Text = clickedAccount.Site;
        }
    }
}

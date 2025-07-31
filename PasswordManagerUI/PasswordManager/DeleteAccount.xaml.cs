using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;

namespace PasswordManager
{
    public partial class DeleteAccount : Page
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static FileManager manager;
        private Account accountToBeDeleted;
        private readonly Navigation nav;

        /**
         * This constructor checks if the filemanager object is created, if it isnt
         * it creates a new object, then it hides multiple ui elements from the user
         * to then later be shown. This constructor also creates a navigation object
         * to deal with all the navigation of the application
        */
        public DeleteAccount()
        {
            InitializeComponent();
            LOG.Info("Delete account initilized");
            if (manager == null) manager = new FileManager();
            if (nav == null) nav = new Navigation();

            deleteBtn.Visibility = Visibility.Hidden;
            otherLbl.Visibility = Visibility.Hidden;

            DisplayAllAccounts();

            Loaded += (sender, e) => SearchBox.FocusInput(); // This allows the searchbox to pull focus when the page is launched
        }

        private void DisplayAllAccounts()
        {
            LOG.Info("Displaying all accounts");
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            nav.GoToDisplayPassword();
        }

        private void SearchHandler(object sender, RoutedEventArgs e)
        {
            string SearchedAccountNameText = ((TextBox)e.OriginalSource).Text;
            if (SearchedAccountNameText.Equals("Exit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0);

            accountToBeDeleted = manager.specificSearch(SearchedAccountNameText);

            if (accountToBeDeleted != null)
            {
                PopulateLabels(accountToBeDeleted);
            }
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            manager.deleteAccount(accountToBeDeleted);
            returnBtn_Click(sender,e);
        }

        private void PopulateLabels(Account accountToBeDeleted)
        {
            AccountName.Text = accountToBeDeleted.Site;
            UserName.Text = accountToBeDeleted.Username;
            Email.Text = accountToBeDeleted.Email;
            Password.Text = accountToBeDeleted.Password;
            deleteBtn.Visibility = Visibility.Visible;

            if (accountToBeDeleted.Other.Length > 0)
            {
                otherLbl.Visibility = Visibility.Visible;
                Other.Visibility = Visibility.Visible;

                Other.Text = accountToBeDeleted.Other;
            }
            else
            {
                otherLbl.Visibility = Visibility.Hidden;
                Other.Visibility = Visibility.Hidden;
            }

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

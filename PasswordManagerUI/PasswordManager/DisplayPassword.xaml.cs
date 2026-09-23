using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using log4net;

namespace PasswordManager
{
    public partial class DisplayPassword : Page
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static FileManager manager = new FileManager();

        private readonly Navigation nav;
        public bool isFileChanged { get; set; }

        /**
         * This constructor checks if the filemanager object is created, if it isnt
         * it creates a new object, then it hides multiple ui elements from the user
         * to then later be shown. This constructor also creates a navigation object
         * to deal with all the navigation of the application
        */
        public DisplayPassword()
        {
            InitializeComponent();

            LOG.Info("Display password initilized");

            if (manager == null) manager = new FileManager();
            if (nav == null) nav = new Navigation();

            //manager.readJson();
            otherLbl.Visibility = Visibility.Hidden;
            Other.Visibility = Visibility.Hidden;
            CopyBtn.Visibility = Visibility.Hidden;
            LastLogin.Content = "Last Log In: " + manager.getLastLogIn();
            manager.updateLastLogin();

            Loaded += (sender, e) => SearchBox.FocusInput(); // This allows the searchbox to pull focus when the page is launched
        }


        /**
         * Whenever the user types in the testbox field, it is checked if the user
         * enteres the enter button. At this point its obvious they are trying to search
         * so once they search, the text is deciphered to wether they are trying to exit
         * the application, found an account or found many accounts with the similar 
         * text that is searched. If it is multiple accounts, that is then deletated to
         * another class
         */
        private void SearchHandler(object sender, RoutedEventArgs e)
        {
            string SearchedAccountNameText = ((TextBox)e.OriginalSource).Text;
            //Instead of directly refering to the "SearchAccountName" textbox, this casting allows us to refrence that block

            if (SearchedAccountNameText.Equals("Exit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0); 
               
            Account foundAccount = manager.searchAccount(SearchedAccountNameText);

            if (foundAccount.Site.Equals("MULTI-FIND"))
                {
                MultipleAccountDisplay mad = new MultipleAccountDisplay();
                LOG.Info("Changing to Multiple Account Display");
                mad.load(SearchedAccountNameText);
                    
                    Application.Current.MainWindow.Content = mad;
            }

            else if (foundAccount != null)
            {
                PopulateLabels(foundAccount);
            }
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

        public void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Copying credentials");
            Utils utils = new Utils();
            utils.CopyOnClick(UserName.Text, Email.Text, Password.Text);

            CopyBtn.Background = (Brush)Application.Current.Resources["PositiveButtonBrush"];
        }

        private void PopulateLabels(Account foundAccount)
        {
            AccountName.Text = foundAccount.Site;
            UserName.Text = foundAccount.Username;
            Email.Text = foundAccount.Email;
            Password.Text = foundAccount.Password;

            if (foundAccount.Password.Equals("")) CopyBtn.Visibility = Visibility.Hidden;

            else CopyBtn.Visibility = Visibility.Visible;


            if (foundAccount.Other.Length > 0)
            {
                otherLbl.Visibility = Visibility.Visible;
                Other.Visibility = Visibility.Visible;

                Other.Text = foundAccount.Other;
            }
            else
            {
                otherLbl.Visibility = Visibility.Hidden;
                Other.Visibility = Visibility.Hidden;
            }
        }

        public void AccountOnClick(object sender, RoutedEventArgs e)
        {
            Utils utils = new Utils();
            Account clickedAccount = utils.AccountOnClick(sender, e);
            PopulateLabels(clickedAccount);
            SearchBox.SearchedAccountName.Text = clickedAccount.Site;
        }
    }
}

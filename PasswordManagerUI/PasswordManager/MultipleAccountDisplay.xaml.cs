using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using log4net;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MultipleAccountDisplay.xaml
    /// </summary>
    public partial class MultipleAccountDisplay : Page
    {
        private static FileManager manager;
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /**
        * This constructor checks if the filemanager object is created, if it isnt
        * it creates a new object, then it hides multiple ui elements from the user
        * to then later be shwon
        */
        public MultipleAccountDisplay()
        {
            InitializeComponent();
            LOG.Info("Multiple account initilized");
            if (manager == null) manager = new FileManager();

            otherLbl.Visibility = Visibility.Hidden;
            Other.Visibility = Visibility.Hidden;
            CopyBtn.Visibility = Visibility.Hidden;
            MyAccountList.AccountListBox.ItemsSource = FileManager.multiAccountFind;

            Loaded += (sender, e) => SearchBox.FocusInput();
        }
        private void SearchHandler(object sender, RoutedEventArgs e)
        {

            string SearchedAccountNameText = ((TextBox)e.OriginalSource).Text;

            if (SearchedAccountNameText.Equals("Exit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0);

            Account account = manager.searchMultipleAccounts(SearchedAccountNameText);

            PopulateLabels(account);
        }


        public void load(String searchTerm)
        {
            manager.searchAccount(searchTerm);
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new DisplayPassword();

        }

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
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

        private void AccountOnClick(object sender, RoutedEventArgs e)
        {
            Utils utils = new Utils();
            Account clickedAccount = utils.AccountOnClick(sender, e); 
            PopulateLabels(clickedAccount);
            SearchBox.SearchedAccountName.Text = clickedAccount.Site;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            SearchedAccountName.Focus();
            if (manager == null) manager = new FileManager();

            otherLbl.Visibility = Visibility.Hidden;
            Other.Visibility = Visibility.Hidden;
            CopyBtn.Visibility = Visibility.Hidden;
            AccountListBox.ItemsSource = FileManager.multiAccountFind;

        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Account account = manager.searchMultipleAccounts(SearchedAccountName.Text);

                AccountName.Text = account.Site;
                UserName.Text = account.Username;
                Email.Text = account.Email;
                Password.Text = account.Password;

                if (account.Password.Equals("")) CopyBtn.Visibility = Visibility.Hidden;

                else CopyBtn.Visibility = Visibility.Visible;

                if (account.Other.Length > 0)
                {
                    otherLbl.Visibility = Visibility.Visible;
                    Other.Visibility = Visibility.Visible;

                    Other.Text = account.Other;
                }
                else
                {
                    otherLbl.Visibility = Visibility.Hidden;
                    Other.Visibility = Visibility.Hidden;
                }
            }
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
            //This is so that the main display password copy function is shared and no code duplication
            DisplayPassword dp = new DisplayPassword();
            dp.CopyBtn_Click(sender, e);
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

        private void AccountTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock tb && tb.DataContext is Account clickedAccount)
            {
                Account foundAccount = manager.searchAccount(clickedAccount.Site);
                PopulateLabels(foundAccount);

            }
        }
    }
}

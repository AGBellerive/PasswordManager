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

        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Account account = manager.searchMultipleAccounts(SearchedAccountName.Text);

                AccountName.Content = account.Site;
                UserName.Content = account.Username;
                Email.Content = account.Email;
                Password.Content = account.Password;

                if (account.Other.Length > 0)
                {
                    otherLbl.Visibility = Visibility.Visible;
                    Other.Visibility = Visibility.Visible;

                    Other.Content = account.Other;
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
    }
}

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
using log4net;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static FileManager manager;
        private Account foundAccount;

        /**
         * This constructor checks if the filemanager object is created, if it isnt
        * it creates a new object, then it hides multiple ui elements from the user
        * to then later be shwon
        */
        public ChangePassword()
        {
            InitializeComponent();

            LOG.Info("Change password initilized");
            if (manager == null) manager = new FileManager();
            
            SearchedAccountName.Focus();
            UpdateBtn.Visibility = Visibility.Hidden;
            NewPassword.Visibility = Visibility.Hidden;
            newPasswordLbl.Visibility = Visibility.Hidden;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            LOG.Info("Password Being updated");
            manager.updatePassword(NewPassword.Text,foundAccount);

            Application.Current.MainWindow.Content = new DisplayPassword(); ;
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
            Application.Current.MainWindow.Content = new DisplayPassword();
        }
    }
}

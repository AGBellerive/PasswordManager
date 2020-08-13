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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        private static FileManager manager;
        private Account foundAccount;

        public ChangePassword()
        {
            InitializeComponent();

            if (manager == null)
            {
                manager = new FileManager();
    }
}

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            manager.updatePassword(NewPassword.Text,foundAccount);
            Application.Current.MainWindow.Content = new DisplayPassword();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                foundAccount = manager.searchAccount(SearchedAccountName.Text);

                if (foundAccount != null)
                {
                    AccountName.Content = foundAccount.Site;
                    UserName.Content = foundAccount.Username;
                    Email.Content = foundAccount.Email;
                    Password.Content = foundAccount.Password;

                    NewPassword.Visibility = Visibility.Visible;
                    UpdateBtn.Visibility = Visibility.Visible;

                }
            }
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new DisplayPassword();
        }
    }
}

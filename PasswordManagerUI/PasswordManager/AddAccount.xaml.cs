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
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : Page
    {
        private static FileManager manager;
        /**
         * This constructor checks if the filemanager object is created, if it isnt
         * it creates a new object, then it hides multiple ui elements from the user
         * to then later be shwon
         */
        public AddAccount()
        {
            InitializeComponent();
            if (manager == null) manager = new FileManager();
            
            UsernameBx.Visibility = Visibility.Hidden;
            EmailBx.Visibility = Visibility.Hidden;
            PasswordBx.Visibility = Visibility.Hidden;
            otherInfoBx.Visibility = Visibility.Hidden;
            ConfirmBtn.Visibility = Visibility.Hidden;

            AccountBx.Focus();
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new DisplayPassword();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AccountBx.Text.Length > 1 && PasswordBx.Text.Length > 1)
            {
                manager.addAccount(AccountBx.Text, UsernameBx.Text, EmailBx.Text, PasswordBx.Text, otherInfoBx.Text);

                Application.Current.MainWindow.Content = new DisplayPassword();
            }
            else
            {
                MessageBox.Show("You're missing a value for "+ (AccountBx.Text.Length == 0 ? "Account!" : "Password!"));
            }

        }

        private void Check_If_Exists_Click(object sender, RoutedEventArgs e)
        {
            if (!manager.accountExists(AccountBx.Text) && AccountBx.Text.Length > 0)
            {
                AccountBx.BorderBrush = Brushes.Green;

                UsernameBx.Visibility = Visibility.Visible;
                EmailBx.Visibility = Visibility.Visible;
                PasswordBx.Visibility = Visibility.Visible; 
                otherInfoBx.Visibility = Visibility.Visible;
                ConfirmBtn.Visibility = Visibility.Visible;

                UsernameBx.Focus();
            }
            else
            {
                MessageBox.Show("This Account Exists Already");
                
                AccountBx.BorderBrush = Brushes.Red;
                UsernameBx.Visibility = Visibility.Hidden;
                EmailBx.Visibility = Visibility.Hidden;
                PasswordBx.Visibility = Visibility.Hidden;
                otherInfoBx.Visibility = Visibility.Hidden;
            }
        }

        private void AccountBx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Check_If_Exists_Click(sender, e);
            }
        }
    }
}

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
        public AddAccount()
        {
            InitializeComponent();
            if (manager == null)
            {
                manager = new FileManager();
            }
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
                returnBtn_Click(null, null);
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
    }
}

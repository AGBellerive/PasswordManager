using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for MultipleAccountDisplay.xaml
    /// </summary>
    public partial class MultipleAccountDisplay : Page
    {
        private static FileManager manager;

        public MultipleAccountDisplay()
        {
            InitializeComponent();
            SearchedAccountName.Focus();
            if (manager == null) manager = new FileManager();

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

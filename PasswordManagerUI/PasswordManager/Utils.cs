using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager
{
    class Utils
    {
        private static FileManager manager = new FileManager();

        public Utils()
        {
            if (manager == null) manager = new FileManager();
        }

        public Account AccountOnClick(object sender, RoutedEventArgs e)
        {
            Account foundAccount = new Account();
            if (e.OriginalSource is TextBlock tb && tb.DataContext is Account clickedAccount)
            {
                foundAccount = manager.specificSearch(clickedAccount.Site);
            }
            return foundAccount;
        }

        public void CopyOnClick(String Username, String Email, String Password)
        {
            if (!string.IsNullOrWhiteSpace(Username))
            {
                Clipboard.SetText(Username);
                System.Threading.Thread.Sleep(300);
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                Clipboard.SetText(Email);
                System.Threading.Thread.Sleep(300);
            }

            Clipboard.SetText(Password);

            MessageBox.Show("Credentials Copied.\nPress Windows Key + V to view credentials");
        }
        

    }
}

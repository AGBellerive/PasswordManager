using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager
{
    /**
     * This class is used to house all the navigation of this application
     */
    class Navigation
    {
        public void GoToAddAccount() {
            Application.Current.MainWindow.Content = new AddAccount();
        }
        public void GoToChangePassword() { 
            Application.Current.MainWindow.Content = new ChangePassword();
        }
        public void GoToDeleteAccount() {
            Application.Current.MainWindow.Content = new DeleteAccount();
        }
        public void GoToDisplayPassword() {
            Application.Current.MainWindow.Content = new DisplayPassword();
        }
        public void GoToGroupedAccounts() {
            Application.Current.MainWindow.Content = new GroupedAccounts();
        }
        public void GoToMainWindow() {
            Application.Current.MainWindow.Content = new MainWindow();
        }

        public void GoToFileConfiguration()
        {
            Application.Current.MainWindow.Content = new FileConfiguration();
        }
    }
}

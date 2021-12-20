using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager
{
    class Navigation
    {
        private static FileManager manager;

        public Navigation()
        {
            if (manager == null) manager = new FileManager();
        }
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
        public void GoToMainWindow() { }
        public void GoToMultipleAccount(string SearchedAccountName) { 
            MultipleAccountDisplay mad = new MultipleAccountDisplay();
            mad.load(SearchedAccountName);
            mad.AccountList.Text = "";

            foreach (Account acc in manager.multiAccountFind)
            {
                mad.AccountList.Text += acc.Site + "\n";
            }
            Application.Current.MainWindow.Content = mad;
        }
    }
}

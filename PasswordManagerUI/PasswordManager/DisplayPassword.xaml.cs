﻿using System;
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
    /// Interaction logic for DisplayPassword.xaml
    /// </summary>
    public partial class DisplayPassword : Page
    {
        private static FileManager manager;

        public DisplayPassword()
        {
            InitializeComponent();
            if (manager == null)
            {
                manager = new FileManager();
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Account foundAccount = manager.searchAccount(SearchedAccountName.Text);

                if (foundAccount != null)
                {
                    AccountName.Content = foundAccount.Site;
                    UserName.Content = foundAccount.Username;
                    Email.Content = foundAccount.Email;
                    Password.Content = foundAccount.Password;

                    if (foundAccount.Other.Length > 0)
                    {
                        otherLbl.Visibility = Visibility.Visible;
                        Other.Visibility = Visibility.Visible;

                        Other.Content = foundAccount.Other;
                    }
                    else
                    {
                        otherLbl.Visibility = Visibility.Hidden;
                        Other.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void DisplayAllAccounts(object sender, RoutedEventArgs e)
        {
            AccountListScroller.Visibility = Visibility.Visible;
            AccountList.Visibility = Visibility.Visible;

            foreach (Account item in manager.allAccounts)
            {
                AccountList.Text += item.Site +"\n";
            }
            dropDown.IsExpanded = false;
        }

        private void Add_Account_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new AddAccount();
        }

        private void Change_Password_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new ChangePassword();
        }

        private void Grouped_Accounts_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new GroupedAccounts();
        }
    }
}

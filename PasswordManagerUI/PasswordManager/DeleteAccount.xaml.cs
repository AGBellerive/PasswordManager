﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;

namespace PasswordManager
{
    public partial class DeleteAccount : Page
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static FileManager manager;
        private Account accountToBeDeleted;
        private readonly Navigation nav;

        /**
         * This constructor checks if the filemanager object is created, if it isnt
         * it creates a new object, then it hides multiple ui elements from the user
         * to then later be shown. This constructor also creates a navigation object
         * to deal with all the navigation of the application
        */
        public DeleteAccount()
        {
            InitializeComponent();
            LOG.Info("Delete account initilized");
            if (manager == null) manager = new FileManager();
            if (nav == null) nav = new Navigation();

            SearchedAccountName.Focus();
            deleteBtn.Visibility = Visibility.Hidden;
            DisplayAllAccounts();
        }

        private void DisplayAllAccounts()
        {
            LOG.Info("Displaying all accounts");
            AccountListScroller.Visibility = Visibility.Visible;
            AccountList.Visibility = Visibility.Visible;
            AccountList.Text = "";

            foreach (Account item in FileManager.allAccounts)
            {
                AccountList.Text += item.Site + "\n";
            }
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            nav.GoToDisplayPassword();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (SearchedAccountName.Text.Equals("Exit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0);
                accountToBeDeleted = manager.specificSearch(SearchedAccountName.Text);

                if (accountToBeDeleted != null)
                {
                    AccountName.Content = accountToBeDeleted.Site;
                    UserName.Content = accountToBeDeleted.Username;
                    Email.Content = accountToBeDeleted.Email;
                    Password.Content = accountToBeDeleted.Password;

                    deleteBtn.Visibility = Visibility.Visible;

                    if (accountToBeDeleted.Other.Length > 0)
                    {
                        otherLbl.Visibility = Visibility.Visible;
                        Other.Visibility = Visibility.Visible;

                        Other.Content = accountToBeDeleted.Other;
                    }
                    else
                    {
                        otherLbl.Visibility = Visibility.Hidden;
                        Other.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            manager.deleteAccount(accountToBeDeleted);
            returnBtn_Click(sender,e);
        }
    }
}

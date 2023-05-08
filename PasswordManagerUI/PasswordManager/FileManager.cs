using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using log4net;

namespace PasswordManager
{
    /**
     * This class is the center of the application.
     * This class communicates with the json file
     * that all the passwords are stored in
     * @Author Alex Bellerive
     */
    class FileManager
    {
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<Account> allAccounts { get; set; } = new List<Account>();
        public List<Account> multiAccountFind { get; set; } = new List<Account>();
        private string path;
        private readonly Navigation nav = new Navigation();

        public FileManager()
        {
            LOG.Info("Filemanager Created");
            checkForJson();
        }

        /**
         * This method reads the json file that contains all the passwords 
         * stored by the user and it converts it into a list to be easily
         * manipulated
         */
        public void readJson()
        {
            string json;
            try
            {
                json = File.ReadAllText(this.path);
                this.allAccounts = JsonConvert.DeserializeObject<List<Account>>(json);
                if (this.allAccounts == null)
                {
                    this.allAccounts = new List<Account>(1);
                    LOG.Info("JSON file empty");
                }
                LOG.Info("JSON file sucessfully read");
            }
            catch (FileNotFoundException)
            {
                LOG.Error("JSON file wasnt found, application exiting");
                MessageBox.Show("An unexpected error occured. Exiting...");
                Environment.Exit(1);
            }
        }

        /**
        * This method will check for the existance for the settings file that 
        * contains all information needed to proceed with this appication.
        * it checks if the file exits in the bin/debug
        */
        public FileSetup checkForJson()
        {
            FileSetup settings = new FileSetup();
            if (File.Exists("setup.json"))
            {
                LOG.Info("File found");
                string setupText = File.ReadAllText("setup.json");
                settings = JsonConvert.DeserializeObject<FileSetup>(setupText);
                this.path = settings.Path;
                updateLastLogin();
            }
            else
            {
                nav.GoToFileConfiguration();
                LOG.Warn("File Not found");
            }
            return settings;
        }


        /**
         * This method takes input an account name, then it searches
         * the list provided. This search ignores teh case, but, if multiple
         * accounts are found, it returns an object that clearly specifies
         * that there is multiple accounts found and adds all those account to a list to be used later. 
         * Other wise, it returns the one specific account
         */
        public Account searchAccount(String accountName)
        {
            readJson();
            LOG.Info("Account being searched for is || " + accountName + " ||");

            bool found = false;
            int count = 0;
            Account foundAccount = new Account();
            multiAccountFind = new List<Account>();


            foreach (Account account in this.allAccounts)
            {
                if (account.Site.ToLower().Contains(accountName.ToLower()))
                {
                    if (account.Site.Equals(accountName, StringComparison.OrdinalIgnoreCase))
                    {
                        foundAccount = account;
                        found = true;
                    }
                    multiAccountFind.Add(account);
                    foundAccount = account;
                }
            }

            if (multiAccountFind.Count > 1)
            {
                LOG.Info("Multiple account found");
                return new Account("MULTI-FIND", "", "", "", "");
            }
            else if (multiAccountFind.Count == 1)
            {
                LOG.Info("Account found was " + foundAccount.Site);
                return foundAccount;
            }
            if (!found)
            {
                if (count == 0)
                {
                    LOG.Info("No Account Found with the name || " + accountName + " ||");
                    MessageBox.Show("No account'" + accountName + "'was found");
                    return new Account();
                }
            }
            LOG.Error("Some how made it to the end of this part of the method");
            return new Account();
        }

        /**
         * This method takes the new password you want to modify and the account
         * and rewrites the orginal file where all the paswords are stored
         */

        public void updatePassword(String newPassword, Account account)
        {
            readJson();

            LOG.Info("Password being modified is for the account || " + account.Site + " ||");

            int position = this.allAccounts.IndexOf(account);
            allAccounts[position].Password = newPassword;

            File.WriteAllText(this.path, JsonConvert.SerializeObject(allAccounts, Formatting.Indented));
            LOG.Info("Password file modified");
            MessageBox.Show("Password Changed. Redirecting home...");
        }

        /**
         * This method is used to add an account. This takes all the information needed to create an account
         * then it adds the account to the list, and that list is converted to a json file and overwrites the 
         * orginal password file
         */
        public void addAccount(string accountName, string userName, string email, string password, string other)
        {
            readJson();

            LOG.Info("New Accouint being added by the name || " + accountName + " ||");
            Account newAccount = new Account(accountName, userName, email, password, other);

            this.allAccounts.Add(newAccount);

            File.WriteAllText(this.path, JsonConvert.SerializeObject(allAccounts, Formatting.Indented));
            LOG.Info("Password file modified");
            MessageBox.Show("Account added, Redirecting...");
        }
        /**
         * This method selects all the accounts with the same password
         * to visually show the user all the accounts with the same password
         */
        public List<Account> groupPassword(String password)
        {
            readJson();

            bool found = false;
            List<Account> matchedAccounts = new List<Account>();

            foreach (Account account in this.allAccounts)
            {
                if (account.Password.Equals(password))
                {
                    found = true;
                    matchedAccounts.Add(account);
                }
            }
            if (!found)
            {
                LOG.Info("No password match found");
                MessageBox.Show("No account uses this password");
            }
            return matchedAccounts;
        }
        /**
        * This method selects all the accounts with the same email
        * to visually show the user all the accounts with the same email
        */
        public List<Account> groupEmail(String email)
        {
            readJson();

            bool found = false;
            List<Account> matchedAccounts = new List<Account>();

            foreach (Account account in this.allAccounts)
            {
                if (account.Email.ToLower().Contains(email.ToLower()))
                {
                    found = true;
                    matchedAccounts.Add(account);
                }
            }
            if (!found)
            {
                LOG.Info("No email match found");
                MessageBox.Show("No account uses this email");
            }
            return matchedAccounts;
        }
        /**
         * This method searches the list with the multiple accounts that 
         * were found exclusively and returns the specific account searched 
         */

        public Account searchMultipleAccounts(String search)
        {
            readJson();

            LOG.Info("Account being searched is || " + search + "||");
            foreach (Account account in multiAccountFind)
            {
                if (account.Site.ToLower().Equals(search.ToLower()))
                {
                    LOG.Info("Account found");
                    return account;
                }
            }
            LOG.Info("Account not found");
            MessageBox.Show("No account'" + search + "'was found in the provided list");
            return new Account();

        }
        /**
         * This method checks if the account exists and ignores the case of the string
         */
        public bool accountExists(String accountName)
        {
            readJson();

            foreach (Account account in this.allAccounts)
            {
                if (account.Site.Equals(accountName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        /**
         * In the case that a specific search needs to be done, this method is used, 
         * and this is a very percise search so it isnt the first to be used in search
         * but is used to change a password or delete an account
         */
        public Account specificSearch(String accountName)
        {
            readJson();

            LOG.Info("Account specifically being searched is || " + accountName + "||");

            foreach (Account account in this.allAccounts)
            {
                if (account.Site.Equals(accountName, StringComparison.OrdinalIgnoreCase))
                {
                    LOG.Info("Specific account found");
                    return account;
                }
            }
            MessageBox.Show("No Account Found");
            LOG.Info("No Specific account found with the name " + accountName);

            return new Account("No Account Found", "Please Be More Specific", "", "", "View the list in the bottom left \n to view all accounts ");
        }

        /** 
         * This method deletes the account from the list and rewrites the json file
         */
        public void deleteAccount(Account deletedAcc)
        {
            readJson();

            LOG.Info("Account with the name || " + deletedAcc.Site + "|| will be deleted");
            LOG.Info(deletedAcc.Site + " " + deletedAcc.Username + " " + deletedAcc.Password + " " + deletedAcc.Other);

            this.allAccounts.Remove(deletedAcc);

            File.WriteAllText(this.path, JsonConvert.SerializeObject(allAccounts, Formatting.Indented));
            LOG.Info("Password file modified");
            MessageBox.Show("Account deleted... Redirecting");
        }

        /*
         * This method is in charge of keeping track when the last log in was
         * This will then be displayed to the user or lock the user out from
         * trying the password again
         */
        public void updateLastLogin()
        {
            FileSetup settings = new FileSetup();
            string setupText = File.ReadAllText("setup.json");
            settings = JsonConvert.DeserializeObject<FileSetup>(setupText);
            settings.LastLogin = DateTime.Now.ToString();

            File.WriteAllText("setup.json", JsonConvert.SerializeObject(settings, Formatting.Indented));

        }
        public string getLastLogIn()
        {
            FileSetup settings = new FileSetup();
            string setupText = File.ReadAllText("setup.json");
            settings = JsonConvert.DeserializeObject<FileSetup>(setupText);

            return settings.LastLogin;
        }
    }
}

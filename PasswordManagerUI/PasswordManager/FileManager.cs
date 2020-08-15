    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Newtonsoft.Json;

    namespace PasswordManager
    {
        class FileManager
        {
            public List<Account> allAccounts { get; set; } 
            public List<Account> multiAccountFind { get; set; }

            public FileManager()
            {
                readJson();
            }

            public void readJson()
            {
                string json = File.ReadAllText(@"C:/Users/Alex/Documents/randocs/abracadabra.json");
                this.allAccounts = JsonConvert.DeserializeObject<List<Account>>(json);
            }

            public Account searchAccount(String accountName)
            {
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
                    }
                }

                if (multiAccountFind.Count > 1)
                {
                    return new Account("MULTI-FIND", "","","","");
                }
                if (!found)
                {
                    if (count == 0)
                    {
                        MessageBox.Show("No account'" + accountName + "'was found");
                        return new Account();
                    }
                }
                return foundAccount;
            }

            public void updatePassword(String newPassword,Account account)
            {

            int position = this.allAccounts.IndexOf(account);
            allAccounts[position].Password = newPassword;

            File.WriteAllText(@"C:/Users/Alex/Documents/randocs/abracadabra.json", JsonConvert.SerializeObject(allAccounts, Formatting.Indented));
            MessageBox.Show("Password Changed. Redirecting home...");
            }

        public void addAccount(string accountName, string userName, string email, string password, string other)
            {
                Account newAccount = new Account(accountName, userName, email, password, other);

                this.allAccounts.Add(newAccount);

                File.WriteAllText(@"C:/Users/Alex/Documents/randocs/abracadabra.json", JsonConvert.SerializeObject(allAccounts, Formatting.Indented));
                MessageBox.Show("Account added, Redirecting...");
            }

            public List<Account> groupPassword(String password)
            {
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
                if (!found) MessageBox.Show("No account uses this password");

                return matchedAccounts;
            }
            public List<Account> groupEmail(String email)
            {
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
                if (!found) MessageBox.Show("No account uses this email");
            
                return matchedAccounts;
            }

            public Account searchMultipleAccounts(String search)
            {

                foreach (Account account in multiAccountFind)
                {
                    if (account.Site.ToLower().Equals(search.ToLower()))
                    {
                        return account;
                    }
                }
                MessageBox.Show("No account'" + search + "'was found in the provided list");
                return new Account();
                
            }

            public bool accountExists(String accountName) 
            {
                foreach (Account account in this.allAccounts)
                {
                    if (account.Site.Equals(accountName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }  
                }
                return false;
            }
        }
    }

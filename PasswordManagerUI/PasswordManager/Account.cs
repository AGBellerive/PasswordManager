using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class Account
    {
        public string Site { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Other { get; set; }

        public Account(string site, string username, string email, string password, string other)
        {
            this.Site = site;
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.Other = other;
        }
        public Account()
        {
            this.Site = "";
            this.Username = "";
            this.Email = "";
            this.Password = "";
            this.Other = "";
        }
    }
}

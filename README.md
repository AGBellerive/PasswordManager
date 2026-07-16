# PasswordManager

Welcome to my Password Manager.

The purpose of this application was to solve a problem of storing my passwords 
and to have a playground area to exersize new concepts learned from my classes.

Yes, there are many other well established services I could use that does this exact
thing but the diffrence is, I can actually use my knowledge to make it myself.


## Walk Through

On first start up of the application, you will be sent to a configuration page.
This page will ask you to create a master password, create an optional hint for that 
password, you can choose the location you want your to store your password file on your machine `../../../abracadabra.json`, 
and lastly your last log in. This process will create a configuration file, `../bin/debug/setup.json` that is crucial

`todo: find a way to link to a remote db`

Once that is set up, you will be redirected to enter the master password to enter the vault. 
When you enter the vault, you can now add accounts to be saved in your password file ([Format](https://github.com/AGBellerive/PasswordManager?tab=readme-ov-file#format) found below) 
and search for accounts. This will display information pertaining to that specific account. 

You will have a few options to navigate the data from the password file. 
* All Accounts - This will display all the accounts found in the password file provided 
* Add Account - This will redirect you to a page to create a new entry in the password file 
* Change Password - This will direct you to a page that will precisely ask you to enter the account that you want to change the password for 
* Grouped Accounts - This will direct you to a page where you can enter an email or password to see which accounts share that attribute in common
* Delete Account - This will direct you to a page where you need to precisely enter the account you want to delete

**Remember to perodically back up your password file elsewhere.** There are logs that will keep track of things done in the application so you can recover it, but it is time consuming
# Format 
<pre>
 
[   
  {
    "Site": "Example Site",
    "Username": "Example Username",
    "Email": "Email@Email.com",
    "Password": "ABC123",
    "Other": "Extra information"
  },
  {
    "Site": "Github",
    "Username": "octocat",
    "Email": "Email@Email.com",
    "Password": "pa55w0rd",
    "Other": "anthropomorphized “octocat” with five octopus-like arms"
  },
  ...
]
</pre> 
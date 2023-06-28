# PasswordManager

Welcome to my password manager.

On the first open of the application, 
the user will be sent to a page to 
create a password, create a hint 
for the password and choose the 
file location. This file will
be saved in bin/debug/ and is 
essentially the set up file.

The application reads the file to
know the password and where to save 
the passwords to. The password file 
is abracadabra.json

This password manager reads a json file
and parses it in the code and it writes
to this same file. This file should be
backed up every now and then just to be 
safe. 

# Installer
To obtain the installer for this application
it requires a few steps.

1) Go into the PasswordManagerInstaller folder
2) Right click on the certificate and click "Instal"
3) In the instalation wizard, choose either "Local" or "Current user", press Next
(Current user is just for the accout adn local is for the whole computer)
4) Choose "Place all certificates in the following store" and choose "Trusted Root Certification Authorities", Press ok
5) Now you can go back to the APPXBUNDLE and install the applications 
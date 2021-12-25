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
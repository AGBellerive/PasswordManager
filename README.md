# PasswordManager

Welcome to my janky password manager.

This password manager reads a json file
and parses it in the code and it writes
to this same file. This file should be
backed up every now and then just to be 
safe. 
 
To properly point to the password file 
go into the file ~Filemanager.cs~
and modify the private property "path".

To have the password set, you need to
find the hashvalue of your password 
and insert it into mainwindow.xaml.cs
in the private property masterPassword
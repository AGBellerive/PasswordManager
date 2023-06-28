using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace PasswordManager
{
    public partial class FileConfiguration : Page
    {
        private FileSetup settings;

        /**
         * Depending on if the user has already set up the application, this is the landing 
         * page. Here the user configers their password, hint and file location. This is then
         * saved in a file called setup.json. The user has to set up a password before they
         * can set a hint or a file location. I must assume the user has the same amount of 
         * braincells as a chicken running with its head cut off. 
         */
        public FileConfiguration()
        {
            InitializeComponent();
            settings = new FileSetup();
            ConfirmBtn.Visibility = Visibility.Hidden;
            PasswordHintBox.Visibility = Visibility.Hidden;
            Browsebtn.Visibility = Visibility.Hidden;
        }

        private void Browsrbtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select where you would like to save the password file";
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;

            if(folderBrowserDialog.SelectedPath.Length != 0)
            {
                settings.Path = path + "\\abracadabra.json";
                PathLbl.Content = settings.Path;
                ConfirmBtn.Visibility = Visibility.Visible;
            }
        }

        private void Confirmbtn_Click(object sender, RoutedEventArgs e)
        {
            settings.HashedPassword = passwordBox.Text.GetHashCode();
            settings.Hint = PasswordHintBox.Text;
            settings.LastLogin = DateTime.Now.ToString();
            File.WriteAllText("setup.json",JsonConvert.SerializeObject(settings, Formatting.Indented));
            
            var stream = File.Create(settings.Path);
            stream.Close();

            Navigation nav = new Navigation();
            nav.GoToDisplayPassword();
        }

        private void passwordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return && passwordBox.Text.Length !=0)
            {
                PasswordHintBox.Visibility = Visibility.Visible;
                Browsebtn.Visibility = Visibility.Visible;
            }

        }
    }
}

using System;
using System.Windows;
using System.Windows.Input;
using log4net;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int masterPassword;
        private int count;
        private int hintCount;
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FileSetup settings; 
        

        public MainWindow()
        {
            InitializeComponent();
            password.Focus();
            FileManager manager = new FileManager();
            settings = manager.checkForJson();
            masterPassword = settings.HashedPassword;

            LOG.Info("Mainwindow initilized, set up file initilized "+settings.ToString());
        }


        /**
         * This method compares the master password, stored in hash,
         * with the hash result of what the user inputs. If the password 
         * is wrong, you have 3 total shots untill it closes it self
         */
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (password.Password.GetHashCode() == masterPassword)
                {
                    LOG.Info("Password correct");
                    Navigation nav = new Navigation();
                    nav.GoToDisplayPassword();
                }

                else if (count == 3)
                {
                    MessageBox.Show("Incorrect Password Exiting");
                    LOG.Info("Password incorrect");
                    Environment.Exit(1);
                }
                else
                {
                    count++;
                    countLbl.Content = count;
                    password.Password = "";
                    LOG.Info("Password incorrect");
                }
            }
        }

        private void Hint_Button_Click(object sender, RoutedEventArgs e)
        {
            hintCount++;
            if (hintCount%2 == 0)
            {
                hintLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                hintLbl.Visibility = Visibility.Visible;
                hintLbl.Content = settings.Hint;
            }
        }
    }
}

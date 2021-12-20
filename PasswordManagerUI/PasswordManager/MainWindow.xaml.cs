using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int masterPassword = 1890847487;
        private int count;
        private int hintCount;
        private static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Navigation nav;
        public MainWindow()
        {
            InitializeComponent();
            password.Focus();
            if (nav == null) nav = new Navigation();
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
                //MessageBox.Show(password.Password.GetHashCode().ToString());
                if (password.Password.GetHashCode() == masterPassword)
                {
                    LOG.Info("Password correct");
                    // Application.Current.MainWindow.Content = new DisplayPassword();
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
                hintLbl.Content = "Most Complex password I have";
            }
        }
    }
}

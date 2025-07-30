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

namespace PasswordManager.Components
{
    /// <summary>
    /// Interaction logic for AccountList.xaml
    /// </summary>
    public partial class AccountList : UserControl
    {

        public static readonly RoutedEvent AccountClickEvent = EventManager.RegisterRoutedEvent(nameof(AccountClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AccountList));

        public event RoutedEventHandler AccountClick
        {
            add { AddHandler(AccountClickEvent, value); }
            remove { RemoveHandler(AccountClickEvent, value); }
        }

        public AccountList()
        {
            InitializeComponent();
            AccountListBox.ItemsSource = FileManager.allAccounts;
        }

        private void OnAccountClick(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AccountClickEvent, sender));
        }
    }
}

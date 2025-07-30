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
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));

        public static readonly RoutedEvent AccountSearchEvent = EventManager.RegisterRoutedEvent(nameof(AccountSearch), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBox));

        public event RoutedEventHandler AccountSearch
        {
            add { AddHandler(AccountSearchEvent, value); }
            remove { RemoveHandler(AccountSearchEvent, value); }
        }


        public SearchBox()
        {
            InitializeComponent();
            SearchedAccountName.Focus();
        }

        private void OnAccountSearch(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                RaiseEvent(new RoutedEventArgs(AccountSearchEvent, sender));
            }
        }

        public void FocusInput()
        {
            SearchedAccountName?.Focus();
        }
    }
}

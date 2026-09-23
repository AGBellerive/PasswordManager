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
    /// https://www.youtube.com/watch?v=d1PqVmmFMSQ&list=PLM6BGsQk9GW75kxMzFBanV8NF_Na8kd2T&ab_channel=SingletonSean
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label
        // This allows me in XAML to do <Searchbox Lable = "..."/> to fill in the variable that is {Binding Label, ElementName=root}
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty)); 
        //Name of the property, the type of the property, what the property is attached to, default value

        public static readonly RoutedEvent AccountSearchEvent = EventManager.RegisterRoutedEvent(nameof(AccountSearch), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBox));
        // Custom routed event nammed AccountSearchEvent which will allow us to attach a method to the XAML tag
        // Name of the function, bubbles" up through parent elements, Tyope of delegate that will handle the event, owner/ who it is attached to

        //This allows a function to be assigned to the searchbox
        public event RoutedEventHandler AccountSearch
        {
            add { AddHandler(AccountSearchEvent, value); }
            remove { RemoveHandler(AccountSearchEvent, value); }
        }


        public SearchBox()
        {
            InitializeComponent();
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

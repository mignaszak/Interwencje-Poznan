using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Interwencje___Poznań.Pages
{
    public partial class SummaryPage : PhoneApplicationPage
    {
        public SummaryPage()
        {
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/Pages/SummaryPage.xaml", UriKind.Relative));
        }
    }
}
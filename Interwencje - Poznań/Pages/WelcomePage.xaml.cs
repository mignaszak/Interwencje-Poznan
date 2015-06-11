using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Interwencje___Poznań.Resources;
using Interwencje___Poznań.Helpers;

namespace Interwencje___Poznań{
    public partial class WelcomePage : PhoneApplicationPage
    {
        // Constructor
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/PhotoPage.xaml", UriKind.Relative));
        }

        private void buttonReadFromMemory_Click(object sender, RoutedEventArgs e)
        {
            Intervention.GetInterventionFromMemory();
            if (Intervention.CurrentInterventionIsEmpty())
                MessageBox.Show("Brak zdarzenia w pamięci");
            else
                NavigationService.Navigate(new Uri("/Pages/SummaryPage.xaml", UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
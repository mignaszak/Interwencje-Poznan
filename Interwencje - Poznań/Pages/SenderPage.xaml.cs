using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Interwencje___Poznań.Helpers;

namespace Interwencje___Poznań.Pages
{
    public partial class SenderPage : PhoneApplicationPage
    {
        public SenderPage()
        {
            GetAndSetUser();
            InitializeComponent();

        }

        private void GetAndSetUser()
        {
            if (Intervention.GetCurrentIntervention().Name != "")
            {
                TxtFirstName.Text = Intervention.GetCurrentIntervention().Name;
            }
            else
            {
                TxtFirstName.Text = DataMemory.CurrentUser.Name;
            }
            if (Intervention.GetCurrentIntervention().Secondname != "")
            {
                TxtSecondname.Text = Intervention.GetCurrentIntervention().Secondname;
            }
            else
            {
                TxtSecondname.Text = DataMemory.CurrentUser.Secondname;
            }
            if (Intervention.GetCurrentIntervention().Email != "")
            {
                TxtMail.Text = Intervention.GetCurrentIntervention().Email;
            }
            else
            {
                TxtMail.Text = DataMemory.CurrentUser.Email;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CbSave.IsChecked)
            {
                DataMemory.CurrentUser = new User(TxtFirstName.Text, TxtSecondname.Text, TxtMail.Text);
                DataMemory.SaveUser(null);
            }
            Intervention.GetCurrentIntervention().Name = TxtFirstName.Text;
            Intervention.GetCurrentIntervention().Secondname = TxtSecondname.Text;
            Intervention.GetCurrentIntervention().Email = TxtMail.Text;
            NavigationService.GoBack();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CbSave.IsChecked)
            {
                DataMemory.CurrentUser = new User(TxtFirstName.Text, TxtSecondname.Text, TxtMail.Text);
                DataMemory.SaveUser(delegate
                {
                    MessageBox.Show(@"Za mało pamięci na telefonie, aby zapisać dane.
                                      Zwolnij trochę miejsca i spróbuj ponownie.");
                });
            }
            Intervention.GetCurrentIntervention().Name = TxtFirstName.Text;
            Intervention.GetCurrentIntervention().Secondname = TxtSecondname.Text;
            Intervention.GetCurrentIntervention().Email = TxtMail.Text;
            NavigationService.Navigate(new Uri("/Pages/SummaryPage.xaml", UriKind.Relative));
        }
    }
}
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
        private static bool CbChecked;
        public SenderPage()
        {
            InitializeComponent();
            GetAndSetUser();
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

        private void SaveUser()
        {

            if ((bool)CbSave.IsChecked)
            {
                DataMemory.CurrentUser.Name = TxtFirstName.Text;
                DataMemory.CurrentUser.Secondname = TxtSecondname.Text;
                DataMemory.CurrentUser.Email = TxtMail.Text;
                DataMemory.SaveUser(null);
            }
        }

        private void SaveInterventionData()
        {
            Intervention.GetCurrentIntervention().Name = TxtFirstName.Text;
            Intervention.GetCurrentIntervention().Secondname = TxtSecondname.Text;
            Intervention.GetCurrentIntervention().Email = TxtMail.Text;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            SaveUser();
            SaveInterventionData();
            NavigationService.GoBack();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            SaveUser();
            SaveInterventionData();
            NavigationService.Navigate(new Uri("/Pages/SummaryPage.xaml", UriKind.Relative));
        }

        private void CbSave_Checked(object sender, RoutedEventArgs e)
        {
        }
    }
}
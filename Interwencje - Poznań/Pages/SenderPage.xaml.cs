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
            User lastUser = User.GetUserFromMemory();
            if (Intervention.GetCurrentIntervention().Name != "")
            {
                TxtFirstName.Text = Intervention.GetCurrentIntervention().Name;
            }
            else
            {
                TxtFirstName.Text = lastUser.Name;
            }
            if (Intervention.GetCurrentIntervention().Secondname != "")
            {
                TxtSecondname.Text = Intervention.GetCurrentIntervention().Secondname;
            }
            else
            {
                TxtSecondname.Text = lastUser.Secondname;
            }
            if (Intervention.GetCurrentIntervention().Email != "")
            {
                TxtMail.Text = Intervention.GetCurrentIntervention().Email;
            }
            else
            {
                TxtMail.Text = lastUser.Email;
            }
        }

        private void SaveUser()
        {

            try
            {
                if ((bool)CbSave.IsChecked)
                {
                    User newUser = new User();
                    newUser.Email = TxtFirstName.Text;
                    newUser.Secondname = TxtSecondname.Text;
                    newUser.Email = TxtMail.Text;
                    AppSettings.CurrentAppSettings.SetSetting(AppSettings.USER_KEY, newUser);
                }
            }
            catch (SaveToMemoryException e)
            {
                MessageBox.Show(e.Message);
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
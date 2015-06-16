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
            if(Intervention.GetCurrentIntervention().Name != ""
                || Intervention.GetCurrentIntervention().Secondname != "" ||
                Intervention.GetCurrentIntervention().Email != "")
            {
                TxtFirstName.Text = Intervention.GetCurrentIntervention().Name;
                TxtSecondname.Text = Intervention.GetCurrentIntervention().Secondname;
                TxtMail.Text = Intervention.GetCurrentIntervention().Email;
                
            }
            else  if(lastUser != null)
            {
                TxtFirstName.Text = lastUser.Name;
                TxtSecondname.Text = lastUser.Secondname;
                TxtMail.Text = lastUser.Email;
            }
        }

        private void SaveUser()
        {
            try
            {
                    User newUser = new User();
                    newUser.Name = TxtFirstName.Text;
                    newUser.Secondname = TxtSecondname.Text;
                    newUser.Email = TxtMail.Text;
                    AppSettings.CurrentAppSettings.SetSetting(AppSettings.USER_KEY, newUser);
             
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
        {        }
    }
}
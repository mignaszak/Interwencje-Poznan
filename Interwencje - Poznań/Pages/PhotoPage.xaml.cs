using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Interwencje___Poznań.Helpers;
using System.Windows.Media.Imaging;

namespace Interwencje___Poznań.Pages
{
    public partial class StepPhotoPage : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;
        BitmapImage bmp;

        public StepPhotoPage()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            GetInterventionData();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            SaveInterventionData();
            NavigationService.GoBack();
        }


        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            SaveInterventionData();
            NavigationService.Navigate(new Uri("/Pages/AddressPage.xaml", UriKind.Relative));
        }

        private void GetInterventionData()
        {
            if (Intervention.GetCurrentIntervention().Picture != null)
            {
                bmp = Intervention.GetCurrentIntervention().Picture;
                ImageField.Source = bmp;
            }
        }

        private void SaveInterventionData()
        {
            if (bmp != null)
                Intervention.GetCurrentIntervention().Picture = bmp;
        }

        private void ButtonChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //Code to display the photo on the page in an image control named myImage.
                bmp = new System.Windows.Media.Imaging.BitmapImage();                
                bmp.SetSource(e.ChosenPhoto);
                ImageField.Source = bmp;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ImageField.Source = null;
            bmp = null;
        }
    }
}
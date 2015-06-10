using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Windows.Devices.Geolocation; 
using System.Windows.Media;
using System.Windows.Shapes;
using Interwencje___Poznań.Helpers;
using Interwencje___Poznań.WebService;

namespace Interwencje___Poznań.Pages
{
    public partial class AddressPage : PhoneApplicationPage
    {
        public static double UserLongitude;
        public static double UserLatitude;
        public static string Street;
        public static string House;

        public AddressPage()
        {
            InitializeComponent();
            GetInterventionData();
            if(Street != null)
                TxtStreet.Text = Street;
            if(House != null)
                TxtHouse.Text = House;
        }

        private void GetInterventionData()
        {
            UserLatitude = double.Parse(Intervention.GetCurrentIntervention().Latitude.Replace(".", ","));
            UserLongitude = double.Parse(Intervention.GetCurrentIntervention().Longitude.Replace(".", ","));
        }

        private void SaveInterventionData()
        {
            Intervention.GetCurrentIntervention().Latitude = UserLatitude.ToString().Replace(".", ",");
            Intervention.GetCurrentIntervention().Longitude = UserLongitude.ToString().Replace(".", ",");
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            SaveInterventionData();
            Street = TxtStreet.Text;
            House = TxtHouse.Text;
            NavigationService.GoBack();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {         
            SaveInterventionData();
            Street = TxtStreet.Text;
            House = TxtHouse.Text;
            NavigationService.Navigate(new Uri("/Pages/DetailsPage.xaml", UriKind.Relative));
        }

        private void ButtonMap_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MapPage.xaml", UriKind.Relative));
        }

        private void ButtonMyPosition_Click(object sender, RoutedEventArgs e)
        {
            GetUserLocation();
        }

        private async void GetUserLocation()
        {
            try
            {
                Geolocator myGeolocator = new Geolocator();
                if (myGeolocator.LocationStatus == PositionStatus.Disabled)
                {
                    MessageBox.Show("Lokalizacja jest wyłączona");
                }
                else
                {
                    Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                    Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
                    GeoCoordinate myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);
                    UserLongitude = myGeoCoordinate.Longitude;
                    UserLatitude = myGeoCoordinate.Latitude;
                    GetFillStreetInformations();
                }
            }
            catch (Exception)
            {
                
               // throw;
            }
        }

        private void ButtonCheckPosition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ulica = TxtStreet.Text.Split(' ')[TxtStreet.Text.Split(' ').Length - 1].ToLower();
                string uri = string.Format("http://www.poznan.pl/featureserver/featureserver.cgi/adresy/all.atom?queryable=wg_nazwiska,nr&wg_nazwiska={0}&nr={1}", ulica, TxtHouse.Text);
                WSMethods.GetMethodNoAuth(uri, GetCoordinatesFromAPI);
            }
            catch (NoInternetConnectionException nicexc)
            {
                MessageBox.Show(nicexc.Message);
            }
            catch (Exception)
            {

                //  throw;
            }
        }

        private void GetCoordinatesFromAPI(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || e.Error != null)
                {
                    throw e.Error;
                }
                else
                {
                    GetCoordinatesFromGeoRSS(e.Result.ToString());
                    MessageBox.Show("Ok");
                }
            }
            catch (Exception ex)
            { }
        }

        private void GetCoordinatesFromGeoRSS(string geoRss)
        {
            try
            {
                string lat = "";
                int start = geoRss.IndexOf("lat");
                start += 14;
                lat = geoRss.Substring(start);
                int end = lat.IndexOf("&lt;br");
                lat = lat.Substring(0, end);
                lat = lat.Trim().Replace(".",",");
                UserLatitude = double.Parse(lat); 

                string lon = "";
                start = geoRss.IndexOf("lon");
                start += 14;
                lat = geoRss.Substring(start);
                end = lat.IndexOf("&lt;br");
                lat = lat.Substring(0, end);
                lat = lat.Trim().Replace(".", ",");
                UserLongitude = double.Parse(lat);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GetStreetInformationsFromAPI(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || e.Error != null)
                {                 
                    throw e.Error;
                }
                else
                {
                    AddressDetails details = Serialize.DesrielizeAddressDetails(e.Result.ToString());
                    TxtStreet.Text = details.features[0].properties.wg_imienia_wydruk;
                    TxtHouse.Text = details.features[0].properties.nr;
                }
            }
            catch (Exception ex)
            { }
        }

        private void GetFillStreetInformations()
        {

            try
            {
                if (UserLatitude > 0.0 && UserLongitude > 0.0)
                {
                    string uri = string.Format("http://www.poznan.pl/mim/plan/plan.html?co=json&service=adresy&y={0}&x={1}&n=2&srs=EPSG:4326", UserLatitude.ToString().Replace(",", "."), UserLongitude.ToString().Replace(",", "."));
                    WSMethods.GetMethodNoAuth(uri, GetStreetInformationsFromAPI);
                }
            }
            catch (NoInternetConnectionException) {
                TextBlockError.Text = "Brak połączenie z internetem, nie można wypełnić informacji o podanym miejscu. Zapytanie może być dalej wypełniane";
            }
            catch (Exception)
            {

//                throw;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetFillStreetInformations();
        }


    }
}
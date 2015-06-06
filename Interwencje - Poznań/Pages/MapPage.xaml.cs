using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using Interwencje___Poznań.WebService;

namespace Interwencje___Poznań.Pages
{
    public partial class MapPage : PhoneApplicationPage
    {

       static private AddressPage addresPage;

        public MapPage()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }

        private void MarkPositionEvent(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point p = e.GetPosition(MapControlLocation);
            GeoCoordinate s = MapControlLocation.ConvertViewportPointToGeoCoordinate(p);
            AddressPage.UserLatitude = s.Latitude;
            AddressPage.UserLongitude = s.Longitude;

            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0, 0);
            myLocationOverlay.GeoCoordinate = s;

            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            MapControlLocation.Layers.Clear();
            MapControlLocation.Layers.Add(myLocationLayer);
        }
    }
}
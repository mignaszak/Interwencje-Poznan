using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Phone.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using Interwencje___Poznań.Helpers;

namespace Interwencje___Poznań.WebService
{
    class WSMethods
    {
        public static void GetMethodNoAuth(string uri, DownloadStringCompletedEventHandler handler)
        {
            try
            {
                if (!CheckNetworkConnection())
                    throw new NoInternetConnectionException();
                WebClient downloader = new WebClient();
                downloader.UseDefaultCredentials = true;
                downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(handler);
                downloader.DownloadStringAsync(new Uri(uri));
            }
            catch (Exception)
            {
                
               // throw;
            }
        }

        public static bool CheckNetworkConnection()
        {
            bool IsConnected = false;
            IsConnected = NetworkInterface.GetIsNetworkAvailable();
            if (IsConnected)
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                NetworkConnectivityLevel connection = InternetConnectionProfile.GetNetworkConnectivityLevel();
                if (connection == NetworkConnectivityLevel.None || connection == NetworkConnectivityLevel.LocalAccess)
                {
                    IsConnected = false;
                }
            }
            return IsConnected;
        }

    }
}

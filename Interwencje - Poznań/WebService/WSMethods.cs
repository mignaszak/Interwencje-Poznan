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
using System.IO;
using System.Windows.Media.Imaging;

namespace Interwencje___Poznań.WebService
{
    class WSMethods
    {
        private static string Response;
        public static event EventHandler ResponseChanged;


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
            catch (NoInternetConnectionException)
            {

                 throw;
            }
            catch (Exception)
            {

                // throw;
            }
        }

        #region POST

        public static string SendTestRequest()
        {
            Response = "";
            string JsonStringParams = Serialize.SerializeIntervention(Intervention.GetCurrentIntervention());// "{\"lon\":16.234, \"lat\":55.1,\"key\":\"c3c2ff2e008b7bfbef77aba8916df2fe\", \"category\":\"1118_9607\",\"subcategory\":\"17399\", \"name\":\"PCSS test\", \"surname\":\"PCSS test\", \"email\":\"asd@dadsa.pl\", \"subject\":\"PCSS test\", \"text\":\"PCSS test\", \"address\":\"ulica 432\"} ";
            var Params = new Dictionary<string, string> { { "json", JsonStringParams } };
            string link = Resources.AppResources.RequestLink_Test;
            string login = Resources.AppResources.Login;
            string pass = Resources.AppResources.Pass;
            NetworkCredential credentials = new NetworkCredential(login, pass);
            string fileName = "test.jpg";
            string fileContentType = "image/jpeg";
            Uri uri = new Uri("https://www.um.poznan.pl/mimtest/public/api/submit.html?service=fixmycity");
            byte[] fileData = ConvertToBytes(Intervention.GetCurrentIntervention().Picture);
            UploadFilesToServer(uri, Params, fileName, fileContentType, fileData, credentials);
            return Response;
        }

        public static string SendRequest()
        {
            Response = "";
            string JsonStringParams = Serialize.SerializeIntervention(Intervention.GetCurrentIntervention());// "{\"lon\":16.234, \"lat\":55.1,\"key\":\"c3c2ff2e008b7bfbef77aba8916df2fe\", \"category\":\"1118_9607\",\"subcategory\":\"17399\", \"name\":\"PCSS test\", \"surname\":\"PCSS test\", \"email\":\"asd@dadsa.pl\", \"subject\":\"PCSS test\", \"text\":\"PCSS test\", \"address\":\"ulica 432\"} ";
            var Params = new Dictionary<string, string> { { "json", JsonStringParams } };
            string link = Resources.AppResources.RequestLink;
            NetworkCredential credentials = null;
            string fileName = "test.jpg";
            string fileContentType = "image/jpeg";
            Uri uri = new Uri("https://www.um.poznan.pl/mimtest/public/api/submit.html?service=fixmycity");
            byte[] fileData = ConvertToBytes(Intervention.GetCurrentIntervention().Picture);
            UploadFilesToServer(uri, Params, fileName, fileContentType, fileData, credentials);
            return Response;
        }

        ///// <summary>
        ///// Creates HTTP POST request & uploads database to server. Author : Farhan Ghumra
        ///// </summary>
        private static void UploadFilesToServer(Uri uri, Dictionary<string, string> data, string fileName, string fileContentType, byte[] fileData, NetworkCredential credential)
        {
            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            if (credential == null)
                httpWebRequest.UseDefaultCredentials = true;
            else
                httpWebRequest.Credentials = credential;
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.Method = "POST";
            httpWebRequest.BeginGetRequestStream((result) =>
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                    using (Stream requestStream = request.EndGetRequestStream(result))
                    {
                        WriteMultipartForm(requestStream, boundary, data, fileName, fileContentType, fileData);
                    }
                    request.BeginGetResponse(a =>
                    {
                        try
                        {
                            var response = request.EndGetResponse(a);
                            var responseStream = response.GetResponseStream();
                            using (var sr = new StreamReader(responseStream))
                            {
                                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                                {
                                    string responseString = streamReader.ReadToEnd();
                                    Response = responseString;
                                    NotifyResponseChanged();
                                }
                            }
                        }
                        catch (WebException we)
                        {
                            var response = ((HttpWebResponse)we.Response);
                            try
                            {
                                using (var stream = response.GetResponseStream())
                                {
                                    using (var reader = new StreamReader(stream))
                                    {
                                        var text = reader.ReadToEnd();
                                        Response = text;
                                        NotifyResponseChanged();
                                    }
                                }
                            }
                            catch (WebException ex)
                            {
                                // Oh, well, we tried
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }, null);
                }
                catch (Exception)
                {

                }
            }, httpWebRequest);
        }

        ///// <summary>
        ///// Writes multi part HTTP POST request. Author : Farhan Ghumra
        ///// </summary>
        private static void WriteMultipartForm(Stream s, string boundary, Dictionary<string, string> data, string fileName, string fileContentType, byte[] fileData)
        {
            /// The first boundary
            byte[] boundarybytes = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
            /// the last boundary.
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "–-\r\n");
            /// the form data, properly formatted
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            /// the form-data file upload, properly formatted
            string fileheaderTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\";\r\nContent-Type: {2}\r\n\r\n";

            /// Added to track if we need a CRLF or not.
            bool bNeedsCRLF = false;

            if (data != null)
            {
                foreach (string key in data.Keys)
                {
                    /// if we need to drop a CRLF, do that.
                    if (bNeedsCRLF)
                        WriteToStream(s, "\r\n");

                    /// Write the boundary.
                    WriteToStream(s, boundarybytes);

                    /// Write the key.
                    WriteToStream(s, string.Format(formdataTemplate, key, data[key]));
                    bNeedsCRLF = true;
                }
            }

            /// If we don't have keys, we don't need a crlf.
            if (bNeedsCRLF)
                WriteToStream(s, "\r\n");

            WriteToStream(s, boundarybytes);
            WriteToStream(s, string.Format(fileheaderTemplate, "uz_file", fileName, fileContentType));
            /// Write the file data to the stream.
            WriteToStream(s, fileData);
            WriteToStream(s, trailer);
        }

        ///// <summary>
        ///// Writes string to stream. Author : Farhan Ghumra
        ///// </summary>
        private static void WriteToStream(Stream s, string txt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(txt);
            s.Write(bytes, 0, bytes.Length);
        }

        ///// <summary>
        ///// Writes byte array to stream. Author : Farhan Ghumra
        ///// </summary>
        private static void WriteToStream(Stream s, byte[] bytes)
        {
            s.Write(bytes, 0, bytes.Length);
        }


        public static byte[] ConvertToBytes(BitmapImage bitmapImage)
        {
            byte[] data = new byte[0];
            // Get an Image Stream
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    WriteableBitmap btmMap = new WriteableBitmap(bitmapImage);

                    // write an image into the stream
                    Extensions.SaveJpeg(btmMap, ms,
                        bitmapImage.PixelWidth, bitmapImage.PixelHeight, 0, 100);

                    // reset the stream pointer to the beginning
                    ms.Seek(0, 0);
                    //read the stream into a byte array
                    data = new byte[ms.Length];
                    ms.Read(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                string asd = e.Message;
            }
            //data now holds the bytes of the image
            return data;
        }
        
        private static void NotifyResponseChanged()
        {
            var handler = ResponseChanged;
            if (handler != null) handler(null, null);
        }
#endregion

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

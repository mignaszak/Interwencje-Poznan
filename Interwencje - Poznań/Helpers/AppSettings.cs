using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Interwencje___Poznań.Helpers;
using Interwencje___Poznań.WebService;

namespace Interwencje___Poznań.Helpers
{
    class AppSettings
    {
        private static IsolatedStorageSettings settings1 = IsolatedStorageSettings.ApplicationSettings;
        private Dictionary<string, object> settings;
        private BitmapImage bmp;
        #region keys
        public const string CATEGORIES_KEY = "Last.Categories";
        public const string INTERVENTION_KEY = "Last.Intervention";
        public const string INTERVENTION_PHOTO_FILE_NAME = @"InterventionPhoto/InterventionPhoto.jpg";
        public const string USER_KEY = "Current.User";
        private const string FILE_DIR = "MySettings/Settings.txt";
        #endregion

        private static AppSettings _appSettings;

        public static AppSettings CurrentAppSettings
        {
            get
            {
                if (_appSettings == null)
                    _appSettings = new AppSettings();

                return _appSettings;
            }
        }

        public AppSettings()
        {
            settings = new Dictionary<string, object>();
            settings.Add(CATEGORIES_KEY, "");
            settings.Add(INTERVENTION_KEY, "");
            settings.Add(USER_KEY, "");
            CreateSettingsFile();
            ReadSettingsFile();
            ReadPhotoFile();
        }

        public void Save()
        {
            SaveSettings();
            SavePhoto(INTERVENTION_PHOTO_FILE_NAME, bmp);
        }

        private void SaveSettings()
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (myIsolatedStorage.FileExists(FILE_DIR))
            {
                using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(FILE_DIR, FileMode.Open, FileAccess.Write, myIsolatedStorage)))
                {
                    string setts = "";
                    foreach (string key in settings.Keys)
                        setts += string.Format("{0}={1}\r\n", key, settings[key]);
                    writeFile.WriteLine(setts);
                    writeFile.Close();
                }
            }
        }

        private void CreateSettingsFile()
        {
            try
            {
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                if (!myIsolatedStorage.FileExists(FILE_DIR))
                {
                    if (!myIsolatedStorage.DirectoryExists(FILE_DIR.Split('/')[0]))
                    {
                        myIsolatedStorage.CreateDirectory(FILE_DIR.Split('/')[0]);
                    }
                    StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(FILE_DIR, FileMode.Create, FileAccess.Write, FileShare.Write, myIsolatedStorage));
                    string setts = "";
                    foreach (string key in settings.Keys)
                        setts += string.Format("{0}={1}\r\n", key, settings[key]);
                    writeFile.WriteLine(setts);
                    writeFile.Close();
                }
            }
            catch (Exception)
            {
                
            //    throw;
            }
        }


        
        private void ReadSettingsFile()
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (myIsolatedStorage.FileExists(FILE_DIR))
            {
                IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(FILE_DIR, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string[] setts = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    foreach (string s in setts)
                    {
                        try
                        {
                            if(s!="")
                                settings[s.Split('=')[0]] = s.Split('=')[1];
                        }
                        catch (Exception) { }
                    }
                    //if you use reader.Readline() it will read the first line of the file
                }
            }
        }
        private void ReadPhotoFile()
        {
           
            using (IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (ISF.FileExists(INTERVENTION_PHOTO_FILE_NAME))
                {
                    using (IsolatedStorageFileStream FS = ISF.OpenFile(INTERVENTION_PHOTO_FILE_NAME, FileMode.Open, FileAccess.Read))
                    {
                        bmp.SetSource(FS);
                    }
                }
            }
        }

        public object GetSetting(string key)
        {
            if(key == INTERVENTION_KEY)
            {
                return bmp;
            }
            else
            {
                return settings[key];
            }
        }

        public bool SetSetting(string key, object value)
        {
            if (key == INTERVENTION_PHOTO_FILE_NAME)
            {
                try
                {
                    SavePhoto(key, (BitmapImage)value);
                    return true;
                }
                catch (Exception e)
                {
                    throw new SaveToMemoryException(e.Message);
                }
            }
            else
            {
                try
                {
                    settings[key] = Serialize.SerializeObject(value);
                    SaveSettings();
                    return true;
                }
                catch (Exception e)
                {
                    throw new SaveToMemoryException(e.Message);
                }

            }
        }
                
        private static void SavePhoto(string fileName, BitmapImage interventionPhoto)
        {
            if (interventionPhoto == null) return;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var bitmap = new WriteableBitmap(interventionPhoto);                   
                    var path = fileName;
                    if (!store.DirectoryExists(fileName))
                    {
                        store.CreateDirectory(fileName);
                    }

                    using (var stream = store.OpenFile(path, FileMode.Create))
                    {
                        bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
                    }
                }
        }
    }
}

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
        public const string INTERVENTION_PHOTO ="Intervention.Photo";
        private const string INTERVENTION_PHOTO_FILE_DIR = @"InterventionPhoto";
        private const string INTERVENTION_PHOTO_FILE_NAME = "InterventionPhoto.jpg";
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
            SavePhoto( bmp);
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
            try
            {
                using (IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var path = Path.Combine( INTERVENTION_PHOTO_FILE_NAME);
                    if (ISF.FileExists(path))
                    {
                        using (IsolatedStorageFileStream FS = ISF.OpenFile(path, FileMode.Open, FileAccess.Read))
                        {
                            bmp.SetSource(FS);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        private void ReadPhotoFile1()
        {
            IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
            var path = Path.Combine(INTERVENTION_PHOTO_FILE_DIR, INTERVENTION_PHOTO_FILE_NAME);
            if (isoStorage.FileExists(path))
            {
                using (IsolatedStorageFileStream fileStream = isoStorage.OpenFile(path, FileMode.Open, FileAccess.Read))
                {
                    bmp.SetSource(fileStream);
                }
            }
        }

        public object GetSetting(string key)
        {
            if (key == INTERVENTION_PHOTO)
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
            if (key == INTERVENTION_PHOTO)
            {
                try
                {
                    SavePhoto( (BitmapImage)value);
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
        
        private static void SavePhoto( BitmapImage interventionPhoto)
        {
            if (interventionPhoto == null) return;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var path = Path.Combine( INTERVENTION_PHOTO_FILE_NAME);
                    var bitmap = new WriteableBitmap(interventionPhoto);
                    //if (!store.DirectoryExists(INTERVENTION_PHOTO_FILE_DIR))
                    //{
                    //    store.CreateDirectory(INTERVENTION_PHOTO_FILE_DIR);
                    //}
                    if (store.FileExists(path)) store.DeleteFile(path);

                    //using (StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(FILE_DIR, FileMode.Open, FileAccess.Write, myIsolatedStorage)))
                    using (var stream = store.CreateFile(path))
                    {
                        bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
                    }
                }
        }
    }
}

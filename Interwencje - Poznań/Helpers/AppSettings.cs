﻿using System;
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
        #region keys
        public const string CATEGORIES_KEY = "Last.Categories";
        public const string SAVED_INTERVENTION_KEY = "Last.Intervention";
        public const string INTERVENTION_PHOTO_FILE_NAME = "InterventionPhoto\InterventionPhoto.jpg";
        public const string USER_KEY = "Current.User";
        public const string FILE_DIR = "MySettings/Settings.txt";
        #endregion

        private AppSettings _appSettings;

        public AppSettings()
        {
            settings = new Dictionary<string, object>();
            settings.Add(CATEGORIES_KEY, "");
            settings.Add(SAVED_INTERVENTION_KEY, "");
            settings.Add(USER_KEY, "");
            CreateSettingsFile();
            ReadSettingsFile();
        }

        public void SaveSettings()
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
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!myIsolatedStorage.FileExists(FILE_DIR))
            {
                using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(FILE_DIR, FileMode.Create, FileAccess.Write, myIsolatedStorage)))
                {                    
                    string setts = "";
                    foreach (string key in settings.Keys)
                        setts += string.Format("{0}={1}\r\n", key, settings[key]);
                    writeFile.WriteLine(setts);
                    writeFile.Close();
                }
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
                            settings[s.Split('=')[0]] = s.Split('=')[1];
                        }
                        catch (Exception) { }
                    }
                    //if you use reader.Readline() it will read the first line of the file
                }
            }
        }

        public object GetSetting(string key)
        {
            if(key == INTERVENTION_PHOTO_FILE_NAME)
            {
                return null;
            }
            else
            {
                return settings[key];
            }
        }        

        public bool SetSetting(string key, object value)
        {
            if(key == INTERVENTION_PHOTO_FILE_NAME)
            {
                SavePhoto(key,(BitmapImage)value);
                return true;
            }
            else
            {
                try
                {            
                    settings[key] = value.ToString();
                    return true;
                }
                catch(Exception)
                {
                    return false;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interwencje___Poznań.Resources;
using Interwencje___Poznań.WebService;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;

namespace Interwencje___Poznań.Helpers
{
    class DataMemory
    {
        private static readonly IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        private const string CATEGORIES_KEY = "Last.Categories";
        private static Categories _lastCats;
        public static event EventHandler LastCategoriesUpdated;

        private const string SAVED_INTERVENTION_KEY = "Last.Intervention"; 
        private const string INTERVENTION_PHOTO_DIR_NAME = "InterventionPhoto";
        private const string INTERVENTION_PHOTO_FILE_NAME = "InterventionPhoto.jpg";
        private static Intervention _lastIntervention;
        public static event EventHandler LastInterventionUpdated;
        
        private const string USER_KEY = "Current.User";
        private static User _currentUser;
        public static event EventHandler CurrentUserUpdated;

        public static Categories LastCategories
        {
            get
            {
                if (_lastCats == null)
                {
                    if (appSettings.Contains(CATEGORIES_KEY))
                    {
                        _lastCats = (Categories)appSettings[CATEGORIES_KEY];
                    }
                    else
                    {
                        _lastCats = new Categories();
                    }
                }
                return _lastCats;
            }
            set
            {
                _lastCats = value;
                NotifyLastCategoriesUpdated();
            }
        }

        public static void SaveCategories(Action errorCallback)
        {
            try
            {
                if (appSettings.Contains(CATEGORIES_KEY))
                {
                    appSettings[CATEGORIES_KEY] = _lastCats;
                }
                else
                {
                    appSettings.Add(CATEGORIES_KEY, _lastCats);
                }
                appSettings.Save();
                NotifyLastCategoriesUpdated();
            }
            catch (IsolatedStorageException)
            {
                errorCallback();
            }
            catch (Exception)
            {
            }
        }

        private static void NotifyLastCategoriesUpdated()
        {
            var handler = LastCategoriesUpdated;
            if (handler != null) handler(null, null);
        }

        public static Intervention LastIntervention
        {
            get
            {
                if (_lastIntervention == null)
                {
                    if (appSettings.Contains(SAVED_INTERVENTION_KEY))
                    {
                        _lastIntervention = (Intervention)appSettings[SAVED_INTERVENTION_KEY];
                    }
                    else
                    {
                        _lastIntervention =  Intervention.GetCurrentIntervention();
                    }
                }
                return _lastIntervention;
            }
            set
            {
                _lastIntervention = value;
                NotifyLastInterventionUpdated();
            }
        }

        public static void SaveIntervention(Action errorCallback)
        {
            try
            {
                appSettings[SAVED_INTERVENTION_KEY] = _lastIntervention;
                appSettings.Save();
                DeleteTempInterventionPhoto();
                SaveInterventionPhoto(INTERVENTION_PHOTO_FILE_NAME, _lastIntervention.Picture, errorCallback);
                NotifyLastInterventionUpdated();
            }
            catch (IsolatedStorageException)
            {
                errorCallback();
            }
        }

        public static void DeleteIntervention()
        {
            appSettings.Remove(SAVED_INTERVENTION_KEY);
            appSettings.Save();
            DeleteInterventionPhoto();
            DeleteTempInterventionPhoto();
            _lastIntervention = null;
            NotifyLastInterventionUpdated();
        }

        private static void NotifyLastInterventionUpdated()
        {
            var handler = LastInterventionUpdated;
            if (handler != null) handler(null, null);
        }

        private static void SaveInterventionPhoto(string fileName, BitmapImage interventionPhoto,
            Action errorCallback)
        {
            if (interventionPhoto == null) return;
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var bitmap = new WriteableBitmap(interventionPhoto);
                    var path = Path.Combine(INTERVENTION_PHOTO_DIR_NAME, fileName);

                    if (!store.DirectoryExists(INTERVENTION_PHOTO_DIR_NAME))
                    {
                        store.CreateDirectory(INTERVENTION_PHOTO_DIR_NAME);
                    }

                    using (var stream = store.OpenFile(path, FileMode.Create))
                    {
                        bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
                    }
                }
            }
            catch (IsolatedStorageException)
            {
                errorCallback();
            }
        }

        private static void DeleteInterventionPhoto(String fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var path = Path.Combine(INTERVENTION_PHOTO_DIR_NAME, fileName);
                if (store.FileExists(path)) store.DeleteFile(path);
            }
        }

        public static void DeleteTempInterventionPhoto()
        {
            DeleteInterventionPhoto(INTERVENTION_PHOTO_FILE_NAME);
        }
        public static void DeleteInterventionPhoto()
        {
            DeleteInterventionPhoto(INTERVENTION_PHOTO_FILE_NAME);
        }

        public static User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    if (appSettings.Contains(USER_KEY))
                    {
                        _currentUser = (User)appSettings[USER_KEY];
                    }
                    else
                    {
                        _currentUser = new User();
                    }
                }
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                NotifyCurrentUserUpdated();
            }
        }

        public static void SaveUser(Action errorCallback)
        {
            try
            {
                if (appSettings.Contains(USER_KEY))
                {
                    appSettings[USER_KEY] = _currentUser;
                }
                else
                {
                    appSettings.Add(USER_KEY, _currentUser);
                }
                appSettings.Save();
                NotifyCurrentUserUpdated();
            }
            catch (IsolatedStorageException)
            {
                errorCallback();
            }
            catch (Exception)
            {
            }
        }

        private static void NotifyCurrentUserUpdated()
        {
            var handler = CurrentUserUpdated;
            if (handler != null) handler(null, null);
        }
    }
}

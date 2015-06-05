using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interwencje___Poznań.Resources;
using Interwencje___Poznań.WebService;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;

namespace Interwencje___Poznań.Helpers
{
    class DataMemory
    {
        private const string CATEGORIES_KEY = "Last.Categories";
        private static readonly IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        private static Categories _lastCats;
        public static event EventHandler LastCategoriesUpdated;

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
                        _lastCats = new Categories()
                        {
                            //FillupHistory = new ObservableCollection<Fillup>()
                        };
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
                appSettings[CATEGORIES_KEY] = _lastCats;
                appSettings.Save();
                NotifyLastCategoriesUpdated();
            }
            catch (IsolatedStorageException)
            {
                errorCallback();
            }
        }
        

        private static void NotifyLastCategoriesUpdated()
        {
            var handler = LastCategoriesUpdated;
            if (handler != null) handler(null, null);
        }
    }
}

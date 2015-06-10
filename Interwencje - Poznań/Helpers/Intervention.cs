using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Interwencje___Poznań.Helpers
{
    class Intervention
    {
        private Intervention()
        {
            _latitude = "0.0";
            _longitude = "0.0";
            _category = "0";
            _subcategory = "0";
            _name = "";
            _secondname = "";
            _email = "";
            _subject = "";
            _text = "";
            _address = "";
            _key = Resources.AppResources.Key;
            _picture = null;
        }

        private static Intervention _CurrentIntervention;
        
        public static Intervention GetCurrentIntervention()
        {
            if (_CurrentIntervention == null)
            {
                _CurrentIntervention = new Intervention();
            }
            return _CurrentIntervention;
        }

        public static void GetInterventionFromMemory()
        {
            _CurrentIntervention = DataMemory.LastIntervention;
        }

        public static void SaveInterventionToMemory()
        {
            DataMemory.SaveIntervention(delegate
            {
                throw new System.IO.IsolatedStorage.IsolatedStorageException();
            });

        }        

        string _longitude;
        [JsonProperty(PropertyName = "lon")]
        public string Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        string _latitude;
        [JsonProperty(PropertyName = "lat")]
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        string _category;
        [JsonProperty(PropertyName = "category")]
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        string _subcategory;
        [JsonProperty(PropertyName = "subcategory")]
        public string Subcategory
        {
            get { return _subcategory; }
            set { _subcategory = value; }
        }

        [JsonProperty(PropertyName = "name")]
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _secondname;
        [JsonProperty(PropertyName = "surname")]
        public string Secondname
        {
            get { return _secondname; }
            set { _secondname = value; }
        }

        string _email;
        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        string _subject;
        [JsonProperty(PropertyName = "subject")]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        string _text;
        [JsonProperty(PropertyName = "text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        string _address;
        [JsonProperty(PropertyName = "address")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        string _key;
        [JsonProperty(PropertyName = "key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private BitmapImage _picture;

        [JsonIgnoreAttribute]
        public BitmapImage Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }
    }
}

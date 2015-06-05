using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interwencje___Poznań.Helpers
{
    class Intervention
    {

        private static Intervention _CurrentIntervention;
        
        public static Intervention GetCurrentIntervention()
        {
            if (_CurrentIntervention == null)
            {
                _CurrentIntervention = new Intervention();
            }
            return _CurrentIntervention;
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

        string _surname;
        [JsonProperty(PropertyName = "surname")]
        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
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
    }
}

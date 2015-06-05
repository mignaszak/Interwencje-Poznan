using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interwencje___Poznań.WebService
{
    public class Categories
    {
        public string timestamp { get; set; }
        public Category[] categories { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string title { get; set; }
        [JsonProperty(PropertyName = "subcategories")]
        public Category[] subcategories { get; set; }
        public string description { get; set; }

    }

    public class Subcategory
    {
        public string id { get; set; }
        public string title { get; set; }
    }
}

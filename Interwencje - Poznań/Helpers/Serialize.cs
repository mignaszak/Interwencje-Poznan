using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Interwencje___Poznań.Helpers;
using Interwencje___Poznań.WebService;

namespace Interwencje___Poznań.Helpers
{
    class Serialize
    {
        public static AddressDetails DesrielizeAddressDetails(string json)
        {
            AddressDetails details = JsonConvert.DeserializeObject<AddressDetails>(json);
            return details;
        }

        public static Categories DesrielizeCategories(string json)
        {
            Categories details = JsonConvert.DeserializeObject<Categories>(json);
            return details;
        }

        public static string SerializeIntervention(Intervention intervention)
        {
            string Json = "";
            Json = JsonConvert.SerializeObject(intervention);
            return Json;
        }
    }
}

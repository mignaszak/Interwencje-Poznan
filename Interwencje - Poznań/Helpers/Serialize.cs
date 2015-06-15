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

        public static T Deserialize<T>(string json)
        {

            T intervention = JsonConvert.DeserializeObject<T>(json);
            return intervention;
        }
        public static string SerializeObject(object obj)
        {
            string Json = "";
            Json = JsonConvert.SerializeObject(obj);
            return Json;
        }


    }
}

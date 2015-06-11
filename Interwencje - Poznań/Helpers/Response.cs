using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Interwencje___Poznań.Helpers
{
    class Response
    {
        [JsonProperty(PropertyName = "response")]
        public ResponseElement element { get; set; }
    }

    public class ResponseElement
    {
        public string msg { get; set; }
        public string instance { get; set; }
        public string id { get; set; }
        public string error_msg { get; set; }
    }

    

}

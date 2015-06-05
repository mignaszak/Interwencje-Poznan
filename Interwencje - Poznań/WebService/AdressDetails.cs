using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interwencje___Poznań.WebService
{
    class AddressDetails
    {
        public Feature[] features { get; set; }
    }

    public class Feature
    {
        public Geometry geometry { get; set; }
        public int id { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public float[][] coordinates { get; set; }
    }

    public class Properties
    {
        public string typ { get; set; }
        public string wg_nazwiska { get; set; }
        public string wg_imienia_wydruk { get; set; }
        public string nr { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interwencje___Poznań.Helpers
{
    public class NoInternetConnectionException : Exception { public NoInternetConnectionException() : base("Brak połączenia z internetem") { } }
}

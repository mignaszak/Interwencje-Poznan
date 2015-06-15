using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interwencje___Poznań.Helpers
{
    public class NoInternetConnectionException : Exception { public NoInternetConnectionException() : base("Brak połączenia z internetem") { } }
    public class SaveToMemoryException : Exception
    {
        public SaveToMemoryException() : base("Błąd zapisywania danych do pamięci") { }

        public SaveToMemoryException(string e) : base("Błąd zapisywania danych do pamięci: " + e) { }
    }
}

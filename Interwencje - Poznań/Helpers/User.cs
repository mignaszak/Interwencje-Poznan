using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interwencje___Poznań.Helpers
{
    class User
    {
        public string Name { get; set; }

        public string Secondname { get; set; }

        public string Email { get; set; }

        public User()
        {
            this.Name = "";
            this.Secondname = "";
            this.Email = "";
        }

        public User(string name, string secondname, string email)
        {
            this.Name = name;
            this.Secondname = secondname;
            this.Email = email;
        }

    }
}

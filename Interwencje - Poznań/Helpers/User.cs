using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interwencje___Poznań.Helpers
{
    public class User
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
        public static User GetUserFromMemory()
        {
            User cats = Serialize.Deserialize<User>((string)AppSettings.CurrentAppSettings.GetSetting(AppSettings.USER_KEY));
            return cats;
        }
    }
}

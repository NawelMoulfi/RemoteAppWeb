
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteApp.Login
{
    public class LoginModel<TUser> where TUser : class
    {
        public string login { get; set; }

        public string password { get; set; }
        public string error { get; set; }

        public bool isSuperUser { get; set; }

        public TUser User { get; set; }

        public DateTime LoginStarted { get; set; }

       
    }

}


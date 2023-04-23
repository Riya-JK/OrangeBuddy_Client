using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace OrangeBuddy_Client
{
    public class LoginDetails
    {
        string username;
        string password;

        public LoginDetails(String UserName, String Password) { 
            username = UserName;
            password = Password;
        }


    }

}


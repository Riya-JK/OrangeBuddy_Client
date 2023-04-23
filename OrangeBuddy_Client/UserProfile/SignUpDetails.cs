using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client
{

    public class SignUpDetails
    {
        string firstName = "";
        string lastName = "";
        string userName = "";
        string emailId = "";
        string dateOfBirth = "";
        string passwordString = "";

        public SignUpDetails(string firstName, string lastName, string userName, string emailId, string dateOfBirth, string passwordString)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.userName = userName;
            this.emailId = emailId;
            this.dateOfBirth = dateOfBirth;
            this.passwordString = passwordString;
        }
    }
}

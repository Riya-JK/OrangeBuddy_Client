using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client
{

    public class SignUpDetails
    {
        [JsonProperty("firstName")]
        public string firstname { get; set; }

        [JsonProperty("lastName")]
        public string lastname { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("userName")]
        public string username { get; set; }

        [JsonProperty("SUID")]
        public string SUID { get; set; }

    }
}

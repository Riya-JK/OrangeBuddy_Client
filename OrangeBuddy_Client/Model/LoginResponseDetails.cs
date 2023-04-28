using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client.Model
{
    public class LoginResponseDetails
    {
        [JsonProperty("login")]
        public string login { get; set; }

        [JsonProperty("surveyFilled")]
        public bool isSurveyFilled { get; set; }

        [JsonProperty("userName")]
        public string userName { get; set; }

    }
}

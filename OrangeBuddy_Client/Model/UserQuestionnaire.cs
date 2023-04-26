using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client.UserProfile
{
    internal class UserQuestionnaire
    {
        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("isSurveyFilled")]
        public string isSurveyFilled { get; set; }

        [JsonProperty("userName")]
        public string userName { get; set; }
    }
}

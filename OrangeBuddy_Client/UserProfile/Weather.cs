using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client.UserProfile
{
    public class Weather
    {
        [JsonProperty("temperature")]
        public string temparature { get; set; }

        [JsonProperty("humidity")]
        public string humidity { get; set; }

        [JsonProperty("windSpeed")]
        public string windSpeed { get; set; }
    }
}
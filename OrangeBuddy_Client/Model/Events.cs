using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client.Model
{
    public class Events
    {
        [JsonProperty("eventName")]
        public string eventName { get; set; }

        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }

        [JsonProperty("eventTime")]
        public string Time { get; set; }
    
    }
}

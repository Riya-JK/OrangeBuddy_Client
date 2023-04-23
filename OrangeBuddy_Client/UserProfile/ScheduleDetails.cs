using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client.UserProfile
{
    public class ScheduleDetails
    {
        [JsonProperty("year_of_study")]
        public string year_of_study { get; set; }

        [JsonProperty("major")]
        public string major { get; set; }

        [JsonProperty("part_time")]
        public string parttime { get; set; }

        [JsonProperty("part_time_loc")]
        public string parttimeloc { get; set; }

        [JsonProperty("courses_enrolled")]
        public List<string> courses_enrolled { get; set; }

        [JsonProperty("parttime_schedule")]
        public List<string> parttime_schedule { get; set; }

        [JsonProperty("personal_appointment")]
        public List<string> personal_appointment { get; set; }
    }
}

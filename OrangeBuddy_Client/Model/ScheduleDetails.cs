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
        [JsonProperty("userEmail")]
        public string user_email { get; set; }

        [JsonProperty("yearOfStudy")]
        public string year_of_study { get; set; }

        [JsonProperty("major")]
        public string major { get; set; }

        [JsonProperty("partTime")]
        public string parttime { get; set; }

        [JsonProperty("partTimeLoc")]
        public string parttimeloc { get; set; }

        [JsonProperty("coursesEnrolled")]
        public List<string> courses_enrolled { get; set; }

        [JsonProperty("partTimeSchedule")]
        public Dictionary<string, string> parttime_schedule { get; set; }

        [JsonProperty("personalAppointment")]
        public Dictionary<string, string> personal_appointment { get; set; }
    }
}

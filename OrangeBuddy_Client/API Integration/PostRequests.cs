using Newtonsoft.Json;
using OrangeBuddy_Client.UserProfile;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBuddy_Client
{
    public class PostRequests
    {
        public PostRequests() { }

        public static async Task<string> postData<T>(T obj, string url, string endpoint)
        {
            using (var client = new HttpClient())
            {
                // Set the base address of the REST microservice
                client.BaseAddress = new Uri(url);

                // Set the content type header
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize the registration details to JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the registration details to the REST microservice
                var response = await client.PostAsync(endpoint, data);

                // Read the response from the REST microservice
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

    }
}

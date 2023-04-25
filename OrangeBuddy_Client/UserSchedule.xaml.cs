using OrangeBuddy_Client.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrangeBuddy_Client
{
    public partial class UserSchedule : Window
    {
        const string BASE_URL = "https://eo8lb10hc206hpi.m.pipedream.net";
        public UserSchedule()
        {
            InitializeComponent();

            MakeGetRequest();
        }

        private async void MakeGetRequest()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(BASE_URL);
            string responseBody = await response.Content.ReadAsStringAsync();
            // Process the response body
            Console.WriteLine(responseBody);
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime startDate = calendar.SelectedDates.FirstOrDefault();
            DateTime endDate = calendar.SelectedDates.LastOrDefault();

        }
    }
}

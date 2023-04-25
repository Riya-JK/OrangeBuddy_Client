using Newtonsoft.Json;
using OrangeBuddy_Client.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace OrangeBuddy_Client
{
    public partial class UserSchedule : Window
    {
        const string BASE_URL_WEATHER = "http://localhost:8080/weather?location=Syracuse";
        //const string BASE_URL = "https://eo976f2zjvsdg4b.m.pipedream.net";
        public UserSchedule(string email)
        {
            InitializeComponent();

            username.Text = email;
            MakeGetRequest();
        }

        private async void MakeGetRequest()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(BASE_URL_WEATHER);
            string responseBody = await response.Content.ReadAsStringAsync();
            // Process the response body
            Console.WriteLine(responseBody);
            Weather weatherObj = JsonConvert.DeserializeObject<Weather>(responseBody);
            Console.WriteLine(weatherObj.temparature);
            temp.Text = weatherObj.temparature;
            humidity.Text = weatherObj.humidity;
            windspeed.Text = weatherObj.windSpeed;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime startDate = calendar.SelectedDates.FirstOrDefault();
            DateTime endDate = calendar.SelectedDates.LastOrDefault();

        }

        private void logout_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Logging you out..");
            this.Visibility = Visibility.Collapsed;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
        }
    }
}

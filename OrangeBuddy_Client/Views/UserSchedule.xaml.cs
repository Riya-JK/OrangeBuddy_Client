using Newtonsoft.Json;
using OrangeBuddy_Client.Model;
using OrangeBuddy_Client.UserProfile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace OrangeBuddy_Client
{
    public partial class UserSchedule : Window
    {
        private ObservableCollection<Events> _items = new ObservableCollection<Events>();
        List<DateTime> highlightedDates;
        // Set the Listbox's ItemsSource to the ObservableCollection
        const string BASE_URL_WEATHER = "http://localhost:8080/weather?location=Syracuse";
        //const string BASE_URL = "https://eo976f2zjvsdg4b.m.pipedream.net";
        public UserSchedule(string email)
        {
            InitializeComponent();

            username.Text = email;

            //fetching weather data
            MakeGetRequest();

            //setting the date and schedule
            DateTime currentDate = DateTime.Now;
            DateTime today = currentDate.Date;
            dateField.Text = currentDate.Day.ToString();
            string monthString = currentDate.ToString("MMMM");
            monthField.Text = monthString;
            dayField.Text = currentDate.DayOfWeek.ToString();
            highlightedDates = new List<DateTime>()
            {
                new DateTime(2023, 04, 27),
                new DateTime(2023, 04, 28),
                new DateTime(2023, 05, 01)
            };

            _items.Add(new Events { Name = "OOD Class ", IsCompleted = false, Time = "6:30-7:50" });
            _items.Add(new Events { Name = "DBMS Class ", IsCompleted = true, Time = "5:15-6:30" });
            taskListBox.ItemsSource = _items;
        }

        private void CalendarDayButtonLoaded(object sender, RoutedEventArgs e)
        {
            CalendarDayButton dayButton = sender as CalendarDayButton;
            if (dayButton != null)
            {
                DateTime date = (DateTime)dayButton.DataContext;
                if (highlightedDates.Contains(date))
                {
                    dayButton.Background = System.Windows.Media.Brushes.Orange;
                }
            }
        }

        private async void MakeGetRequest()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(BASE_URL_WEATHER);
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            Console.WriteLine("From weather microservice = " + responseBody);
            Weather weatherObj = JsonConvert.DeserializeObject<Weather>(responseBody);
            Console.WriteLine(weatherObj.temparature);
            temp.Text = weatherObj.temparature + " F";
            humidity.Text = weatherObj.humidity + " %";
            windspeed.Text = weatherObj.windSpeed + " mph";
        }
        private void MyCalendar_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event here
            DateTime selectedDate = scheduleCalendar.SelectedDate ?? DateTime.MinValue;
            dateField.Text = selectedDate.Day.ToString();
            string monthString = selectedDate.ToString("MMMM");
            monthField.Text = monthString;
            dayField.Text = selectedDate.DayOfWeek.ToString();
        }

        private void logout_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Logging you out..");
            this.Visibility = Visibility.Collapsed;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
                this.DragMove();
        }

        private void lblNote_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            txtNote.Focus();
            lblNote.Text = "";
        }



        private void txtNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNote.Text) && txtNote.Text.Length> 0)
            {
                lblNote.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblNote.Visibility = Visibility.Visible;
            }
        }

        private void lblTime_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            txtTime.Focus();
            lblTime.Text = "";
        }

        private void txtTime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text) && txtTime.Text.Length > 0)
            {
                lblTime.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblTime.Visibility = Visibility.Visible;
            }
        }

        private void Button_ClickAddEvents(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtNote.Text) && !string.IsNullOrEmpty(txtTime.Text))
            {
                _items.Add(new Events { Name = txtNote.Text, IsCompleted = false, Time = txtTime.Text });
                taskListBox.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Enter valid name and timings for event");
            }
        }
    }
}

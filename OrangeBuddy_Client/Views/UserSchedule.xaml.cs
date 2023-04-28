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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Point = System.Drawing.Point;
using System.Globalization;

namespace OrangeBuddy_Client
{
    public partial class UserSchedule : Window
    {
        private ObservableCollection<Events> _items = new ObservableCollection<Events>();
        List<DateTime> highlightedDates;
        string userName;
        string emailId;
        const string BASE_URL_SCHEDULE = "http://localhost:8081/api/";
        const string BASE_URL_WEATHER = "http://localhost:8080/weather?location=Syracuse";
        const string BASE_URL_ADD_EVENT = "http://localhost:8081/api/userschedule?emailId=";
        const string BASE_URL_GET_EVENT_BY_DATE = "http://localhost:8081/api/dailyschedule?emailId=";
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public UserSchedule(string email, string userName)
        {
            InitializeComponent();

            username.Text = userName;
            emailId = email;

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

            for (int i = 0; i < 24; i++)
            {
                startHourComboBox.Items.Add(i.ToString("00"));
                endHourComboBox.Items.Add(i.ToString("00"));
            }

            for (int i = 0; i < 60; i += 15)
            {
                startMinuteComboBox.Items.Add(i.ToString("00"));
                endMinuteComboBox.Items.Add(i.ToString("00"));
            }

            _items.Add(new Events { eventName = "No events schedule for today ", IsCompleted = false, Time = "" });
            taskListBox.ItemsSource = _items;
            taskcount.Text = "You have " + _items.Count + " tasks scheduled for today!";
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

        private async void MakeGetRequestToGetEvents(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(url);
            Console.WriteLine(responseBody);
            Events events = JsonConvert.DeserializeObject<Events>(responseBody);
            if (events.Time != null)
            {
                _items.Clear();
                _items.Add(events);
                taskListBox.SelectedIndex = -1;
                taskcount.Text = "You have " + _items.Count + " tasks scheduled for today!";
            }
            else
            {
                _items.Clear();
                _items.Add(new Events { eventName = "No events schedule for today ", IsCompleted = false, Time = "" });
                taskListBox.ItemsSource = _items;
                taskcount.Text = "You have " + _items.Count + " tasks scheduled for today!";
            }
        }

        private void MyCalendar_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event here
            DateTime selectedDate = scheduleCalendar.SelectedDate ?? DateTime.MinValue;
            dateField.Text = selectedDate.Day.ToString();
            string monthString = selectedDate.ToString("MMMM");
            monthField.Text = monthString;
            dayField.Text = selectedDate.DayOfWeek.ToString();
            string url = BASE_URL_GET_EVENT_BY_DATE + emailId + "&calendarDate=" + selectedDate.Month + "/" + selectedDate.Day.ToString() + "/" + selectedDate.Year.ToString();
            MakeGetRequestToGetEvents(url);
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

        private async Task<ScheduleDetails> MakeGetRequestToAddTask()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(BASE_URL_ADD_EVENT+ emailId);
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            ScheduleDetails userSchedule = JsonConvert.DeserializeObject<ScheduleDetails>(responseBody);
            return userSchedule;
        }

        private async void Button_ClickAddEvents(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtNote.Text))
            {
                int hour = int.Parse(startHourComboBox.SelectedItem.ToString());
                int minute = int.Parse(startMinuteComboBox.SelectedItem.ToString());
                int endhour = int.Parse(endHourComboBox.SelectedItem.ToString());
                int endminute = int.Parse(endMinuteComboBox.SelectedItem.ToString());
                string displaytime = hour.ToString() + ":" + minute.ToString() + " - " + endhour.ToString() + ":" + endminute.ToString();
                _items.Add(new Events { eventName = txtNote.Text, IsCompleted = false, Time = displaytime });
                taskListBox.SelectedIndex = -1;
                taskcount.Text = "You have " + _items.Count + " tasks scheduled for today!";
                ScheduleDetails userSchedule =  await MakeGetRequestToAddTask();
                DateTime selectedDate;
                if(scheduleCalendar.SelectedDate.Value == null)
                {
                    selectedDate= DateTime.Now;
                }
                else
                {
                    selectedDate = scheduleCalendar.SelectedDate.Value.Date;
                }
                DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
                DateTime selectedEndTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, endhour, endminute, 0);
                userSchedule.personal_appointment.Add(selectedTime.ToString(), selectedEndTime.ToString());
                var response = await PostRequests.postData<ScheduleDetails>(userSchedule, BASE_URL_SCHEDULE, "userschedule");
                Console.WriteLine(response);
            }
            else
            {
                MessageBox.Show("Enter valid name and timings for event");
            }
        }

        public void captureMyScreen(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("Take a snap of my screen");
            CaptureActiveWindow(GetForegroundWindow());
        }

        private void CaptureActiveWindow(IntPtr handle)
        {
            try
            {

                var rect = new Rect();
                GetWindowRect(handle, ref rect);
                var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                var result = new Bitmap(bounds.Width, bounds.Height);
                using (var graphics = Graphics.FromImage(result))
                {
                    graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                result.Save(@"C:\Users\riyak\Downloads\Capture.jpg", ImageFormat.Jpeg); ;
                // Show the form again
                MessageBox.Show("Screen Captured"); 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using Newtonsoft.Json;
using OrangeBuddy_Client.UserControls;
using OrangeBuddy_Client.UserProfile;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrangeBuddy_Client
{
    public partial class UserQuestionnaire : Window
    {

        List<string> courses_selected = new List<string>();
        Dictionary<string, string> work_schedule = new Dictionary<string, string>();
        Dictionary<string, string> personal_appointments = new Dictionary<string, string>();
        string user_email;
        //replace with microservice endpoint
        const string BASE_URL_SCHEDULE = "http://localhost:8081/api/";
        const string BASE_URL_COURSECODES = "http://localhost:8081/api/course/codes";
        string stringpattern = @"^[a-zA-Z\s]+$";
        public UserQuestionnaire(string email)
        {
            InitializeComponent();
            user_email = email;

            //fetching course codes
            MakeGetRequest();
        }

        private async void MakeGetRequest()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(BASE_URL_COURSECODES);
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            string[] courses = JsonConvert.DeserializeObject<string[]>(responseBody);
            foreach (string course in courses)
            {
                myComboBox.Items.Add(course);
            }
            Console.WriteLine("courses = " + courses.Length);
        }

        public static readonly DependencyProperty SelectedDatesPropertyParttime =
        DependencyProperty.Register("SelectedDatesWork", typeof(ObservableCollection<DateTime>), typeof(MultiDateTimePicker), new PropertyMetadata(new ObservableCollection<DateTime>()));

        public static readonly DependencyProperty SelectedDatesPersonalAppointments =
        DependencyProperty.Register("SelectedDatesPersonal", typeof(ObservableCollection<DateTime>), typeof(MultiDateTimePicker), new PropertyMetadata(new ObservableCollection<DateTime>()));

        public ObservableCollection<DateTime> SelectedDatesParttime
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesPropertyParttime); }
            set { SetValue(SelectedDatesPropertyParttime, value); }
        }

        public ObservableCollection<DateTime> SelectedDatesPersonal
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesPersonalAppointments); }
            set { SetValue(SelectedDatesPersonalAppointments, value); }
        }
        private void AddButton_ClickParttime(object sender, RoutedEventArgs e)
        {
            int hour = int.Parse(multiDateTimePickerParttime.hourComboBox.SelectedItem.ToString());
            int minute = int.Parse(multiDateTimePickerParttime.minuteComboBox.SelectedItem.ToString());
            int endhour = int.Parse(multiDateTimePickerParttime.endhourComboBox.SelectedItem.ToString());
            int endminute = int.Parse(multiDateTimePickerParttime.endminuteComboBox.SelectedItem.ToString());

            DateTime selectedDate = multiDateTimePickerParttime.datePicker.SelectedDate.Value.Date;
            DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
            DateTime selectedEndTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, endhour, endminute, 0);

            SelectedDatesParttime.Add(selectedTime);
            dateTimeListBoxWork.Items.Add(selectedTime);
            if (!work_schedule.ContainsKey(selectedTime.ToString()))
            {
                work_schedule.Add(selectedTime.ToString(), selectedEndTime.ToString());
            }
        }

        private void RemoveButton_ClickParttime(object sender, RoutedEventArgs e)
        {
            int hour = int.Parse(multiDateTimePickerParttime.hourComboBox.SelectedItem.ToString());
            int minute = int.Parse(multiDateTimePickerParttime.minuteComboBox.SelectedItem.ToString());

            DateTime selectedDate = multiDateTimePickerParttime.datePicker.SelectedDate.Value.Date;
            DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
            SelectedDatesParttime.Remove(selectedTime);
            dateTimeListBoxWork.Items.Remove(selectedTime);
            if (work_schedule.ContainsKey(selectedTime.ToString()))
            {
                work_schedule.Remove(selectedTime.ToString());
            }
        }

        private void AddButton_ClickPersonal(object sender, RoutedEventArgs e)
        {
            int hour = int.Parse(multiDateTimePickerPersonal.hourComboBox.SelectedItem.ToString());
            int minute = int.Parse(multiDateTimePickerPersonal.minuteComboBox.SelectedItem.ToString());
            int endhour = int.Parse(multiDateTimePickerPersonal.endhourComboBox.SelectedItem.ToString());
            int endminute = int.Parse(multiDateTimePickerPersonal.endminuteComboBox.SelectedItem.ToString());

            DateTime selectedDate = multiDateTimePickerPersonal.datePicker.SelectedDate.Value.Date;
            DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
            DateTime selectedEndTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, endhour, endminute, 0);
            
            SelectedDatesPersonal.Add(selectedTime);
            dateTimeListBoxPeronal.Items.Add(selectedTime);
            if (!personal_appointments.ContainsKey(selectedTime.ToString()))
            {
                personal_appointments.Add(selectedTime.ToString(), selectedEndTime.ToString());
            }
        }

        private void RemoveButton_ClickPersonal(object sender, RoutedEventArgs e)
        {
            int hour = int.Parse(multiDateTimePickerPersonal.hourComboBox.SelectedItem.ToString());
            int minute = int.Parse(multiDateTimePickerPersonal.minuteComboBox.SelectedItem.ToString());

            DateTime selectedDate = multiDateTimePickerPersonal.datePicker.SelectedDate.Value.Date;
            DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
            SelectedDatesPersonal.Remove(selectedTime);
            dateTimeListBoxPeronal.Items.Remove(selectedTime);
            if (personal_appointments.ContainsKey(selectedTime.ToString()))
            {
                personal_appointments.Remove(selectedTime.ToString());
            }
        }

        private void myComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item from the ComboBox
            if (myComboBox.SelectedItem != null)
            {
                string selectedItem = myComboBox.SelectedItem.ToString();
                courses_selected.Add((string)selectedItem);
                Console.WriteLine(selectedItem);
            }
        }

        private async void submitQuestionnaire(object sender, RoutedEventArgs e)
        {
            string yearofStudy = yearOfStudy.Text;
            string major = majorOfStudy.Text;
            string parttime = isparttime.Text;
            string parttimeloc = whereparttime.Text;
            var isValid = ValidateScheduleDetails(yearofStudy, major, parttime, parttimeloc);
            if (isValid)
            {
                ScheduleDetails schedule = new ScheduleDetails()
                {
                    user_email = user_email,
                    major = major,
                    parttimeloc = parttimeloc,
                    year_of_study = yearofStudy,
                    parttime = parttime,
                    courses_enrolled = courses_selected,
                    parttime_schedule = work_schedule,
                    personal_appointment = personal_appointments
                };
                var response = await PostRequests.postData<ScheduleDetails>(schedule, BASE_URL_SCHEDULE, "userschedule");
                Console.WriteLine(response);
                //var login_response = await PostRequests.postData<ScheduleDetails>(schedule, BASE_URL_SCHEDULE, "login");
                //Console.WriteLine(login_response);
                MessageBox.Show("Form submitted successfully.Building your schedule now..");
                this.Visibility = Visibility.Collapsed;
                UserSchedule userSchedule = new UserSchedule(user_email);
                userSchedule.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Failed to submit form.");
            }
        }

        private bool ValidateScheduleDetails(string yearofStudy, string major, string parttime, string parttimeloc)
        {
            // Check if the first name is valid
            if (string.IsNullOrEmpty(yearofStudy) || (!yearofStudy.ToLower().Equals("undergrad") && !yearofStudy.ToLower().Equals("grad")))
            {
                // year of study is not valid
                // Handle the error
                MessageBox.Show("Invalid year of study. Enter grad or undergrad");
                return false;
            }

            // Check if the last name is valid
            if (string.IsNullOrEmpty(major) || !Regex.IsMatch(major, stringpattern))
            {
                // major is not valid
                // Handle the error

                MessageBox.Show("Invalid major.");
                return false;
            }

            // Check if the user name is valid
            if (string.IsNullOrEmpty(parttime) || !Regex.IsMatch(parttime, stringpattern))
            {
                // User name is not valid
                // Handle the error
                MessageBox.Show("Invalid part time input");
                return false;
            }
            if (string.IsNullOrEmpty(parttimeloc) || !Regex.IsMatch(parttimeloc, stringpattern))
            {
                // User name is not valid
                // Handle the error
                MessageBox.Show("Invalid part time location");
                return false;
            }
            return true;
        }


    }
}

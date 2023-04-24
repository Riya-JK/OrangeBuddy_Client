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
        List<string> work_schedule = new List<string>();
        List<string> personal_appointments = new List<string>();
        //replace with microservice endpoint
        const string BASE_URL = "http://localhost:8082/api/users/";
        public UserQuestionnaire()
        {
            InitializeComponent();
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

            DateTime selectedDate = multiDateTimePickerParttime.datePicker.SelectedDate.Value.Date;
            DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
            SelectedDatesParttime.Add(selectedTime);
            dateTimeListBoxWork.Items.Add(selectedTime);
            if (!work_schedule.Contains(selectedTime.ToString()))
            {
                work_schedule.Add(selectedTime.ToString());
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
            if (work_schedule.Contains(selectedTime.ToString()))
            {
                work_schedule.Remove(selectedTime.ToString());
            }
        }

        private void AddButton_ClickPersonal(object sender, RoutedEventArgs e)
        {
            int hour = int.Parse(multiDateTimePickerPersonal.hourComboBox.SelectedItem.ToString());
            int minute = int.Parse(multiDateTimePickerPersonal.minuteComboBox.SelectedItem.ToString());

            DateTime selectedDate = multiDateTimePickerPersonal.datePicker.SelectedDate.Value.Date;
            DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
            SelectedDatesPersonal.Add(selectedTime);
            dateTimeListBoxPeronal.Items.Add(selectedTime);
            if (!personal_appointments.Contains(selectedTime.ToString()))
            {
                personal_appointments.Add(selectedTime.ToString());
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
            if (personal_appointments.Contains(selectedTime.ToString()))
            {
                personal_appointments.Remove(selectedTime.ToString());
            }
        }

        private void myComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            string[] courses = new string[] { "CIS600", "CIS700", "CIS800", "CIS667" };
            // Populate the ComboBox with options
            foreach( string course in courses){
                myComboBox.Items.Add(course);
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
                    major = major,
                    parttimeloc = parttimeloc,
                    year_of_study = yearofStudy,
                    parttime = parttime,
                    courses_enrolled = courses_selected,
                    parttime_schedule = work_schedule,
                    personal_appointment = personal_appointments
                };
                var response = await sendScheduleDetails(schedule);
                MessageBox.Show("Form submitted successfully.Building your schedule now..");
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Failed to submit form.");
            }
        }

        private bool ValidateScheduleDetails(string yearofStudy, string major, string parttime, string parttimeloc)
        {
            // Check if the first name is valid
            if (string.IsNullOrEmpty(yearofStudy))
            {
                // First name is not valid
                // Handle the error
                MessageBox.Show("Invalid year of study");
                return false;
            }

            // Check if the last name is valid
            if (string.IsNullOrEmpty(major))
            {
                // Last name is not valid
                // Handle the error
                MessageBox.Show("Invalid major");
                return false;
            }

            // Check if the user name is valid
            if (string.IsNullOrEmpty(parttime))
            {
                // User name is not valid
                // Handle the error
                MessageBox.Show("Invalid parttime input");
                return false;
            }
            if (string.IsNullOrEmpty(parttimeloc))
            {
                // User name is not valid
                // Handle the error
                MessageBox.Show("Invalid parttime location");
                return false;
            }
            return true;
        }

        public static async Task<string> sendScheduleDetails(ScheduleDetails scheduleDetails)
        {
            using (var client = new HttpClient())
            {
                // Set the base address of the REST microservice
                client.BaseAddress = new Uri(BASE_URL);

                // Set the content type header
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize the registration details to JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(scheduleDetails);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the registration details to the REST microservice
                var response = await client.PostAsync("schedule", data);

                // Read the response from the REST microservice
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

    }
}

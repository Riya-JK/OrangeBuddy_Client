﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrangeBuddy_Client.UserControls
{
    public partial class MultiDateTimePicker : UserControl
    {
        public MultiDateTimePicker()
        {
            InitializeComponent();

            // Populate the hour and minute ComboBox controls
            hourComboBox.Items.Clear();
            minuteComboBox.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                hourComboBox.Items.Add(i.ToString("00"));
            }

            for (int i = 0; i < 60; i += 15)
            {
                minuteComboBox.Items.Add(i.ToString("00"));
            }

            // Set the default date and time
            datePicker.SelectedDate = DateTime.Today;
            hourComboBox.SelectedIndex = 0;
            minuteComboBox.SelectedIndex = 0;

            // Handle the SelectedDateChanged event of the DatePicker control
            datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the available times based on the selected date
            DateTime selectedDate = datePicker.SelectedDate.Value;
            DateTime startDateTime = selectedDate.Date.AddHours(9);
            DateTime endDateTime = selectedDate.Date.AddHours(18).AddMinutes(45);

            hourComboBox.Items.Clear();
            minuteComboBox.Items.Clear();

            for (DateTime time = startDateTime; time <= endDateTime; time = time.AddMinutes(15))
            {
                hourComboBox.Items.Add(time.Hour.ToString("00"));
                minuteComboBox.Items.Add(time.Minute.ToString("00"));
            }

            hourComboBox.SelectedIndex = 0;
            minuteComboBox.SelectedIndex = 0;
        }

        // Add a property to get the selected date and time
        public DateTime SelectedDateTime
        {
            get
            {
                int hour = int.Parse(hourComboBox.SelectedItem.ToString());
                int minute = int.Parse(minuteComboBox.SelectedItem.ToString());

                DateTime selectedDate = datePicker.SelectedDate.Value.Date;
                DateTime selectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);

                return selectedTime;
            }
        }

    }
}
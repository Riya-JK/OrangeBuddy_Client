﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
namespace OrangeBuddy_Client
{
    public partial class SignUp : Window
    {
        string firstName;
        string lastName;
        string userName;
        string emailId;
        string SUID;
        string passwordString;
        string nameRegex = "^[A-Za-z]+$";
        string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        const string BASE_URL = "https://eo8lb10hc206hpi.m.pipedream.net";

        public SignUp()
        {
            InitializeComponent();
        }

        private void backToLogin(object sender, RoutedEventArgs e)
        {
            MainWindow objLogin = new MainWindow();
            this.Visibility = Visibility.Collapsed;
            objLogin.Show();
        }

        private async void registerUser(object sender, RoutedEventArgs e)
        {
            string firstName = first_name.Text;
            string lastName = last_name.Text;
            string userName = user_name.Text;
            string emailId = email.Text;
            string SUID = suid.Text;
            string passwordString = password.Password.ToString();
            var isValid = validateUserDetails(firstName, lastName, userName, emailId, SUID, passwordString);
            if (isValid)
            {
                SignUpDetails signUpObj = new SignUpDetails() { email=emailId, first_name=firstName, last_name=lastName, password=passwordString, user_name=userName,
                SUID=SUID};
                var response = await SendRegistrationDetails(signUpObj);
                Console.WriteLine("response = " + response);
                MessageBox.Show("Account created successfully. You can login to your account now.");
                backToLogin(sender, e);
            }
        }

        private bool validateUserDetails(string firstName, string lastName, string userName, string emailId, string suid, string passwordString)
        {
            // Check if the first name is valid
            if (string.IsNullOrEmpty(firstName) || !Regex.IsMatch(firstName, nameRegex))
            {
                // First name is not valid
                // Handle the error
                MessageBox.Show("Invalid First Name");
                return false;
            }

            // Check if the last name is valid
            if (string.IsNullOrEmpty(lastName) || !Regex.IsMatch(lastName, nameRegex))
            {
                // Last name is not valid
                // Handle the error
                MessageBox.Show("Invalid Last Name");
                return false;
            }

            // Check if the user name is valid
            if (string.IsNullOrEmpty(userName))
            {
                // User name is not valid
                // Handle the error
                MessageBox.Show("Invalid Username");
                return false;
            }

            // Check if the email id is valid
            if (string.IsNullOrEmpty(emailId) || !Regex.IsMatch(emailId, emailRegex))
            {
                // Email id is not valid
                // Handle the error
                MessageBox.Show("Invalid EmailId");
                return false;
            }

            // Check if the date of birth is valid
            if (string.IsNullOrEmpty(suid))
            {
                // Date of birth is not valid
                // Handle the error
                MessageBox.Show("Invalid Date of Birth");
                return false;
            }

            // Check if the password is valid
            if (string.IsNullOrEmpty(passwordString) || !Regex.IsMatch(passwordString, passwordRegex))
            {
                // Password is not valid
                // Handle the error
                MessageBox.Show("Invalid Password.Your password needs to have 8 digits, one capital letter, a digit and a special character $ or @.");
                return false;
            }
            return true;
        }

        public static async Task<string> SendRegistrationDetails(SignUpDetails signUpDetails)
        {
            using (var client = new HttpClient())
            {
                // Set the base address of the REST microservice
                client.BaseAddress = new Uri(BASE_URL);

                // Set the content type header
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize the registration details to JSON
                Console.WriteLine(signUpDetails.password);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(signUpDetails);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the registration details to the REST microservice
                var response = await client.PostAsync("register", data);

                // Read the response from the REST microservice
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

    }
}
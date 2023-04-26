using System;
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
        //string dateRegex = @"^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$";
        string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        const string BASE_URL = "http://localhost:8082/api/users/";

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
            string firstName = firstname.Text;
            Console.WriteLine(firstName);
            string lastName = lastname.Text;
            string userName = username.Text;
            string emailId = email.Text;
            string suiduser = suid.Text;
            string passwordString = password.Password.ToString();
            var isValid = validateUserDetails(firstName, lastName, userName, emailId, suiduser, passwordString);
            if (isValid)
            {
                SignUpDetails signUpObj = new SignUpDetails()
                {
                    email = emailId,
                    firstname = firstName,
                    lastname = lastName,
                    password = passwordString,
                    username = userName,
                    SUID = suiduser
                };
                var response = await PostRequests.postData<SignUpDetails>(signUpObj, BASE_URL, "register");
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

            // Check if the suid is valid
            if (string.IsNullOrEmpty(suid))
            {
                // First name is not valid
                // Handle the error
                MessageBox.Show("Invalid SUID");
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

    }
}

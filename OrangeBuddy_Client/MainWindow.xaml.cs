using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace OrangeBuddy_Client
{
    public partial class MainWindow : Window
    {
        const string BASE_URL = "http://localhost:8082/api/users/";
        //const string BASE_URL = "http://localhost:8082/api/users/";
        string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        public MainWindow()
        {
            InitializeComponent();
        }

        async void userLogin(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            var isValid = validateUserDetails(email, password);
            if (isValid)
            {
                LoginDetails login = new LoginDetails() { email=email, password=password};
                var response = await PostRequests.postData<LoginDetails>(login, BASE_URL, "login");
                Console.WriteLine(response);
                MessageBox.Show("Successfully signed in");
                UserQuestionnaire userQuestionnaire = new UserQuestionnaire(login.email);
                this.Visibility = Visibility.Collapsed;
                userQuestionnaire.Visibility = Visibility.Visible;
                //if (response != null)
                //{
                //    if (response.Equals("{login:successful, isSurveyFilled:False}"))
                //    {
                //        MessageBox.Show("Successfully signed in");
                //        UserQuestionnaire userQuestionnaire = new UserQuestionnaire(login.email);
                //        this.Visibility = Visibility.Collapsed;
                //        userQuestionnaire.Visibility = Visibility.Visible;
                //    }
                //    else if (response.Equals("{login:successful, isSurveyFilled:True}"))
                //    {
                //        MessageBox.Show("Successfully signed in");
                //        this.Visibility = Visibility.Collapsed;
                //    }
                //    else
                //    {
                //        MessageBox.Show("Login unsuccessful");
                //    }
                //}
            }
        }

        private bool validateUserDetails(string emailId, string passwordString)
        {
            // Check if the email id is valid
            if (string.IsNullOrEmpty(emailId) || !Regex.IsMatch(emailId, emailRegex))
            {
                // Email id is not valid
                // Handle the error
                MessageBox.Show("Invalid EmailId");
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

        public void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            textEmail.Text = "";
            txtEmail.Focus();
        }

        public void txtEmail_MouseLeave(object sender, MouseButtonEventArgs e)
        {
            textEmail.Text = "Email";
        }

        public void txtEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility= Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility= Visibility.Visible;
            }
        }

        public void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            textPassword.Text = "";
            txtPassword.Focus();
        }

        public void textPassword_MouseLeave(object sender, MouseButtonEventArgs e)
        {
            textPassword.Text = "Password";
        }

        public void txtPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        public void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            SignUp objSignUp= new SignUp();
            this.Visibility = Visibility.Hidden;
            objSignUp.Show();

        }
    }
}

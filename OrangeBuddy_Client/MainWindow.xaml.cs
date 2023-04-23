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

namespace OrangeBuddy_Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        async void userLogin(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                LoginDetails login = new LoginDetails(txtEmail.Text, txtPassword.Password);
                var response = await SendLoginDetails(login);
                Console.WriteLine(response);
                MessageBox.Show("Successfully signed in");
            }
        }

        public static async Task<string> SendLoginDetails(LoginDetails loginDetails)
        {
            using (var client = new HttpClient())
            {
                // Set the base address of the REST microservice
                client.BaseAddress = new Uri("https://eo8lb10hc206hpi.m.pipedream.net");

                // Set the content type header
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize the registration details to JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginDetails);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the registration details to the REST microservice
                var response = await client.PostAsync("signup", data);

                // Read the response from the REST microservice
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

        public void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
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
            txtPassword.Focus();
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

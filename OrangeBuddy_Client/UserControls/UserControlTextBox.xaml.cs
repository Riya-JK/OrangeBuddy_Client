using System.Windows;
using System.Windows.Controls;


namespace OrangeBuddy_Client.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlTextBox.xaml
    /// </summary>
    public partial class UserControlTextBox : UserControl
    {
        public UserControlTextBox()
        {
            InitializeComponent();
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register
            ("Hint", typeof(string), typeof(UserControlTextBox));
    }
}

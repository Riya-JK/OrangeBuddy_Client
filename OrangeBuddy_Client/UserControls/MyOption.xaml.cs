using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace OrangeBuddy_Client.UserControls
{
    public partial class MyOption : UserControl
    {
        public MyOption()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            ("Text", typeof(string), typeof(MyOption));

        public FontAwesome.WPF.FontAwesome Icon
        {
            get { return (FontAwesome.WPF.FontAwesome)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register
            ("Icon", typeof(FontAwesome.WPF.FontAwesome), typeof(MyOption));
    }
}

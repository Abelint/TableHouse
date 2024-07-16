using System.Windows;

namespace TableHouse
{
    public partial class PasswordWindow : Window
    {
        public bool IsPasswordCorrect { get; private set; }

        public PasswordWindow()
        {
            InitializeComponent();
            IsPasswordCorrect = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox.Password;

            // Проверка пароля (замените "your_password" на нужный вам пароль)
            if (password == "qwerty")
            {
                IsPasswordCorrect = true;
                this.Close();
            }
            else
            {
                errorTextBlock.Visibility = Visibility.Visible;
                passwordBox.Clear();
            }
        }
    }
}

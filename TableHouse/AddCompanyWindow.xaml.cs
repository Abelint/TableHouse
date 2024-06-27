using System.Windows;

namespace TableHouse
{
    public partial class AddCompanyWindow : Window
    {
        public AddCompanyWindow()
        {
            InitializeComponent();
        }

        private void ChooseLogo_Click(object sender, RoutedEventArgs e)
        {
            // Логика для выбора логотипа (например, через OpenFileDialog)
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png;";

            if (openFileDialog.ShowDialog() == true)
            {
                // Сохранение пути к выбранному файлу
                string logoPath = openFileDialog.FileName;
                tbFloor.Tag = logoPath; // Используем Tag для хранения пути к логотипу
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка и возврат введенных данных
            if (int.TryParse(tbFloor.Text, out int floor) && !string.IsNullOrWhiteSpace(tbName.Text))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
            }
        }

        public int Floor => int.Parse(tbFloor.Text);
        public string Office => tbOffice.Text;
        public int Side =>int.Parse(tbSide.Text);
        public string Name => tbName.Text;
        public string LogoPath => tbFloor.Tag as string;
    }
}

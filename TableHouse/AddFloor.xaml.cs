using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace TableHouse
{
    /// <summary>
    /// Логика взаимодействия для AddFloor.xaml
    /// </summary>
    public partial class AddFloor : Window
    {
        public AddFloor()
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
            if (int.TryParse(tbFloor.Text, out int floor) && !string.IsNullOrWhiteSpace(tbSide.Text))
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
        public int Side => int.Parse(tbSide.Text);      
        public string LogoPath => tbFloor.Tag as string;
    }
}

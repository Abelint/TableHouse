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
    /// Логика взаимодействия для ChangeFloor.xaml
    /// </summary>
    public partial class ChangeFloor : Window
    {
        private Place company;
        public ChangeFloor(Place selectedCompany)
        {
            InitializeComponent();
            company = selectedCompany;
            FillData();
        }

        private void FillData()
        {
            tbFloor.Text = company.Stage.ToString();
            tbSide.Text = company.Side.ToString();
         
            // Логика для отображения логотипа
        }
        private void ChooseLogo_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png;";

            if (openFileDialog.ShowDialog() == true)
            {
                // Сохранение пути к выбранному файлу
                string logoPath = openFileDialog.FileName;
                tbFloor.Tag = logoPath; // Используем Tag для хранения пути к логотипу
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновление данных компании
                company.Stage = int.Parse(tbFloor.Text);
                company.Side = int.Parse(tbSide.Text); // Изменено на целое значение, если сторона - это число
              

                // Инициализация адаптера базы данных
                DBadapter dBadapter = new DBadapter();
                dBadapter.Init();

                // Создание словаря с новыми данными
                var updatedData = new Dictionary<string, object>
        {
            { "Stage", company.Stage },
            { "Side", company.Side },
          
        };

                // Обновление данных в базе данных
                dBadapter.ReplaceData("Place", updatedData, $"id = {company.Id}");

                // Обновление данных в ObservableCollection
                var existingCompany = AdapterClass.Places.FirstOrDefault(c => c.Id == company.Id);
                if (existingCompany != null)
                {
                    existingCompany.Stage = company.Stage;
                    existingCompany.Side = company.Side;
                   
                }

                Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Проверьте данные. Этаж должен быть числом.");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика удаления данных
            try
            {
                // Инициализация адаптера базы данных
                DBadapter dBadapter = new DBadapter();
                dBadapter.Init();

                // Удаление данных из базы данных
                dBadapter.DeleteData("CompanyInHouse", $"id = {company.Id}");

                // Удаление данных из ObservableCollection
                var existingCompany = AdapterClass.Places.FirstOrDefault(c => c.Id == company.Id);
                if (existingCompany != null)
                {
                    AdapterClass.Places.Remove(existingCompany);
                }

                Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка при удалении данных. Попробуйте снова.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика отмены изменений
            this.Close();
        }
    }
}

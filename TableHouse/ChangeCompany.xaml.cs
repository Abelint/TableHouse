using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TableHouse
{
    public partial class ChangeCompany : Window
    {
        private CompanyInHouse company;
        public ChangeCompany(CompanyInHouse selectedCompany)
        {
            InitializeComponent();
            company = selectedCompany;
            FillData();
        }
        private void FillData()
        {
            tbFloor.Text = company.Stage.ToString();
            tbSide.Text = company.Side.ToString();
            tbOffice.Text = company.Office;
            tbName.Text = company.Name;
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
                company.Office = tbOffice.Text;
                company.Name = tbName.Text;

                // Инициализация адаптера базы данных
                DBadapter dBadapter = new DBadapter();
                dBadapter.Init();

                // Создание словаря с новыми данными
                var updatedData = new Dictionary<string, object>
        {
            { "Stage", company.Stage },
            { "Side", company.Side },
            { "Office", company.Office },
            { "Name", company.Name }
        };

                // Обновление данных в базе данных
                dBadapter.ReplaceData("CompanyInHouse", updatedData, $"id = {company.Id}");

                // Обновление данных в ObservableCollection
                var existingCompany = AdapterClass.inHouse.FirstOrDefault(c => c.Id == company.Id);
                if (existingCompany != null)
                {
                    existingCompany.Stage = company.Stage;
                    existingCompany.Side = company.Side;
                    existingCompany.Office = company.Office;
                    existingCompany.Name = company.Name;
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
                var existingCompany = AdapterClass.inHouse.FirstOrDefault(c => c.Id == company.Id);
                if (existingCompany != null)
                {
                    AdapterClass.inHouse.Remove(existingCompany);
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

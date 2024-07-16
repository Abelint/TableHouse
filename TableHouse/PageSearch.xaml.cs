using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TableHouse
{
    /// <summary>
    /// Логика взаимодействия для PageSearch.xaml
    /// </summary>
    public partial class PageSearch : Page
    {
       
        ObservableCollection<CompanyInHouse> filter = new ObservableCollection<CompanyInHouse>();
        public PageSearch()
        {
            InitializeComponent();

         
            lvCompanies.ItemTemplate = (DataTemplate)this.Resources["CompanyTemplate"];
            imgSearch.Source = AdapterClass.SearchImage;

            lvCompanies.ItemsSource = AdapterClass.inHouse;
            // Подписываемся на событие TextChanged для tbSearch
            tbSearch.TextChanged += TbSearch_TextChanged;
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Фильтруем _inHouse на основе содержимого tbSearch
            FilterCompanies();
        }

        private void FilterCompanies()
        {
            // Получаем текст из tbSearch
            string searchText = tbSearch.Text.ToLower();

            // Фильтруем _inHouse, оставляя только компании, где Name содержит searchText
            var infilt = AdapterClass.inHouse.Where(c => c.Name.ToLower().Contains(searchText)).ToList();

            // Обновляем привязку данных
            lvCompanies.ItemsSource = infilt;
            
        }
        private void lvCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCompanies.SelectedItem != null)
            {
                try
                {

               
                // Пример: Получаем объект данных, связанный с выбранным элементом
                var selectedItem = lvCompanies.SelectedItem as CompanyInHouse; // 

                // Выводим содержимое элемента (например, в MessageBox)
                //MessageBox.Show($"Этаж: {selectedItem.Num}, Название: {selectedItem.Name}");
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                // Создаем экземпляр страницы PageWay
                PageWay pageWay = new PageWay(selectedItem);

                // Загружаем страницу PageWay в Frame
                mainWindow.Main.Content = pageWay;
                // Снимаем выделение, чтобы можно было снова выбрать тот же элемент
                lvCompanies.SelectedItem = null;
                }
                catch(Exception exc) {
                    MessageBox.Show(exc.Message);
                }
            }
        }
    }
}

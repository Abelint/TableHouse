using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PageWay.xaml
    /// </summary>
    public partial class PageWay : Page
    {
        public CompanyInHouse Company { get; private set; }

        public PageWay(CompanyInHouse company)
        {
            InitializeComponent();
            Company = company;
           
            DisplayData();
        }

        private void DisplayData()
        {
            try
            {
                // Пример: Отобразим данные на странице
                // Допустим, у нас есть TextBlock с именем tbInfo
                string addstr = Company.Side == 1 ? ", левое крыло" : Company.Side == 2 ? ", правое крыло" : "";
                tbInfo.Text = $"Этаж {Company.Stage}, Офис {Company.Office}{addstr} -  {Company.Name}";
                // Путь к изображению
                string imagePath = Directory.GetCurrentDirectory() + "\\logos\\noimage.png";

                foreach (var pos in AdapterClass.Places)
                {
                    if (Company.Stage == pos.Stage && Company.Side == pos.Side)
                    {
                        imagePath = pos.LogoPath;
                        break;
                    }
                }
                // Создаем объект BitmapImage для загрузки изображения
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();

                // Устанавливаем изображение в элемент Image (imgWay)
                imgWay.Source = bitmapImage;
            }
            catch (Exception exc) {
                MessageBox.Show(exc.Message);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Логика взаимодействия для PageStart.xaml
    /// </summary>
    public partial class PageStart : Page
    {
        public PageStart()
        {
            InitializeComponent();            

            this.Background = AdapterClass.BackgroundImage;
            
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            // Создаем экземпляр страницы PageWay
            PageSearch pageWay = new PageSearch();

            // Загружаем страницу PageWay в Frame
            mainWindow.Main.Content = pageWay;
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            passwordWindow.Owner = Window.GetWindow(this);
            passwordWindow.ShowDialog();

            if (passwordWindow.IsPasswordCorrect)
            {
                // Переход на страницу PageAdmin
                NavigationService.Navigate(new PageAdmin());
            }
        }
    }
}

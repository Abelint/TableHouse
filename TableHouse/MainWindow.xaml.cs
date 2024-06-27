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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InactivityWatcher inactivityWatcher;
        public MainWindow()
        {
            InitializeComponent();

            Bitmap src = Properties.Resources.house2;
            AdapterClass.BackgroundImage = AdapterClass.Brushing(src);
            inactivityWatcher = new InactivityWatcher(TimeSpan.FromSeconds(20), OnInactivity);
            inactivityWatcher.Start();

            Main.Content = new PageStart();
        }

        private void ButtonSinchronization_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnInactivity()
        {
            // Выполните переход на начальную страницу при истечении времени бездействия
            this.Dispatcher.Invoke(() =>
            {
                if (Main.Content is NavigationWindow navigationWindow)
                {
                    NavigationService navigationService = navigationWindow.NavigationService;
                    if (navigationService != null)
                    {
                        navigationService.Navigate(new PageStart());
                    }
                }
                else
                {
                    // Если Content не является NavigationWindow, создайте новый экземпляр PageStart
                    Main.Content = new PageStart();
                }
            });
        }
    }
}

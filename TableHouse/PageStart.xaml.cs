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
using System.Windows.Markup;
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
        private DBadapter _DBadapter;

        public PageStart()
        {
            InitializeComponent();            

            this.Background = AdapterClass.BackgroundImage;

            _DBadapter = new DBadapter();
            _DBadapter.Init();
            Dictionary<string, string[]> tables = new Dictionary<string, string[]>();
            tables.Add("CompanyInHouse", new string[] { "Stage INT", "Side INT", "Office TEXT", "Name TEXT", "LogoPath TEXT" });
            tables.Add("Place", new string[] { "Stage INT", "Side INT", "LogoPath TEXT" });
            var tbl = _DBadapter.Tables;
            if (tbl.Length == 0)
            {
                _DBadapter.CreateTables(tables);
                //foreach(var k in tables.Keys)
                //{
                //    var data2 = new Dictionary<string, object>();
                //    foreach (string c in tables[k])
                //    {
                //        string[] str = c.Split(' ');
                //        data2[str[0]] = str[0];
                //    }
                //    _DBadapter.AddData(k, data2);
                //}
                
            }
            AdapterClass.inHouse.Clear();
           var inbase = _DBadapter.GetData("CompanyInHouse");
            foreach (var company in inbase)
            {
                CompanyInHouse companyInHouse = new CompanyInHouse
                {
                    Id = Convert.ToInt32(company["id"]),
                    Stage = Convert.ToInt32(company["Stage"]),
                    Side = Convert.ToInt32(company["Side"]),
                    Name = company["Name"].ToString(),
                    LogoPath = company["LogoPath"].ToString()
                };
                AdapterClass.inHouse.Add(companyInHouse);
            }

            AdapterClass.Places.Clear();
           inbase = _DBadapter.GetData("Place");
            foreach (var company in inbase)
            {
                Place companyInHouse = new Place
                {
                    Id = Convert.ToInt32(company["id"]),
                    Stage = Convert.ToInt32(company["Stage"]),
                    Side = Convert.ToInt32(company["Side"]),
                    LogoPath = company["LogoPath"].ToString()
                };
                AdapterClass.Places.Add(companyInHouse);
            }

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

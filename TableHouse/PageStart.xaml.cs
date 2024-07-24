using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            ImageBrush backgroundBrush = new ImageBrush
            {
                ImageSource = AdapterClass.BackgroundImage.ImageSource,
                Stretch = Stretch.None,
                //AlignmentX = AlignmentX.Center,
                //AlignmentY = AlignmentY.Top,
                Viewbox = new Rect(0, 0.1, 1,1), // 0.95, чтобы создать отступ снизу 5% от высоты
                ViewboxUnits = BrushMappingMode.RelativeToBoundingBox
            };

            // Установка фона для Grid
          //  this.Background = backgroundBrush;
            //this.Background = AdapterClass.BackgroundImage;
            BackgroundContainer.Background = backgroundBrush;
            _DBadapter = new DBadapter();
            _DBadapter.Init();
            Dictionary<string, string[]> tables = new Dictionary<string, string[]>();
            tables.Add("CompanyInHouse", new string[] { "Stage INT", "Side INT", "Office TEXT", "Name TEXT", "LogoPath TEXT" });
            tables.Add("Place", new string[] { "Stage INT", "Side INT", "LogoPath TEXT" });
            var tbl = _DBadapter.Tables;
            if (tbl.Length == 0) _DBadapter.CreateTables(tables);

            if(!File.Exists(Directory.GetCurrentDirectory() + "\\htacc"))
            {
                
                File.Create(Directory.GetCurrentDirectory() + "\\htacc");
                /////////////для автоматического добавления
                try
                {
                    var filePath = Directory.GetCurrentDirectory() + "\\Таблица организаций для табло.csv";
                    AddCompaniesToDatabase(filePath);
                }
                catch(Exception exc) {
                    MessageBox.Show(exc.Message);
                }

                try
                {
                    AddPlacesFromLogosFolder(Directory.GetCurrentDirectory() + "\\Floors");
                }
                catch { }               
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
                    Office = company["Office"].ToString(),
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
        public void AddCompaniesToDatabase(string filePath)
        {
            var companies = ReadCompaniesFromCsv(filePath);

            foreach (var company in companies)
            {
                var data = new Dictionary<string, object>
        {
            { "Stage", company.Stage },
            { "Office", company.Office },
            { "Name", company.Name },
            { "Side", company.Side },
            { "LogoPath", company.LogoPath }
        };

               
                _DBadapter.AddData("CompanyInHouse", data);
            }
         //  MessageBox.Show("Записей в таблице " + companies.Count);
        }
        public static List<CompanyInHouse> ReadCompaniesFromCsv(string filePath)
        {
            var companies = new List<CompanyInHouse>();

            using (var reader = new StreamReader(filePath))
            {
                var header = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    if (values[0] == "") continue;
                    var company = new CompanyInHouse
                    {
                        Stage = int.Parse(values[0], CultureInfo.InvariantCulture),
                        Office = values[1],
                        Name = values[3],//.Trim('"'), // Удаление кавычек из имени
                        Side = values[5].ToLower() == "левое" ? 1 : values[5].ToLower() == "правое" ? 2 : 0
                    };

                    companies.Add(company);
                }
            }

            return companies;
        }

        public void AddPlacesFromLogosFolder(string logosFolderPath)
        {
            var logoFiles = Directory.GetFiles(logosFolderPath);

            foreach (var file in logoFiles)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(file);

                // Регулярное выражение для извлечения данных из имени файла
                Regex regex = new Regex(@"(\d+|\d+-\d+)\s*этаж\s*(левое|правое)?\s*крыло", RegexOptions.IgnoreCase);
                Match match = regex.Match(fileName);

                if (match.Success)
                {
                    var stagesPart = match.Groups[1].Value;
                    var sidePart = match.Groups[2].Value;

                    int side = 0;
                    if (sidePart.Equals("левое", StringComparison.OrdinalIgnoreCase))
                        side = 1;
                    else if (sidePart.Equals("правое", StringComparison.OrdinalIgnoreCase))
                        side = 2;

                    if (stagesPart.Contains('-'))
                    {
                        var rangeParts = stagesPart.Split('-');
                        int startStage = int.Parse(rangeParts[0]);
                        int endStage = int.Parse(rangeParts[1]);

                        for (int stage = startStage; stage <= endStage; stage++)
                        {
                            var place = new Place
                            {
                                Stage = stage,
                                Side = side,
                                LogoPath = file
                            };

                            AddPlaceToDatabase(place);
                        }
                    }
                    else
                    {
                        int stage = int.Parse(stagesPart);

                        var place = new Place
                        {
                            Stage = stage,
                            Side = side,
                            LogoPath = file
                        };

                        AddPlaceToDatabase(place);
                    }
                }
            }
        }

        private void AddPlaceToDatabase(Place place)
        {
            var data = new Dictionary<string, object>
            {
                { "Stage", place.Stage },
                { "Side", place.Side },
                { "LogoPath", place.LogoPath }
            };

           _DBadapter.AddData("Place", data);
        }
    }
}

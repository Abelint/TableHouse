using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        DBadapter _DBadapter = null;

        bool status = true; // true компании / false этажи
        public PageAdmin()
        {
            InitializeComponent();

            this.Background = AdapterClass.BackgroundImage;
           

            _DBadapter = new DBadapter();
            _DBadapter.Init();

           
        
                lvCompaniesAdmin.ItemsSource = AdapterClass.inHouse;

            tbSearchAdmin.TextChanged += TbSearchAdmin_TextChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            status = !status;

            if (status)
            {
                btnSwitch.Content = "Редактирование этажей ";
                lvCompaniesAdmin.ItemsSource = AdapterClass.inHouse;
            }
            else
            {
                btnSwitch.Content = "Редактирование организаций ";
                lvCompaniesAdmin.ItemsSource = AdapterClass.Places;
            }

         
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (status)
            {
                var addCompanyWindow = new AddCompanyWindow();
                if (addCompanyWindow.ShowDialog() == true)
                {
                    // Добавление новой компании в список
                    var newCompany = new CompanyInHouse
                    {
                        Stage = addCompanyWindow.Floor,
                        Side = addCompanyWindow.Side,
                        Office = addCompanyWindow.Office,
                        Name = addCompanyWindow.Name,
                        LogoPath = addCompanyWindow.LogoPath
                    };
                    var data = new Dictionary<string, object>
                {
                    { "Stage", newCompany.Stage },
                    { "Side", newCompany.Side },
                    {"Office", newCompany.Office },
                    { "Name", newCompany.Name },
                    { "LogoPath", newCompany.LogoPath }
                };
                    _DBadapter.AddData("CompanyInHouse", data);
                    AdapterClass.inHouse.Add(newCompany);
                }
            }
            else
            {
                var addStage = new AddFloor();
                if (addStage.ShowDialog() == true)
                {
                    // Добавление нового этажа в список
                    var newPlace = new Place
                    {
                        Stage = addStage.Floor,
                        Side = addStage.Side,                      
                        LogoPath = addStage.LogoPath
                    };
                    var data = new Dictionary<string, object>
                {
                    { "Stage", newPlace.Stage },
                    { "Side", newPlace.Side },                   
                    { "LogoPath", newPlace.LogoPath }
                };
                    _DBadapter.AddData("Place", data);
                    AdapterClass.Places.Add(newPlace);
                }
            }
           
        }
        private void TbSearchAdmin_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Фильтруем _inHouse на основе содержимого tbSearch
            FilterCompanies();
        }
        private void FilterCompanies()
        {
            // Получаем текст из tbSearch
            string searchText = tbSearchAdmin.Text.ToLower();

            // Фильтруем _inHouse, оставляя только компании, где Name содержит searchText
            var infilt = AdapterClass.inHouse.Where(c => c.Name.ToLower().Contains(searchText)).ToList();

            // Обновляем привязку данных
            lvCompaniesAdmin.ItemsSource = infilt;

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void lvCompaniesAdmin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (lvCompaniesAdmin.SelectedItem is CompanyInHouse selectedCompany)
            {
                ChangeCompany changeCompanyWindow = new ChangeCompany(selectedCompany);
                changeCompanyWindow.ShowDialog();
                lvCompaniesAdmin.ItemsSource = null;
                lvCompaniesAdmin.ItemsSource = AdapterClass.inHouse;
            }
            if (lvCompaniesAdmin.SelectedItem is Place selected)
            {
                ChangeFloor changeCompanyWindow = new ChangeFloor(selected);
                changeCompanyWindow.ShowDialog();
                lvCompaniesAdmin.ItemsSource = null;
                lvCompaniesAdmin.ItemsSource = AdapterClass.Places;
            }
        }
    }
}

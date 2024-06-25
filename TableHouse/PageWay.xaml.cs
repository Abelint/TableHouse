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
            // Пример: Отобразим данные на странице
            // Допустим, у нас есть TextBlock с именем tbInfo
            tbInfo.Text = $"Этаж: {Company.Num}, Название: {Company.Name}";
        }
    }
}

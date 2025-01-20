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

namespace Шарафутдинов41размер
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        List<Product> CurrentPageList = new List<Product>();
        List<Product> TableList;
        public ProductPage()
        {
            InitializeComponent();
            var currentProducts = Шарафутдинов41размерEntities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentProducts = Шарафутдинов41размерEntities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts.ToList();

            ProductListView.ItemsSource = currentProducts;

            TableList = currentProducts;
        }
        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}

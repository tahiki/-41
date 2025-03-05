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
        int CountPage;

        int CurrentCountPage;
        public ProductPage(User user)
        {
            InitializeComponent();
            if (user == null)
            {
                FIOTB.Text = "Гость";
                RoleTB.Text = "Гость";
            }
            else
            {
                FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                switch (user.UserRole)
                {
                    case 1:
                        RoleTB.Text = "Клиент"; break;
                    case 2:
                        RoleTB.Text = "Менеджер"; break;
                    case 3:
                        RoleTB.Text = "Администратор"; break;
                }
            }
            var currentProducts = Шарафутдинов41размерEntities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
            CurrentCountPage = currentProducts.Count;
            EveryPages.Text = CurrentCountPage.ToString();
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentProducts = Шарафутдинов41размерEntities.GetContext().Product.ToList();
            if (DiscountSort.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) < 10)).ToList();
            }
            if (DiscountSort.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 10 && Convert.ToInt32(p.ProductDiscountAmount) < 15)).ToList();
            }
            if (DiscountSort.SelectedIndex == 3)
            {
                currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 15 && Convert.ToInt32(p.ProductDiscountAmount) <= 100)).ToList();
            }

            currentProducts = currentProducts.Where(p => p.ProductDescription.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (RadioButton_1.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }
            if (RadioButton_2.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }
            ProductListView.ItemsSource = currentProducts.ToList();
            TableList = currentProducts;
            ChangeText();
        }


        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void DiscountSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ChangeText()
        {
            CurrentPageList.Clear();
            CountPage = TableList.Count;
            currentPages.Text = CountPage.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

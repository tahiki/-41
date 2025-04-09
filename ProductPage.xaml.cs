﻿using System;
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
using Шарафутдинов41размер;

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

        List<Product> selectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();


        int newOrderId;

        User _currentUser;
        private int EveryPage { get; set; }
        public ProductPage(User UserIn)
        {
            InitializeComponent();
            _currentUser = UserIn;
            Manager.OrderBtn = OrderBtn;
            OrderBtn.Visibility = Visibility.Hidden;
            if (UserIn != null)
            {
                NameUserTB.Text = UserIn.UserSurname + " " + UserIn.UserName + " " + UserIn.UserPatronymic + " ";
                switch (UserIn.UserRole)
                {
                    case 1:
                        RoleUserTB.Text = "Клиент";
                        break;
                    case 2:
                        RoleUserTB.Text = "Менеджер";
                        break;
                    case 3:
                        RoleUserTB.Text = "Администратор";
                        break;
                    default:
                        RoleUserTB.Text = "Гость";
                        break;
                }
            }
            else
            {
                NameUserTB.Text = "Гость";
                RoleUserTB.Text = "Гость";
            }



            var currentProducts = Шарафутдинов41размерEntities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
            ComboBoxFilter.SelectedIndex = 0;
            CurrentCountPage = TableList.Count;
            EveryPages.Text = CurrentCountPage.ToString();
            UpdateProduct();

        }

        private void UpdateProduct()
        {
            var currentProducts = Шарафутдинов41размерEntities.GetContext().Product.ToList();

            if (ComboBoxFilter.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount <= 9.99).ToList();
            }
            if (ComboBoxFilter.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount > 10 && p.ProductDiscountAmount < 14.99).ToList();
            }
            if (ComboBoxFilter.SelectedIndex == 3)
            {
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 15).ToList();
            }



            if (RButtonBiggist.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }

            if (RbutttonSmallist.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }

            currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();


            ProductListView.ItemsSource = currentProducts.ToList();
            ProductListView.ItemsSource = currentProducts;
            TableList = currentProducts;
            ChangeText();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();

        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void RButtonBiggist_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void RbutttonSmallist_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void ChangeText()
        {
            CurrentPageList.Clear();
            CountPage = TableList.Count;
            currentPages.Text = CountPage.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {


            if (ProductListView.SelectedIndex >= 0)
            {
                var prod = ProductListView.SelectedItem as Product;

                //int newOrderID = selectedOrderProducts.Last().Order.OrderID;
                var newOrderProd = new OrderProduct();//новый заказ

                //номер продукта в новую запись
                newOrderProd.ProductArticleNumber = prod.ProductArticleNumber;
                newOrderProd.ProductCount = 1;

                //проверии есть ли уже такой заказ
                var selOP = selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, prod.ProductArticleNumber));
                //MessageBox.Show(selOP.Count().ToString());
                if (selOP.Count() == 0)
                {
                    //MessageBox.Show(newOrderProd. OrderID.ToString() + " " + newOrderProd.ProductArticleNumber.ToString() + " " + newOrderProd.Quantity.ToString());
                    selectedOrderProducts.Add(newOrderProd);
                    selectedProducts.Add(prod);
                    //MessageBox.Shom("колво в selecteOP = " + selectedOrderProducts.Count().ToString());
                }
                else
                {
                    foreach (OrderProduct p in selectedOrderProducts)
                    {
                        if (p.ProductArticleNumber == prod.ProductArticleNumber)
                            p.ProductCount++;
                        //MessageBox.Show("колво = " + p.Quantity.ToString());
                    }
                }

                OrderBtn.Visibility = Visibility.Visible;
                ProductListView.SelectedIndex = -1;
            }

        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            selectedProducts = selectedProducts.Distinct().ToList();

            // Добавьте этот код для инициализации Quantity в Product
            foreach (var product in selectedProducts)
            {
                // Находим соответствующий OrderProduct для текущего Product
                var orderProduct = selectedOrderProducts.FirstOrDefault(op =>
                    op.ProductArticleNumber == product.ProductArticleNumber);

                if (orderProduct != null)
                {
                    // Устанавливаем Quantity в Product на основе ProductCount из OrderProduct
                    product.Quantity = orderProduct.ProductCount;
                }
                else
                {
                    // Если OrderProduct не найден (хотя это маловероятно), устанавливаем значение по умолчанию
                    product.Quantity = 1;
                }
            }

            OrderWindow orderWindow = new OrderWindow(selectedOrderProducts, selectedProducts, _currentUser);
            bool? result = orderWindow.ShowDialog();

            // Если заказ успешно сохранен (DialogResult = true)
            if (result == true)
            {
                selectedProducts.Clear();
                selectedOrderProducts.Clear();
                ProductListView.Items.Refresh(); // Обновить отображение списка
            }

            // Обновить видимость кнопки
            OrderBtn.Visibility = selectedProducts.Any() ? Visibility.Visible : Visibility.Hidden;

            //orderWindow.ShowDialog();

            // После закрытия окна:
            if (selectedProducts.Count == 0)
            {
                OrderBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                OrderBtn.Visibility = Visibility.Visible;
            }
        }


    }
}
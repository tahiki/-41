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
using System.Windows.Shapes;

namespace Шарафутдинов41размер
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        List<Product> selectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        private OrderProduct currentOrderProduct = new OrderProduct();

        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, string FIO)
        {
            InitializeComponent();
            var currentPickups = Шарафутдинов41размерEntities.GetContext().PickUpPoint.ToList();
            PickUPCombo.ItemsSource = currentPickups;

            FIOTB.Text = FIO;
            OrderNumber.Text = selectedOrderProducts.First().OrderID.ToString();

            OrderListView.ItemsSource = selectedProducts;
            foreach (Product p in selectedProducts)
            {
                p.ProductQuantityInStock = 1;
                foreach (OrderProduct q in selectedOrderProducts)
                {
                    if (p.ProductArticleNumber == q.ProductArticleNumber)
                        p.ProductQuantityInStock = q.ProductCount;
                }
            }
            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            OrderTB.Text = DateTime.Now.ToString();
            SetDeliveryDate();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.ClientsFIO = ClientTB.Text;
        }

    private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

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

namespace Sales_Data_Control_LINQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NorthwindDataContext ctx;
        public MainWindow()
        {
            ctx = new NorthwindDataContext();
            InitializeComponent();
            var q = from o in ctx.Orders select o;
            dg_Orders.ItemsSource = q;

            SaleByCountry();
        }

        public void LoadOrderDetail()
        {
            Order order = dg_Orders.SelectedItem as Order;
            var q_OrderDetail = from od in ctx.Order_Details
                                where order.OrderID == od.OrderID
                                from p in ctx.Products
                                where p.ProductID == od.ProductID
                                select new
                                {
                                    od.ProductID,
                                    p.ProductName,
                                    od.Quantity,
                                    od.UnitPrice
                                };
            dg_OrderDetail.ItemsSource = q_OrderDetail;
        }

        public void SaleByCountry()
        {
            var q_SaleTotal = from c in ctx.Customers
                              join o in ctx.Orders on c.CustomerID equals o.CustomerID
                              join od in ctx.Order_Details on o.OrderID equals od.OrderID
                              group c by c.Country into Country
                              select new
                              {
                                  Country,
                                  TotalAmount = Country.Sum()
                              };
            dg_SaleByCountry.ItemsSource = q_SaleTotal;
        }

        private void dg_Orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadOrderDetail();
        }
    }
}

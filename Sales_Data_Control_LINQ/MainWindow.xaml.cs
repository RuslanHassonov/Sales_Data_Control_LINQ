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
            var q_SaleTotal = from od in ctx.Order_Details
                              group od by od.Order.ShipCountry into odByCountry
                              orderby odByCountry.Sum(s => s.Quantity * s.UnitPrice) descending
                              select new CountryInfo
                              {
                                  Country = odByCountry.Key,
                                  Total = odByCountry.Sum(s => s.Quantity * s.UnitPrice)
                              };

            dg_SaleByCountry.ItemsSource = q_SaleTotal;
        }

        public void CustomerByCountry()
        {
            CountryInfo selectedCountry = dg_SaleByCountry.SelectedItem as CountryInfo;
            var q_CustByCount = from od in ctx.Order_Details
                                where od.Order.ShipCountry == selectedCountry.Country
                                group od by od.Order.ShipName into odByCustName
                                orderby odByCustName.Sum(s => s.Quantity * s.UnitPrice) descending
                                select new CustomerInfo
                                {
                                    CustomerName = odByCustName.Key,
                                    Total = odByCustName.Sum(s => s.Quantity * s.UnitPrice)
                                };

            dg_SaleByCustomerForCountry.ItemsSource = q_CustByCount;
        }

        public void OrdersByCustomer()
        {
            CustomerInfo selectedCustomer = dg_SaleByCustomerForCountry.SelectedItem as CustomerInfo;
            var q_OrderByCustomer = from od in ctx.Order_Details
                                    where od.Order.Customer.CompanyName == selectedCustomer.CustomerName
                                    group od by od.Order into odByOrder
                                    select new
                                    {
                                        OrderID = odByOrder.Key.OrderID,
                                        OrderDate = odByOrder.Key.OrderDate,
                                        ShippingDate = odByOrder.Key.ShippedDate,
                                        NameSalesman = odByOrder.Key.Employee.FirstName + " " + odByOrder.Key.Employee.LastName
                                    };
            dg_OrdersByCustomer.ItemsSource = q_OrderByCustomer;
        }

        private void dg_Orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadOrderDetail();
        }

        private void dg_SaleByCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerByCountry();
        }

        private void dg_SaleByCustomerForCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdersByCustomer();
        }
    }
}

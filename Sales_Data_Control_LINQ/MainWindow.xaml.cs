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
        }

        public void LoadOrderDetail()
        {
            var q_OrderDetail = from o in ctx.Orders
                                join 
                                select od;
            dg_OrderDetail.ItemsSource = q_OrderDetail;
        }

        private void dg_Orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadOrderDetail();
        }
    }
}

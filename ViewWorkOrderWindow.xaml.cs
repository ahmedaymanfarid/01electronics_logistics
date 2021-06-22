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
using System.Windows.Navigation;
using _01electronics_erp;

namespace _01electronics_logistics
{
    /// <summary>
    /// Interaction logic for ViewWorkOrderWindow.xaml
    /// </summary>
    public partial class ViewWorkOrderWindow : NavigationWindow
    {
        public ViewWorkOrderWindow()
        {
            InitializeComponent();

            WorkOrder order = new WorkOrder();
            order.InitializeWorkOrderInfo(2, 15);
            WorkOrderBasicInfoPage viewBasicInfo = new WorkOrderBasicInfoPage(order);
            this.NavigationService.Navigate(viewBasicInfo);
        }
    }
}

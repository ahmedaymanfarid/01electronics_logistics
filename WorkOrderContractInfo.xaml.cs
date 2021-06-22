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
using _01electronics_erp;

namespace _01electronics_logistics
{
    /// <summary>
    /// Interaction logic for WorkOrderContractInfo.xaml
    /// </summary>
    public partial class WorkOrderContractInfo : Page
    {
        WorkOrder order;
        public WorkOrderContractInfo(WorkOrder order)
        {
            this.order = order;
            InitializeComponent();
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            contractTypeLabel.Content = order.GetOfferContractType();
        }

        private void OnClickBasicInfo(object sender, RoutedEventArgs e)
        {
            WorkOrderBasicInfoPage page = new WorkOrderBasicInfoPage(this.order);
            this.NavigationService.Navigate(page);
        }

        private void OnClickProductsInfo(object sender, RoutedEventArgs e)
        {
            WorkOrderProductInfoPage page = new WorkOrderProductInfoPage(this.order);
            this.NavigationService.Navigate(page);
        }

        private void OnClickContractInfo(object sender, RoutedEventArgs e)
        {
           
        }

        private void OnClickAdditionalInfo(object sender, RoutedEventArgs e)
        {

        }

    }
}

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
    /// Interaction logic for WorkOrderBasicInfoPage.xaml
    /// </summary>
    public partial class WorkOrderBasicInfoPage : Page
    {
        WorkOrder order;
        public WorkOrderBasicInfoPage(WorkOrder order)
        {
            this.order = order;
            InitializeComponent();
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            orderSerialLabel.Content = order.GetAssigneeId();
            issueDateLabel.Content = order.GetOrderIssueDate();
            salesPersonLabel.Content = order.GetSalesPersonName();
            offerProposerLabel.Content = order.GetOfferProposerName();
            companyNameLabel.Content = order.GetCompanyName();
            contactPersonNameLabel.Content = order.GetContactName();
        }

        private void OnClickBasicInfo(object sender, RoutedEventArgs e)
        {
           
        }

        private void OnClickProductsInfo(object sender, RoutedEventArgs e)
        {
            WorkOrderProductInfoPage page = new WorkOrderProductInfoPage(this.order);
            this.NavigationService.Navigate(page);
        }

        private void OnClickContractInfo(object sender, RoutedEventArgs e)
        {
            WorkOrderContractInfo page = new WorkOrderContractInfo(this.order);
            this.NavigationService.Navigate(page);
        }

        private void OnClickAdditionalInfo(object sender, RoutedEventArgs e)
        {

        }

       
        

    }
}

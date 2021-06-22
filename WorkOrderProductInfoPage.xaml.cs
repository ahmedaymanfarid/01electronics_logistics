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
    /// Interaction logic for WorkOrderProductInfo.xaml
    /// </summary>
    public partial class WorkOrderProductInfoPage : Page
    {
        WorkOrder order;
        public WorkOrderProductInfoPage(WorkOrder order)
        {
            this.order = order;
            InitializeComponent();
            UpdateInfo();
        }
        public void UpdateInfo()
        {
            product1TypeLabel.Content = order.GetOfferProduct1Type();
            product1BrandLabel.Content = order.GetOfferProduct1Brand();
            product1ModelLabel.Content = order.GetOfferProduct1Model();
            product1QuantityLabel.Content = order.GetOfferProduct1Quantity();
            product1PriceLabel.Content = order.GetProduct1PriceValue();

            product2TypeLabel.Content = order.GetOfferProduct1Type();
            product2BrandLabel.Content = order.GetOfferProduct1Brand();
            product2ModelLabel.Content = order.GetOfferProduct1Model();
            product2QuantityLabel.Content = order.GetOfferProduct1Quantity();
            product2PriceLabel.Content = order.GetProduct1PriceValue();

            product3TypeLabel.Content = order.GetOfferProduct1Type();
            product3BrandLabel.Content = order.GetOfferProduct1Brand();
            product3ModelLabel.Content = order.GetOfferProduct1Model();
            product3QuantityLabel.Content = order.GetOfferProduct1Quantity();
            product3PriceLabel.Content = order.GetProduct1PriceValue();

            product4TypeLabel.Content = order.GetOfferProduct1Type();
            product4BrandLabel.Content = order.GetOfferProduct1Brand();
            product4ModelLabel.Content = order.GetOfferProduct1Model();
            product4QuantityLabel.Content = order.GetOfferProduct1Quantity();
            product4PriceLabel.Content = order.GetProduct1PriceValue();
        }
        private void OnClickBasicInfo(object sender, RoutedEventArgs e)
        {
            WorkOrderBasicInfoPage page = new WorkOrderBasicInfoPage(this.order);
            this.NavigationService.Navigate(page);
        }

        private void OnClickProductsInfo(object sender, RoutedEventArgs e)
        {
           
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

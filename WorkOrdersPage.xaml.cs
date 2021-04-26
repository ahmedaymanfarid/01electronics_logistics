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
    /// Interaction logic for WorkOrdersPage.xaml
    /// </summary>
    public partial class WorkOrdersPage : Page
    {
        private List<COMPANY_WORK_MACROS.WORK_ORDER_MAX_STRUCT> listOfOrders = new List<COMPANY_WORK_MACROS.WORK_ORDER_MAX_STRUCT>();

        private StackPanel previousSelectedOrderStackPanel;
        private StackPanel currentSelectedOrderStackPanel;

        private CommonQueries CommonQueriesObject = new CommonQueries();

        private Employee loggedInUser;
        private Employee selectedEmployee;

        public WorkOrdersPage(ref Employee mLoggedInUser)
        {
            loggedInUser = mLoggedInUser;

            InitializeComponent();

            InitializeOrderList();
        }

        private void InitializeOrderList()
        {
            listOfOrders.Clear();
            OrdersStackPanel.Children.Clear();

            if (!CommonQueriesObject.GetWorkOrders(ref listOfOrders))
                return;

            for (int i = 0; i < listOfOrders.Count; i++)
            {
                System.Windows.Controls.StackPanel selectedOrderStackPanel = new StackPanel();
                selectedOrderStackPanel.Height = 80;
                selectedOrderStackPanel.MouseLeftButtonDown += OnMouseLeftButtonDownOrderItem;

                System.Windows.Controls.Label currentEmployeeName = new Label();
                currentEmployeeName.Content = listOfOrders[i].order_id;
                currentEmployeeName.Style = (Style)FindResource("stackPanelItemHeader");

                System.Windows.Controls.Label currentEmployeeDepartment = new Label();
                currentEmployeeDepartment.Content = listOfOrders[i].offer_proposer_name;
                currentEmployeeDepartment.Style = (Style)FindResource("stackPanelItemBody");

                System.Windows.Controls.Label currentEmployeeTeam = new Label();
                currentEmployeeTeam.Content = listOfOrders[i].sales_person_name;
                currentEmployeeTeam.Style = (Style)FindResource("stackPanelItemBody");

                System.Windows.Controls.Label currentEmployeePosition = new Label();
                currentEmployeePosition.Content = listOfOrders[i].order_status;
                currentEmployeePosition.Style = (Style)FindResource("stackPanelItemBody");

                selectedOrderStackPanel.Children.Add(currentEmployeeName);
                selectedOrderStackPanel.Children.Add(currentEmployeeDepartment);
                selectedOrderStackPanel.Children.Add(currentEmployeeTeam);
                selectedOrderStackPanel.Children.Add(currentEmployeePosition);

                OrdersStackPanel.Children.Add(selectedOrderStackPanel);
            }
        }

        private void OnMouseLeftButtonDownOrderItem(object sender, RoutedEventArgs e)
        {
            previousSelectedOrderStackPanel = currentSelectedOrderStackPanel;
            currentSelectedOrderStackPanel = (StackPanel)sender;
            BrushConverter brush = new BrushConverter();

            if (previousSelectedOrderStackPanel != null)
            {
                previousSelectedOrderStackPanel.Background = (Brush)brush.ConvertFrom("#FFFFFF");

                foreach (Label childLabel in previousSelectedOrderStackPanel.Children)
                    childLabel.Foreground = (Brush)brush.ConvertFrom("#000000");
            }

            currentSelectedOrderStackPanel.Background = (Brush)brush.ConvertFrom("#105A97");

            foreach (Label childLabel in currentSelectedOrderStackPanel.Children)
                childLabel.Foreground = (Brush)brush.ConvertFrom("#FFFFFF");
        }

        private void OnButtonClickedMyProfile(object sender, RoutedEventArgs e)
        {
            UserPortalPage userPortal = new UserPortalPage(ref loggedInUser);
            this.NavigationService.Navigate(userPortal);
        }
        private void OnButtonClickedWorkOrders(object sender, RoutedEventArgs e)
        {
            WorkOrdersPage workOrdersPage = new WorkOrdersPage(ref loggedInUser);
            this.NavigationService.Navigate(workOrdersPage);
        }
    }

    
}

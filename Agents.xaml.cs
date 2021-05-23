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
    /// Interaction logic for Agents.xaml
    /// </summary>
    public partial class Agents : Page
    {
        private Employee loggedInUser;

        private List<AGENT_MACROS.AGENT_STRUCT> listOfAgents = new List<AGENT_MACROS.AGENT_STRUCT>();

        private LogisticsQueries LogisticsQueryObject = new LogisticsQueries();

        private StackPanel previousSelectedOrderStackPanel;
        private StackPanel currentSelectedOrderStackPanel;

        public Agents(ref Employee mLoggedInUser)
        {
            loggedInUser = mLoggedInUser;

            InitializeComponent();

            InitializeList();
        }

        private void InitializeList()
        {

            listOfAgents.Clear();

            if (!LogisticsQueryObject.GetAgentsSerialsAndNames(ref listOfAgents))
                return;

            OrdersStackPanel.Children.Clear();
            for (int i = 0; i < listOfAgents.Count; i++)
            {
                System.Windows.Controls.StackPanel selectedOrderStackPanel = new StackPanel();
                selectedOrderStackPanel.Height = 80;
                selectedOrderStackPanel.MouseLeftButtonDown += OnMouseLeftButtonDownOrderItem;

                System.Windows.Controls.Label currentEmployeeName = new Label();
                currentEmployeeName.Content = listOfAgents[i].agent_serial;
                currentEmployeeName.Style = (Style)FindResource("stackPanelItemHeader");

                System.Windows.Controls.Label currentEmployeeDepartment = new Label();
                currentEmployeeDepartment.Content = listOfAgents[i].agent_name;
                currentEmployeeDepartment.Style = (Style)FindResource("stackPanelItemBody");

                selectedOrderStackPanel.Children.Add(currentEmployeeName);
                selectedOrderStackPanel.Children.Add(currentEmployeeDepartment);

                OrdersStackPanel.Children.Add(selectedOrderStackPanel);
            }
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
        private void OnButtonClickedAgents(object sender, RoutedEventArgs e)
        {
            Agents agentsPage = new Agents(ref loggedInUser);
            this.NavigationService.Navigate(agentsPage);
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

        private void OnAdd(object sender,RoutedEventArgs e)
        {
            InitializeList();
        }
        private void OnModify(object sender, RoutedEventArgs e)
        {

        }
        private void OnDelete(object sender, RoutedEventArgs e)
        {

        }
    }
}

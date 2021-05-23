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

        private StackPanel previousSelectedAgentStackPanel;
        private StackPanel currentSelectedAgentStackPanel;

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

            AgentsStackPanel.Children.Clear();
            for (int i = 0; i < listOfAgents.Count; i++)
            {
                System.Windows.Controls.StackPanel selectedAgentStackPanel = new StackPanel();
                selectedAgentStackPanel.Height = 80;
                selectedAgentStackPanel.MouseLeftButtonDown += OnMouseLeftButtonDownOrderItem;

                System.Windows.Controls.Label currentEmployeeName = new Label();
                currentEmployeeName.Content = listOfAgents[i].agent_serial;
                currentEmployeeName.Style = (Style)FindResource("stackPanelItemHeader");

                System.Windows.Controls.Label currentEmployeeDepartment = new Label();
                currentEmployeeDepartment.Content = listOfAgents[i].agent_name;
                currentEmployeeDepartment.Style = (Style)FindResource("stackPanelItemBody");

                selectedAgentStackPanel.Children.Add(currentEmployeeName);
                selectedAgentStackPanel.Children.Add(currentEmployeeDepartment);

                AgentsStackPanel.Children.Add(selectedAgentStackPanel);
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
            previousSelectedAgentStackPanel = currentSelectedAgentStackPanel;
            currentSelectedAgentStackPanel = (StackPanel)sender;
            BrushConverter brush = new BrushConverter();

            if (previousSelectedAgentStackPanel != null)
            {
                previousSelectedAgentStackPanel.Background = (Brush)brush.ConvertFrom("#FFFFFF");

                foreach (Label childLabel in previousSelectedAgentStackPanel.Children)
                    childLabel.Foreground = (Brush)brush.ConvertFrom("#000000");
            }

            currentSelectedAgentStackPanel.Background = (Brush)brush.ConvertFrom("#105A97");

            foreach (Label childLabel in currentSelectedAgentStackPanel.Children)
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
            if(currentSelectedAgentStackPanel != null)
            {
                Label myLa = new Label();
                myLa.Content = Int32.Parse(currentSelectedAgentStackPanel.Children[0].ToString());
                AgentsStackPanel.Children.Add(myLa);
            }
            
            //            String deleteQuery = "delete from agent_field_of_work where agent_serial = " + currentSelectedAgentStackPanel +
            //"delete from agent_telephone where branch_serial = " + currentSelectedAgentStackPanel +
            //"delete from agent_fax where branch_serial = " + currentSelectedAgentStackPanel +
            //"delete from agent_address where agent_serial = " + currentSelectedAgentStackPanel +
            //"delete from agent_name where agent_serial = " + currentSelectedAgentStackPanel;
        }
    }
}

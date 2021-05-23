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
    /// Interaction logic for FreightAgents.xaml
    /// </summary>
    public partial class FreightAgentsPage : Page
    {
        private Employee loggedInUser;

        private List<AGENT_MACROS.AGENT_STRUCT> listOfAgents = new List<AGENT_MACROS.AGENT_STRUCT>();

        private LogisticsQueries LogisticsQueryObject = new LogisticsQueries();

        private StackPanel previousSelectedAgentStackPanel;
        private StackPanel currentSelectedAgentStackPanel;

        public FreightAgentsPage(ref Employee mLoggedInUser)
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
            
        }


        private void OnBtnClickedAdd(object sender,RoutedEventArgs e)
        {
            InitializeList();
        }
        private void OnBtnClickedView(object sender, RoutedEventArgs e)
        {

        }
        
    }
}

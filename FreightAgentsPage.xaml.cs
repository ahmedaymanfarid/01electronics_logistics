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
        private CommonQueries CommonQueriesObject = new CommonQueries();

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
            for (int i = 0; i < listOfAgents.Count; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = listOfAgents[i].agent_name;
                agentsTree.Items.Add(item);
            }
            List<BASIC_STRUCTS.COUNTRY_STRUCT> countries = new List<BASIC_STRUCTS.COUNTRY_STRUCT>();
            CommonQueriesObject.GetAllCountries(ref countries);
            for (int i = 0; i < countries.Count; i++)
                countriesCombo.Items.Add(countries[i].country_name);
        }

        /***********************************************************************************
        ***********************************************************************************
        *********************************CheckBox Listeners ********************************
        ***********************************************************************************
        ***********************************************************************************/
        private void OnCountriesCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                countriesCombo.IsEnabled = true;
            else
            {
                countriesCombo.SelectedIndex = -1;
                countriesCombo.IsEnabled = false;
            }
        }
        private void OnStateCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                statesCombo.IsEnabled = true;
            else
            {
                statesCombo.SelectedIndex = -1;
                statesCombo.IsEnabled = false;
            }
        }
        private void OnCityCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                citiesCombo.IsEnabled = true;
            else
            {
                citiesCombo.SelectedIndex = -1;
                citiesCombo.IsEnabled = false;
            }
        }
        private void OnDistrictCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                districtsCombo.IsEnabled = true;
            else
            {
                districtsCombo.SelectedIndex = -1;
                districtsCombo.IsEnabled = false;
            }
        }

        /***********************************************************************************
        ***********************************************************************************
        *********************************ComboBox Listeners ********************************
        ***********************************************************************************
        ***********************************************************************************/
        private void OnCountriesComboBoxClick(object sender, RoutedEventArgs e)
        {
            statesCombo.Items.Clear();
            List<BASIC_STRUCTS.STATE_STRUCT> states = new List<BASIC_STRUCTS.STATE_STRUCT>();
            CommonQueriesObject.GetAllCountryStates(countriesCombo.SelectedIndex, ref states);
            for (int i = 0; i < states.Count; i++)
                statesCombo.Items.Add(states[i].state_name);
        }

        private void OnStatesComboBoxClick(object sender, RoutedEventArgs e)
        {
            citiesCombo.Items.Clear();
            List<BASIC_STRUCTS.CITY_STRUCT> cities = new List<BASIC_STRUCTS.CITY_STRUCT>();
            CommonQueriesObject.GetAllStateCities(4900+statesCombo.SelectedIndex, ref cities);
            for (int i = 0; i < cities.Count; i++)
                citiesCombo.Items.Add(cities[i].city_name);
        }

        private void OnCitiesComboBoxClick(object sender, RoutedEventArgs e)
        {
            districtsCombo.Items.Clear();
            List<BASIC_STRUCTS.DISTRICT_STRUCT> districts = new List<BASIC_STRUCTS.DISTRICT_STRUCT>();
            int cityId = ((4900 + statesCombo.SelectedIndex)*100)+citiesCombo.SelectedIndex;
            CommonQueriesObject.GetAllCityDistricts(cityId, ref districts);
            for (int i = 0; i < districts.Count; i++)
                districtsCombo.Items.Add(districts[i].district_name);
        }
        /***********************************************************************************
        ***********************************************************************************
        *********************************Buttons OnClick Listeners ************************
        ***********************************************************************************
        ***********************************************************************************/
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


        private void OnBtnClickedAdd(object sender, RoutedEventArgs e)
        {
            
        }
        private void OnBtnClickedView(object sender, RoutedEventArgs e)
        {
            //ViewAgent va = new ViewAgent(agentsTreeagentsTree.SelectedItem);
        }

    }
}

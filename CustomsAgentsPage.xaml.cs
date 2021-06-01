﻿using System;
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
    /// Interaction logic for CustomsAgentsPage.xaml
    /// </summary>
    public partial class CustomsAgentsPage : Page
    {
        private Employee loggedInUser;

        private List<AGENCY_MACROS.AGENCY_STRUCT> listOfAgents = new List<AGENCY_MACROS.AGENCY_STRUCT>();
        private List<AGENCY_MACROS.AGENCY_FULL_INFO> listOfFullInfoAgents =
            new List<AGENCY_MACROS.AGENCY_FULL_INFO>();

        private LogisticsQueries LogisticsQueryObject = new LogisticsQueries();
        private CommonQueries CommonQueriesObject = new CommonQueries();
        public CustomsAgentsPage(ref Employee mLoggedInUser)
        {
            loggedInUser = mLoggedInUser;

            InitializeComponent();

            InitializeLists();
            UpdateTree();

            // Initialize countries combo box
            List<BASIC_STRUCTS.COUNTRY_STRUCT> countries = new List<BASIC_STRUCTS.COUNTRY_STRUCT>();
            CommonQueriesObject.GetAllCountries(ref countries);
            for (int i = 0; i < countries.Count; i++)
                countriesCombo.Items.Add(countries[i].country_name);
        }

        private void InitializeLists()
        {
            listOfAgents.Clear();
            listOfFullInfoAgents.Clear();

            if (!LogisticsQueryObject.GetCustomsAgentsSerialsAndNames(ref listOfAgents))
                return;
            for (int i = 0; i < listOfAgents.Count; i++)
            {
                // To get full info about each agent
                AGENCY_MACROS.AGENCY_FULL_INFO tempAgent = new AGENCY_MACROS.AGENCY_FULL_INFO();
                LogisticsQueryObject.GetFullCustomsAgentInfo(listOfAgents[i].agency_serial, ref tempAgent);
                listOfFullInfoAgents.Add(tempAgent);
            }
        }

        private void UpdateTree()
        {
            agentsTree.Items.Clear();
            for (int i = 0; i < listOfAgents.Count; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = listOfAgents[i].agency_name;
                agentsTree.Items.Add(item);
            }
        }

        /***********************************************************************************
        ***********************************************************************************
        *********************************CheckBox Listeners ********************************
        ***********************************************************************************
        ***********************************************************************************/
        private void OnCountriesCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                countriesCombo.IsEnabled = true;
                statesCheckBox.IsEnabled = true;
            }
            else
            {
                countriesCombo.SelectedIndex = -1;
                countriesCombo.IsEnabled = false;

                statesCombo.SelectedIndex = -1;
                statesCombo.IsEnabled = false;
                statesCheckBox.IsEnabled = false;
                statesCheckBox.IsChecked = false;

                citiesCombo.SelectedIndex = -1;
                citiesCombo.IsEnabled = false;
                citiesCheckBox.IsEnabled = false;
                citiesCheckBox.IsChecked = false;

                districtsCombo.SelectedIndex = -1;
                districtsCombo.IsEnabled = false;
                districtCheckBox.IsEnabled = false;
                districtCheckBox.IsChecked = false;

                InitializeLists();
                UpdateTree();
            }
        }
        private void OnStateCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                statesCombo.IsEnabled = true;
                citiesCheckBox.IsEnabled = true;
            }

            else
            {
                statesCombo.SelectedIndex = -1;
                statesCombo.IsEnabled = false;

                citiesCombo.SelectedIndex = -1;
                citiesCombo.IsEnabled = false;
                citiesCheckBox.IsEnabled = false;
                citiesCheckBox.IsChecked = false;

                districtsCombo.SelectedIndex = -1;
                districtsCombo.IsEnabled = false;
                districtCheckBox.IsEnabled = false;
                districtCheckBox.IsChecked = false;

                CountriesComboBoxFunction();
            }
        }
        private void OnCityCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                citiesCombo.IsEnabled = true;
                districtCheckBox.IsEnabled = true;
            }
            else
            {
                citiesCombo.SelectedIndex = -1;
                citiesCombo.IsEnabled = false;

                districtsCombo.SelectedIndex = -1;
                districtsCombo.IsEnabled = false;
                districtCheckBox.IsEnabled = false;
                districtCheckBox.IsChecked = false;

                StatesComboBoxFunction();
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

                CitiesComboBoxFunction();
            }
        }

        /***********************************************************************************
        ***********************************************************************************
        *********************************ComboBox Listeners ********************************
        ***********************************************************************************
        ***********************************************************************************/
        private void OnCountriesComboBoxClick(object sender, RoutedEventArgs e)
        {
            CountriesComboBoxFunction();
        }
        private void CountriesComboBoxFunction()
        {
            InitializeLists();

            statesCombo.Items.Clear();
            List<BASIC_STRUCTS.STATE_STRUCT> states = new List<BASIC_STRUCTS.STATE_STRUCT>();
            CommonQueriesObject.GetAllCountryStates(countriesCombo.SelectedIndex, ref states);
            for (int i = 0; i < states.Count; i++)
                statesCombo.Items.Add(states[i].state_name);

            for (int i = 0; i < listOfFullInfoAgents.Count; i++)
            {
                int j;
                for (j = 0; j < listOfFullInfoAgents[i].branches.Count; j++)
                    if ((listOfFullInfoAgents[i].branches[j].addressId / 1000000 == countriesCombo.SelectedIndex))
                        break;

                if (j == listOfFullInfoAgents[i].branches.Count)
                {
                    listOfAgents.RemoveAt(i);
                    listOfFullInfoAgents.RemoveAt(i);
                    i--;
                }
            }

            UpdateTree();
        }

        private void OnStatesComboBoxClick(object sender, RoutedEventArgs e)
        {
            StatesComboBoxFunction();
        }
        private void StatesComboBoxFunction()
        {
            InitializeLists();

            citiesCombo.Items.Clear();
            List<BASIC_STRUCTS.CITY_STRUCT> cities = new List<BASIC_STRUCTS.CITY_STRUCT>();
            CommonQueriesObject.GetAllStateCities(countriesCombo.SelectedIndex * 100
                + statesCombo.SelectedIndex, ref cities);
            for (int i = 0; i < cities.Count; i++)
                citiesCombo.Items.Add(cities[i].city_name);

            for (int i = 0; i < listOfFullInfoAgents.Count; i++)
            {
                int j;
                for (j = 0; j < listOfFullInfoAgents[i].branches.Count; j++)
                    if ((listOfFullInfoAgents[i].branches[j].addressId / 10000 % 100 == statesCombo.SelectedIndex
                    && listOfFullInfoAgents[i].branches[j].addressId / 1000000 == countriesCombo.SelectedIndex))
                        break;
                if (j == listOfFullInfoAgents[i].branches.Count)
                {
                    listOfAgents.RemoveAt(i);
                    listOfFullInfoAgents.RemoveAt(i);
                    i--;
                }
            }


            UpdateTree();
        }
        private void OnCitiesComboBoxClick(object sender, RoutedEventArgs e)
        {
            CitiesComboBoxFunction();
        }
        private void CitiesComboBoxFunction()
        {
            InitializeLists();

            districtsCombo.Items.Clear();
            List<BASIC_STRUCTS.DISTRICT_STRUCT> districts = new List<BASIC_STRUCTS.DISTRICT_STRUCT>();
            int cityId = ((countriesCombo.SelectedIndex * 100 + statesCombo.SelectedIndex) * 100)
                + citiesCombo.SelectedIndex;
            CommonQueriesObject.GetAllCityDistricts(cityId, ref districts);
            for (int i = 0; i < districts.Count; i++)
                districtsCombo.Items.Add(districts[i].district_name);

            for (int i = 0; i < listOfFullInfoAgents.Count; i++)
            {
                int j;
                for (j = 0; j < listOfFullInfoAgents[i].branches.Count; j++)
                    if ((listOfFullInfoAgents[i].branches[j].addressId / 100 % 100 == citiesCombo.SelectedIndex
                    && listOfFullInfoAgents[i].branches[j].addressId / 10000 % 100 == statesCombo.SelectedIndex
                    && listOfFullInfoAgents[i].branches[j].addressId / 1000000 == countriesCombo.SelectedIndex))
                        break;
                if (j == listOfFullInfoAgents[i].branches.Count)
                {
                    listOfAgents.RemoveAt(i);
                    listOfFullInfoAgents.RemoveAt(i);
                    i--;
                }
            }

            UpdateTree();
        }

        private void OnDistrictsComboBoxClick(object send, RoutedEventArgs e)
        {
            if (districtsCombo.SelectedItem == null)
                return;
            InitializeLists();

            for (int i = 0; i < listOfFullInfoAgents.Count; i++)
            {
                int j;
                for (j = 0; j < listOfFullInfoAgents[i].branches.Count; j++)
                {
                    int districtId = LogisticsQueryObject.GetDistrictAddress(districtsCombo.SelectedItem.ToString());
                    if ((listOfFullInfoAgents[i].branches[j].addressId / 100 % 100 == citiesCombo.SelectedIndex
                        && listOfFullInfoAgents[i].branches[j].addressId / 10000 % 100 == statesCombo.SelectedIndex
                        && listOfFullInfoAgents[i].branches[j].addressId / 1000000 == countriesCombo.SelectedIndex
                        && listOfFullInfoAgents[i].branches[j].addressId % 100 == districtId % 100))
                        break;
                }
                if (j == listOfFullInfoAgents[i].branches.Count)
                {
                    listOfAgents.RemoveAt(i);
                    listOfFullInfoAgents.RemoveAt(i);
                    i--;
                }

            }
            UpdateTree();
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
            FreightAgentsPage agentsPage = new FreightAgentsPage(ref loggedInUser);
            this.NavigationService.Navigate(agentsPage);
        }
        private void OnButtonClickedCustomsAgents(object sender,RoutedEventArgs e)
        {

        }

        private void OnTreeSelectionChange(object sender, RoutedEventArgs e)
        {
            if (agentsTree.SelectedItem == null)
                viewButton.IsEnabled = false;
            else
                viewButton.IsEnabled = true;
        }
        private void OnBtnClickedAdd(object sender, RoutedEventArgs e)
        {
            // -1 is an indication that there is no agent passed therefore the ViewAgent constructor
            // acts as a page to create a new agent
            AgencyProfileWindow va = new AgencyProfileWindow(-1);
            va.Show();
            va.Closed += (sender1, e1) =>
            {
                InitializeWholeView();
            };
        }
        private void OnBtnClickedView(object sender, RoutedEventArgs e)
        {
            if (agentsTree.SelectedItem == null)
                return;
            int agentSerial = listOfAgents[agentsTree.Items.IndexOf(agentsTree.SelectedItem)].agency_serial;
            CustomsAgencyProfileWindow va = new CustomsAgencyProfileWindow(agentSerial);
            va.Show();
            va.Closed += (sender1, e1) =>
            {
                InitializeWholeView();
            };
        }

        private void InitializeWholeView()
        {
            ResetFilters();
            InitializeLists();
            UpdateTree();
        }

        private void ResetFilters()
        {
            countriesCombo.SelectedIndex = -1;
            statesCombo.SelectedIndex = -1;
            statesCombo.IsEnabled = false;
            citiesCombo.SelectedIndex = -1;
            citiesCombo.IsEnabled = false;
            districtsCombo.SelectedIndex = -1;
            districtsCombo.IsEnabled = false;

            countriesCheckBox.IsChecked = false;
            statesCheckBox.IsEnabled = false;
            statesCheckBox.IsChecked = false;
            citiesCheckBox.IsEnabled = false;
            citiesCheckBox.IsChecked = false;
            districtCheckBox.IsEnabled = false;
            districtCheckBox.IsChecked = false;
        }
    }
}

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
using _01electronics_erp;

namespace _01electronics_logistics
{
    /// <summary>
    /// Interaction logic for ViewAgent.xaml
    /// </summary>
    public partial class ViewAgent : Window
    {
        AGENT_MACROS.AGENT_FULL_INFO agent = new AGENT_MACROS.AGENT_FULL_INFO();

        private CommonQueries CommonQueriesObject = new CommonQueries();

        private LogisticsQueries logisticsQueryObject = new LogisticsQueries();

        int agentId;
        public ViewAgent(int agentId)
        {
            InitializeComponent();
            this.agentId = agentId;
            if (agentId != -1)
                logisticsQueryObject.GetFullAgentInfo(agentId, ref agent);
            FulfillFields();

        }
        /***********************************************************************************
        ***********************************************************************************
        *********************************CheckBox Listeners ********************************
        ***********************************************************************************
        ***********************************************************************************/
        private void OnAgentCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                agentNameTextBox.IsEnabled = true;
            else
                agentNameTextBox.IsEnabled = false;
        }
        private void OnEmployeesCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                employeesCombo.IsEnabled = true;
            else
                employeesCombo.IsEnabled = false;
        }
        private void OnCountriesCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                countriesCombo.IsEnabled = true;
            else
                countriesCombo.IsEnabled = false;
        }
        private void OnStateCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                statesCombo.IsEnabled = true;
            else
                statesCombo.IsEnabled = false;
        }
        private void OnCityCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                citiesCombo.IsEnabled = true;
            else
                citiesCombo.IsEnabled = false;
        }
        private void OnDistrictCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                districtsCombo.IsEnabled = true;
            else
                districtsCombo.IsEnabled = false;
        }
        private void OnTelephoneCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                telephoneTextBox.IsEnabled = true;
            else
                telephoneTextBox.IsEnabled = false;
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
            CommonQueriesObject.GetAllStateCities(4900 + statesCombo.SelectedIndex, ref cities);
            for (int i = 0; i < cities.Count; i++)
                citiesCombo.Items.Add(cities[i].city_name);
        }

        private void OnCitiesComboBoxClick(object sender, RoutedEventArgs e)
        {
            districtsCombo.Items.Clear();
            List<BASIC_STRUCTS.DISTRICT_STRUCT> districts = new List<BASIC_STRUCTS.DISTRICT_STRUCT>();
            int cityId = ((4900 + statesCombo.SelectedIndex) * 100) + citiesCombo.SelectedIndex;
            CommonQueriesObject.GetAllCityDistricts(cityId, ref districts);
            for (int i = 0; i < districts.Count; i++)
                districtsCombo.Items.Add(districts[i].district_name);
        }

        /***********************************************************************************
        ***********************************************************************************
        ******************************** Functions ****************************************
        ***********************************************************************************
        ***********************************************************************************/
        private void FulfillFields()
        {
            agentNameTextBox.Text = agent.agent_name;

            // Employee ComboBox
            List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT> marketingEmployees = new List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT>();
            CommonQueriesObject.GetDepartmentEmployees(102, ref marketingEmployees);
            for (int i = 0; i < marketingEmployees.Count; i++)
                employeesCombo.Items.Add(marketingEmployees[i].employee_name);
            employeesCombo.SelectedItem = agent.employee_name;

            // Country,State,City and District
            List<BASIC_STRUCTS.COUNTRY_STRUCT> countries = new List<BASIC_STRUCTS.COUNTRY_STRUCT>();
            CommonQueriesObject.GetAllCountries(ref countries);
            for (int i = 0; i < countries.Count; i++)
                countriesCombo.Items.Add(countries[i].country_name);
            countriesCombo.SelectedItem = agent.country;

            List<BASIC_STRUCTS.STATE_STRUCT> states = new List<BASIC_STRUCTS.STATE_STRUCT>();
            CommonQueriesObject.GetAllCountryStates(countriesCombo.SelectedIndex, ref states);
            for (int i = 0; i < states.Count; i++)
                statesCombo.Items.Add(states[i].state_name);
            statesCombo.IsEnabled = true;
            statesCombo.SelectedItem = agent.state;
            statesCombo.IsEnabled = false;

            List<BASIC_STRUCTS.CITY_STRUCT> cities = new List<BASIC_STRUCTS.CITY_STRUCT>();
            CommonQueriesObject.GetAllStateCities(4900 + statesCombo.SelectedIndex, ref cities);
            for (int i = 0; i < cities.Count; i++)
                citiesCombo.Items.Add(cities[i].city_name);
            citiesCombo.IsEnabled = true;
            citiesCombo.SelectedItem = agent.city;
            citiesCombo.IsEnabled = false;

            List<BASIC_STRUCTS.DISTRICT_STRUCT> districts = new List<BASIC_STRUCTS.DISTRICT_STRUCT>();
            int cityId = ((4900 + statesCombo.SelectedIndex) * 100) + citiesCombo.SelectedIndex;
            CommonQueriesObject.GetAllCityDistricts(cityId, ref districts);
            for (int i = 0; i < districts.Count; i++)
                districtsCombo.Items.Add(districts[i].district_name);
            districtsCombo.IsEnabled = true;
            districtsCombo.SelectedItem = agent.district;
            districtsCombo.IsEnabled = false;

            //district.Text = agent.district;
            telephoneTextBox.Text = agent.telephone;
        }

        private void OnDoneButtonClick(object sender, RoutedEventArgs e)
        {
            agent.agent_name = agentNameTextBox.Text;
            agent.employee_name = employeesCombo.Text;
            agent.country = countriesCombo.Text;
            agent.state = statesCombo.Text;
            agent.city = citiesCombo.Text;
            agent.district = districtsCombo.Text;
            agent.telephone = telephoneTextBox.Text;
            agent.addressId = countriesCombo.SelectedIndex * 1000000 + statesCombo.SelectedIndex * 10000 + citiesCombo.SelectedIndex * 100 + districtsCombo.SelectedIndex;

            if (agent.agent_name.Length == 0 || agent.employee_name.Length == 0 || agent.country.Length == 0 || agent.state.Length == 0 || agent.city.Length == 0
                || agent.district.Length == 0 || agent.telephone.Length == 0 || agent.addressId == 0)
                return;

            if (this.agentId != -1)
            {
                if (!logisticsQueryObject.UpdateAgent(agentId, ref agent)) ;
                // show error message
                else
                    this.Close();
                return;
            }

            if (!logisticsQueryObject.AddAgent(ref agent)) ;
            // show error message
            else
                this.Close();
        }

    }
}

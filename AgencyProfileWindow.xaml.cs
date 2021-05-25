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
using System.Windows.Shapes;
using _01electronics_erp;

namespace _01electronics_logistics
{
    /// <summary>
    /// Interaction logic for AgencyProfileWindow.xaml
    /// </summary>
    public partial class AgencyProfileWindow : Window
    {
        AGENT_MACROS.AGENT_FULL_INFO agency = new AGENT_MACROS.AGENT_FULL_INFO();

        private CommonQueries CommonQueriesObject = new CommonQueries();

        private LogisticsQueries logisticsQueryObject = new LogisticsQueries();

        protected bool agencyNameEdited;

        //THIS TEXTBOX SHALL REPLACE THE LABEL WHEN DOUBLE CLICKED
        private TextBox agencyNameTextBox;

        //YOU SHOULD DESIGN AN AGENCY CLASS
        //AND AGENT CONTACT CLASS
        //YOU THEN SHALL PASS AGENCY CLASS OBJECT REFERENCE TO THIS CLASS CONSTRUCTOR
        //NOT AGENCY SERIAL

        //SAVE CHANGES BUTTON SHALL BE DISABLED UNLESS A CHANGE IS MADE
        //ADD BRANCH BUTTON SHALL OPEN A NEW WINDOW WHERE COUNTRY, STATE, CITY AND DISTRICT COMBOBOXES SHALL BE SELECTED

        //THE STACK PANEL SHALL VIEW ALL WORK ORDERS THIS AGENCY HAS WORKED ON
        //ON SINGLE CLICK ON ANY ITEM, THE BACKGROUND SHALL BE BLUE AND FOREGROUND SHALL BE WHITE
        //ON DOUBLE CLICK ON ANY ITEM, A NEW WINDOW SHALL OPEN THE VIEW ALL DETAILS OF THIS WORK ORDER
        int agencySerial;
        public AgencyProfileWindow(int agencySerial)
        {
            InitializeComponent();
            this.agencySerial = agencySerial;
            if (agencySerial != -1)
                logisticsQueryObject.GetFullAgentInfo(agencySerial, ref agency);
            FulfillFields();

        }

        /////////////////////////////////////////////////////////////////////////////////////
        /// <DOUBLE CLICK HANDLERS>
        ///////////////////////////////////////////////////////////////////////////////////// 
        protected void OnMouseDoubleClickLabelToTextBox(object sender, RoutedEventArgs e)
        {
            Label currentLabel = (Label)sender;
            WrapPanel currentWrapPanel = (WrapPanel)currentLabel.Parent;
            TextBox currentTextBox = currentWrapPanel.Children[2] as TextBox;

            currentLabel.Visibility = Visibility.Collapsed;
            currentTextBox.Visibility = Visibility.Visible;

            currentTextBox.Text = (string)currentLabel.Content;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        /// <MOUSE LEAVE HANDLERS>
        ///////////////////////////////////////////////////////////////////////////////////// 
        ///
        protected void OnMouseLeaveAgencyName(object sender, RoutedEventArgs e)
        {
            agencyNameLabel.Visibility = Visibility.Visible;
            agencyNameTextBox.Visibility = Visibility.Collapsed;

            if (agencyNameTextBox.Text != agency.GetAgencyName())
            {
                BrushConverter brush = new BrushConverter();

                agencyNameLabel.Content = agencyNameTextBox.Text;
                agencyNameLabel.Foreground = (Brush)brush.ConvertFrom("#FF0000");

                agencyNameEdited = true;
            }
            else
                agencyNameEdited = false;
        }

        
        //DONT USE NUMBERS, USE WILL FIND ALL NUMBERS DEFINED AS MACROS IN ONE OF THE .cs FILES
        private void FulfillFields()
        {
            agencyNameTextBox.Text = agency.agency_name;

            // Employee ComboBox
            List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT> marketingEmployees = new List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT>();
            CommonQueriesObject.GetDepartmentEmployees(102, ref marketingEmployees);
            for (int i = 0; i < marketingEmployees.Count; i++)
                employeesCombo.Items.Add(marketingEmployees[i].employee_name);
            employeesCombo.SelectedItem = agency.employee_name;

            // Country,State,City and District
            List<BASIC_STRUCTS.COUNTRY_STRUCT> countries = new List<BASIC_STRUCTS.COUNTRY_STRUCT>();
            CommonQueriesObject.GetAllCountries(ref countries);
            for (int i = 0; i < countries.Count; i++)
                countriesCombo.Items.Add(countries[i].country_name);
            countriesCombo.SelectedItem = agency.country;

            List<BASIC_STRUCTS.STATE_STRUCT> states = new List<BASIC_STRUCTS.STATE_STRUCT>();
            CommonQueriesObject.GetAllCountryStates(countriesCombo.SelectedIndex, ref states);
            for (int i = 0; i < states.Count; i++)
                statesCombo.Items.Add(states[i].state_name);
            statesCombo.IsEnabled = true;
            statesCombo.SelectedItem = agency.state;
            statesCombo.IsEnabled = false;

            List<BASIC_STRUCTS.CITY_STRUCT> cities = new List<BASIC_STRUCTS.CITY_STRUCT>();
            CommonQueriesObject.GetAllStateCities(4900 + statesCombo.SelectedIndex, ref cities);
            for (int i = 0; i < cities.Count; i++)
                citiesCombo.Items.Add(cities[i].city_name);
            citiesCombo.IsEnabled = true;
            citiesCombo.SelectedItem = agency.city;
            citiesCombo.IsEnabled = false;

            List<BASIC_STRUCTS.DISTRICT_STRUCT> districts = new List<BASIC_STRUCTS.DISTRICT_STRUCT>();
            int cityId = ((4900 + statesCombo.SelectedIndex) * 100) + citiesCombo.SelectedIndex;
            CommonQueriesObject.GetAllCityDistricts(cityId, ref districts);
            for (int i = 0; i < districts.Count; i++)
                districtsCombo.Items.Add(districts[i].district_name);
            districtsCombo.IsEnabled = true;
            districtsCombo.SelectedItem = agency.district;
            districtsCombo.IsEnabled = false;

            //district.Text = agency.district;
            telephoneTextBox.Text = agency.telephone;
        }

        private void OnBtnClkSaveChanges(object sender, RoutedEventArgs e)
        {
            agency.agency_name = agencyNameTextBox.Text;
            agency.employee_name = employeesCombo.Text;
            agency.country = countriesCombo.Text;
            agency.state = statesCombo.Text;
            agency.city = citiesCombo.Text;
            agency.district = districtsCombo.Text;
            agency.telephone = telephoneTextBox.Text;
            agency.addressId = countriesCombo.SelectedIndex * 1000000 + statesCombo.SelectedIndex * 10000 + citiesCombo.SelectedIndex * 100 + districtsCombo.SelectedIndex;

            if (agency.agency_name.Length == 0 || agency.employee_name.Length == 0 || agency.country.Length == 0 || agency.state.Length == 0 || agency.city.Length == 0
                || agency.district.Length == 0 || agency.telephone.Length == 0 || agency.addressId == 0)
                return;

            if (this.agencySerial != -1)
            {
                if (!logisticsQueryObject.UpdateAgent(agencySerial, ref agency)) ;
                // show error message
                else
                    this.Close();
                return;
            }

            if (!logisticsQueryObject.AddAgent(ref agency)) ;
            // show error message
            else
                this.Close();
        }

    }
}

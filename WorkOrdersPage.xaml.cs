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
        private CommonFunctions CommonFunctionObject = new CommonFunctions();

        private Employee loggedInUser;
        private Employee selectedEmployee;

        private List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT> marketingEmployees = new List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT>();
        private List<COMPANY_WORK_MACROS.PRODUCT_STRUCT> productTypes = new List<COMPANY_WORK_MACROS.PRODUCT_STRUCT>();
        private List<COMPANY_WORK_MACROS.BRAND_STRUCT> brandsTypes = new List<COMPANY_WORK_MACROS.BRAND_STRUCT>();

        public WorkOrdersPage(ref Employee mLoggedInUser)
        {
            loggedInUser = mLoggedInUser;

            //DISABLING COMBOBOXES SHALL BE SEPARATE FROM INITIALIZATION FUNCTIONS
            //EITHER YOU DO THEM IN RESET FUNCTION
            //OR DO IT IN THE CONSTRUCTOR
            //BUT IT IS PREFERED TO DO IT IN RESET FUNCTIONS

           

            InitializeComponent();

            InitializeOrderList();

            InitializeComboBoxes();

            DisableComboBoxes();
        }

        private void DisableComboBoxes()
        {
            yearCombo.IsEnabled = false;
            quarterCombo.IsEnabled = false;
            employeeCombo.IsEnabled = false;
            productCombo.IsEnabled = false;
            brandCombo.IsEnabled = false;
            statusCombo.IsEnabled = false;
        }

        private void InitializeYearsComboBox()
        {
            for (int year = 2020; year <= DateTime.Now.Year; year++)
                yearCombo.Items.Add(year);
        }
        private void InitializeQuartersComboBox()
        {
            //INSTEAD OF HARD CODING YOUR COMBO, 
            //THIS FUNCTION IS BETTER SO EVERYTIME IN OUR PROJECT WE NEED TO LIST QUARTERS
            //WE ARE SURE THAT ALL HAVE THE SAME FORMAT
            for (int i = 0; i < BASIC_MACROS.NO_OF_QUARTERS; i++)
                quarterCombo.Items.Add(CommonFunctionObject.GetQuarterName(i + 1));
            //ALSO IF YOU NOTICE, I DIDN'T EVEN USE i < 4, I USED A PRE-DEFINED MACRO INSTEAD, SO THE CODE IS ALWAYS READABLE
        }
        
        //THIS FUNCTIONS ACCESS SQL SERVER, SO YOU SHALL ALWAYS CHECK IF THE QUERY IS COMPLETED SUCCESSFULLY
        // IF NOT YOU SHALL STOP DATA ACCESS FOR THE CODE NOT TO CRASH
        private bool InitializeProductsComboBox()
        {
            if (!CommonQueriesObject.GetCompanyProducts(ref productTypes))
                return false;

            for (int i = 0; i < productTypes.Count; i++)
                productCombo.Items.Add(productTypes[i].typeName);

            return true;
        }

        private bool InitializeBrandsComboBox()
        {
            if (!CommonQueriesObject.GetCompanyBrands(ref brandsTypes))
                return false;

            for (int i = 0; i < brandsTypes.Count; i++)
                brandCombo.Items.Add(brandsTypes[i].brandName);

            return true;
        }

        private bool InitialzieEmployeesComboBox()
        {
            //YOU CAN'T USE DIGITS IN YOUR CODE, OTHERWISE IF WE CHANGED THE IDs LATER, YOU WILL FACE A HARD TIME MANAGING YOUR CODE
            //ALSO IT IS UNREADABLE
            if (!CommonQueriesObject.GetDepartmentEmployees(COMPANY_ORGANISATION_MACROS.MARKETING_AND_SALES_DEPARTMENT_ID, ref marketingEmployees))
                return false;

            for (int i = 0; i < marketingEmployees.Count; i++)
                employeeCombo.Items.Add(marketingEmployees[i].employee_name);

            return true;
        }
        private bool InitializeComboBoxes()
        {
            InitializeYearsComboBox();
            InitializeQuartersComboBox();

            if (!InitializeProductsComboBox())
                return false;

            if (!InitializeBrandsComboBox())
                return false;

            if (!InitialzieEmployeesComboBox())
                return false;

            //PLEASE CREATE A FUNCTION IN COMMON QUERIES FOR THIS QUERY
            statusCombo.Items.Add("Open");
            statusCombo.Items.Add("Closed");

            return true;
        }


        private void UpdateList()
        {
            OrdersStackPanel.Children.Clear();
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

        private void InitializeOrderList()
        {
            listOfOrders.Clear();

            if (!CommonQueriesObject.GetWorkOrders(ref listOfOrders))
                return;

            UpdateStatus(statusCombo);
            UpdateYear(yearCombo);
            UpdateQuarter(quarterCombo);
            UpdateEmployee(employeeCombo);
            UpdateProduct(productCombo);
            UpdateBrand(brandCombo);

            UpdateList();
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
        private void OnButtonClickedAgents(object sender, RoutedEventArgs e)
        {
            FreightAgentsPage agentsPage = new FreightAgentsPage(ref loggedInUser);
            this.NavigationService.Navigate(agentsPage);
        }
        private void OnButtonClickedCustomsAgents(object sender, RoutedEventArgs e)
        {
            CustomsAgentsPage agentsPage = new CustomsAgentsPage(ref loggedInUser);
            this.NavigationService.Navigate(agentsPage);
        }

        /***********************************************************************************
        ***********************************************************************************
        *********************************CheckBox Listeners ********************************
        ***********************************************************************************
        ***********************************************************************************/
        private void OnYearCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                yearCombo.IsEnabled = true;
            else
            {
                yearCombo.SelectedIndex = -1;
                yearCombo.IsEnabled = false;
            }
        }

        private void OnQuarterCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                quarterCombo.IsEnabled = true;
            else
            {
                quarterCombo.SelectedIndex = -1;
                quarterCombo.IsEnabled = false;
            }
        }

        private void OnEmployeeCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                employeeCombo.IsEnabled = true;
            else
            {
                employeeCombo.SelectedIndex = -1;
                employeeCombo.IsEnabled = false;
            }
        }

        private void OnProductCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                productCombo.IsEnabled = true;
            else
            {
                productCombo.SelectedIndex = -1;
                productCombo.IsEnabled = false;
            }
        }

        private void OnBrandCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                brandCombo.IsEnabled = true;
            else
            {
                brandCombo.SelectedIndex = -1;
                brandCombo.IsEnabled = false;
            }
        }

        private void OnStatusCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
                statusCombo.IsEnabled = true;
            else
            {
                statusCombo.SelectedIndex = -1;
                statusCombo.IsEnabled = false;
            }
        }

        /***********************************************************************************
        ***********************************************************************************
        *********************************ComboBox Listeners ********************************
        ***********************************************************************************
        ***********************************************************************************/

        private void OnStatusComboBoxChange(object sender, RoutedEventArgs e)
        {
            InitializeOrderList();
            UpdateList();

        }
        private void UpdateStatus(object sender)
        {
            if (((ComboBox)sender).SelectedItem != null)
                for (int i = 0; i < listOfOrders.Count; i++)
                    if (!listOfOrders[i].order_status.Contains(((ComboBox)sender).SelectedItem.ToString()))
                    {
                        listOfOrders.RemoveAt(i);
                        i--;
                    }
        }
        
        private void OnQuarterComboBoxChange(object sender, RoutedEventArgs e)
        {
            InitializeOrderList();
        }
        private void UpdateQuarter(object sender)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                int quarter = ((ComboBox)sender).Items.IndexOf(((ComboBox)sender).SelectedItem);
                for (int i = 0; i < listOfOrders.Count; i++)
                {
                    int month = Int16.Parse(listOfOrders[i].issue_date.Substring(0,1));
                    if (!((quarter*3) <= month && ((quarter+1)*3) >month))
                    {
                        listOfOrders.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private void OnYearComboBoxChange(object sender = null, EventArgs e = null)
        {
            InitializeOrderList();
        }
        private void UpdateYear(object sender)
        {
            if (((ComboBox)sender).SelectedItem != null)
                for (int i = 0; i < listOfOrders.Count; i++)
                    if (!listOfOrders[i].issue_date.Contains(((ComboBox)sender).SelectedItem.ToString()))
                    {
                        listOfOrders.RemoveAt(i);
                        i--;
                    }
        }

        private void OnEmployeeComboBoxChange(object sender, RoutedEventArgs e)
        {
            InitializeOrderList();
        }
        private void UpdateEmployee(object sender)
        {
            if (((ComboBox)sender).SelectedItem != null)
                for (int i = 0; i < listOfOrders.Count; i++)

                    if (!listOfOrders[i].sales_person_name.Equals(((ComboBox)sender).SelectedItem.ToString()))
                    {
                        listOfOrders.RemoveAt(i);
                        i--;
                    }
        }

        private void OnProductComboBoxChange(object sender, RoutedEventArgs e)
        {
            InitializeOrderList();
        }
        private void UpdateProduct(object sender)
        {
            if (((ComboBox)sender).SelectedItem != null)
                for (int i = 0; i < listOfOrders.Count; i++)
                {
                    int j;
                    for (j = 0; j < listOfOrders[i].products.Count; j++)
                        if (listOfOrders[i].products[j].productType.typeName.Equals(((ComboBox)sender).SelectedItem.ToString()))
                        {
                            break;
                        }
                    if (j == listOfOrders[i].products.Count)
                    {
                        listOfOrders.RemoveAt(i);
                        i--;
                    }
                }
        }

        private void OnBrandComboBoxChange(object sender, RoutedEventArgs e)
        {
            InitializeOrderList();
        }
        private void UpdateBrand(object sender)
        {
            if (((ComboBox)sender).SelectedItem != null)
                for (int i = 0; i < listOfOrders.Count; i++)
                {
                    int j;
                    for (j = 0; j < listOfOrders[i].products.Count; j++)
                        if (listOfOrders[i].products[j].productBrand.brandName.Equals(((ComboBox)sender).SelectedItem.ToString()))
                        {
                            break;
                        }
                    if (j == listOfOrders[i].products.Count)
                    {
                        listOfOrders.RemoveAt(i);
                        i--;
                    }
                }

        }


    }

    
}

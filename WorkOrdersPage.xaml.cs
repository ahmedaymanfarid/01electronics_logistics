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

            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {

            yearCombo.IsEnabled = false;
            int initialYear = 2020;
            int finalYear = Int32.Parse(DateTime.Now.Year.ToString());
            for (; initialYear <= finalYear; initialYear++)
                yearCombo.Items.Add(initialYear);


            quarterCombo.IsEnabled = false;
            quarterCombo.Items.Add("First");
            quarterCombo.Items.Add("Second");
            quarterCombo.Items.Add("Third");
            quarterCombo.Items.Add("Fourth");

            employeeCombo.IsEnabled = false;
            List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT> marketingEmployees = new List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT>();
            CommonQueriesObject.GetDepartmentEmployees(102,ref marketingEmployees);
            for (int i = 0; i < marketingEmployees.Count; i++)
                    employeeCombo.Items.Add(marketingEmployees[i].employee_name);

            productCombo.IsEnabled = false;
            List<COMPANY_WORK_MACROS.PRODUCT_STRUCT> productTypes = new List<COMPANY_WORK_MACROS.PRODUCT_STRUCT>();
            CommonQueriesObject.GetCompanyProducts(ref productTypes);
            for (int i = 0; i < productTypes.Count; i++)
                productCombo.Items.Add(productTypes[i].typeName);

            brandCombo.IsEnabled = false;
            List<COMPANY_WORK_MACROS.BRAND_STRUCT> brandsTypes = new List<COMPANY_WORK_MACROS.BRAND_STRUCT>();
            CommonQueriesObject.GetCompanyBrands(ref brandsTypes);
            for (int i = 0; i < brandsTypes.Count; i++)
                brandCombo.Items.Add(brandsTypes[i].brandName);

            statusCombo.IsEnabled = false;
            statusCombo.Items.Add("Open");
            statusCombo.Items.Add("Closed");
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
                    for (j = 0; j < listOfOrders[i].products_type.Count; j++)
                        if (listOfOrders[i].products_type[j].typeName.Equals(((ComboBox)sender).SelectedItem.ToString()))
                        {
                            break;
                        }
                    if (j == listOfOrders[i].products_type.Count)
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
                    for (j = 0; j < listOfOrders[i].products_brand.Count; j++)
                        if (listOfOrders[i].products_brand[j].brandName.Equals(((ComboBox)sender).SelectedItem.ToString()))
                        {
                            break;
                        }
                    if (j == listOfOrders[i].products_brand.Count)
                    {
                        listOfOrders.RemoveAt(i);
                        i--;
                    }
                }

        }


    }

    
}

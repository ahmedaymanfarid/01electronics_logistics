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

namespace _01electronics_logistics
{
    /// <summary>
    /// Interaction logic for UserPortalPage.xaml
    /// </summary>
    /// 
    public partial class UserPortalPage : Page
    {

        private Employee loggedInUser;

        public UserPortalPage(ref Employee mLoggedInUser)
        {
            InitializeComponent();

            loggedInUser = mLoggedInUser;

            InitializeEmployeeDashboard();
        }
        
        private void InitializeEmployeeDashboard()
        {
            employeeNameLabel.Content = loggedInUser.GetEmployeeName();
            employeeBirthdateLabel.Content = loggedInUser.GetEmployeeBirthDate();
            employeeJoiningDateLabel.Content = loggedInUser.GetEmployeeJoinDate();
            employeeDepartmentLabel.Content = loggedInUser.GetEmployeeDepartment();
            employeeTeamLabel.Content = loggedInUser.GetEmployeeTeam();
            employeeBusinessEmailLabel.Content = loggedInUser.GetEmployeeBusinessEmail();
            employeePersonalEmailLabel.Content = loggedInUser.GetEmployeePersonalEmail();
        }

        private void OnButtonClickedMyProfile(object sender, RoutedEventArgs e)
        {
            UserPortalPage userPortal = new UserPortalPage(ref loggedInUser);
            this.NavigationService.Navigate(userPortal);
        }

        private void OnButtonClickedEmployees(object sender, RoutedEventArgs e)
        {
            EmployeesPage employees = new EmployeesPage(ref loggedInUser);
            this.NavigationService.Navigate(employees);
        }

        private void OnButtonClickedPayrollInfo(object sender, RoutedEventArgs e)
        {
            PayrollInfoPage payrollInfo = new PayrollInfoPage(ref loggedInUser);
            this.NavigationService.Navigate(payrollInfo);
        }

        private void OnButtonClickedSalaries(object sender, RoutedEventArgs e)
        {
            SalariesPage salaries = new SalariesPage(ref loggedInUser);
            this.NavigationService.Navigate(salaries);
        }

        private void OnButtonClickedAbsence(object sender, RoutedEventArgs e)
        {

        }

        private void OnButtonClickedVacations(object sender, RoutedEventArgs e)
        {

        }

        private void OnEmployeeInfoDoubleClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

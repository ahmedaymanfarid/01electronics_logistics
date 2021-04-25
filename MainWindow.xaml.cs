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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow(ref Employee mLoggedInUser)
        {
            InitializeComponent();

            UserPortalPage userPortal = new UserPortalPage(ref mLoggedInUser);
            this.NavigationService.Navigate(userPortal);
        }
    }
}

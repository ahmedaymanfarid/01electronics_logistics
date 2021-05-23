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

namespace _01electronics_logistics
{
    /// <summary>
    /// Interaction logic for ViewAgent.xaml
    /// </summary>
    public partial class ViewAgent : Window
    {
        private LogisticsQueries logisticsQueryObject = new LogisticsQueries();
        public ViewAgent(int agentId)
        {
            InitializeComponent();

            AGENT_MACROS.AGENT_FULL_INFO agent = new AGENT_MACROS.AGENT_FULL_INFO();

            logisticsQueryObject.GetFullAgentInfo(agentId,ref agent);

            agentName.Text = agent.agent_name;
            employeeName.Text = agent.employee_name;
            district.Text = agent.district;
            telephone.Text = agent.telephone;
            
        }
    }
}

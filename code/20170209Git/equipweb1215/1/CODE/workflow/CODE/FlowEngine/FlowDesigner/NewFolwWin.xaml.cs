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

namespace FlowDesigner
{
    /// <summary>
    /// NewFolwWin.xaml 的交互逻辑
    /// </summary>
    public partial class NewFolwWin : Window
    {
        public NewFolwWin()
        {
            InitializeComponent();
        }

        public string flowName { get; set; }
        public string flowDesc { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flowName = flow_name.Text;
            flowDesc = flow_desc.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}

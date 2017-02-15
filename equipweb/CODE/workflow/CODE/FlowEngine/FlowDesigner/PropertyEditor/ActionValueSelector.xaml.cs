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

namespace FlowDesigner.PropertyEditor
{
    /// <summary>
    /// ActionValueSelector.xaml 的交互逻辑
    /// </summary>
    public partial class ActionValueSelector : Window
    {
        private List<string> _linkParams = new List<string>();


        public List<string> LinkParams
        {
            get
            {
                return _linkParams;
            }
            set
            {
                _linkParams = value;
                foreach (var lp in _linkParams)
                {
                    TreeViewItem ti = new TreeViewItem();
                    ti.Header = lp;
                    (LinkValue.Items.GetItemAt(1) as TreeViewItem).Items.Add(ti);
                }
            }
        }

        public string ret_value = "";
        public ActionValueSelector()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string selHeader = (LinkValue.SelectedItem as TreeViewItem).Header.ToString();
            if (selHeader == "当前事件关联变量" || selHeader == "工作流属性")
                return;            
            else
            {
                if (((LinkValue.SelectedItem as TreeViewItem).Parent as TreeViewItem).Header.ToString() == "工作流属性")
                {
                    ret_value = @"@ATT_" + (LinkValue.SelectedItem as TreeViewItem).Header.ToString();
                }
                else if (((LinkValue.SelectedItem as TreeViewItem).Parent as TreeViewItem).Header.ToString() == "当前事件关联变量")
                {
                    ret_value = @"@PAR_" + (LinkValue.SelectedItem as TreeViewItem).Header.ToString();
                }
                else
                {
                    this.DialogResult = false;
                    this.Close();
                    return;
                }
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

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
    /// EventActionParamAdd.xaml 的交互逻辑
    /// </summary>
    public partial class EventActionParamAdd : Window
    {
        public E_Param _NewParam = new E_Param();
        public List<string> _linkParams = new List<string>();
        public EventActionParamAdd()
        {
            InitializeComponent();
        }

        private void LinkValue(object sender, RoutedEventArgs e)
        {
            ActionValueSelector vs = new ActionValueSelector();
            vs.LinkParams = _linkParams;
            if (vs.ShowDialog().Equals(true))
            {
                Param_Value.Text = vs.ret_value;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if ((Param_Value.Text == "") || (Param_Name.Text == ""))
                return;

            _NewParam.p_name = Param_Name.Text;
            _NewParam.p_value = Param_Value.Text;

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

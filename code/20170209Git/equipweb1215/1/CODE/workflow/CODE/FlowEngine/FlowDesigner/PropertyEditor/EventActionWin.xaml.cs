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
    public class E_Param
    {
        public string p_name { get; set; }
        public string p_value { get; set; }
    }
    /// <summary>
    /// EventActionWin.xaml 的交互逻辑
    /// </summary>
    public partial class EventActionWin : Window
    {
        /// <summary>
        /// action的url地址
        /// </summary>
        public string action_url
        {
            get
            {
                return Action_Url.Text;
            }

            set
            {
                Action_Url.Text = value;
            }
        }

        public Dictionary<string, string> event_linkParams;

        private Dictionary<string, string> _params = new Dictionary<string, string>();

        public Dictionary<string , string> action_params
        {
            get
            {
                return _params;
            }
            set{
                _params = value;
                Params_List.Items.Clear();
                foreach(var p in _params)
                {
                    Params_List.Items.Add(new E_Param() { p_name = p.Key, p_value = p.Value });
                }
            }
        }
        public EventActionWin()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {            
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Add_Param(object sender, RoutedEventArgs e)
        {
            EventActionParamAdd pa = new EventActionParamAdd();
            pa._linkParams.Clear();

            foreach (var lp in event_linkParams)
            {
                pa._linkParams.Add(lp.Key);
            }

            if (pa.ShowDialog().Equals(true))
            {
                Dictionary<string, string> oldValue = action_params;
                if (!oldValue.ContainsKey(pa._NewParam.p_name))
                {
                    oldValue[pa._NewParam.p_name] = pa._NewParam.p_value;
                }
                action_params = oldValue;
            }
        }

        private void Del_Param(object sender, RoutedEventArgs e)
        {
            E_Param ep = Params_List.SelectedItem as E_Param;

            if (ep == null)
                return;

            Dictionary<string, string> pas = action_params;
            pas.Remove(ep.p_name);

            action_params = pas;
        }
    }
}

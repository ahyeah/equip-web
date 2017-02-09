using FlowDesigner.Management;
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
    public class Param_APPRes
    {
        public string p_name { get; set; }
        public string app_res { get; set; }
    }
    /// <summary>
    /// LinkParamsWin.xaml 的交互逻辑
    /// </summary>
    public partial class LinkParamsWin : Window
    {
        public Dictionary<string, string> _newValue;
        public Dictionary<string, string> newValue
        {
            get {
                return _newValue;
            }
            set
            {
                _newValue = value;
                //Dest.ItemsSource = _newValue;
                foreach(var s in _newValue)
                {

                    Dest.Items.Add(new Param_APPRes() { p_name = s.Key, app_res = s.Value});
                }
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                foreach(param pa in mw.main_proj.CurrentWF.params_table)
                {
                    if (!_newValue.ContainsKey(pa.Name))
                        Source.Items.Add(pa);
                }
            }
        }
        public LinkParamsWin()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            _newValue.Clear();
            //newValue.Add("aaaa");
            foreach(var i in Dest.Items)
            {
                Param_APPRes pl = (i as Param_APPRes);
                _newValue[pl.p_name] = pl.app_res;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).ItemsSource = ((MainWindow)Application.Current.MainWindow).main_proj.Param_AppRes;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            param pa = (Source.SelectedItem as param);
            Source.Items.Remove(pa);
            Dest.Items.Add(new Param_APPRes() { p_name = pa.Name, app_res = "" });
            
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Param_APPRes pl = (Dest.SelectedItem as Param_APPRes);
            Dest.Items.Remove(pl);

            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            foreach (param pa in mw.main_proj.CurrentWF.params_table)
            {
                if (pa.Name == pl.p_name)
                    Source.Items.Add(pa);
            }
        }
    }
}

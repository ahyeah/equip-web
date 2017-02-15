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
    /// AddNewParam.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewParam : Window
    {
        public AddNewParam()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text == "")
                this.DialogResult = false;
            else
                this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach(LinkParam lp in Event_List.Items)
            {
                if (lp.Name == (string)(((CheckBox)sender).Tag))
                    lp.Selected = (bool)((CheckBox)sender).IsChecked;
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            (sender as ComboBox).ItemsSource = ((MainWindow)Application.Current.MainWindow).main_proj.Param_AppRes;
        }
    }
}

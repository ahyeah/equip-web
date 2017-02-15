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
using Xceed.Wpf.Toolkit;

namespace FlowDesigner.PropertyEditor
{
    /// <summary>
    /// TimeoutWin.xaml 的交互逻辑
    /// </summary>
    public partial class TimeoutWin : Window
    {
        public string time_start = "";

        public TimeSpan? time_offset = null;

        public DateTime? time_exact = null;

        public string time_action = "";

        public string time_callback = "INVALID";

        public TimeoutWin()
        {
            InitializeComponent();
            
        }

        private void radio_offset_Unchecked(object sender, RoutedEventArgs e)
        {
            select_timestart.IsEnabled = false;
            select_escape.IsEnabled = false;
            input_timeoffset.IsEnabled = false;
        }

        private void radio_offset_Checked(object sender, RoutedEventArgs e)
        {
            select_timestart.IsEnabled = true;
            select_escape.IsEnabled = true;
            input_timeoffset.IsEnabled = true;
        }

        private void radio_exact_Checked(object sender, RoutedEventArgs e)
        {
            exact_picker.IsEnabled = true;
        }

        private void radio_exact_Unchecked(object sender, RoutedEventArgs e)
        {
            exact_picker.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (radio_offset.IsChecked == true)
            {
                time_start = ((select_timestart.SelectedItem as ComboBoxItem).Tag as string);

                TimeSpan t = new TimeSpan(0);
                switch((select_escape.SelectedItem as ComboBoxItem).Tag as string)
                {
                    case "Date":
                        t = t + new TimeSpan(Convert.ToInt32(input_timeoffset.Text), 0, 0, 0);
                        break;

                    case "Hour":
                        t = t + new TimeSpan(0, Convert.ToInt32(input_timeoffset.Text), 0, 0);
                        break;

                    case "Minute":
                        t = t + new TimeSpan(0, 0, Convert.ToInt32(input_timeoffset.Text), 0);
                        break;
                }
                time_offset = t;
                time_exact = null;
            }
            else if (radio_exact.IsChecked == true)
            {
                time_exact = exact_picker.Value;

                time_start = "";
                time_offset = null;
            }

            time_callback = input_callback.Text;
            time_action = (select_action.SelectedItem as ComboBoxItem).Tag as string;
            this.DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (time_exact != null)
            {
                radio_exact.IsChecked = true;
                exact_picker.Value = time_exact;

                select_timestart.IsEnabled = false;
                select_escape.IsEnabled = false;
                input_timeoffset.IsEnabled = false;
            }
            else if (time_start != "")
            {
                exact_picker.IsEnabled = false;

                foreach(var item in select_timestart.Items)
                {
                    if (((item as ComboBoxItem).Tag as string) == time_start)
                        (item as ComboBoxItem).IsSelected = true;
                }
                radio_offset.IsChecked = true;
                exact_picker.Value = null;
                if (time_offset != null)
                {
                    if (time_offset.Value.Days != 0)
                    {
                        select_escape.Text = "天";
                        input_timeoffset.Text = Convert.ToString(time_offset.Value.Days);
                    }
                    else if (time_offset.Value.Hours != 0)
                    {
                        select_escape.Text = "小时";
                        input_timeoffset.Text = Convert.ToString(time_offset.Value.Hours);
                    }
                    else if (time_offset.Value.Minutes != 0)
                    {
                        select_escape.Text = "分钟";
                        input_timeoffset.Text = Convert.ToString(time_offset.Value.Minutes);
                    }
                }
            }
            else
            {
                radio_offset.IsChecked = true;
                exact_picker.IsEnabled = false;
                select_timestart.IsEnabled = true;
                select_escape.IsEnabled = true;
                input_timeoffset.IsEnabled = true;
            }

            foreach(var item in select_action.Items)
            {
                if (((item as ComboBoxItem).Tag as string) == time_action)
                    (item as ComboBoxItem).IsSelected = true;
            }

            input_callback.Text = time_callback;
        }
    }
}

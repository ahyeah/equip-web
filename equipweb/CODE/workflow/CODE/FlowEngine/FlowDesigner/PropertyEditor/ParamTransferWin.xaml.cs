﻿using FlowDesigner.ConfigItems;
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
    /// <summary>
    /// ParamTransferWin.xaml 的交互逻辑
    /// </summary>
    public partial class ParamTransferWin : Window
    {
        public List<paramtransfer_item> params_transfer;
        public ParamTransferWin(List<paramtransfer_item> par_tran)
        {
            params_transfer = par_tran;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Source_Params.ItemsSource = ((MainWindow)Application.Current.MainWindow).main_proj.CurrentWF.params_table;
            ConfigSubEvent se = (ConfigSubEvent)(((MainWindow)Application.Current.MainWindow).main_proj.CurrentWF.SelectedItem);
            Dest_Params.ItemsSource = ((MainWindow)Application.Current.MainWindow).main_proj.GetWorkFlow(se.LinkWorkFLow).params_table;

            foreach(paramtransfer_item pi in params_transfer)
            {
                if (pi.direction == transfer_direction.to)
                    Param_transfer.Items.Add(pi.parent + "--->" + pi.child);
                else if (pi.direction == transfer_direction.from)
                    Param_transfer.Items.Add(pi.parent + "<---" + pi.child);
            }
        }

        private void To_Link_Click(object sender, RoutedEventArgs e)
        {
            paramtransfer_item pt = new paramtransfer_item();
            pt.parent = ((param)Source_Params.SelectedItem).Name;
            pt.child = ((param)Dest_Params.SelectedItem).Name;
            pt.direction = transfer_direction.to;

            bool flag = false;
            foreach(paramtransfer_item pi in params_transfer)
            {
                if (pi.parent == pt.parent && pi.parent == pt.parent && pi.direction == pt.direction)
                    flag = true;
            }

            if (!flag)
            {
                params_transfer.Add(pt);
                Param_transfer.Items.Add(pt.parent + "--->" + pt.child);
            }
        }

        private void From_Link_Click(object sender, RoutedEventArgs e)
        {
            paramtransfer_item pt = new paramtransfer_item();
            pt.parent = ((param)Source_Params.SelectedItem).Name;
            pt.child = ((param)Dest_Params.SelectedItem).Name;
            pt.direction = transfer_direction.from;

            bool flag = false;
            foreach (paramtransfer_item pi in params_transfer)
            {
                if (pi.parent == pt.parent && pi.parent == pt.parent && pi.direction == pt.direction)
                    flag = true;
            }

            if (!flag)
            {
                params_transfer.Add(pt);
                Param_transfer.Items.Add(pt.parent + "<---" + pt.child);
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void param_trans_delete(object sender, RoutedEventArgs e)
        {
            string pt = (string)Param_transfer.SelectedItem;
            Param_transfer.Items.RemoveAt(Param_transfer.SelectedIndex);

            if (pt.Contains("<---"))
            {
                string va = pt;
                string[] strs = va.Replace("<---", ",").Split(',');
                foreach (paramtransfer_item p in params_transfer)
                {
                    if (p.parent == strs[0] && p.child == strs[1] && p.direction == transfer_direction.from)
                    {
                        params_transfer.Remove(p);
                        break;
                    }
                }
            }

            if (pt.Contains("--->"))
            {
                string va = pt;
                string[] strs = va.Replace("--->", ",").Split(',');
                foreach (paramtransfer_item p in params_transfer)
                {
                    if (p.parent == strs[0] && p.child == strs[1] && p.direction == transfer_direction.to)
                    {
                        params_transfer.Remove(p);
                        break;
                    }
                }
            }
        }


    }
}

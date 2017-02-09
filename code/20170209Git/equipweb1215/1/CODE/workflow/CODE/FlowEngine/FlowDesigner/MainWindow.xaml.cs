using FlowDesigner.ConfigItems;
using FlowDesigner.customcontrol;
using FlowDesigner.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Control;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace FlowDesigner
{
    class LinkParam
    {
        public string Name { get; set; }
        public bool Selected { get; set; }

        public string App_res { get; set; }

    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public WorkFlowsProj main_proj;

        private int id_for_workflow = 0;
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddNEvent.IsEnabled = false;
            AddSEvent.IsEnabled = false;
            AddNewFlow.IsEnabled = false;
            AddLEvent.IsEnabled = false;
            Save_button.IsEnabled = false;
            AddNewParam.IsEnabled = false;
            Complie.IsEnabled = false;
            ToDb.IsEnabled = false;
        }
        
        public void OnConfigItem_Selected(ConfigItem sender)
        {
            main_proj.CurrentWF.SelectedItem = sender;
            property.SelectedObject = sender;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void mainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 新建工作流工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void new_proj_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            if (main_proj == null)
            {
                main_proj = new WorkFlowsProj();
                NewWorkFlowProj newWF = new NewWorkFlowProj();
                newWF.Owner = this;
                if (!newWF.ShowDialog().Value)
                {
                    main_proj = null;
                    return;
                }
            
                string proj_name = newWF.newProj_name;
                string proj_path = newWF.newProj_path;
            
                
                main_proj.Name = proj_name;
                main_proj.Path = proj_path;
                main_proj.win_main = this;
                main_proj.DB_Config = newWF.db_config;
                main_proj.Record_Items = newWF.Record_Items;
                TreeViewItem projItem = new TreeViewItem();
                StackPanel stack = new StackPanel();
                stack.Orientation = System.Windows.Controls.Orientation.Horizontal;
                Image im = new Image();
                im.Width = 16;
                im.Height = 16;
                im.Source = new BitmapImage(new Uri("new_wfs.ico", UriKind.RelativeOrAbsolute));
                stack.Children.Add(im);
                TextBlock tb = new TextBlock();
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.Text = main_proj.Name;
                stack.Children.Add(tb);
                projItem.Header = stack;

                System.Windows.Controls.ContextMenu cm = this.FindResource("proj_prop") as System.Windows.Controls.ContextMenu;
                cm.PlacementTarget = sender as System.Windows.Controls.TreeViewItem;
                projItem.ContextMenu = cm;

                
                main_treeView.Items.Add(projItem);
                AddFlow.IsEnabled = true;


                
                Save_button.IsEnabled = true;
                
                
                
            }
        }

        private void AddFlow_Click(object sender, RoutedEventArgs e)
        {
            if (main_proj != null)
            {
                NewFolwWin fw = new NewFolwWin();
                fw.Owner = this;
                if (fw.ShowDialog().Value != true)
                {
                    return;
                }
                WorkFlowMan new_wf = main_proj.NewWrokFlowMan(fw.flowName, fw.flowDesc);
                if (new_wf == null)
                {
                    System.Windows.MessageBox.Show(fw.flow_name + "已存在");
                    return;
                }
                TreeViewItem projItem = new TreeViewItem();
                StackPanel stack = new StackPanel();
                stack.Orientation = System.Windows.Controls.Orientation.Horizontal;
                Image im = new Image();
                im.Width = 16;
                im.Height = 16;
                im.Source = new BitmapImage(new Uri("workflow.ico", UriKind.RelativeOrAbsolute));
                stack.Children.Add(im);
                TextBlock tb = new TextBlock();
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.Text = new_wf.Name;
                stack.Children.Add(tb);
                projItem.Header = stack;
                
                                
                ((TreeViewItem)main_treeView.Items[0]).Items.Add(projItem);
                projItem.MouseDoubleClick += new_wf.OnSelected;
                projItem.MouseRightButtonUp += this.ProjectItem_menu;
                projItem.Tag = new_wf.Name;
                new_wf.LinkToMainTab(mainTab);
                id_for_workflow += 1;
                main_proj.CurrentWF = new_wf;

                AddNEvent.IsEnabled = true;
                AddSEvent.IsEnabled = true;
                AddLEvent.IsEnabled = true;
                AddNewFlow.IsEnabled = true;
                Save_button.IsEnabled = true;
                AddNewParam.IsEnabled = true  ;
                Complie.IsEnabled = true;
                ToDb.IsEnabled = true;
            }
        }

        private void ProjectItem_menu(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.ContextMenu cm = this.FindResource("proj_menu") as System.Windows.Controls.ContextMenu;
            cm.PlacementTarget = sender as System.Windows.Controls.TreeViewItem;
            cm.Tag = (sender as System.Windows.Controls.TreeViewItem).Tag;
            cm.IsOpen = true;
            
            
        }

        private void AddNEvent_Click(object sender, RoutedEventArgs e)
        {
            //((System.Windows.Controls.RadioButton)sender).IsChecked = !((System.Windows.Controls.RadioButton)sender).IsChecked;
            if (sender == AddNEvent)
                main_proj.CurrentWF.NewEventType = "NormalEvent";
            else if (sender == AddSEvent)
                main_proj.CurrentWF.NewEventType = "SubProcess";
            else if (sender == AddLEvent)
                main_proj.CurrentWF.NewEventType = "LoopEvent";

            main_proj.CurrentWF.PerAddNEvent = ((System.Windows.Controls.CheckBox)sender).IsChecked.Value;
            //this.Cou
        }

        public void EndAddNEvent()
        {
            AddNEvent.IsChecked = false;
            AddSEvent.IsChecked = false;
        }

        private void main_treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }

        private void mainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// 添加Flow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewFlow_Click(object sender, RoutedEventArgs e)
        {
            main_proj.CurrentWF.perAddNFlow = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<LinkParam> lv_source = new ObservableCollection<LinkParam>();

            AddNewParam par_win = new AddNewParam();
            par_win.Owner = this;

            List<ConfigEvent> evs = main_proj.CurrentWF.GetAllEvents();
            foreach (ConfigEvent ce in evs)
            {
                lv_source.Add(new LinkParam() { Name = ce.name, Selected = false, App_res="" });
                
            }
            par_win.Event_List.ItemsSource = lv_source;
            if (par_win.ShowDialog().Value == true)
            {
                //检查是否有重名的变量名
                foreach (param pa in main_proj.CurrentWF.params_table)
                    if (pa.Name == par_win.name.Text.Trim())
                        break;
                
                //添加变量
                main_proj.CurrentWF.params_table.Add(new param()
                {
                    Name = par_win.name.Text,
                    Desc = par_win.desc.Text,
                    Type = par_win.type.SelectedItem.ToString()
                });

                foreach (LinkParam it in par_win.Event_List.Items)
                {
                    foreach(ConfigEvent ee in evs)
                    {
                        if (ee.name == it.Name)
                        {
                            if (it.Selected == true)
                                ee.LinkParams.Add(par_win.name.Text, it.App_res);
                        }
                    }
                }
            }
        }

        public void Confi_Param(param pa)
        {
            ObservableCollection<LinkParam> lv_source = new ObservableCollection<LinkParam>();

            AddNewParam par_win = new AddNewParam();
            par_win.Owner = this;
            par_win.name.Text = pa.Name;
            par_win.name.IsEnabled = false;
            par_win.desc.Text = pa.Desc;
            par_win.type.SelectedItem = pa.Type;

            List<ConfigEvent> evs = main_proj.CurrentWF.GetAllEvents();
            
            foreach (ConfigEvent ce in evs)
            {
                lv_source.Add(new LinkParam() { 
                    Name = ce.name, 
                    Selected = ce.LinkParams.ContainsKey(pa.Name),
                    App_res = (ce.LinkParams.ContainsKey(pa.Name) ? ce.LinkParams[pa.Name] : "")
                    
                });
                    
            }
            par_win.Event_List.ItemsSource = lv_source;
            if (par_win.ShowDialog().Value == true)
            {
                pa.Type = par_win.type.Text;
                pa.Desc = par_win.desc.Text;

                foreach (LinkParam it in par_win.Event_List.Items)
                {
                    foreach (ConfigEvent ee in evs)
                    {
                        if (ee.name == it.Name)
                        {
                            if (it.Selected == true)
                            {
                                if (!ee.LinkParams.ContainsKey(par_win.name.Text))
                                    ee.LinkParams.Add(par_win.name.Text, it.App_res);
                                else
                                    ee.LinkParams[par_win.name.Text] = it.App_res;
                            }
                        }
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main_proj.SaveWorkFlowProj();
        }

        private void CloseAll()
        {
            if (main_proj != null)
            {
                main_treeView.Items.Clear();
                mainTab.Items.Clear();
                main_proj = null;
                property.SelectedObject = null;

                
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CloseAll();
            XmlDocument doc = new XmlDocument();
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "工程文件(*.wfproj)|*.wfproj";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                doc.Load(fd.FileName);
            else
                return;


            main_proj = new WorkFlowsProj();
            main_proj.win_main = this;
            main_proj.Path = System.IO.Path.GetDirectoryName(fd.FileName);
            main_proj.LoadWorkFlowProj((XmlElement)doc.SelectSingleNode("work_wlow_project"));
            
            TreeViewItem projItem = new TreeViewItem();
            StackPanel stack = new StackPanel();
            stack.Orientation = System.Windows.Controls.Orientation.Horizontal;
            Image im = new Image();
            im.Width = 16;
            im.Height = 16;
            im.Source = new BitmapImage(new Uri("new_wfs.ico", UriKind.RelativeOrAbsolute));
            stack.Children.Add(im);
            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Text = main_proj.Name;
            stack.Children.Add(tb);
            projItem.Header = stack;

            System.Windows.Controls.ContextMenu cm = this.FindResource("proj_prop") as System.Windows.Controls.ContextMenu;
            cm.PlacementTarget = sender as System.Windows.Controls.TreeViewItem;
            projItem.ContextMenu = cm;

            main_treeView.Items.Add(projItem);
            AddFlow.IsEnabled = true;

            foreach (WorkFlowMan new_wf in main_proj.GetWorkFlows())
            {
                TreeViewItem projItem1 = new TreeViewItem();
                StackPanel stack1 = new StackPanel();
                stack1.Orientation = System.Windows.Controls.Orientation.Horizontal;
                Image im1 = new Image();
                im1.Width = 16;
                im1.Height = 16;
                im1.Source = new BitmapImage(new Uri("workflow.ico", UriKind.RelativeOrAbsolute));
                stack1.Children.Add(im1);
                TextBlock tb1 = new TextBlock();
                tb1.VerticalAlignment = VerticalAlignment.Center;
                tb1.Text = new_wf.Name;
                stack1.Children.Add(tb1);
                projItem1.Header = stack1;
                projItem1.Tag = new_wf.Name;


                ((TreeViewItem)main_treeView.Items[0]).Items.Add(projItem1);
                projItem1.MouseDoubleClick += new_wf.OnSelected;
                projItem1.MouseRightButtonUp += this.ProjectItem_menu;
                new_wf.LinkToMainTab(mainTab);
            }

            AddNEvent.IsEnabled = true;
            AddSEvent.IsEnabled = true;
            AddLEvent.IsEnabled = true;
            AddNewFlow.IsEnabled = true;
            Save_button.IsEnabled = true;
            AddNewParam.IsEnabled = true;
            Complie.IsEnabled = true;
            ToDb.IsEnabled = true;

        }

        private void Comple_Click(object sender, RoutedEventArgs e)
        {
            main_proj.SaveWorkFlowProj();
            main_proj.Complie();
        }

        private void proj_menu_delete(object sender, RoutedEventArgs e)
        {
            //System.Windows.MessageBox.Show(sender.GetType().Name);
            System.Windows.Controls.MenuItem mi = sender as System.Windows.Controls.MenuItem;
            string cur_name = (mi.Parent as System.Windows.Controls.ContextMenu).Tag.ToString();
            ((TreeViewItem)main_treeView.Items[0]).Items.Remove((mi.Parent as System.Windows.Controls.ContextMenu).PlacementTarget);

            WorkFlowMan deleted = main_proj.DeleteWorkFlowMan(cur_name);
            deleted.UnLinkFromMainTab(mainTab);
        }

        private void param_menu_delete(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem mi = sender as System.Windows.Controls.MenuItem;
            string p_name = ((param)(mi.Parent as System.Windows.Controls.ContextMenu).Tag).Name;
            main_proj.CurrentWF.DeleteParam(p_name);
        }

        public void pop_itemmenu(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.ContextMenu cm = this.FindResource("item_menu") as System.Windows.Controls.ContextMenu;
            cm.PlacementTarget = (sender as ConfigItem).GetShowItem();
            cm.Tag = sender;
            cm.IsOpen = true;
        }

        private void item_menu_delete(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Controls.MenuItem mi = sender as System.Windows.Controls.MenuItem;

            if (property.SelectedObject == (mi.Parent as System.Windows.Controls.ContextMenu).Tag)
                property.SelectedObject = null;

            main_proj.CurrentWF.DeleteItem((mi.Parent as System.Windows.Controls.ContextMenu).Tag as ConfigItem);
        }

        private void Refresh_toDB(object sender, RoutedEventArgs e)
        {
            main_proj.Complie();
            main_proj.SaveWorkFlowProj();

            ObservableCollection<WorkFlowMan> wfs = main_proj.GetWorkFlows();

            SqlConnection sql_conn = new SqlConnection(main_proj.DB_Config);
            try
            {
                sql_conn.Open();
                if (sql_conn.State == ConnectionState.Open)
                {
                    
                    foreach (WorkFlowMan wfm in wfs)
                    {
                        string fileName = main_proj.Path.TrimEnd('\\') + "\\workflows\\" + wfm.Name + ".wfd";
                        XmlDocument doc = new XmlDocument();
                        doc.Load(fileName);

                        byte[] w_xml = Encoding.Default.GetBytes(doc.InnerXml);

                        SqlCommand sql_comm = new SqlCommand("select count(*) from WorkFlow_Define where W_Name='" + wfm.Name + "'", sql_conn);
                        SqlDataReader dr = sql_comm.ExecuteReader();
                        dr.Read();
                        if (Convert.ToInt32(dr[0].ToString()) != 0)
                        {
                            dr.Close();
                            sql_comm.CommandText = "update WorkFlow_Define set W_Xml = @param1, W_Attribution = @param2 where W_Name='" + wfm.Name + "'";
                            sql_comm.Parameters.Add("@param1", SqlDbType.VarBinary).Value = w_xml;
                            sql_comm.Parameters.Add("@param2", SqlDbType.NVarChar).Value = wfm.Description;
                            sql_comm.ExecuteNonQuery();
                        }
                        else
                        {
                            dr.Close();
                            sql_comm.CommandText = "insert into WorkFlow_Define(W_Attribution, W_Name, W_Xml) values (@Attr, @Name, @Xml)";
                            sql_comm.Parameters.Add("@Xml", SqlDbType.VarBinary).Value = w_xml;
                            sql_comm.Parameters.Add("@Attr", SqlDbType.NVarChar).Value = wfm.Description;
                            sql_comm.Parameters.Add("@Name", SqlDbType.NVarChar).Value = wfm.Name;
                            sql_comm.ExecuteNonQuery();
                        }
                        
                    }
                    sql_conn.Close();
                    
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("更新失败");
            }
        }

        private void proj_prop_show(object sender, RoutedEventArgs e)
        {
            NewWorkFlowProj wfp = new NewWorkFlowProj();
            wfp.Owner = this;
            wfp.Title = "工程属性";
            wfp.newProj_name = main_proj.Name;
            wfp.newProj_path = main_proj.Path;
            wfp.proj_name.IsEnabled = false;
            wfp.proj_path.IsEnabled = false;
            wfp.Record_Items = main_proj.Record_Items;

            SqlConnection sqlConn = new SqlConnection(main_proj.DB_Config);
            wfp.server.Text = sqlConn.DataSource;
            wfp.SetDBConfigration(sqlConn.Database);
            if (wfp.ShowDialog().Value == true)
            {
                main_proj.DB_Config = wfp.db_config;
                main_proj.Record_Items = wfp.Record_Items;
            }
        }
                
    }
}

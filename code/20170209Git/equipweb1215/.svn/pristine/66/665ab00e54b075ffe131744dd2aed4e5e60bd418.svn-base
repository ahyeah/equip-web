using FlowDesigner.Management;
using Microsoft.Win32;
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
//using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlowDesigner
{
    /// <summary>
    /// NewWorkFlowProj.xaml 的交互逻辑
    /// </summary>
    public partial class NewWorkFlowProj : Window
    {
        public NewWorkFlowProj()
        {
            db_config = "";
            Record_Items = new ObservableCollection<RecordItem>();
            InitializeComponent();
        }

        public ObservableCollection<RecordItem> Record_Items { get; set; }
        /// <summary>
        /// 设置，获取proj_name
        /// </summary>
        public string newProj_name {
            get
            {
                return this.proj_name.Text;
            }
            set
            {
                this.proj_name.Text = value;
            }
        }

        /// <summary>
        /// 设置、获取proj_path
        /// </summary>
        public string newProj_path
        {
            get
            {
                return this.proj_path.Text;
            }
            set
            {
                this.proj_path.Text = value;
            }
        }

        public string db_config
        {
            get;
            set;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newProj_name == "" || newProj_path == "" || db_config == "")
            {
                this.DialogResult = false;
                this.Close();
                return;
            }

            newProj_path = newProj_path.TrimEnd('\\') + "\\" + newProj_name;
            if (Directory.Exists(newProj_path))
            {
                this.DialogResult = false;
                this.Close();
                return;
            }

            Directory.CreateDirectory(newProj_path);
            Directory.CreateDirectory(newProj_path + "\\workflows");
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// 选择文件夹事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog sfdiag = new FolderBrowserDialog();
            sfdiag.Description = "选择工程保存路径";
            sfdiag.RootFolder = System.Environment.SpecialFolder.MyComputer;
            
            if (sfdiag.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            this.proj_path.Text = sfdiag.SelectedPath;
        }
        public void SetDBConfigration(string dbName)
        {
            databases_DropDownOpened(null, null);
            databases.SelectedItem = dbName;
        }

        private void databases_DropDownOpened(object sender, EventArgs e)
        {
            string str_server = server.Text;
            if (str_server == "")
                return;

            string str_conn = @"Data Source=" + str_server + ";Initial Catalog=master;Integrated Security=True"; 
            SqlConnection sql_conn = new SqlConnection();
            try
            {
                sql_conn.ConnectionString = str_conn;
                sql_conn.Open();
                if (sql_conn.State == ConnectionState.Open)
                {
                    SqlCommand sql_comm = new SqlCommand();
                    sql_comm.Connection = sql_conn;
                    sql_comm.CommandText = "select Name from sysdatabases where Name not in ('master', 'model','msdb','tempdb')";
                    SqlDataReader dr = sql_comm.ExecuteReader();
                    List<string> dbs = new List<string>();
                    while(dr.Read())
                    {
                        dbs.Add(dr[0].ToString());
                    }
                    databases.ItemsSource = dbs;

                    dr.Close();
                    sql_conn.Close();
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("连接失败");
                
            }
        }

        private void databases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            db_config = @"Data Source=" + server.Text + ";Initial Catalog=" + e.AddedItems[0].ToString() + ";Integrated Security=True";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            items_list.ItemsSource = Record_Items;
            if (((MainWindow)System.Windows.Application.Current.MainWindow).main_proj != null)
                Res_list.ItemsSource = ((MainWindow)System.Windows.Application.Current.MainWindow).main_proj.Param_AppRes;
        }

        private void Add_NewRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (Item_Name.Text == "")
                return;

            foreach(RecordItem ri in Record_Items)
            {
                if (ri.Name == Item_Name.Text.Trim())
                    return;
            }

            Record_Items.Add(new RecordItem() { Name = Item_Name.Text.Trim(), Desc = Item_Desc.Text.Trim() });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = sender as System.Windows.Controls.Button;
            string iName = bt.Tag.ToString();
            foreach (RecordItem ri in Record_Items)
            {
                if (ri.Name == iName)
                {
                    Record_Items.Remove(ri);
                    return;
                }
            }

        }

        private void Add_NewResButton_Click(object sender, RoutedEventArgs e)
        {
            if (Res_Name.Text == "")
                return;

            foreach(string res in (ObservableCollection<string>)Res_list.ItemsSource)
            {
                if (res == Res_Name.Text.Trim())
                    return;
            }
            ((ObservableCollection<string>)Res_list.ItemsSource).Add(Res_Name.Text.Trim());
        }
    }
}

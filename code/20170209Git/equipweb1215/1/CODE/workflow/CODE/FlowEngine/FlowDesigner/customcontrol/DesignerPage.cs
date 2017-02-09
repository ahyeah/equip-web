using FlowDesigner.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Control;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace FlowDesigner.customcontrol
{
    class DesignerPage : CloseableTab
    {
        public WorkFlowMan parent_man { get; set; }

        private Canvas main_Canvas = null;
        private ListView params_List = null;
        private Button ImportButton = null;
        private ComboBox WorkFlows = null;

        public Canvas Client
        {
            get
            {
                return main_Canvas;
            }
        }

        public ListView ParamsList
        {
            get
            {
                return params_List;
            }
        }

        public DesignerPage()
        {
            string grid_xaml = "<Grid xmlns = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
                                "   <Grid.ColumnDefinitions>" +
                                "       <ColumnDefinition Width=\"164*\"/>" +
                                "        <ColumnDefinition Width=\"405*\"/>" +
                                "   </Grid.ColumnDefinitions>" +
                                "</Grid>";
            StringReader sr_grid = new StringReader(grid_xaml);
            XmlTextReader xr_grid = new XmlTextReader(sr_grid);
            Grid grid = (Grid)(XamlReader.Load(xr_grid));

            string grid_inner = @"<Grid Grid.Column='0' xmlns = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                                    <Grid.ColumnDefinitions>
                                        <ColumnDefinition Width='90*'/>
                                        <ColumnDefinition Width='74*'/>
                                    </Grid.ColumnDefinitions>
                                    <Grid.RowDefinitions>
                                        <RowDefinition Height='27'/>
                                        <RowDefinition />
                                    </Grid.RowDefinitions>
                                    <ComboBox Name='Events_List' Grid.Column='0' Grid.Row='0' Margin='5, 2, 0, 0' DisplayMemberPath='Name'/>
                                    <Button IsEnabled='false' Name='Import_Button' Content='导入' Width='50' Grid.Column='1' Grid.Row='0'/>                                    
                                   </Grid>";
            Grid inner_grid = (Grid)(XamlReader.Load(new XmlTextReader(new StringReader(grid_inner))));
            WorkFlows = (inner_grid.FindName("Events_List") as ComboBox);
            WorkFlows.ItemsSource = ((MainWindow)Application.Current.MainWindow).main_proj.GetWorkFlows();
            (inner_grid.FindName("Events_List") as ComboBox).SelectionChanged += this.Events_List_Changed;
            ImportButton = (inner_grid.FindName("Import_Button") as Button);
            ImportButton.Click += this.Import_Click;
  
            string params_xaml = "<ListView xmlns = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation' " + 
                                 "xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' " +
                                 "xmlns:i='clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity' " + 
                                 "xmlns:System='clr-namespace:System;assembly=mscorlib' " +
                                 "Grid.Row=\"1\" Grid.ColumnSpan=\"2\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Stretch\">" +
                                 "  <ListView.Resources>" +
                                 "      <DataTemplate x:Key=\"CeilItem\">" +
                                 "          <ComboBox Width=\"70\" SelectedItem=\"{Binding Type, Mode=TwoWay}\">" +                                 
                                 "              <System:String>string</System:String>" +
                                 "              <System:String>int</System:String>" +
                                 "              <System:String>float</System:String>" +
                                 "              <System:String>bool</System:String>" +
                                 "          </ComboBox>" +
                                 "      </DataTemplate>" +
                                 "  </ListView.Resources>" +
                                 "  <ListView.View>" +
                                 "      <GridView>" +
                                 "          <GridViewColumn Header=\"名称\" DisplayMemberBinding=\"{Binding Path = Name}\"/>" +
                                 "          <GridViewColumn Header=\"类型\" DisplayMemberBinding=\"{Binding Path = Type}\"/>" +
                                 "          <GridViewColumn Header=\"描述\" DisplayMemberBinding=\"{Binding Path = Desc}\"/>" +
                                 "      </GridView>" +
                                 "  </ListView.View>" +
                                 "</ListView>";
            StringReader sr_params = new StringReader(params_xaml);
            XmlTextReader xr_params = new XmlTextReader(sr_params);
            params_List = XamlReader.Load(xr_params) as ListView;
            params_List.MouseDoubleClick += this.ParamDoubleClick;
            params_List.MouseRightButtonUp += this.pop_param_menu;

            inner_grid.Children.Add(params_List as UIElement);
            grid.Children.Add(inner_grid as UIElement);
            grid.Children.Add(XamlReader.Load(new XmlTextReader(new StringReader("<GridSplitter xmlns = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation' Width=\"5\"/>"))) as UIElement);

            StringReader canvas = new StringReader("<Canvas xmlns = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation' Grid.Column=\"1\" VerticalAlignment=\"Stretch\" HorizontalAlignment=\"Stretch\"/>");
            main_Canvas = XamlReader.Load(new XmlTextReader(canvas)) as Canvas;

            grid.Children.Add(main_Canvas);
            this.AddChild(grid);           


            main_Canvas.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        protected void pop_param_menu(object sender, MouseButtonEventArgs e)
        {
            if ((sender as System.Windows.Controls.ListView).SelectedItem != null)
            {
                System.Windows.Controls.ContextMenu cm = Application.Current.MainWindow.FindResource("param_menu") as System.Windows.Controls.ContextMenu;
                cm.PlacementTarget = sender as System.Windows.Controls.ListView;
                cm.Tag = (sender as ListView).SelectedItem;
                cm.IsOpen = true;
            }
        }

        public void LinkClick(MouseButtonEventHandler e)
        {
            main_Canvas.MouseLeftButtonUp += e;
        }

        public void LinkMouseMove(MouseEventHandler e)
        {
            main_Canvas.MouseMove += e;
        }
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            parent_man.Proj.CurrentWF = parent_man;
            parent_man.Proj.win_main.OnConfigItem_Selected(parent_man.SelectedItem);
        }

        protected void ParamDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (params_List.SelectedItem != null)   
                ((MainWindow)Application.Current.MainWindow).Confi_Param((param)params_List.SelectedItem);
        }

        protected void Events_List_Changed(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems[0] as WorkFlowMan).Name != ((MainWindow)Application.Current.MainWindow).main_proj.CurrentWF.Name)
                ImportButton.IsEnabled = true;
        }

        protected void Import_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确实要从工作流" + (WorkFlows.SelectedItem as WorkFlowMan).Name + "中导入变量（重名变量将不被导入）", "确定导入", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;
            foreach(param pa in (WorkFlows.SelectedItem as WorkFlowMan).params_table)
            {
                param newP = new param()
                {
                    Name = pa.Name,
                    Desc = pa.Desc,
                    Type = pa.Type
                };
                
                if (((MainWindow)Application.Current.MainWindow).main_proj.CurrentWF.params_table.Where(s => s.Name == newP.Name).Count() == 0)
                {
                    ((MainWindow)Application.Current.MainWindow).main_proj.CurrentWF.params_table.Add(newP);
                }
            }
        }
    }
}

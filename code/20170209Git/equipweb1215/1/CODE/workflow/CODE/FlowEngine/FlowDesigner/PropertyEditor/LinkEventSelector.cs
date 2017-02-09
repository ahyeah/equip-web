using FlowDesigner.ConfigItems;
using FlowDesigner.Management;
using System;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace FlowDesigner.PropertyEditor
{
    class LinkEventSelector : PropertyValueEditor
    {
        public LinkEventSelector()
        {
            string template = @"
                <DataTemplate
                    xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:pe='clr-namespace:System.Activities.Presentation.PropertyEditing;assembly=System.Activities.Presentation'
                    xmlns:System='clr-namespace:System;assembly=mscorlib'>
                    <DockPanel LastChildFill='True'>                        
                        <ComboBox SelectedItem='{Binding Value}' Margin='2,0,0,0' VerticalAlignment='Center'>";

            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            ObservableCollection<WorkFlowMan> wfs = mw.main_proj.GetWorkFlows();

            foreach (WorkFlowMan wf in wfs)
            {
                if (mw.main_proj.CurrentWF.Name != wf.Name)
                    template += ("<System:String>" + wf.Name + "</System:String>");
            }
            template += @"</ComboBox>
                    </DockPanel>
                </DataTemplate>";
            

            using (var sr = new MemoryStream(Encoding.UTF8.GetBytes(template)))
            {
                this.InlineEditorTemplate = XamlReader.Load(sr) as DataTemplate;                
                
            }
        }
    }
}

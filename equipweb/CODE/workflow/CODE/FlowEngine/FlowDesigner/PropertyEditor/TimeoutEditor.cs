using FlowDesigner.ConfigItems;
using System;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FlowDesigner.PropertyEditor
{
    public class TimeoutEditor : DialogPropertyValueEditor
    {
        public TimeoutEditor()
        {
            string template = @"
                <DataTemplate
                    xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:pe='clr-namespace:System.Activities.Presentation.PropertyEditing;assembly=System.Activities.Presentation'>
                    <DockPanel LastChildFill='True'>
                        <pe:EditModeSwitchButton TargetEditMode='Dialog' Name='EditButton_Action' 
                        DockPanel.Dock='Right'>...</pe:EditModeSwitchButton>
                        <TextBlock Text='设置' Margin='2,0,0,0' VerticalAlignment='Center'/>
                    </DockPanel>
                </DataTemplate>";

            using (var sr = new MemoryStream(Encoding.UTF8.GetBytes(template)))
            {
                this.InlineEditorTemplate = XamlReader.Load(sr) as DataTemplate;
            }
        }

        public override void ShowDialog(PropertyValue propertyValue, System.Windows.IInputElement commandSource)
        {
            TimeoutWin wn = new TimeoutWin();
            Dictionary<string, object> action = propertyValue.Value as Dictionary<string, object>;

            wn.time_start = action.ContainsKey("start") ? (action["start"] as string) : "";
            wn.time_offset = action.ContainsKey("offset") ? (action["offset"] as TimeSpan?) : null;
            wn.time_exact = action.ContainsKey("exact") ? (action["exact"] as DateTime?) : null;
            wn.time_callback = action.ContainsKey("callback") ? (action["callback"] as string) : "";
            wn.time_action = action.ContainsKey("action") ? (action["action"] as string) : "";

            if (wn.ShowDialog().Equals(true))
            {
                var ownerActivityConverter = new ModelPropertyEntryToOwnerActivityConverter();
                ModelItem activityItem = ownerActivityConverter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null) as ModelItem;
                using (ModelEditingScope editingScope = activityItem.BeginEdit())
                {
                    Dictionary<string, object> newValue = new Dictionary<string, object>();
                    newValue["start"] = wn.time_start;
                    newValue["offset"] = wn.time_offset;
                    newValue["exact"] = wn.time_exact;
                    newValue["action"] = wn.time_action;
                    newValue["callback"] = wn.time_callback;

                    propertyValue.Value = newValue;
                    editingScope.Complete();

                    var control = commandSource as Control;
                    var oldData = control.DataContext;
                    control.DataContext = null;
                    control.DataContext = oldData;
                }

            }
        }
    }
}

using FlowDesigner.ConfigItems;
using System;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Design;
using System.Windows.Markup;

namespace FlowDesigner.PropertyEditor
{
    public class EventActionEditor : DialogPropertyValueEditor
    {
        public EventActionEditor()
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
            EventActionWin wn = new EventActionWin();
            EventActionDefine action = propertyValue.Value as EventActionDefine;

            wn.action_url = action.url;
            wn.action_params = action.action_params;
            wn.event_linkParams = action.event_params;

            if (wn.ShowDialog().Equals(true))
            {
                var ownerActivityConverter = new ModelPropertyEntryToOwnerActivityConverter();
                ModelItem activityItem = ownerActivityConverter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null) as ModelItem;
                using (ModelEditingScope editingScope = activityItem.BeginEdit())
                {
                    EventActionDefine newValue = new EventActionDefine();
                    newValue.url = wn.action_url;
                    newValue.action_params = wn.action_params;
                    newValue.event_params = wn.event_linkParams;

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

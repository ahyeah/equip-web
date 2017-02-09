using FlowDesigner.PropertyEditor;
using System;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace FlowDesigner.ConfigItems
{
    public class EventActionDefine
    {
        
        public string url { get; set; }

        private Dictionary<string, string> _params = new Dictionary<string, string>();

        public Dictionary<string, string> action_params
        {
            get
            {
                return _params;
            }
            set
            {
                _params = value;
            }
        }

        public Dictionary<string, string> event_params;
    }
    /// <summary>
    /// 事件的定制元素
    /// </summary>
    public class ConfigEvent : ConfigItem
    {
        /// <summary>
        /// 宽度
        /// </summary>
        private int nWidth;

        /// <summary>
        /// 高度
        /// </summary>
        private int nHeight;

        /// <summary>
        /// 事件的类型
        /// </summary>
        private string eventType;

        private EventActionDefine _beforeaction = new EventActionDefine();
        private EventActionDefine _aftereaction = new EventActionDefine();

        //事件
        [CategoryAttribute("事件")]
        [Editor(typeof(EventActionEditor), typeof(PropertyValueEditor))]
        public EventActionDefine BeforeAction {
            get
            {
                return _beforeaction;
            }
            set {
                _beforeaction = value;
            }
        }
        [CategoryAttribute("事件")]
        public string CurrentAction { get; set; }
        [CategoryAttribute("事件")]
        [Editor(typeof(EventActionEditor), typeof(PropertyValueEditor))]
        public EventActionDefine AfterAction {
            get {
                return _aftereaction;
            }
            set
            {
                _aftereaction = value;
            }
        }
        [CategoryAttribute("权限")]
        public string authority { get; set; }

        //关联变量
        private Dictionary<string, string> _LinkParams = new Dictionary<string, string>();
        [Category("参数")]
        [Editor(typeof(LinkParamsEditor), typeof(PropertyValueEditor))]
        public object Link_Params
        { 
            get 
            {
                return _LinkParams;
            } 
            set 
            {
                _LinkParams = (value as Dictionary<string, string>);
            } 
        }

        //timeout
        private Dictionary<string, object> _TimeOut = new Dictionary<string, object>();
        [Category("超时")]
        [Editor(typeof(TimeoutEditor), typeof(PropertyValueEditor))]
        public object Time_Out
        {
            get
            {
                return _TimeOut;
            }
            set
            {
                _TimeOut = (value as Dictionary<string, object>);
            }
        }

        public Dictionary<string, string> LinkParams
        { 
            get{
                return _LinkParams;
            }
            set {
                _LinkParams = value;
            }
        }

        public string GetEventType()
        {
            return eventType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigEvent(string eType)
        {
            nWidth = 150;
            nHeight = 70;

            Color bkColor = Color.FromRgb(255, 255, 255) ;
            Color storkColor = Color.FromRgb(255, 255, 255);
            eventType = eType;
            switch(eventType)
            {
                case "NormalEvent":
                    bkColor = Color.FromRgb(51, 144, 208);
                    storkColor = Color.FromRgb(17, 50, 72);
                    break;

                case "StartEvent":
                    bkColor = Color.FromRgb(4, 181, 13);
                    storkColor = Color.FromRgb(2, 113, 7);
                    break;

                case "EndEvent":
                    bkColor = Color.FromRgb(255, 0, 128);
                    storkColor = Color.FromRgb(162, 0, 81);
                    break;

                case "SubProcess":
                    bkColor = Color.FromRgb(255, 255, 0);
                    storkColor = Color.FromRgb(128, 128, 0);
                    break;

                case "LoopEvent":
                    bkColor = Color.FromRgb(163, 73, 164);
                    storkColor = Color.FromRgb(88, 39, 88);
                    break;
            }

            VisualBrush myVisualBrush = new VisualBrush();

            StackPanel myStackPanel = new StackPanel();
            myStackPanel.Background = new SolidColorBrush(bkColor);

            TextBlock somtext = new TextBlock();
            FontSizeConverter myFontSizeConverter = new FontSizeConverter();
            somtext.FontSize = 0.7F;//(double)myFontSizeConverter.ConvertFrom("6px");
            somtext.FontFamily = new FontFamily("Arial");
            somtext.Text = name + "\r\n" + description;
            somtext.Margin = new Thickness(2);
            myStackPanel.Children.Add(somtext);
            

            myVisualBrush.Visual = myStackPanel;

            Show_item = new Rectangle()
            {
                Width = nWidth,
                Height = nHeight,
                Fill = myVisualBrush,//new SolidColorBrush(Color.FromRgb(51, 144, 208)),
                Stroke = new SolidColorBrush(storkColor),
                StrokeThickness = 2,
                Cursor = Cursors.Cross,
                Opacity = 0.6,
                RadiusX = 5,
                RadiusY = 5
            };

            builidEventLink();

            _beforeaction.event_params = this.LinkParams;
            _aftereaction.event_params = this.LinkParams;
        }

        public override void UnSelect()
        {
            m_selected = false;
            Canvas.SetZIndex(this.Show_item, 1);
            Show_item.Opacity = 0.6;
        }

        public override void Selected()
        {
            m_selected = true;
            Canvas.SetZIndex(this.Show_item,  Int16.MaxValue - 1 );
        }
        
        /// <summary>
        /// 重载设置位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void SetPos(int x, int y)
        {
            ((Rectangle)Show_item).Margin = new Thickness(x, y, 0, 0);
            
        }

        protected override void NameChanged()
        {
            ((TextBlock)((StackPanel)((VisualBrush)Show_item.Fill).Visual).Children[0]).Text = name + "\r\n" + description;
        }

        protected override void DescChanged()
        {
            ((TextBlock)((StackPanel)((VisualBrush)Show_item.Fill).Visual).Children[0]).Text = name + "\r\n" + description;
        }

        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && m_selected)
            {
                Point ptCur = e.GetPosition(Show_item);
                double dx = ptCur.X - pt.X;
                double dy = ptCur.Y - pt.Y;

                Thickness tk = ((Rectangle)Show_item).Margin;
                tk.Left += (int)dx;
                tk.Top += (int)dy;
                ((Rectangle)Show_item).Margin = tk;
                //pt = ptCur;
                //Trace.WriteLine("X = " + ptCur.X + "; Y = " + ptCur.Y);

                foreach (ConfigFlow cf in attach_flows)
                    cf.Connect();
            }
        }

        public override XmlElement SaveConfigItem(XmlDocument Owner)
        {
            //base
            XmlElement base_xml = base.SaveConfigItem(Owner);
            base_xml.SetAttribute("item_type", "event");
            base_xml.SetAttribute("event_type", eventType);
            base_xml.SetAttribute("authority", authority);

            //shape
            XmlElement shape_xml = Owner.CreateElement("shape");
            shape_xml.SetAttribute("type", Show_item.ToString());
            shape_xml.SetAttribute("margin", Show_item.Margin.ToString());            
            base_xml.AppendChild(shape_xml);

            //actions
            XmlElement actions_xml = Owner.CreateElement("actions");
            //beforeaction
            XmlElement ba = Owner.CreateElement("beforeaction");
            ba.SetAttribute("url", BeforeAction.url);
            XmlElement ba_pars = Owner.CreateElement("action_params");
            foreach(var pa in BeforeAction.action_params)
            {
                XmlElement xpa = Owner.CreateElement("param");
                xpa.SetAttribute("name", pa.Key);
                xpa.InnerText = pa.Value;
                ba_pars.AppendChild(xpa);
            }
            ba.AppendChild(ba_pars);
            actions_xml.AppendChild(ba);
            //afteraction
            XmlElement aa = Owner.CreateElement("afteraction");
            aa.SetAttribute("url", AfterAction.url);            
            XmlElement aa_pars = Owner.CreateElement("action_params");
            foreach (var pa in AfterAction.action_params)
            {
                XmlElement xpa = Owner.CreateElement("param");
                xpa.SetAttribute("name", pa.Key);
                xpa.InnerText = pa.Value;
                aa_pars.AppendChild(xpa);
            }
            aa.AppendChild(aa_pars);
            actions_xml.AppendChild(aa);
            
            //currentaction
            actions_xml.SetAttribute("currentaction", CurrentAction);            
            base_xml.AppendChild(actions_xml);

            //link_params
            XmlElement pas_xml = Owner.CreateElement("link_params");
            foreach(var pa in _LinkParams)
            {
                XmlElement pa_xml = Owner.CreateElement("param");
                pa_xml.SetAttribute("name", pa.Key);
                pa_xml.SetAttribute("app_reserve", pa.Value);
                pas_xml.AppendChild(pa_xml);
            }
            base_xml.AppendChild(pas_xml);

            //TimeOut
            XmlElement time_xml = Owner.CreateElement("time_out");
            XmlElement time_start = Owner.CreateElement("start");
            if ( _TimeOut.ContainsKey("start") == false || _TimeOut["start"] == null)
                time_start.InnerText = "";
            else
                time_start.InnerText = (_TimeOut["start"] as string);
            time_xml.AppendChild(time_start);

            XmlElement time_offset = Owner.CreateElement("offset");
            if (_TimeOut.ContainsKey("offset") == false || _TimeOut["offset"] == null)
                time_offset.InnerText = "";
            else
            {
                TimeSpan? ts = (_TimeOut["offset"] as TimeSpan?);
                time_offset.AppendChild(Owner.CreateCDataSection(ts.Value.ToString()));
            }
            time_xml.AppendChild(time_offset);

            XmlElement time_exact = Owner.CreateElement("exact");
            if (_TimeOut.ContainsKey("exact") == false || _TimeOut["exact"] == null)
                time_exact.InnerText = "";
            else
            {
                time_exact.AppendChild(Owner.CreateCDataSection((_TimeOut["exact"] as DateTime?).Value.ToString()));
            }
            time_xml.AppendChild(time_exact);

            XmlElement time_action = Owner.CreateElement("action");
            if (_TimeOut.ContainsKey("action") == false || _TimeOut["action"] == null)
            {
                time_action.InnerText = "";
            }
            else
            {
                time_action.InnerText = (_TimeOut["action"] as string);
            }
            time_xml.AppendChild(time_action);

            XmlElement time_callback = Owner.CreateElement("callback");
            if (_TimeOut.ContainsKey("callback") == false || _TimeOut["callback"] == null)
                time_callback.InnerText = "";
            else
                time_callback.AppendChild(Owner.CreateCDataSection(_TimeOut["callback"] as string));
            time_xml.AppendChild(time_callback);
            
            base_xml.AppendChild(time_xml);
    
            return base_xml;
        }
        
    }
}

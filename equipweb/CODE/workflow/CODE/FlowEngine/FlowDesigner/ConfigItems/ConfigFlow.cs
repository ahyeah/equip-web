﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class ConfigFlow : ConfigItem
    {
        [CategoryAttribute("流程属性"),Browsable(false)]
        public ConfigEvent Start {
            get
            {
                return start_event;
            }
            set {
                start_event = value;
                Rectangle SItem = (Rectangle)start_event.GetShowItem();

                Thickness tc = SItem.Margin;
                //Point pt = start_event.GetShowItem().TransformToVisual(Window.GetWindow(SItem.Parent)).Transform(new Point(150, 0));
                //Point pt = new Point(tc.Left + 150, tc.Top + 35);
                //GeometryConverter gc = new GeometryConverter();
                //(Show_item as Path).Data = (Geometry)gc.ConvertFromString("M " + pt.X + "," + pt.Y + " 500, 100");
                start_pt.X = tc.Left + 150;
                start_pt.Y = tc.Top + 35;
                //GeometryConverter gc = new GeometryConverter();
                //(Show_item as Path).Data = (Geometry)gc.ConvertFromString("M " + start_pt.X + "," + start_pt.Y + " 500, 100");
            }
        }
        [CategoryAttribute("流程属性"), Browsable(true)]
        public string StartEvent { get { return Start.name; } }

        [CategoryAttribute("流程属性"),Browsable(false)]
        public ConfigEvent End { get; set; }

        [CategoryAttribute("流程属性"), Browsable(true)]
        public string EndEvent { get { return End.name; } }

        [CategoryAttribute("流程属性")]
        public string Express {get; set;}

        private ConfigEvent start_event = null;
        private Point start_pt = new Point(); 
        public ConfigFlow()
        {
            Show_item = new Path
            {
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Color.FromRgb(128, 128, 128)),
                Cursor = Cursors.Cross
            };
            builidEventLink();
        }
        
        public void DynamicDraw(Point dest)
        {
            GeometryConverter gc = new GeometryConverter();
            (Show_item as Path).Data = (Geometry)gc.ConvertFromString("M " + start_pt.X + "," + start_pt.Y + " " + dest.X + ", " + dest.Y);
        }
        public void Connect()
        {
            Rect bb1 = new Rect(Start.GetShowItem().Margin.Left,
                                Start.GetShowItem().Margin.Top,
                                Start.GetShowItem().Width,
                                Start.GetShowItem().Height);
            Rect bb2 = new Rect(End.GetShowItem().Margin.Left,
                                End.GetShowItem().Margin.Top,
                                End.GetShowItem().Width,
                                End.GetShowItem().Height);

            Point[] p = new Point[]{
                new Point{ X = bb1.X + bb1.Width / 2, Y = bb1.Y - 1},
                new Point{ X = bb1.X + bb1.Width / 2, Y = bb1.Y + bb1.Height + 1},
                new Point{ X = bb1.X - 1, Y = bb1.Y + bb1.Height / 2},
                new Point{ X = bb1.X + bb1.Width + 1, Y = bb1.Y + bb1.Height / 2},
                new Point{ X = bb2.X + bb2.Width / 2, Y = bb2.Y - 1},
                new Point{ X = bb2.X + bb2.Width / 2, Y = bb2.Y + bb2.Height + 1},
                new Point{ X = bb2.X - 1, Y = bb2.Y + bb2.Height / 2},
                new Point{ X = bb2.X + bb2.Width + 1, Y = bb2.Y + bb2.Height / 2}
            };

            List<double> dis = new List<double>();
            Dictionary<double, int[]> d = new Dictionary<double, int[]>();
            for (int i = 0; i < 4; i ++)
            {
                for (int j = 4; j < 8; j ++)
                {
                    double dx = Math.Abs(p[i].X - p[j].X),
                           dy = Math.Abs(p[i].Y - p[j].Y);

                    if ((i == j - 4) || (((i != 3 && j != 6) || p[i].X < p[j].X) && ((i != 2 && j != 7) || p[i].X > p[j].X) && ((i != 0 && j != 5) || p[i].Y > p[j].Y) && ((i != 1 && j != 4) || p[i].Y < p[j].Y)))
                    {
                        dis.Add(dx + dy);
                        d[dis[dis.Count - 1]] = new int[] {i, j};
                    }
                }
            }
            int[] res = {0, 4};
            if (dis.Count == 0)
            {
                res[0] = 0; res[1] = 4;
            }
            else
            {
                res = d[dis.Min()];
            }
            double x1 = p[res[0]].X,
                   y1 = p[res[0]].Y,
                   x4 = p[res[1]].X,
                   y4 = p[res[1]].Y;
            double dx1 = Math.Max(Math.Abs(x1 - x4) / 2, 10),
                   dy1 = Math.Max(Math.Abs(y1 - y4) / 2, 10);

            double x2 = (new double[] { x1, x1, x1 - dx1, x1 + dx1 })[res[0]],
                   y2 = (new double[] { y1 - dy1, y1 + dy1, y1, y1 })[res[0]],
                   x3 = (new double[] { 0, 0, 0, 0, x4, x4, x4 - dx1, x4 + dx1 })[res[1]],
                   y3 = (new double[] { 0, 0, 0, 0, y1 + dy1, y1 - dy1, y4, y4 })[res[1]];

            string strPath = "M " + x1 + ", " + y1 + " C " + x2 + ", " + y2 + " " + x3 + ", " + y3 + " " + x4 + ", " + y4 + "";

            //画箭头
            double arr_angle = (30.0 / 180) * Math.PI;
            double angle = Math.Atan((y3 - y4) / (x3 - x4));
            if (angle == 0.0 && (x3 - x4) < 0)
                angle = Math.PI;

            double sin1 = Math.Sin(angle - arr_angle);
            double cos1 = Math.Cos(angle - arr_angle);
            double sin2 = Math.Sin(angle + arr_angle);
            double cos2 = Math.Cos(angle + arr_angle);
            double x5 = x4 + 10 * cos1,
                   y5 = y4 + 10 * sin1,
                   x6 = x4 + 10 * cos2,
                   y6 = y4 + 10 * sin2;
            strPath += " L " + x5 + ", " + y5 + " M " + x4 + ", " + y4 + " L " + x6 + ", " + y6 + "";
            //strPath += " L " + x5 + ", " + y5;

            GeometryConverter gc = new GeometryConverter();
            (Show_item as Path).Data = (Geometry)gc.ConvertFromString(strPath);
        }

        protected override void NameChanged()
        {
            //throw new NotImplementedException();
        }

        protected override void DescChanged()
        {
            //throw new NotImplementedException();
        }

        protected override void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public override void SetPos(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void Selected()
        {
            //throw new NotImplementedException();
            Show_item.Stroke = new SolidColorBrush(Color.FromRgb(255, 128, 128));
        }

        public override void UnSelect()
        {
            Show_item.Stroke = new SolidColorBrush(Color.FromRgb(128, 128, 128));
        }

        public override XmlElement SaveConfigItem(XmlDocument Owner)
        {
            XmlElement base_xml = base.SaveConfigItem(Owner);
            base_xml.SetAttribute("item_type", "flow");
            base_xml.SetAttribute("condition", Express);

            XmlElement shape_xml = Owner.CreateElement("shape");
            shape_xml.SetAttribute("type", Show_item.ToString());
            shape_xml.SetAttribute("source", Start.name);
            shape_xml.SetAttribute("dest", End.name);
            
            base_xml.AppendChild(shape_xml);

            return base_xml;
        }
    }
}

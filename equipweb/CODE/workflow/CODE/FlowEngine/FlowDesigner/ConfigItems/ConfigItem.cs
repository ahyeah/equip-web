﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml;

namespace FlowDesigner.ConfigItems
{
    /// <summary>
    /// 定制元素的基类
    /// </summary>
    public abstract class ConfigItem
    {
        protected List<ConfigFlow> attach_flows = new List<ConfigFlow>();
        public void AttachFlows(ConfigFlow cf)
        {
            if (cf != null)
                attach_flows.Add(cf);
        }

        public void DetachFlow(ConfigFlow cf)
        {
            attach_flows.Remove(cf);
        }

        public List<ConfigFlow> GetAttachFolws()
        {
            return attach_flows;
        }
        /// <summary>
        /// 代表该元素的形状
        /// </summary>
        protected Shape Show_item;

        /// <summary>
        /// 获得显示该元素的形状
        /// </summary>
        /// <returns></returns>
        public Shape GetShowItem()
        {
            return Show_item;
        }

        /// <summary>
        /// 名称
        /// </summary>
        private string m_name;
        //元素的名称
        [CategoryAttribute("基本属性")]
        public string name { 
            get
            {
                return m_name;
            }
            set { 
                bool changed = false;
                if (m_name != value)
                    changed = true;
                m_name = value;

                if (changed)
                    NameChanged();
            }
        }

        

        protected abstract void NameChanged();

        /// <summary>
        /// 描述
        /// </summary>
        private string m_description;
        //元素的描述
        [CategoryAttribute("基本属性")]
        public string description {
            get {
                return m_description;
            }
            set{
                bool changed = false;
                if (m_description != value)
                    changed = true;
                m_description = value;

                if (changed)
                    DescChanged();
            }
        }

        protected abstract void DescChanged();

        protected Point pt = new Point();
        /// <summary>
        /// 设置元素的位置
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public abstract void SetPos(int x, int y);

        public abstract void UnSelect();
        public abstract void Selected();
        

        public virtual XmlElement SaveConfigItem(XmlDocument Owner)
        {
            XmlElement item = Owner.CreateElement("item");

            item.SetAttribute("name", name);
            item.SetAttribute("desc", description);
            
            return item;
        }

        #region 事件注册
        /// <summary>
        /// 选中元素处理
        /// </summary>
        protected bool m_selected = false;
        protected void builidEventLink()
        {
            if (Show_item == null)
                return;

            Show_item.MouseLeftButtonUp += this.RaiseSelectedEvent;
            Show_item.MouseLeftButtonDown += this.OnLeftButtonDown;
            Show_item.MouseRightButtonUp += this.RaiseRButtonEvent;
            Show_item.MouseMove += this.OnMouseMove;
        }
        public delegate void selectedHandler(ConfigItem sender);
        public event selectedHandler OnSelected;

        public delegate void OnRButtonUpHandler(object sender, MouseButtonEventArgs e);
        public event OnRButtonUpHandler OnRightButtonUp;
        public void RaiseSelectedEvent(object sender, MouseButtonEventArgs e)
        {
            //pt = e.GetPosition(Show_item);
            //if (m_selected)
            //{
                //m_selected = true;
            //    Show_item.Opacity = 0.9;

            //    if (OnSelected != null)
            //        OnSelected(this);
            //}
            
        }

        public void RaiseRButtonEvent(object sender, MouseButtonEventArgs e)
        {
            if (OnRightButtonUp != null)
                OnRightButtonUp(this, e);
        }

        protected abstract void OnMouseMove(object sender, MouseEventArgs e);

        protected void OnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pt = e.GetPosition(Show_item);
            m_selected = true;
            Show_item.Opacity = 0.9;

            if (OnSelected != null)
                OnSelected(this);
        }
        #endregion

    }
}

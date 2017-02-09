using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlowDesigner.ConfigItems
{
    class ConfigLoopEvent : ConfigEvent
    {
        public ConfigLoopEvent(string eType) : base(eType)
        {

        }

        [CategoryAttribute("循环设置")]
        public string LoopCondition { get; set; }

        [CategoryAttribute("循环设置")]
        public string TimeWaiting { get; set; }

        public override XmlElement SaveConfigItem(XmlDocument Owner)
        {
            XmlElement base_xml = base.SaveConfigItem(Owner);

            XmlElement loopSetting = Owner.CreateElement("LoopSetting");
            XmlElement loopcondition = Owner.CreateElement("condition");
            loopcondition.AppendChild(Owner.CreateCDataSection(LoopCondition));
            loopSetting.AppendChild(loopcondition);

            XmlElement waitingtime = Owner.CreateElement("waiting_time");
            waitingtime.AppendChild(Owner.CreateCDataSection(TimeWaiting));
            loopSetting.AppendChild(waitingtime);

            base_xml.AppendChild(loopSetting);
            return base_xml;
        }
    }
}

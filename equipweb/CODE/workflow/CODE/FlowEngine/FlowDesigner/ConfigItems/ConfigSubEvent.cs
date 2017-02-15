using FlowDesigner.PropertyEditor;
using System;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlowDesigner.ConfigItems
{
    public enum WorkModel { parallel, serial};

    public enum transfer_direction {from, to};
    public class paramtransfer_item
    {
        public string parent { get; set; }
        public string child { get; set; }
        public transfer_direction direction {get; set;}
    }

    class ConfigSubEvent : ConfigEvent
    {
        public ConfigSubEvent(string eType) : base(eType)
        {

        }

        [CategoryAttribute("子工作流")]
        public WorkModel WorkingModel { get; set; }

        public void SetWorkModel(string wm)
        {
            switch(wm)
            {
                case "parallel":
                    WorkingModel = WorkModel.parallel;
                    break;

                case "serial":
                    WorkingModel = WorkModel.serial;
                    break;
            }
        }

        [CategoryAttribute("子工作流")]
        [Editor(typeof(LinkEventSelector), typeof(PropertyValueEditor))]
        public string LinkWorkFLow { get; set; }

        private List<paramtransfer_item> _paramtransfer = new List<paramtransfer_item>();
        [CategoryAttribute("子工作流")]
        [Editor(typeof(ParamTransferEditor), typeof(PropertyValueEditor))]
        public List<paramtransfer_item> ParamTransfer
        {
            get
            {
                return _paramtransfer;
            }
            set
            {
                _paramtransfer = value;
            }
        }

        public override XmlElement SaveConfigItem(XmlDocument Owner)
        {
            //base
            XmlElement base_xml = base.SaveConfigItem(Owner);
            
            //LinkWrokFlow
            XmlElement linkwf = Owner.CreateElement("LinkWorkflow");
            linkwf.SetAttribute("name", LinkWorkFLow);
            linkwf.SetAttribute("model", WorkingModel.ToString("G"));

            foreach(paramtransfer_item pi in ParamTransfer)
            {
                XmlElement pi_xml = Owner.CreateElement("ParamTransfer");
                pi_xml.SetAttribute("source", pi.parent);
                pi_xml.SetAttribute("dest", pi.child);
                pi_xml.SetAttribute("direction", pi.direction.ToString());
                linkwf.AppendChild(pi_xml);
            }

            base_xml.AppendChild(linkwf);
            return base_xml;
        }
    }
}

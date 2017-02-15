using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlowDesigner.Management
{
    public class RecordItem
    {
        public string Name { get; set; }

        public string Desc { get; set; }

    }
    /// <summary>
    /// 工作流工程
    /// </summary>
    public class WorkFlowsProj
    {
        #region private property
        /// <summary>
        /// 包含的工作流管理
        /// </summary>
        private ObservableCollection<WorkFlowMan> wfs_man = new ObservableCollection<WorkFlowMan>();

       

        #endregion

        #region public property
        /// <summary>
        /// 工程名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 工程描述
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 保存的路径
        /// </summary>
        public string Path { set; get; }

        public string DB_Config { get; set; }

        public ObservableCollection<RecordItem> Record_Items { get; set; }
        
        public WorkFlowMan CurrentWF { get; set; }
        public MainWindow win_main { get; set; }

        public ObservableCollection<string> Param_AppRes = new ObservableCollection<string>();
        #endregion

        #region method
        /// <summary>
        /// 添加一个新工作流管理
        /// </summary>
        /// <param name="wfName">工作流名称</param>
        /// <param name="wfDesc">工作流描述</param>
        /// <returns>添加成功则返回新建的WorkFlowMan</returns>
        public WorkFlowMan NewWrokFlowMan(string wfName, string wfDesc, bool init = true)
        {
            foreach(WorkFlowMan i in wfs_man)
            {
                if (i.Name == wfName)
                    return null;
            }

            WorkFlowMan wfm = new WorkFlowMan(wfName, wfDesc);
            wfm.Proj = this;
            if (init == true)
                wfm.InitStartAndEnd();
            wfs_man.Add(wfm);
            return wfm;
        }

        public WorkFlowMan DeleteWorkFlowMan(string wfName)
        {
            WorkFlowMan deleted = null;
            foreach (WorkFlowMan i in wfs_man)
            {
                if (i.Name == wfName)
                {
                    wfs_man.Remove(i);
                    deleted = i;
                    break;
                }
            }
            foreach (WorkFlowMan j in wfs_man)
            {
                j.UnlinkOtherWFM(deleted);
            }


            return deleted;
        }

        public void SaveWorkFlowProj()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement projNode = doc.CreateElement("work_wlow_project");
            projNode.SetAttribute("name", Name) ;
            projNode.SetAttribute("db", DB_Config);

            foreach (WorkFlowMan wfm in wfs_man)
                projNode.AppendChild(wfm.SaveWorkFlow(doc));

            

            XmlElement records = doc.CreateElement("record_items");
            foreach(RecordItem ri in Record_Items)
            {
                XmlElement item = doc.CreateElement("item");
                item.SetAttribute("name", ri.Name);
                item.SetAttribute("desc", ri.Desc);
                records.AppendChild(item);
            }
            projNode.AppendChild(records);

            XmlElement dbcon = doc.CreateElement("database");
            dbcon.SetAttribute("connection", DB_Config);
            projNode.AppendChild(dbcon);

            XmlElement app_res = doc.CreateElement("param_app");
            foreach(string res in Param_AppRes)
            {
                XmlElement res_item = doc.CreateElement("item");
                res_item.InnerText = res;
                app_res.AppendChild(res_item);
            }
            projNode.AppendChild(app_res);

            doc.AppendChild(projNode);

            doc.Save(Path.TrimEnd('\\') + "\\" + Name + ".wfproj");
        }

        public bool LoadWorkFlowProj(XmlElement source)
        {
            XmlElement projNode = source;
            if (projNode.Name != "work_wlow_project")
                return false;

            Name = projNode.GetAttribute("name");
            DB_Config = projNode.GetAttribute("db");
            XmlNodeList wfs = projNode.SelectNodes("work_flow");
            foreach (XmlNode xn in wfs)
            {
                XmlElement xe = (XmlElement)xn;
                WorkFlowMan wfm = NewWrokFlowMan(xe.GetAttribute("name"), xe.GetAttribute("desc"), false);
                if (!wfm.LoadWorkFlow(xe))
                    return false;
            }

            XmlNodeList records = projNode.SelectSingleNode("record_items").SelectNodes("item");
            if (Record_Items == null)
                Record_Items = new ObservableCollection<RecordItem>();
            foreach(XmlNode ri in records)
            {
                Record_Items.Add(new RecordItem()
                {
                    Name = ((XmlElement)ri).GetAttribute("name"),
                    Desc = ((XmlElement)ri).GetAttribute("desc")
                });
            }

            DB_Config = projNode.SelectSingleNode("database").Attributes["connection"].Value;

            XmlElement app_res = (XmlElement)projNode.SelectSingleNode("param_app");
            Param_AppRes.Clear();
            if (app_res != null)
            {
                
                foreach (XmlNode res in app_res.SelectNodes("item"))
                {
                    Param_AppRes.Add(res.InnerText);
                }
            }
            return true;
        }

        public ObservableCollection<WorkFlowMan> GetWorkFlows()
        {
            return wfs_man;
        }

        public WorkFlowMan GetWorkFlow(string wf_name)
        {
            foreach(WorkFlowMan wfm in wfs_man)
            {
                if (wfm.Name == wf_name)
                    return wfm;
            }
            return null;
        }

        public bool Complie()
        {
            foreach (WorkFlowMan wfm in wfs_man)
            {
                string wfPath = Path.TrimEnd('\\') + "\\workflows\\" + wfm.Name + ".wfd";

                if (!wfm.Complie(wfPath))
                    return false;

            }

            return true;
        }
        #endregion
    }
}

using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace WebApp.Models
{
    public class WorkFlowInit : DropCreateDatabaseIfModelChanges<WorkFlowContext>
    {
        protected override void Seed(WorkFlowContext context)
        {
            var wd = new WorkFlow_Define { W_Name = "A13dot1", W_Attribution = "缺陷故障管理流程" };
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath(@"~/Scripts/A13dot1.xml"));
            wd.W_Xml = Encoding.Default.GetBytes(doc.InnerXml);
            context.workflow_define.Add(wd);
            //base.Seed(context);

            wd = new WorkFlow_Define { W_Name = "A11dot2", W_Attribution = "设备隐患排查流程" };            
            doc.Load(HttpContext.Current.Server.MapPath(@"~/Scripts/A11dot2.xml"));
            wd.W_Xml = Encoding.Default.GetBytes(doc.InnerXml);
            context.workflow_define.Add(wd);

            wd = new WorkFlow_Define { W_Name = "A8dot2", W_Attribution = "检修计划实施管理" };
            doc.Load(HttpContext.Current.Server.MapPath(@"~/Scripts/A8dot2.xml"));
            wd.W_Xml = Encoding.Default.GetBytes(doc.InnerXml);
            context.workflow_define.Add(wd);

            base.Seed(context);
        }
    }
}
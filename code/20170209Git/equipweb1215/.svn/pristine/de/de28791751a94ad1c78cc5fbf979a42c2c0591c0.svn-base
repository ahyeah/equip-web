using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace MVCTest.Models
{
    public class WorkFlowInit : DropCreateDatabaseIfModelChanges<WorkFlowContext>
    {
        protected override void Seed(WorkFlowContext context)
        {
            var wd = new WorkFlow_Define { W_Name = "holiday", W_Attribution = "请假审批流程" };
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath(@"~/Scripts/holiday.xml"));
            wd.W_Xml = Encoding.Default.GetBytes(doc.InnerXml);
            context.workflow_define.Add(wd);
            base.Seed(context);
        }
    }
}
using FlowEngine.DAL;
using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private WorkFlows wf = new WorkFlows();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<WorkFlow_Define> wf_def = wf.GetAllWorkFlows();
        }
    }
}
using FlowEngine.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.UserInterface
{
    public interface UI_WorkFlow_Entity
    {
        string name { get; }

        int EntityID { get; }

        string description { get; }

        string EntitySerial { get; }

        bool CreateEntity(string wf_name, string Ser_Num = null/*2016/2/12--保证子工作流串号与父工作流相同*/);

        //工作流引擎开始工作，并返回需要跳转的页面
        string Start(IDictionary<string, string> record);
         IDictionary<string, IEvent> events{get;}

        Dictionary<string, string> GetRecordItems();

        IEvent GetCurrentEvent();
    }
}

using FlowEngine;
using FlowEngine.DAL;
using FlowEngine.Flow;
using FlowEngine.Modals;
using FlowEngine.Param;
using FlowEngine.TimerManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace EXPRESS_TEST
{
    class Program
    {      
        static void Main(string[] args)
        {
            CTimerManage.InitTimerManager();
            CTimerManage.Start();
            var wfe = CWFEngine.CreateAWFEntityByName("test1");
            wfe.Start(null);
            CWFEngine.SubmitSignal(wfe.EntityID, new Dictionary<string, string>());

            
            Thread.Sleep(10 * 60 * 1000);
        }
    }
}

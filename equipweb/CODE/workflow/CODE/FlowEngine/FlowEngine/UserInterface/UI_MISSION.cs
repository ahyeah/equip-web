﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.UserInterface
{
    /// <summary>
    /// 用户用以返回任务的类
    /// </summary>
    public class UI_MISSION
    {
        public int Miss_Id {get; set;}
        //任务所属工作流实体的ID
        public int WE_Entity_Id { get; set; }

        public string WE_Entity_Ser { get; set; }

        //任务所属工作流名称
        public string WE_Name { get; set; }

        //任务所对应的事件(Event)名称
        public string WE_Event_Name { get; set; }

        //任务所对应事件(Event)描述
        public string WE_Event_Desc { get; set; }

        //处理任务的页面地址
        public string Mission_Url { get; set; }

        //任务关联的参数列表-(参数名称，参数值）
        public Dictionary<string, object> Miss_Params = new Dictionary<string, object>();

        //任务关联参数对应的描述-(参数名称，参数描述）用于显示-xwm modify
        public Dictionary<string, object> Miss_ParamsDesc = new Dictionary<string, object>();

        //任务关联参数对应的App_Res (参数名称， 参数的App_res)
        public Dictionary<string, string> Miss_ParamsAppRes = new Dictionary<string, string>();
    }
}

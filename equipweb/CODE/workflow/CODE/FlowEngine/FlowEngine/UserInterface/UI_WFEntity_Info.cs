﻿using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.UserInterface
{
    public class UI_WFEntity_Info
    {
        public string name { get; set; }

        public int EntityID { get; set; }

        public string description { get; set; }

        public string serial { get; set; }
        public string Binary { get; set; }

        public WE_STATUS Status
        {
            get;
            set;
        }

        public DateTime? Last_TransTime
        {
            get;
            set;
        }

    }
}

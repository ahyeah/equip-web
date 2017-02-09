using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment
{
    public class TreeListNode
    {   
        public  class stateModel
        {
            public stateModel()
            { selected = false; }
            public  bool selected { get; set; }
        }
        public string text{get;set;}
        public int id { get; set; }
        public bool selectable{get;set;}
        public stateModel state { get; set; }
       public  List<TreeListNode> nodes{get;set;}
       public TreeListNode()
       {
           nodes=new List<TreeListNode>();
           state = new stateModel();
       }

    }
}

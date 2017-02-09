using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment.MenuConfig
{
    public class MenuListNode
    {
        public MenuListNode()
        {
            Childs = new List<int>();
        }
        public int Menu_Id { get; set; }

        //菜单名称
        public string Menu_Name { get; set; }

        //菜单连接的URL
        public string Link_Url { get; set; }
        public string url_id { get; set; }

        //菜单对应ICON
        public string Menu_Icon { get; set; }

        public int Parent_id { get; set; }

        public List<int> Childs { get; set; }
        public int level { get; set; }
    }
    //2016-4-28修改开始
    public class MenuListNode1
    {
        public MenuListNode1()
        {
            Childs = new List<int>();
        }
        public int EA_Id { get; set; }

        //菜单名称
        public string EA_Name { get; set; }

        //菜单连接的URL
        public string EA_Code { get; set; }

        //菜单对应ICON
        public string EA_Title { get; set; }

        public int Parent_id { get; set; }

        public List<int> Childs { get; set; }
        public int level { get; set; }
    }

    //结束

}

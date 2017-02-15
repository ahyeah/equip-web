using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment.MenuConfig
{
    public class MenuTree
    {
        public MenuTree()
        {
            childs = new List<MenuTree>();
        }
        public int Menu_Id { get; set; }
        
        //菜单名称
        public string Menu_Name { get; set; }

        //菜单连接的URL
        public string Link_Url { get; set; }

        //菜单对应ICON
        public string Menu_Icon { get; set; }
        
        public List<MenuTree> childs { get; set; }
    }
}

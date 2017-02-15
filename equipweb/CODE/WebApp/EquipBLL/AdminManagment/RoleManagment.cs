using System;
using EquipDAL.Implement;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using EquipModel.Context;
using EquipModel.Entities;




namespace EquipBLL.AdminManagment
{
   public class RoleManagment
    {
        //   角色model........THL
        public class Role_viewModal
        {
            public int Role_Ids;
            public string Role_Names;
            public string Role_Descs;

            public string Menu_Ids;
            public string Menu_Names;
           
            public List<Menu> Menus;


        }

        private Roles RR = new Roles();
        public bool AddPersons(int Role_Id,List<int> PersonIdSet)
        {
            try
            {
                foreach(var i in PersonIdSet)
                {
                    RR.AddPerson(Role_Id, i);
                }
                return true;
            }
            catch { return false; }
        }


        public bool Add_Role(Role_Info R, List<int> Menu_IDs)
        {
            try
            {
                int NewpR_id = RR.AddRole(R);

                RR.Roles_LinkMenus(NewpR_id, Menu_IDs);
                return true;
            }
            catch { return false; }
        }
        public List<Person_Info> getRole_Persons(int Role_Id)
        {
            return RR.getRole_Persons(Role_Id);
        }
        public List<Role_Info> get_ALl_Roles()
        {
            try { return RR.getRoles(); }
            catch { return null; }
        }



        public Role_viewModal Get_RoleModal(int Role_Id)
        {
            int i;

            var r = RR.Get_RoleModal(Role_Id);
            Role_viewModal r_item = new Role_viewModal();
            r_item.Role_Ids = r.RInfo.Role_Id;
            r_item.Role_Names = r.RInfo.Role_Name;
            r_item.Role_Descs = r.RInfo.Role_Desc;


            r_item.Menu_Names = "";
            r_item.Menu_Ids = "";
            if (r.M_Info.Count > 0)
            {
                for (i = 0; i < r.M_Info.Count - 1; i++)
                {
                    r_item.Menu_Ids = r_item.Menu_Ids + (r.M_Info[i]).Menu_Id + ",";
                 r_item.Menu_Names = r_item.Menu_Names + (r.M_Info[i]).Menu_Name + "\r\n";
                }
                r_item.Menu_Ids = r_item.Menu_Ids + (r.M_Info[i]).Menu_Id;
                r_item.Menu_Names = r_item.Menu_Names + (r.M_Info[i]).Menu_Name;
            }
            r_item.Menus = r.M_Info;
            return r_item;

        }
        //public Role_viewModal G_M_Model(int Role_Id)
        //{
        //    int i;
        //    var r = RR.Get_RoleModal(Role_Id);
        //    Role_viewModal r_item = new Role_viewModal();
          
        //    if (r.M_Info.Count > 0)
        //    {
        //        for (i = 0; i < r.M_Info.Count - 1; i++)
        //        {
                    

        //        }
                
        //    }
 
        //}

     



        public bool Update_Role(Role_Info r, List<int> Menu_IDs)
        {
            try
            {
                RR.ModifyRole(r);


                RR.Roles_LinkMenus(r.Role_Id, Menu_IDs);
                return true;
            }
            catch { return false; }
        }


        public bool Delete_Role(int id)
        {
            return RR.DeleteRole(id);
        }

        //  新增角色信息时获取权限树



        // ......专为添加角色菜单时显示使用.....新增加thl
        public List<TreeListNode> BuildMenuList()
        {
            TreeListNode rnode = new TreeListNode();


            rnode.text = "root";
            rnode.selectable = false;

            List<TreeListNode> list = new List<TreeListNode>();
            BuildMenuList_inter(rnode, list);
            return list;
        }

        //  添加角色时系统菜单显示时使用.....新增加thl
        private void BuildMenuList_inter(TreeListNode parent, List<TreeListNode> list)
        {
             Menus M_D=new Menus();
            List<Menu> Childs = M_D.GetChildMenu(parent.text);
            foreach (Menu item in Childs)
            {
                TreeListNode mn = new TreeListNode();
                mn.text = item.Menu_Name;
                mn.id = item.Menu_Id;
                if (M_D.GetChildMenu(item.Menu_Id).Count == 0) mn.selectable = true;


                parent.nodes.Add(mn);
                if (parent.text == "root") list.Add(mn);
                BuildMenuList_inter(mn, list);
            }
        }
    }
}

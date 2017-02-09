using EquipDAL.Implement;
using EquipDAL.Interface;
using EquipModel.Context;
using EquipModel.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment.MenuConfig
{
    public class EquipArchiManagment
    {
        private IEquipArchis db_equip_archi = new Equip_Archis();
        private Equip_Archis EAs = new Equip_Archis();
        public string getEa_namebyId(int Ea_id)
        {

            return EAs.getEa_namebyId(Ea_id);
        }
        public int getEa_idbyname(string Ea_name)
        {

            return EAs.getEa_idbyname(Ea_name);
        }
        public string getEa_codebyname(string Ea_name)
        {

            return EAs.getcodebyname(Ea_name);
        }
        //添加一个菜单
        //parentID == -1 ,添加到根节点
        public bool AddEquipArchiItem(int parent_Id, Equip_Archi add)
        {
            if (parent_Id == -1)
            {
                Equip_Archi root = GetRootItem();
                return db_equip_archi.AddEquipArchi(root.EA_Id, add);
            }
            else
                return db_equip_archi.AddEquipArchi(parent_Id, add);
        }

        //修改一个菜单
        public bool ModifyEquipArchiItem(Equip_Archi modify)
        {
            return db_equip_archi.ModifyEquipArchi(modify.EA_Id, modify);
        }

        //删除一个菜单
        public bool DeleteEquipArchiItem(int del)
        {
            return db_equip_archi.DeleteEquipArchi(del);
        }

        //获得菜单的根节点
        public Equip_Archi GetRootItem()
        {
            try
            {
                List<Equip_Archi> res = db_equip_archi.GetEquipArchi("root");
                if (res.Count > 1)
                    throw new Exception("DB has not only one root item");
                return res[0];
            }
            catch (Exception e)
            {
                return null;
            }

        }

        //public Equip_Archi GetItemById(int id)
        //{
        //    return db_equip_archi.GetEA_Archi(id);
        //}

        //获得某一个菜单项的子节点
        public List<Equip_Archi> GetChildsMenu(int EA_Id)
        {
            List<Equip_Archi> childs = new List<Equip_Archi>();
            childs = db_equip_archi.GetChildMenu(EA_Id);
            return childs;
        }

        //private MenuTree BuildChildTree(int parentId)
        //{
        //    MenuTree cRoot = new MenuTree();

        //    Menu cRootM = GetItemById(parentId);
        //    cRoot.Menu_Id = cRootM.Menu_Id;
        //    cRoot.Menu_Name = cRootM.Menu_Name;
        //    cRoot.Menu_Icon = cRootM.Menu_Icon;
        //    cRoot.Link_Url = cRootM.Link_Url;

        //    List<Menu> childs = GetChildsMenu(parentId);
        //    foreach(Menu m in childs)
        //    {
        //        MenuTree mt = BuildChildTree(m.Menu_Id);
        //        cRoot.childs.Add(mt);
        //    }
        //    return cRoot;

        //}
        //public MenuTree BuildMenuTree()
        //{
        //    Menu root = GetRootItem();

        //    return BuildChildTree(root.Menu_Id);
        //}

        private void BuildMenuList_inter(MenuListNode1 parent, List<MenuListNode1> list)
        {
            List<Equip_Archi> childs = GetChildsMenu(parent.EA_Id);

            foreach (Equip_Archi m in childs)
            {
                MenuListNode1 mn = new MenuListNode1();
                mn.EA_Id = m.EA_Id;
                mn.EA_Name = m.EA_Name;
                mn.EA_Code = m.EA_Code;
                mn.EA_Title = m.EA_Title;
                mn.Parent_id = parent.EA_Id;
                mn.level = parent.level + 1;
                parent.Childs.Add(mn.EA_Id);
                list.Add(mn);
                BuildMenuList_inter(mn, list);
                mn.Childs.ForEach(i => parent.Childs.Add(i));
            }
        }
        public List<MenuListNode1> BuildMenuList()
        {
            Equip_Archi root = GetRootItem();
            MenuListNode1 rmn = new MenuListNode1();
            rmn.EA_Id = root.EA_Id;
            rmn.EA_Name = root.EA_Name;
            rmn.EA_Code = root.EA_Code;
            rmn.EA_Title = root.EA_Title;
            rmn.level = -1;

            List<MenuListNode1> list = new List<MenuListNode1>();
            BuildMenuList_inter(rmn, list);
            foreach (MenuListNode1 mn in list)
            {
                if (mn.Parent_id == root.EA_Id)
                    mn.Parent_id = 0;
            }
            return list;
        }
    }
}

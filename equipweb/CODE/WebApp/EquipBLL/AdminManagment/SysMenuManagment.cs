﻿using EquipDAL.Implement;
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
    public class SysMenuManagment
    {
        private IMenus db_menus = new Menus();

        //添加一个菜单
        //parentID == -1 ,添加到根节点
        public bool AddMenuItem(int parentID, Menu add)
        {
            if (parentID == -1)
            {
                Menu root = GetRootItem();
                return db_menus.AddMenu(root.Menu_Id, add);
            }
            else
                return db_menus.AddMenu(parentID, add);
        }

        //修改一个菜单
        public bool ModifyMenuItem(Menu modify)
        {
            return db_menus.ModifyMenu(modify.Menu_Id, modify);
        }

        //删除一个菜单
        public bool DeleteMenuItem(int del)
        {
            return db_menus.DeleteMenu(del);
        }

        //获得菜单的根节点
        public Menu GetRootItem()
        {
            try
            {
                List<Menu> res = db_menus.GetMenu("root");
                if (res.Count > 1)
                    throw new Exception("DB has not only one root item");
                return res[0];
            }
            catch(Exception e)
            {
                return null;
            }
            
        }

        public Menu GetItemById(int id)
        {
            return db_menus.GetMenu(id);
        }

        //获得某一个菜单项的子节点
        public List<Menu> GetChildsMenu(int menuID)
        {
            List<Menu> childs = new List<Menu>();
            childs = db_menus.GetChildMenu(menuID);
            return childs;
        }

        private MenuTree BuildChildTree(int parentId)
        {
            MenuTree cRoot = new MenuTree();

            Menu cRootM = GetItemById(parentId);
            cRoot.Menu_Id = cRootM.Menu_Id;
            cRoot.Menu_Name = cRootM.Menu_Name;
            cRoot.Menu_Icon = cRootM.Menu_Icon;
            cRoot.Link_Url = cRootM.Link_Url;

            List<Menu> childs = GetChildsMenu(parentId);
            foreach(Menu m in childs)
            {
                MenuTree mt = BuildChildTree(m.Menu_Id);
                cRoot.childs.Add(mt);
            }
            return cRoot;

        }
        public MenuTree BuildMenuTree()
        {
            Menu root = GetRootItem();

            return BuildChildTree(root.Menu_Id);
        }

        private void BuildMenuList_inter(MenuListNode parent, List<MenuListNode> list)
        {
            List<Menu> childs = GetChildsMenu(parent.Menu_Id);
            
            foreach (Menu m in childs)
            {
                MenuListNode mn = new MenuListNode();
                mn.Menu_Id = m.Menu_Id;
                mn.Menu_Name = m.Menu_Name;
                mn.Menu_Icon = m.Menu_Icon;
                mn.Link_Url = m.Link_Url;
                if (m.Link_Url != null)
                {
                    //在前台的页面中，本意是选择用Link_Url作为一个标签的id值，但是在id值中出现“/”的情况会使得根据id选择对象时出现问题，故在此统一转换。这段添加的代码功能是：将例如“/A3dot/Index”的字符串转成“A3dot3”
                    string[] s = (m.Link_Url).Split(new char[] { '/' });
                    string str = s[1];
                    mn.url_id = str;

                };




                mn.Parent_id = parent.Menu_Id;                
                mn.level = parent.level + 1;
                parent.Childs.Add(mn.Menu_Id);
                list.Add(mn);
                BuildMenuList_inter(mn, list);
                mn.Childs.ForEach(i => parent.Childs.Add(i));
            }            
        }
        public List<MenuListNode> BuildMenuList()
        {
            Menu root = GetRootItem();
            MenuListNode rmn = new MenuListNode();
            rmn.Menu_Id = root.Menu_Id;
            rmn.Menu_Name = root.Menu_Name;
            rmn.Menu_Icon = root.Menu_Icon;
            rmn.Link_Url = root.Link_Url;
            rmn.level = -1;

            List<MenuListNode> list = new List<MenuListNode>();
            BuildMenuList_inter(rmn, list);
            foreach (MenuListNode mn in list)
            {
                if (mn.Parent_id == root.Menu_Id)
                    mn.Parent_id = 0;
            }
            return list;
        }
    }
}

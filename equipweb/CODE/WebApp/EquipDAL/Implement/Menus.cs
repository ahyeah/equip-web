﻿using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Menus : BaseDAO, IMenus
    {
       
        public EquipModel.Entities.Menu GetMenu(int id)
        {
            using(var db = base.NewDB())
            {
                return db.Sys_Menus.Where(a => a.Menu_Id == id).First();
            }
        }

        public List<EquipModel.Entities.Menu> GetMenu(string name)
        {
            using (var db = base.NewDB())
            {
                return db.Sys_Menus.Where(a => a.Menu_Name == name).OrderBy(a => a.Menu_Name).ToList();
            }
        }

        public List<EquipModel.Entities.Menu> GetChildMenu(int id)
        {
            using (var db = base.NewDB())
            {
                var menu = db.Sys_Menus.Where(a => a.Menu_Id == id).First();
                return menu.child_menus.ToList();
            }
        }

        public List<EquipModel.Entities.Menu> GetChildMenu(string name)
        {
            using (var db = base.NewDB())
            {
                var menu = db.Sys_Menus.Where(a => a.Menu_Name == name).First();
                return menu.child_menus.ToList();
            }
        }

        public bool AddMenu(int parentID, Menu newMenu)
        {
            using (var db = base.NewDB())
            {
                try 
                {
                    Menu parent = db.Sys_Menus.Where(a => a.Menu_Id == parentID).First();
                    if (parent == null)
                        return false;

                    parent.child_menus.Add(newMenu);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteMenu(int id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var menu = db.Sys_Menus.Where(a => a.Menu_Id == id).First();
                    if (menu == null)
                        return true;

                    foreach (var child in menu.child_menus)
                    {
                        DeleteMenu(child.Menu_Id);
                    }
                    db.Sys_Menus.Remove(menu);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ModifyMenu(int id, EquipModel.Entities.Menu nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var menu = db.Sys_Menus.Where(a => a.Menu_Id == id).First();

                    if (menu == null)
                        return false;

                    menu.Menu_Name = nVal.Menu_Name;
                    menu.Menu_Icon = nVal.Menu_Icon;
                    menu.Link_Url = nVal.Link_Url;

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
               
    }
}

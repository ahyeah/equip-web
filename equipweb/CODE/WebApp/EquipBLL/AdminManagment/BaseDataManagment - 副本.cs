﻿using EquipDAL.Implement;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment
{
    public class BaseDataManagment
    {
        private Depart_Archis DA = new Depart_Archis();
        private Speciaties SD = new Speciaties();
        private Equip_Archis EA = new Equip_Archis();
        private Roles RD = new Roles();
        private Menus MD = new Menus();
        //功能：获取系统基础数据库中定义的所有角色，返回List<Role_Info>集合信息
        //参数：空
        //返回值：List<Role_Info>
        public List<Role_Info> GetAllRoles()
        {
            try
            {
                var r = RD.getRoles();
                return r;
            }
            catch { return null; }
        }

        //功能：获取权限所对应的所有Menu集合List<Menu>,各权限重复的Menu会自动剔除
        //参数：PersonRoles,为权限Id,注意：这里的PersonRoles可以是一个权限的Id,也可以是多个权限的Id,如果是多个权限的Id,各个权限Id之间用","隔开，Example:"1,4"
        //返回值：List<Menu>
        public List<Menu> GetRole_Menus(string PersonRoles)
        {
            try
            {


                var r = RD.getPerson_Role_Menus(PersonRoles);


                return r;
            }
            catch { return null; }
        }
        public Depart_Archi GetRootTree()
        {
            try
            {
                Depart_Archi res = DA.getDepart_root("root");
                if (res == null)
                    throw new Exception("DB has not only one root item");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        //显示部门树形机构表
        private void BuildDepartList_inter(TreeListNode parent, List<TreeListNode> list)
        {
            List<Depart_Archi> Childs = DA.getDepart_Childs(parent.text);
            foreach (Depart_Archi item in Childs)
            {
                TreeListNode mn = new TreeListNode();
                mn.text = item.Depart_Name;
                mn.id = item.Depart_Id;
                if (DA.getDepart_Childs(item.Depart_Id).Count == 0) mn.selectable = true;
                parent.nodes.Add(mn);
                if (parent.text == "root") list.Add(mn);
                BuildDepartList_inter(mn, list);
            }
        }
        //功能：获取系统基础数据所有的部门层次树形结构节点集合
        //参数：空
        //返回值：List<TreeListNode> 在前台程序中可以利用Treeview显示树形结构
        public List<TreeListNode> BuildDepartList()
        {
            Depart_Archi root = GetRootTree();
            TreeListNode rnode = new TreeListNode();

            rnode.text = root.Depart_Name;
            rnode.selectable = false;

            List<TreeListNode> list = new List<TreeListNode>();
            BuildDepartList_inter(rnode, list);
            return list;
        }

        private void BuildDepartPersonList_inter(TreeListNode parent, List<TreeListNode> list)
        {
            List<Depart_Archi> Childs = DA.getDepart_Childs(parent.text);
            foreach (Depart_Archi item in Childs)
            {
                TreeListNode mn = new TreeListNode();
                mn.text = item.Depart_Name;
                mn.id = item.Depart_Id;
                mn.selectable = false;

                parent.nodes.Add(mn);
                if (parent.text == "root") list.Add(mn);
                if (DA.getDepart_Childs(item.Depart_Id).Count > 0) BuildDepartPersonList_inter(mn, list);
                else
                {
                    List<Person_Info> listP = DA.getPersons_Belong(item.Depart_Id);
                    TreeListNode mn_person;
                    foreach (Person_Info pitem in listP)
                    {
                        mn_person = new TreeListNode();
                        mn_person.text = pitem.Person_Name;
                        mn_person.id = pitem.Person_Id;
                        mn_person.selectable = true;
                        mn.nodes.Add(mn_person);
                    }
                }
            }
        }

        private void BuildEquipArchiList_inter(TreeListNode parent, List<TreeListNode> list)
        {
            List<Equip_Archi> Childs = EA.getEA_Childs(parent.text);
            foreach (Equip_Archi item in Childs)
            {
                TreeListNode mn = new TreeListNode();
                mn.text = item.EA_Name;
                mn.id = item.EA_Id;
                //if (EA.getEA_Childs(item.EA_Id).Count == 0) 
                mn.selectable = true;
                parent.nodes.Add(mn);
                if (parent.text == "root") list.Add(mn);
                BuildEquipArchiList_inter(mn, list);
            }
        }
        //功能：获取系统基础数据中所有专业信息的层次树形结构节点集合
        //参数：空
        //返回值：List<TreeListNode> 在前台程序中可以利用Treeview显示树形结构
        public List<TreeListNode> BuildEquipArchiList()
        {

            TreeListNode rnode = new TreeListNode();

            rnode.text = "root";
            rnode.selectable = false;

            List<TreeListNode> list = new List<TreeListNode>();
            BuildEquipArchiList_inter(rnode, list);
            return list;
        }

        //功能：获取系统基础数据中所有人员，此处按照人员所在的部门返回层次结构树形结构节点集合
        //参数：空
        //返回值：List<TreeListNode> 在前台程序中可以利用Treeview显示树形结构

        public List<TreeListNode> BuildDepartPersonList()
        {
            Depart_Archi root = GetRootTree();
            TreeListNode rnode = new TreeListNode();

            rnode.text = root.Depart_Name;
            rnode.selectable = false;

            List<TreeListNode> list = new List<TreeListNode>();
            BuildDepartPersonList_inter(rnode, list);
            return list;
        }

        //显示专业部门树形结构表
        private void BuildSpecialtyList_inter(TreeListNode parent, List<TreeListNode> list)
        {
            List<Speciaty_Info> Childs = SD.getSepciaty_Childs(parent.text);
            foreach (Speciaty_Info item in Childs)
            {
                TreeListNode mn = new TreeListNode();
                mn.text = item.Specialty_Name;
                mn.id = item.Specialty_Id;
                if (SD.getSepciaty_Childs(item.Specialty_Id).Count == 0) mn.selectable = true;
                parent.nodes.Add(mn);
                if (parent.text == "root") list.Add(mn);
                BuildSpecialtyList_inter(mn, list);
            }
        }
        //功能：获取系统基础数据中所有专业信息的层次树形结构节点集合
        //参数：空
        //返回值：List<TreeListNode> 在前台程序中可以利用Treeview显示树形结构
        public List<TreeListNode> BuildSpeciatyList()
        {

            TreeListNode rnode = new TreeListNode();

            rnode.text = "root";
            rnode.selectable = false;

            List<TreeListNode> list = new List<TreeListNode>();
            BuildSpecialtyList_inter(rnode, list);
            return list;
        }


        //显示系统菜单部门树形结构表
        private void BuildMenuList_inter(TreeListNode parent, List<TreeListNode> list, List<Menu> hasMenus)
        {
            List<Menu> Childs = MD.GetChildMenu(parent.text);
            foreach (Menu item in Childs)
            {
                TreeListNode mn = new TreeListNode();
                mn.text = item.Menu_Name;
                mn.id = item.Menu_Id;
                if (MD.GetChildMenu(item.Menu_Id).Count == 0) mn.selectable = true;
                if (hasMenus.Where(s => s.Menu_Id == item.Menu_Id).Count() > 0) mn.state.selected = true;

                parent.nodes.Add(mn);
                if (parent.text == "root") list.Add(mn);
                BuildMenuList_inter(mn, list, hasMenus);
            }
        }
        //功能：获取系统基础数据中所有系统菜单信息的层次树形结构节点集合
        //参数：空
        //返回值：List<TreeListNode> 在前台程序中可以利用Treeview显示树形结构
        public List<TreeListNode> BuildMenuList(string Role_ids)
        {

            TreeListNode rnode = new TreeListNode();
            List<Menu> hasMenus = RD.getPerson_Role_Menus(Role_ids);

            rnode.text = "root";
            rnode.selectable = false;

            List<TreeListNode> list = new List<TreeListNode>();
            BuildMenuList_inter(rnode, list, hasMenus);
            return list;
        }


    }
}

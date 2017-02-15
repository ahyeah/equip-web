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

namespace EquipBLL.AdminManagment.ZyConfig
{
    public class SpeciatyManagment
    {
        private ISpecialty db_specialty = new Zy();

        //添加一个菜单
        //parentID == -1 ,添加到根节点
        public bool AddSpecialtyItem(int parentID, Speciaty_Info add)
        {
            if (parentID == -1)
            {
                Speciaty_Info root = GetRootItem();
                return db_specialty.AddSpecialty(root.Specialty_Id, add);
            }
            else
                return db_specialty.AddSpecialty(parentID, add);
        }

        //修改一个菜单
        public bool ModifySpecialtyItem(Speciaty_Info modify)
        {
            return db_specialty.ModifySpecialty(modify.Specialty_Id, modify);
        }

        //删除一个菜单
        public bool DeleteSpecialtyItem(int del)
        {
            return db_specialty.DeleteSpecialty(del);
        }

        //获得菜单的根节点
        public Speciaty_Info GetRootItem()
        {
            try
            {
                List<Speciaty_Info> res = db_specialty.GetSpecialty("root");
                if (res.Count > 1)
                    throw new Exception("DB has not only one root item");
                return res[0];
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public Speciaty_Info GetItemById(int id)
        {
            return db_specialty.GetSpecialty(id);
        }

        //获得某一个菜单项的子节点
        public List<Speciaty_Info> GetChildsspecialty(int specialtyID)
        {
            List<Speciaty_Info> childs = new List<Speciaty_Info>();
            childs = db_specialty.GetChildSpecialty(specialtyID);
            return childs;
        }

        private SpecialtyTree BuildChildTree(int parentId)
        {
            SpecialtyTree cRoot = new SpecialtyTree();

            Speciaty_Info cRootM = GetItemById(parentId);
            cRoot.Specialty_Id = cRootM.Specialty_Id;
            cRoot.Specialty_Name = cRootM.Specialty_Name;


            List<Speciaty_Info> childs = GetChildsspecialty(parentId);
            foreach (Speciaty_Info m in childs)
            {
                SpecialtyTree mt = BuildChildTree(m.Specialty_Id);
                cRoot.childs.Add(mt);
            }
            return cRoot;

        }
        public SpecialtyTree BuildSpecialtyTree()
        {
            Speciaty_Info root = GetRootItem();

            return BuildChildTree(root.Specialty_Id);
        }

        private void BuildSpeciatyList_inter(SpecialtyListNode parent, List<SpecialtyListNode> list)
        {
            List<Speciaty_Info> childs = GetChildsspecialty(parent.Specialty_Id);

            foreach (Speciaty_Info m in childs)
            {
                SpecialtyListNode mn = new SpecialtyListNode();
                mn.Specialty_Id = m.Specialty_Id;
                mn.Specialty_Name = m.Specialty_Name;

                mn.Parent_id = parent.Specialty_Id;
                mn.level = parent.level + 1;
                parent.Childs.Add(mn.Specialty_Id);
                list.Add(mn);
                BuildSpeciatyList_inter(mn, list);
                mn.Childs.ForEach(i => parent.Childs.Add(i));
            }
        }
        public List<SpecialtyListNode> BuildSpeciatyList()
        {
            Speciaty_Info root = GetRootItem();
            SpecialtyListNode rmn = new SpecialtyListNode();
            rmn.Specialty_Id = root.Specialty_Id;
            rmn.Specialty_Name = root.Specialty_Name;
            
            rmn.level = -1;

            List<SpecialtyListNode> list = new List<SpecialtyListNode>();
            BuildSpeciatyList_inter(rmn, list);
            foreach (SpecialtyListNode mn in list)
            {
                if (mn.Parent_id == root.Specialty_Id)
                    mn.Parent_id = 0;
            }
            return list;
        }

        private Speciaties sps = new Speciaties();
        public List<Speciaty_Info> getsps()
        {
            List<Speciaty_Info> r = sps.getSepciaty_Parent();
            return r;
        }
        public List<Speciaty_Info> getsps_child(int sp_id)
        {
            List<Speciaty_Info> r = sps.getSepciaty_Childs(sp_id);
            return r;
        }
    }
}

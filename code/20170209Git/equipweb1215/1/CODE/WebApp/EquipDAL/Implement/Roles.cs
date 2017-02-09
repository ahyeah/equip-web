using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
   public  class Roles : BaseDAO
    {
        //获取拥有指定角色的用户信息列表
       public class RoleModal
       {
           public Role_Info RInfo;//用户基本信
           public List<Menu> M_Info;//系统菜单信息
       }


        public List<Person_Info> getRole_Persons(int Role_id)
        {
            using (var db = base.NewDB())
            {
                var R = db.Roles.Where(a => a.Role_Id==Role_id).First();
                return R.Role_Persons.ToList();
            }
        }

       //获取所有的角色
        public List<Role_Info> getRoles()
        {
            using (var db = base.NewDB())
            {
                var R = db.Roles.ToList();
                return R;
            }
        }
       //获取某制定角色对应的系统菜单
       public List<Menu> getRole_Menus(int Role_id)
        {
            using (var db = base.NewDB())
            {
                var R = db.Roles.Where(a=>a.Role_Id==Role_id).First().Role_Menus.ToList();
                return R;
            }
        }
        //给特定的角色添加用户

       public List<Menu> getPerson_Role_Menus(string PersonRoles)
       {
           using (var db = base.NewDB())
           {
               int i; int ids;
               List<Menu> rm = new List<Menu>();
              string[] s1 = PersonRoles.Split(new char[] { ',' });
            
               for (i = 0; i < s1.Length;i++)
               {
                  ids = (Convert.ToInt32(s1[i]));
                   var R = db.Roles.Where(a => a.Role_Id == ids).First().Role_Menus.ToList();
                   foreach(var item in R)
                   rm.Add(item);
                  
               }
               rm = rm.Distinct().ToList();
               return rm;
           }
       }

       //修改某一个已定义的角色
       public bool ModifyRole(Role_Info New_Role)
       {
           int id = New_Role.Role_Id;

           using (var db = base.NewDB())
           {
               try
               {
                   var r = db.Roles.Where(a => a.Role_Id==id).First();
                   if (r == null)
                       return false;
                   else
                   {
                       r.Role_Name = New_Role.Role_Name;
                       r.Role_Desc = New_Role.Role_Desc;
                       db.SaveChanges();
                       return true;

                   }
               }
               catch { return false; }
           }
       }


        public bool AddPerson(int  Role_id,int  Person_id )
        {
            using (var db = base.NewDB())
            {
                try
                {
                   
                    var PR = db.Roles.Where(a => a.Role_Id == Role_id).First();
                    var P = db.Persons.Where(a => a.Person_Id == Person_id).First();
                    PR.Role_Persons.Add(P);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        //添加一个新的角色
        public int AddRole(Role_Info New_Role)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Role_Info newR = db.Roles.Add(New_Role);
                    db.SaveChanges();
                    return newR.Role_Id;

                    // return true;
                }
                catch
                {
                    return 0;
                }
            }

        }

        
        //删除某一个已定义的角色 
        public bool DeleteRole(int roleId)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var r = db.Roles.Where(a => a.Role_Id == roleId).First();
                    if (r == null)
                        return false;
                    else
                    {  //foreach(var p in r.Role_Persons )
                       // { var person = db.Persons.Where(a => a.Person_Id == p.Person_Id).First();
                        //  person.Person_roles.Remove(person.Person_roles.Where(s => s.Role_Id == r.Role_Id).First());
                       // }

                        r.Role_Persons.Clear();
                        db.Roles.Remove(r);
                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }
        }
        //修改某一个已定义的角色
        public bool ModifyRole(int roleId, Role_Info New_Role)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var r = db.Roles.Where(a => a.Role_Id == roleId).First();
                    if (r == null)
                        return false;
                    else
                    {
                        r.Role_Name = New_Role.Role_Name;
                        r.Role_Desc = New_Role.Role_Desc;
                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }
        }


        public bool Roles_LinkMenus(int Role_id, List<int> Menu_IDs)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var r = db.Roles.Where(s => s.Role_Id == Role_id).First();

                    var r_old = r.Role_Menus.ToList();
                    if (r_old.Count > 0)
                    {
                        r.Role_Menus.Clear();
                    }

                    foreach (var item in Menu_IDs)
                    {
                        Menu rm = db.Sys_Menus.Where(s => s.Menu_Id == item).First();
                        r.Role_Menus.Add(rm);

                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }



        public RoleModal Get_RoleModal(int Role_Id)
        {
            try
            {
                RoleModal RM = new RoleModal();
                using (var db = base.NewDB())
                {

                    RM.RInfo = db.Roles.Where(a => a.Role_Id == Role_Id).First();
                    RM.M_Info = RM.RInfo.Role_Menus.ToList();
                    return RM;
                }
            }
            catch (Exception e)
            { return null; }

        }

       
    }
    
}

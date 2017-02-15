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


namespace EquipBLL.AdminManagment
{
   public  class PersonManagment
    {  
       public class P_viewModal
       {
          public  int Person_Id;
           public string Person_Name;
           public string Person_mail;
          public  string Person_tel;
          public int Department_Id;
           public string Department_Name;
           public string EquipArchi_Ids;
           public string EquipArchi_Names;
          public  string Speciaty_Ids;
          public  string Speciaty_Names;
          public  string Role_Ids;
          public  string Role_Names;
          public string Menu_Ids;
          public List<Menu> Menus;
       
           
       }
       public class Role_viewModal
       {
           public int Role_Id;
           public string Role_Name;
           public bool Role_selected;
       }
     
        private Person_Infos  Persons= new Person_Infos();
       //功能：已知用户名称，查询用户对应信息
       //参数：userName 用户姓名
       //返回值：Person_Info
       public Person_Info  Get_Person(string userName)
        {
             var P = Persons.GetPerson_info(userName);
            return P;
        }
       //20161212月添加的方法，获取用户管理的某指定车间的装置
       public List<Equip_Archi> Get_Person_Zz_ofCj(int id, int cj_id)
       {
           List<Equip_Archi> r = Persons.getZz_of_Person(id, cj_id);
           return r;
       }

       /// <summary>
       /// 获取用户管辖的装置
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public List<Equip_Archi> Get_Person_Zz(int id)
       {
           List<Equip_Archi> r = Persons.getZzByPerson(id);
           return r;
       }
       //功能:获取所有角色的List集合，并且用户已分配的角色默认为选中状态
       //参数：PersonRoles,用户已分配角色Id串，若干角色Id之间以","分隔，Example:"1,4,17"，该参数默认为"",若只是显示所有角色集合，则取默认值
       //返回值：List<Role_viewModal>，前台程序可以在这个的基础上将角色信息显示在Select下拉框中
       /* Role_viewModal的定义如下：
         public class Role_viewModal
         {
           public int Role_Id;
           public string Role_Name;
           public bool Role_selected;
       }
        */

       public List<Role_viewModal> Get_All_Roles( string PersonRoles = "")
       {
           RoleManagment RM = new RoleManagment();
           var P = RM.get_ALl_Roles();
           List<int> RoleList = new List<int>();
           string[] s1 = PersonRoles.Split(new char[] { ',' });
           if (PersonRoles != "")
           {
               for (int i = 0; i < s1.Length; i++)
               { RoleList.Add(Convert.ToInt32(s1[i])); }
           }
           List<Role_viewModal> pr = new List<Role_viewModal>();
           Role_viewModal ii ;
           foreach(var item in  P)
           {
               ii = new Role_viewModal();
               ii.Role_Id = item.Role_Id;
               ii.Role_Name = item.Role_Name;
               if (RoleList.Contains(item.Role_Id))
                   ii.Role_selected = true;
               else
                   ii.Role_selected = false;
               pr.Add(ii);
           }
           return pr;
       }


       //功能：获取某用户的详细信息,为用户管理模块做数据准备
       //参数：userId 用户Id
       //返回值：P_viewModal

       /* P_viewModal的定义如下：
         public class P_viewModal
        {
           public  int Person_Id;
           public string Person_Name;
           public string Person_mail;
           public  string Person_tel;
           public int Department_Id;
           public string Department_Name;
           public  string Speciaty_Ids;  //若用户有多个专业，各专业Id之间用","隔开
           public  string Speciaty_Names;//若用户有多个专业，各专业名称之间用","隔开
           public  string Role_Ids;    //若用户有多个角色
           public  string Role_Names;
           public string Menu_Ids;
           public List<Menu> Menus;
       
           
        }
         */
       public P_viewModal Get_PersonModal(int userId)
       {  int i;
           var P = Persons.Get_PersonModal(userId);
           P_viewModal p_item=new P_viewModal();
           p_item.Person_Id=P.P_Info.Person_Id;
           p_item.Person_Name=P.P_Info.Person_Name;
           p_item.Person_mail=P.P_Info.Person_mail;
           p_item.Person_tel=P.P_Info.Person_tel;
           p_item.Department_Id=P.D_Info.Depart_Id;
           p_item.Department_Name=P.D_Info.Depart_Name;
           //管理区域
           p_item.EquipArchi_Ids = "";
           if(P.EA_Info.Count>0)
           {
               for (i = 0; i < P.EA_Info.Count - 1; i++)
               {
                   p_item.EquipArchi_Ids = p_item.EquipArchi_Ids + (P.EA_Info[i]).EA_Id + ",";
               }
               p_item.EquipArchi_Ids = p_item.EquipArchi_Ids + (P.EA_Info[i]).EA_Id;
           }

           p_item.EquipArchi_Names = "";
           if (P.EA_Info.Count > 0)
           {
               for (i = 0; i < P.EA_Info.Count - 1; i++)
               {
                   p_item.EquipArchi_Names = p_item.EquipArchi_Names + (P.EA_Info[i]).EA_Name + ",";
               }
               p_item.EquipArchi_Names = p_item.EquipArchi_Names + (P.EA_Info[i]).EA_Name;
           }
           //专业信息
           p_item.Speciaty_Ids="";
           if (P.S_Info.Count > 0)
           {
               for (i = 0; i < P.S_Info.Count - 1; i++)
               {
                   p_item.Speciaty_Ids = p_item.Speciaty_Ids + (P.S_Info[i]).Specialty_Id + ",";
               }
               p_item.Speciaty_Ids = p_item.Speciaty_Ids + (P.S_Info[i]).Specialty_Id;
           }
            p_item.Speciaty_Names="";
            if (P.S_Info.Count > 0)
            {
                for (i = 0; i < P.S_Info.Count - 1; i++)
                {
                    p_item.Speciaty_Names = p_item.Speciaty_Names + (P.S_Info[i]).Specialty_Name + ",";
                }
                p_item.Speciaty_Names = p_item.Speciaty_Names + (P.S_Info[i]).Specialty_Name;
            }
          //角色信息
             p_item.Role_Ids="";
             if (P.R_Info.Count > 0)
           { for( i=0;i<P.R_Info.Count-1;i++)
            { p_item.Role_Ids= p_item.Role_Ids + (P.R_Info[i]).Role_Id +",";
            }
            p_item.Role_Ids=p_item.Role_Ids +(P.R_Info[i]).Role_Id ;
           }
             p_item.Role_Names="";
           if(P.R_Info.Count>0)
           {
           for( i=0;i<P.R_Info.Count-1;i++)
           { p_item.Role_Names= p_item.Role_Names + (P.R_Info[i]).Role_Name +",";
           }
            p_item.Role_Names=p_item.Role_Names +(P.R_Info[i]).Role_Name ;
           }
           //系统权限
            p_item.Menu_Ids = "";
            if (P.M_Info.Count > 0)
            {
                for (i = 0; i < P.M_Info.Count - 1; i++)
                {
                    p_item.Menu_Ids = p_item.Menu_Ids + (P.M_Info[i]).Menu_Id + ",";
                }
                p_item.Menu_Ids = p_item.Menu_Ids + (P.M_Info[i]).Menu_Id;
            }
            p_item.Menus = P.M_Info;
           return p_item ;

       }


       /*
        * 功能：获取某用户所有信息
        * 参数：Id 用户Id
        * 返回值：PersonModal 其具体定义见Person_Info.cs文件
        *  public class PersonModal
        {
            public Person_Info P_Info;//用户基本信息
            public Depart_Archi D_Info;//部门信息
            public List<Speciaty_Info> S_Info;//专业信息
            public List<Role_Info> R_Info;//角色信息
            public List<Menu> M_Info;//系统菜单信息


        }
        * */
       public Person_Infos.PersonModal get_PersonInfo(int userId)
       {
           try
           {
               var P = Persons.Get_PersonModal(userId);
               return P;
           }
           catch { return null; }
       }
       //功能：获取所有用户的基本信息
       //参数：空
       //返回值：List<Person_Info>
       public List<Person_Info> Get_All_basePersonInfo()
       {
           var p = Persons.GetAllPerson_info();
           return p;

       }
       private List<int> Get_EquipArchi_Allleafs(List<int> EquipArchi_IDs)
       {
           Equip_Archis EA = new Equip_Archis();
           List<int> leafEA_IDs = new List<int>();
           foreach (int Id in EquipArchi_IDs)
           {
               if (EA.getEA_Childs(Id).Count == 0) leafEA_IDs.Add(Id);
               else
                   SearchLeafEA(Id, leafEA_IDs);

           }
           return leafEA_IDs;
       }
       private void SearchLeafEA(int Id, List<int> leafEA_IDs)
       {
           Equip_Archis EA = new Equip_Archis();
           List<Equip_Archi> Childs = EA.getEA_Childs(Id);

           foreach (Equip_Archi item in Childs)
           {

               if (EA.getEA_Childs(item.EA_Id).Count == 0) leafEA_IDs.Add(item.EA_Id);
               else SearchLeafEA(item.EA_Id, leafEA_IDs);
           }
       }
     
       /*功能：往数据库添加用户基本信息，并设置用户部门、角色、专业和系统菜单信息
         参数： p           新用户Person_Info对象
                Depart_id   所属部门Id
        *       Role_IDs    分配的所有角色Id集合
        *       Speciaty_IDs 所属专业Id集合
        *       Menu_IDs     分配系统菜单Id集合
        *返回值：bool       
        * */

       public bool Add_Person(Person_Info p,int Depart_id,List<int> Role_IDs,List<int> EquipArchi_IDs,List<int> Speciaty_IDs,List<int> Menu_IDs)
        {
            try
            {
                int NewpP_id=Persons.AddPerson_info(p);
                Persons.Person_LinkDepart(NewpP_id, Depart_id);
                Persons.Person_LinkSpeciaties(NewpP_id, Speciaty_IDs);
                Persons.Person_LinkEAs(NewpP_id, EquipArchi_IDs);
                List<int> Zz_IDs = Get_EquipArchi_Allleafs(EquipArchi_IDs);
                Persons.Person_LinkEquip(NewpP_id, Zz_IDs, Speciaty_IDs);
                Persons.Person_LinkRole(NewpP_id, Role_IDs);
                Persons.Person_LinkMenus(NewpP_id, Menu_IDs);
                return true;
            }
            catch { return false; }
        }
       /*功能：修改用户的基本信息，并设置用户新的部门、角色、专业和系统菜单信息
         参数： p          提交的用户基本新信息Person_Info对象
                Depart_id   新的所属部门Id
        *       Role_IDs    新分配的所有角色Id集合
        *       Speciaty_IDs 新的所属专业Id集合
        *       Menu_IDs     新分配的系统菜单Id集合
        *返回值：bool       
        * */
        public bool Update_Person(Person_Info p,int Depart_id,List<int> Role_IDs,List<int> EquipArchi_IDs,List<int> Speciaty_IDs,List<int> Menu_IDs)
        {
            try
            {
                Persons.ModifyPerson_info(p);

                Persons.Person_LinkDepart(p.Person_Id, Depart_id);
                Persons.Person_LinkSpeciaties(p.Person_Id, Speciaty_IDs);
                Persons.Person_LinkEAs(p.Person_Id, EquipArchi_IDs);
                List<int> Zz_IDs = Get_EquipArchi_Allleafs(EquipArchi_IDs);
                Persons.Person_LinkEquip(p.Person_Id, Zz_IDs, Speciaty_IDs);
                Persons.Person_LinkRole(p.Person_Id, Role_IDs);
                Persons.Person_LinkMenus(p.Person_Id, Menu_IDs);
                return true;
            }
            catch { return false; }
        }


        /*功能：删除用户，并自动删除用户的部门、角色、专业和系统菜单等相关链接信息
         参数： id          用户Id
              
        *返回值：bool       
        * */
        public bool Delete_Person(int id)
        {
            return Persons.DeletePerson_info(id);
        }

       /*功能：获取某用户已分配的所有角色信息集合
        *参数：id 用户Id
        *返回值：List<Role_Info> 
       */
        public List<Role_Info> Get_Person_Roles(int id)
        {
            List<Role_Info> r = new List<Role_Info>();
            r=Persons.GetPerson_Roles(id);
            return r;
        }

        /*
         * 功能：获取某用户管理的所有设备对应的车间信息列表
         * 参数：id 用户Id
         * 返回值：List<Equip_Archi>
        */
        public List<Equip_Archi> Get_Person_Cj(int id)
        {
            List<Equip_Archi> r = Persons.getEA_of_Person(id);
            return r;
        }

        /// <summary>
        ///16/9/8新增，获取润滑油间列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Runhua_Info> Get_Runhua_Cj()
        {
            List<Runhua_Info> r = Persons.getRunhuaCj();
            return r;
        }
        /*
        * 功能：获取某用户管理的所有设备信息列表
        * 参数：id 用户Id
        * 返回值：List<Equip_Info>
       */
        public List<Equip_Info> Get_Person_Equips(int id)
        {
            List<Equip_Info> r = new List<Equip_Info>();
            r = Persons.GetPerson_Equips(id);
            return r;
        }
        /*
        * 功能：获取某用户所属部门信息
        * 参数：id 用户Id
        * 返回值：Depart_Archi
       */
        public Depart_Archi Get_Person_Depart(int id)
        {
            Depart_Archi r = new Depart_Archi();
            r = Persons.GetPerson_Depart(id);
            return r;
        }
        /*
          * 功能：获取某装置管理员所属的车间
          * 参数：id 用户Id
          * 返回值：Depart_Archi
         */
        public Depart_Archi Get_Person_DepartOfParent(int id)
        {
            Depart_Archi r = new Depart_Archi();
            r = Persons.GetPerson_Depart(id);
            Depart_Archis DA = new Depart_Archis();
            r = DA.getDepart_Parent(r.Depart_Id);

            
            return r;
        }

        /*
        * 功能：获取某用户所分配的所有系统菜单
        * 参数：id 用户Id
        * 返回值：List<Menu>
       */

       public List<Menu> Get_Person_Menus(int id)
        {
            try { 
            List<Menu> lm = Persons.GetPerson_Menus(id);
            return lm;
                }
            catch { return null; }
        }
       //给用户添加一组角色
      
       public bool Add_Equips(int Person_Id,List<int> EquipIdSet)
       {
           try { 
                   foreach(var i in EquipIdSet)
                   {
                       Persons.AddEquip(Person_Id, i);
                   }
                   return true;
                }
            catch { return false; }
        }


       public int is_Role_admin(string username)
       {

           int r = Persons.isAdmin(username);
           return r;
       }

       public bool ModifyPerson_Pwd(string userName, byte[] userPwd)
       {
           return Persons.ModifyPerson_Pwd(userName, userPwd);
       }

       public Person_Info Get_Person(string userName, byte[] userPwd)
       {
           var P = Persons.GetPerson_info(userName, userPwd);
           return P;
       }

       public string Get_PqnamebyCjname(string CjName)
       {
           var P = Persons.Get_PqnamebyCjname(CjName);
           return P;
       }

       //2016.10.8
       public bool AddPW(int personid, int menuid)
       {
           PersonAndWorkflow PW = new PersonAndWorkflow();
           bool res = PW.AttachtoMenu(personid, menuid);
           return res;
       }
       public int GetPersonId(string Person_Name)
       {
           PersonAndWorkflow PW = new PersonAndWorkflow();
           int PersonId = PW.GetPersonIdByName(Person_Name);
           return PersonId;
       }
       public int GetMenuId(string Menu_Name)
       {
           PersonAndWorkflow PW = new PersonAndWorkflow();
           int MenuId = PW.GetMenuIdByName(Menu_Name);
           return MenuId;
       }



    }

  
    
}

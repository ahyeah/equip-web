using EquipDAL.Implement;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment
{
   public class DsWorkManagment
   {

       MyDsWorks md = new MyDsWorks();


       //public bool IsAlreadySave(string work_name, string event_name, string year, string month)
       //{
       //    return md.IsAlreadySave(work_name,event_name,year,month);
       //}
       public string getDstime(string work_name,string event_name)
       {

           return md.getDstime(work_name, event_name);
       }
       public int AddDsEvent(DSEventDetail ds)
       {
           return md.AddDsEvent(ds);
       }
       public DSEventDetail getdetailbyE_id(int entity_id)
       {
           return md.getdetailbyE_id(entity_id);
       }
       public bool modifystate(int entity_id, string event_name)
       {
           return md.modifystate(entity_id, event_name);
       }
    }
}

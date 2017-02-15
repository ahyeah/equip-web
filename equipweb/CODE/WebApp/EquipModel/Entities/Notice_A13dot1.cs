using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
   public  class Notice_A13dot1
   {
       public string Notice_ID { get; set; }
      
       public string Notice_Desc { get; set; }
       
       public string Notice_EquipCode { get; set; }
       public string Notice_EquipType { get; set; }
      
       public string Notice_State { get; set; }

       public string Pq_Name { get; set; }
       public string Zz_Name { get; set; }
       public string Notice_LongTxt { get; set; }
       public string Notice_CR_Person { get; set; }
       public string Notice_CR_Date { get; set; }

       public string Notice_EquipSpeciality { get; set; }
       public int Notice_A13dot1State { get; set; }//被A13dot1处理状态 "0:待处理"，"1:已处理"
       public string Notice_A13dot1_DoDateTime { get; set; }//在A13dot1处理的日期时间 ”yyy-mm-dd HH:SS:mm"
       public string Notice_A13dot1_DoUserName { get; set; }//在A13dot1处理的用户名
       public string Notice_LoadinDateTime { get; set; }//导入完整性系统的日期时间 

       public string Notice_Speciality { get; set; }
       public string Notice_UpdateDate { get; set; }

    }
}

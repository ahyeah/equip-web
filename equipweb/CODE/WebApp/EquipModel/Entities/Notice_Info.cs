using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Notice_Info
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string Notice_ID { get; set; }
        public string Notice_Type { get; set; }
        public string Notice_Desc { get; set; }
        public string Notice_Yx { get; set; }
        public string Notice_EquipCode { get; set; }
        public string Notice_Equip_ABCmark { get; set; }
        public string Notice_OverCondition { get; set; }
        public string Notice_State { get; set; }

        public string Notice_Order_ID { get; set; }

        public string Notice_FaultStart { get; set; }

        public string Notice_FaultEnd { get; set; }
        public string Notice_FaultStartTime { get; set; }
        public string Notice_FaultEndTime { get; set; }
        public string Notice_PlanGroup { get; set; }

        public string Notice_CR_Person { get; set; }
        public string Notice_CR_Date { get; set; }
        public string Notice_Stop { get; set; }

        public string Notice_Commit_Time { get; set; }
        public string Notice_FunLoc { get; set; }
        

        public string Notice_Catalog { get; set; }

       

        public string Notice_Priority { get; set; }

        public string Notice_Commit_Date { get; set; }
    
        public string Notice_QMcode { get; set; }

       
       

        

        public string Notice_FaultYx { get; set; }


        public string Notice_LongTxt { get; set; }
        public string Notice_EquipSpeciality{ get; set; }
        public int Notice_A13dot1State { get; set; }//被A13dot1处理状态 "0:待处理"，"1:已处理"
        public string Notice_A13dot1_DoDateTime { get; set; }//在A13dot1处理的日期时间 ”yyy-mm-dd HH:SS:mm"
        public string Notice_A13dot1_DoUserName { get; set; }//在A13dot1处理的用户名
        public string Notice_LoadinDateTime { get; set; }//导入完整性系统的日期时间 

        public string Notice_Speciality { get; set; }
        public string Notice_UpdateDate { get; set; }
    }
}

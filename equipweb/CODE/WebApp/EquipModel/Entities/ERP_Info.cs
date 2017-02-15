using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
   public  class GongDan
    {
        public string GD_Id { get; set; }
        public string GD_Notice_Id { get; set; }

        public string GD_Desc { get; set; }
        public string GD_State { get; set; }
        public string GD_CRDate { get; set; }
        public string GD_OrderDate { get; set; }  //下达日期
        public string GD_Order_Type { get; set; }
        public string GD_WorkingCenter { get; set; }
        public string GD_PlanGroup { get; set; }
        public string GD_Version { get; set; }

        public Decimal GD_PlanCost { get; set; }  //计划成本
        public string GD_Inputer { get; set; }  //输入者
        public string GD_Condition { get; set; }
        public string GD_EquipCode { get; set; }
        public string GD_Equip_ABCmark { get; set; }

        public string GD_EndDate { get; set; }
        public string GD_JobType { get; set; }
        public string GD_Fun_Location { get; set; }
        public string GD_Fun_Desc { get; set; }
        public string GD_PriorityTxt { get; set; }

        public string GD_WBS_Title { get; set; }
        public Decimal GD_Cost { get; set; }  //实绩成本
        public string GD_PreCG { get; set; } //预留采购
        public string GD_Real_StartDate { get; set; }//实绩开始时间

        public string GD_Equip_Catalog { get; set; }

    }

    public class Notice
    {
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
        public double  Notice_Stop { get; set; }

        public string Notice_Commit_Time { get; set; }
        public string Notice_FunLoc { get; set; }
        public string Notice_FunLocDesc { get; set; }
        
        public string Notice_Catalog { get; set; }

        public string Notice_TechDesc { get; set; }

        public string Notice_Priority { get; set; }

        public string Notice_Commit_Date { get; set; }

        public string Notice_QMcode { get; set; }

        public string Notice_FaultXX { get; set; }

        public string Notice_FaultLoc { get; set; }

        public string Notice_FaultLoss { get; set; }

        public string Notice_FaultReason { get; set; }

        public string Notice_FaultMethod { get; set; }

        public string Notice_FaultYx { get; set; }

        public string Notice_AUSP { get; set; }


    }
    public class OilInfo
    {
        public string oil_EquipCode { get; set; }
        public string oil_EquipDesc { get; set; }
        public string oil_Fun_Loc { get; set; }
        public string oil_Fun_LocDesc { get; set; }
        public string oil_Loc { get; set; }
        public string oil_Loc_Desc { get; set; }
        public string oil_Interval { get; set; }
        public string oil_Unit { get; set; }
        public string oil_Unit2 { get; set; }
        public string oil_LastDate{get;set;}
        public Decimal oil_LastNum { get; set; }

        public DateTime oil_NextDate{get;set;}

        public Decimal oil_NextNum{get;set;}

        public string  oil_Code{get;set;}
        public string oil_Desc{get;set;}


       
    }
}

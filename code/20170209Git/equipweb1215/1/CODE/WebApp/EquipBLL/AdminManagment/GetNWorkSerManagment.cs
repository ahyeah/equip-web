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
   public class GetNWorkSerManagment
   {

       GetNWorkSer NSer = new GetNWorkSer();
       public NWorkFlowSer AddNWorkEntity(string wfName, string WE_Ser = "")
       {


           return NSer.AddNWorkEntity(wfName, WE_Ser);
       }
    }
}

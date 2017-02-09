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
    public class TablesManagment
    {
        public List<A5dot1Tab1> get_Allbytime(DateTime time)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                var e = wft.get_Allbytime(time);
                return e;
            }
            catch
            {
                return null;
            }

        }
        public bool Zzsubmit(A5dot1Tab1 a5dot1Tab1)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.Zzsubmit(a5dot1Tab1);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool savefivesb(A5dot1Tab2 a5dot1Tab2)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.savefivesb(a5dot1Tab2);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool setstate_byid(int id, int state, string name)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.setstate_byid(id, state, name);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool setisworst10_byid(int id, int isworst10)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.setisworst10_byid(id, isworst10);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<A5dot1Tab1> Getdcl_listbyisZG(int isRectified, List<string> cjname)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                List<A5dot1Tab1> E = wft.Getdcl_listbyisZG(isRectified, cjname);
                return E;
            }
            catch
            {
                return null;
            }
        }

        public List<A5dot1Tab1> GetLS_listbywfe_id(List<int> wfe_id)
            {
            WorkFlowTables wft = new WorkFlowTables();
            try
                {
                List<A5dot1Tab1> E = wft.GetLS_listbywfe_id(wfe_id);
                return E;
                }
            catch
                {
                return null;
                }
            }
        public List<A5dot1Tab1> GetAll1_byym(string ym, List<string> cjname)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                List<A5dot1Tab1> E = wft.GetAll1_byym(ym, cjname);
                return E;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// code:1代表车间，2代表片区，3代表全厂
        /// 获取对应不完好数目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<A5dot1Tab1> Getbwh_byname(string name, int code)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                List<A5dot1Tab1> E = wft.Getbwh_byname(name, code);
                return E;
            }
            catch
            {
                return null;
            }
        }

        public List<A5dot1Tab2> GetAll2_byymandstate(string ym, int state)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                List<A5dot1Tab2> E = wft.GetAll2_byymandstate(ym, state);
                return E;
            }
            catch
            {
                return null;
            }
        }
        public List<A5dot1Tab2> GetAll_byymandcode(string ym, string sbcode)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                List<A5dot1Tab2> E = wft.GetAll_byymandcode(ym, sbcode);
                return E;
            }
            catch
            {
                return null;
            }
        }
        public List<A5dot1Tab1> GetAll1_byymandcode(string ym, string sbcode)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                List<A5dot1Tab1> E = wft.GetAll1_byymandcode(ym, sbcode);
                return E;
            }
            catch
            {
                return null;
            }
        }
        public List<A5dot1Tab1> GetAll1_bycode(string sbcode)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                List<A5dot1Tab1> E = wft.GetAll1_bycode(sbcode);
                return E;
            }
            catch
            {
                return null;
            }
        }
        public A5dot1Tab1 GetA_byid(int a_id)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                A5dot1Tab1 E = wft.GetA_byid(a_id);
                return E;
            }
            catch
            {
                return null;
            }
        }

        public bool Pqcheck_byid(int a_id, string pqname, DateTime time)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.Pqcheck_byid(a_id, pqname, time);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool modifytNG_byid(int a_id, int tNG)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.modifytNG_byid(a_id, tNG);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool modifytemp1_byid(int a_id, string temp1)
        {

            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.modifytemp1_byid(a_id, temp1);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool delete_byid(int id)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                wft.delete_byid(id);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public List<A5dot1Tab1> get_cj_bwh(string cj_name, int quip_num)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                var e = wft.get_cj_bwh(cj_name, quip_num);
                return e;
            }
            catch
            {
                return null;
            }
        }
        public List<A5dot1Tab1> get_pq_bwh(string pq_name, int equip_num)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                var e = wft.get_pq_bwh(pq_name, equip_num);
                return e;
            }
            catch
            {
                return null;
            }
        }
        public List<A5dot1Tab2> get_worst10byym(string ym)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                var e = wft.get_worst10byym(ym);
                return e;
            }
            catch
            {
                return null;
            }


        }

        public List<A5dot1Tab1> get_All()
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                var e = wft.get_All();
                return e;
            }
            catch
            {
                return null;
            }

        }
        public List<A5dot1Tab1> get_recordbyentity(string entity_id)
        {
            WorkFlowTables wft = new WorkFlowTables();
            try
            {
                var e = wft.getrecordbyentity(entity_id);
                return e;
            }
            catch
            {
                return null;
            }
        }
    }

}

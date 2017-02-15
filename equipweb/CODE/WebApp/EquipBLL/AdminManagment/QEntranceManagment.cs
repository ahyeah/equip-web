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
    public class QEntranceManagment
    {
        Q_Entrance qe = new Q_Entrance();
        public bool AddQ_Entrance(int p_id, int m_id, int q_id)
        {
            try
            {
                qe.AddQ_Entrance(p_id,m_id,q_id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Quick_Entrance> GetQ_EbyP_Id(int p_id)
        {
            List<Quick_Entrance> QE = new List<Quick_Entrance>();
            QE = qe.GetQ_EbyP_Id(p_id);
            return QE;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment.ZyConfig
{
    public class SpecialtyListNode
    {
        public SpecialtyListNode()
        {
            Childs = new List<int>();
        }
        public int Specialty_Id { get; set; }

        //专业名称
        public string Specialty_Name { get; set; }

        public int Parent_id { get; set; }

        public List<int> Childs { get; set; }
        public int level { get; set; }
    }
}

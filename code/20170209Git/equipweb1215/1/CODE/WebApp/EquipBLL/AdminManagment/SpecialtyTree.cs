using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment.ZyConfig
{
    public class SpecialtyTree
    {
        public SpecialtyTree()
        {
            childs = new List<SpecialtyTree>();
        }
        public int Specialty_Id { get; set; }

        //专业名称
        public string Specialty_Name { get; set; }

       


        public List<SpecialtyTree> childs { get; set; }
    }
}

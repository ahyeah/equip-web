using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Pq_Zz_map
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pq_Zz_mapId{get;set;}
        public string Pq_Name{get;set;}
        public string Zz_Code { get; set; }
        public string Zz_Name { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Pq_EC_map
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pq_EC_mapId { get; set; }
        public string Pq_Name { get; set; }
        public string Equip_Code { get; set; }

    }
}

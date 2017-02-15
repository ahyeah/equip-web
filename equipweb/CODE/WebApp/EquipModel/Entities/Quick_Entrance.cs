using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EquipModel.Entities
{
    public class Quick_Entrance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int xuhao { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Person_Info Person_Info { get; set; }
    }
}
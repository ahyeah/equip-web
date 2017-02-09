using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
   public class DsTimeOfWork
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string work_name { get; set; }

        public string event_name { get; set; }

        public string time { get; set; }
        public string time_describe { get; set; }
    }
}

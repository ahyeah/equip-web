using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
   public class DSEventDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int year { get; set; }

        public int month { get; set; }
        public int day { get; set; }
        public int week { get; set; }
        public int entity_id  { get; set; }
        public string event_name { get; set; }
        public string DSTime_Desc { get; set; }
       /// <summary>
       /// state:0代表超时完成，1代表按时完成
       /// </summary>
        public int state { get; set; }

        public DateTime? done_time { get; set; }



    }
}

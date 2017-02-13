using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
  public  class NWorkFlowSer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string WF_Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public string  WE_Ser { get; set; }

        public DateTime time { get; set; }

    }
}

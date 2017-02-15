using MVCTest.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTest.Models.User
{
    public class Depart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int dep_id { get; set; }

        public string dep_name { get; set; }

        public virtual ICollection<UserInfo> career { get; set; }
    }
}

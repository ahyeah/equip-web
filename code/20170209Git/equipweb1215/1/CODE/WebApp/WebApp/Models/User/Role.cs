using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApp.Models.User;

namespace WebApp.Models.User
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int r_id { get; set; }

        public string r_name { get; set; }

        public virtual ICollection<UserInfo> conn_users { get; set; }
    }
}

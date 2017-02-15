
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCTest.Models.User
{
    public class UserInfo
    {
        [Key]
        public string name { get; set; }
        public string password { get; set; }

        public virtual Role my_role { get; set; }

        public virtual Depart my_depart { get; set; }
    }
}
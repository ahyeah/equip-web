
using MVCTest.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCTest.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base("UserContext")
        {

        }

        public DbSet<UserInfo> users { get; set; }
        public DbSet<Depart> department { get; set; }
        public DbSet<Role> roles { get; set; }
    }
}
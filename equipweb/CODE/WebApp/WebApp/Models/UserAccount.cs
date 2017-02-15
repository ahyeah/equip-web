using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApp.Models;
using WebApp.Models.User;


namespace WebApp.Models
{
    public class UserAccount : DropCreateDatabaseIfModelChanges<UserContext>
    {
        protected override void Seed(UserContext context)
        {

            var partments = new List<Depart>
            {
                new Depart {dep_name = "设计"}
            };
            partments.ForEach(s => context.department.Add(s));

            var roles = new List<Role>
            {
                new Role {r_name = "职员"},
                new Role {r_name = "经理"}
            };
            roles.ForEach(s => context.roles.Add(s));
            context.SaveChanges();

            var users = new List<UserInfo>
            {
                new UserInfo {name = "cb", 
                    password="123", 
                    my_depart = context.department.Where(s=>s.dep_name=="设计").First(),
                    my_role = context.roles.Where(s=>s.r_name=="职员").First()},                    
                new UserInfo {name = "cb1", 
                    password="123", 
                    my_depart = context.department.Where(s=>s.dep_name=="设计").First(),
                    my_role = context.roles.Where(s=>s.r_name=="经理").First()}
            };
            users.ForEach(s => context.users.Add(s));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
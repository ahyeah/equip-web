using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class PersonAndWorkflow : BaseDAO
    {
        public int GetPersonIdByName(string Person_Name)
        {
            using (var db = base.NewDB())
            {
                int PersonId = db.Persons.Where(a => a.Person_Name == Person_Name).First().Person_Id;
                return PersonId;
            }
        }
        public int GetMenuIdByName(string Menu_Name)
        {
            using (var db = base.NewDB())
            {
                int MenuId = db.Sys_Menus.Where(a => a.Menu_Name == Menu_Name).First().Menu_Id;
                return MenuId;
            }
        }
        public bool AttachtoMenu(int personid, int menuid)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    Person_Info person = db.Persons.Where(s => s.Person_Id == personid).First();
                    Menu menu = db.Sys_Menus.Where(s => s.Menu_Id == menuid).First();
                    person.Person_Menus.Add(menu);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

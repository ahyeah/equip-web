using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class guidelines : BaseDAO
    {
        public bool delete(int fId)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    db.guidelines.Remove(db.guidelines.Where(s => s.File_Id == fId).First());
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

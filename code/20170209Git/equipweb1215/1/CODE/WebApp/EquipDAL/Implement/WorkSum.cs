using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class WorkSum : BaseDAO
    {
        public bool delete(int fId)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    db.WorkSumFiles.Remove(db.WorkSumFiles.Where(s => s.File_Id == fId).First());
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

using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Files : BaseDAO
    {
        public bool delete(int fId)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    db.Files.Remove(db.Files.Where(s => s.File_Id == fId).First());
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool AttachtoEuip(int fId, int EuipID)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    File_Info file = db.Files.Where(s => s.File_Id == fId).First();
                    Equip_Info equip = db.Equips.Where(s => s.Equip_Id == EuipID).First();
                    file.File_Equips.Add(equip);
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

using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Zhidus : BaseDAO
    {

        public List<ZhiduFile> get_zhidu(int menuid)
        {

            using (var db = base.NewDB())
            {

                try
                {
                    var E = db.ZhiduFile.Where(a => a.Menu.Menu_Id == menuid).ToList();
                    return E;
                }
                catch
                { return null; }
            }
        }
    }
}

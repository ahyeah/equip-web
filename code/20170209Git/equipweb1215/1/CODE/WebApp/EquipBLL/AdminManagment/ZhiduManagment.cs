using EquipDAL.Implement;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment
{
    public class ZhiduManagment
    {
        private Zhidus Zs = new Zhidus();
        public List<ZhiduFile> get_zhidu(int menuid)
        {
            List <ZhiduFile> E = Zs.get_zhidu(menuid);
            return E;
        }


    }
}

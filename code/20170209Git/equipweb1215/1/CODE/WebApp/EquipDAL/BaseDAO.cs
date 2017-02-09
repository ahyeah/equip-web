using EquipModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL
{
    public class BaseDAO
    {
        protected EquipWebContext NewDB()
        {            
            return new EquipWebContext();
        }
    }
}

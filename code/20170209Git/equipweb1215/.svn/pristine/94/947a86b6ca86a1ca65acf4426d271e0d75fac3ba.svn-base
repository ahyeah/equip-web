using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Interface
{
    public interface IMenus
    {
        Menu GetMenu(int id);

        List<Menu> GetMenu(string name);

        List<Menu> GetChildMenu(int id);

        bool AddMenu(int parentID, Menu newMenu);

        bool DeleteMenu(int id);

        bool ModifyMenu(int id, Menu nVal);

        
    }
}

using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Interface
{
    public interface IEquipArchis
    {
        Equip_Archi GetEquipArchi(int id);

        List<Equip_Archi> GetEquipArchi(string name);

        List<Equip_Archi> GetChildMenu(int id);

        bool AddEquipArchi(int parentID, Equip_Archi newMenu);
        bool ModifyEquipArchi(int parentID, Equip_Archi nVal);
        bool DeleteEquipArchi(int id);


    }
}

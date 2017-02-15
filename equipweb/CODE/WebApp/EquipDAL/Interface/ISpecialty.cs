using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Interface
{
    public interface ISpecialty
    {
        Speciaty_Info GetSpecialty(int id);

        List<Speciaty_Info> GetSpecialty(string name);

        List<Speciaty_Info> GetChildSpecialty(int id);

        bool AddSpecialty(int parentID, Speciaty_Info newSpecialty);

        bool DeleteSpecialty(int id);

        bool ModifySpecialty(int id, Speciaty_Info nVal);


    }
}

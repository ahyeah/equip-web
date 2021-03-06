﻿using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EquipDAL.Interface
{
    public interface IPerson_Infos
    {
        Person_Info GetPerson_info(int id);
        Person_Info GetPerson_info(string Person_Name);

         List<Role_Info> GetPerson_Roles(int id);

         List<Equip_Info> GetPerson_Equips(int id);

         Depart_Archi GetPerson_Depart(int id);


         int AddPerson_info(Person_Info New_Person);
        bool DeletePerson_info(int id);

         bool ModifyPerson_info(Person_Info New_Person);

        

    }

   
}

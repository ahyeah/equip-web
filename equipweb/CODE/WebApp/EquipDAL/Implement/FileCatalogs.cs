using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    /// <summary>
    /// 对表File_Catalog的访问接口
    /// </summary>
    public class FileCatalogs : BaseDAO
    {
        //根据分类号查找分类
        public File_Catalog GetCatalog(int cat_ID)
        {
            using(var db = base.NewDB())
            {
                var fc = db.FCatalogs.Where(s => s.Catalog_Id == cat_ID).First();
                return fc;
            }
        }

        //根据分类名称查找分类
        public List<File_Catalog> GetCatalog(string cat_name)
        {
            using(var db = base.NewDB())
            {
                return db.FCatalogs.Where(s => s.Catalog_Name == cat_name).ToList();
            }
        }

        //查找分类的子分类
        public List<File_Catalog> GetChildCatalogs(int cat_ID)
        {
            using(var db = base.NewDB())
            {
                var fc = db.FCatalogs.Where(s => s.Catalog_Id == cat_ID).First();

                if (fc == null)
                    return null;

                return fc.Child_Catalogs.ToList();
            }
        }

        //查找分类的父类
        public File_Catalog GetParentCatalog(int cat_ID)
        {
            using(var db = base.NewDB())
            {
                var fc = db.FCatalogs.Where(s => s.Catalog_Id == cat_ID).First();

                if (fc == null)
                    return null;

                return fc.parent_Catalog;
            }
        }

        public DbSet<File_Catalog> GetCatalogsSet()
        {
            return base.NewDB().FCatalogs;
        }

        /// <summary>
        /// 添加一个新分类
        /// </summary>
        /// <param name="p_ID">父节点ID</param>
        /// <param name="n_Name">新节点名称</param>
        /// <returns></returns>
        public bool AddNewCatalog(int p_ID, string n_Name)
        {
            using(var db = base.NewDB())
            {
                File_Catalog nfc = new File_Catalog();
                nfc.Catalog_Name = n_Name;

                if (p_ID != -1)
                {
                    var fc = db.FCatalogs.Where(s => s.Catalog_Id == p_ID).First();
                    if (fc == null)
                        return false;
                    fc.Child_Catalogs.Add(nfc);
                }
                else
                {
                    db.FCatalogs.Add(nfc);
                }
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteCatalog(int ID)
        {
            bool bResult = false;
            try
            {
                using(var db = base.NewDB())
                {
                    db.FCatalogs.Remove(db.FCatalogs.Where(s => s.Catalog_Id == ID).First());
                    db.SaveChanges();
                }
                bResult = true;
            }
            catch
            {
                bResult = false;
            }
            return bResult;
        }

        public bool ModifyCatalog(int ID, string name)
        {
            bool bResult = false;
            try
            {
                using(var db = base.NewDB())
                {
                    db.FCatalogs.Where(s => s.Catalog_Id == ID).First().Catalog_Name = name;
                    db.SaveChanges();
                }
            }
            catch
            {
                bResult = false;
            }
            return bResult;
        }

        public List<File_Info> GetFiles(int ID)
        {
            List<File_Info> files = null;
            try
            {
                using (var db = base.NewDB())
                {
                    files = db.FCatalogs.Where(s => s.Catalog_Id == ID).First().Files_Included.ToList();
                }
            }
            catch
            {
                files = null;
            }
            return files;
        }

        public bool AddFiletoCatalog(int pID, File_Info nf)
        {
            try
            {
                using(var db = base.NewDB())
                {
                    db.FCatalogs.Where(s => s.Catalog_Id == pID).First().Files_Included.Add(nf);
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

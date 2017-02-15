using EquipDAL.Implement;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.guidelinesmanagment
{
    public class guide_Cat_manager
    {
        private CatalogTreeNode BuildCatNodes(guidelines_catalog parent)
        {
            CatalogTreeNode pd = new CatalogTreeNode();
            pd.text = parent.Catalog_Name;
            pd.tags.Add(parent.Catalog_Id.ToString());
            pd.selectable = true;

            foreach (var cd in parent.Child_Catalogs.ToList())
            {
                pd.nodes.Add(BuildCatNodes(cd));
            }
            return pd;

        }

        //构建文件种类树
        public List<CatalogTreeNode> BuildCatTree()
        {
            List<CatalogTreeNode> nodes = new List<CatalogTreeNode>();

            guidelinescatalogs fcs = new guidelinescatalogs();

            DbSet<guidelines_catalog> ta = fcs.GetCatalogsSet();

            var roots = ta.Where(s => s.parent_Catalog == null).ToList();

            foreach (var root in roots)
            {
                nodes.Add(BuildCatNodes(root));
            }

            return nodes;
        }

        /// <summary>
        /// 增加一个文件分类
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="newCatalog"></param>
        /// <returns></returns>
        public bool AddNewCatalog(int parentID, string newCatalog)
        {
            guidelinescatalogs fcs = new guidelinescatalogs();
            return fcs.AddNewCatalog(parentID, newCatalog);
        }

        public bool DeleteCatalog(int ID)
        {
            return (new guidelinescatalogs()).DeleteCatalog(ID);
        }

        public bool ModifyFileCatalog(int ID, string name)
        {
            return (new guidelinescatalogs()).ModifyCatalog(ID, name);
        }

        public List<guiditem> GetFilesInCatalog(int ID)
        {
            List<guidelines_info> files = (new guidelinescatalogs()).GetFiles(ID);
            List<guiditem> fs = new List<guiditem>();
            foreach (var f in files)
            {
                guiditem fi = new guiditem();
                fi.id = Convert.ToString(f.File_Id);
                fi.fileName = f.File_OldName;
                fi.updateTime = f.File_UploadTime.ToString();
                fi.ext = f.File_ExtType;
                fi.path = f.File_SavePath;
                fi.fileNamePresent = f.File_NewName;
                fs.Add(fi);
            }
            return fs;
        }

        public bool AddNewFile(int pID, guiditem fi, int uID)
        {
            guidelines_info nf = new guidelines_info();
            nf.File_NewName = fi.fileNamePresent;
            nf.File_OldName = fi.fileName;
            nf.File_UploadTime = Convert.ToDateTime(fi.updateTime);
            nf.File_Submiter = (new Person_Infos()).GetPerson_info(uID);
            nf.File_SavePath = fi.path;
            nf.File_ExtType = fi.ext;
            return (new guidelinescatalogs()).AddFiletoCatalog(pID, nf);

        }

        public bool DeleteFile(int fID)
        {
            return (new guidelines()).delete(fID);
        }
    }
}

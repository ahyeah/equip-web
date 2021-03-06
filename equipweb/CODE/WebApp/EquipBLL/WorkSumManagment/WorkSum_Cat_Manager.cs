﻿using EquipDAL.Implement;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.FileManagment
{
    public class WorkSum_Cat_Manager
    {
        private WSCatalogTreeNode BuildCatNodes(WorkSumCatalog parent)
        {
            WSCatalogTreeNode pd = new WSCatalogTreeNode();
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
        public List<WSCatalogTreeNode> BuildCatTree()
        {
            List<WSCatalogTreeNode> nodes = new List<WSCatalogTreeNode>();

            WorkSummaryCatalog fcs = new WorkSummaryCatalog();
            
            DbSet<WorkSumCatalog> ta = fcs.GetCatalogsSet();

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
            WorkSummaryCatalog fcs = new WorkSummaryCatalog();
            return fcs.AddNewCatalog(parentID, newCatalog);
        }

        public bool DeleteCatalog(int ID)
        {
            return (new WorkSummaryCatalog()).DeleteCatalog(ID);
        }

        public bool ModifyFileCatalog(int ID, string name)
        {
            return (new WorkSummaryCatalog()).ModifyCatalog(ID, name);
        }

        public List<WSFileItem> GetFilesInCatalog(int ID)
        {
            List<WorkSummary> files = (new WorkSummaryCatalog()).GetFiles(ID);
            List<WSFileItem> fs = new List<WSFileItem>();
            foreach (var f in files)
            {
                Person_Infos submiter = new Person_Infos();
                
                WSFileItem fi = new WSFileItem();
                fi.id = Convert.ToString(f.File_Id);
                //fi.uploader = f.File_Submiter.Person_Name.ToString();
                fi.uploader = submiter.GetPerson_info(f.Mission_Id).Person_Name;
                fi.fileName = f.File_OldName;
                fi.updateTime = f.File_UploadTime.ToString();
                fi.ext = f.File_ExtType;
                fi.path = f.File_SavePath;
                fi.fileNamePresent = f.File_NewName;
                fs.Add(fi);
            }
            return fs;
        }

        public bool AddNewFile(int pID, WSFileItem fi, int uID)
        {
            WorkSummary nf = new WorkSummary();
            nf.File_NewName = fi.fileNamePresent;
            nf.File_OldName = fi.fileName;
            nf.File_UploadTime = Convert.ToDateTime(fi.updateTime);
            nf.File_Submiter = (new Person_Infos()).GetPerson_info(uID);
            nf.File_SavePath = fi.path;
            nf.File_ExtType = fi.ext;
            return (new WorkSummaryCatalog()).AddFiletoCatalog(pID, nf);

        }

        public bool DeleteFile(int fID)
        {
            return (new WorkSum()).delete(fID);
        }
    }
}

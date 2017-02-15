using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
//using System.Runtime.Serialization.Json;
using EquipBLL.guidelinesmanagment;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class A1dot1Controller : Controller
    {
        public class File
        {
            public string text;
            public Boolean selectable;
            public ArrayList nodes;
            public File(string text, Boolean selectable, ArrayList nodes)
            {
                this.text = text;
                this.selectable = selectable;
                this.nodes = nodes;
            }
        }
        public ActionResult Index()
        {
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            return View();
        }

        string url_father = "<a target=" + "'_blank'" + " href=";
        string url_last = "</a>";

        public ArrayList GetAll(DirectoryInfo dir, string lianjie)//搜索文件夹中的文件
        {
            ArrayList FileList = new ArrayList();
            FileInfo[] allFile = dir.GetFiles();
            //遍历文件
            foreach (FileInfo fi in allFile)
            {
                File temp = new File(url_father + lianjie + @"/" + fi.Name + "'>" + fi.Name + url_last, true, null);
                FileList.Add(temp);//+" "+flag);
            }

            //遍历文件夹
            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                File temp = new File(d.Name, false, GetAll(d, lianjie + @"/" + d.Name));
                FileList.Add(temp);
            }
            return FileList;
        }
        public string tree(object sender, EventArgs e)
        {
            //string url = @"'../A3.3FileArch";

            //string strPath = Server.MapPath(@"..\A3.3FileArch\");
            //DirectoryInfo d = new DirectoryInfo(strPath);
            //ArrayList Flst = new ArrayList();
            //Flst.Add(GetAll(d, url));
            guide_Cat_manager fcm = new guide_Cat_manager();
            var Flst = fcm.BuildCatTree();

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(Flst);
            return result;
        }

        [HttpPost]
        public bool AddFileCatalog(int parentID, string newCataName)
        {
            guide_Cat_manager fcm = new guide_Cat_manager();
            return fcm.AddNewCatalog(parentID, newCataName);
        }

        [HttpPost]
        public bool DeleteFileCatalog(int cataID)//一个
        {
            return (new guide_Cat_manager()).DeleteCatalog(cataID);
        }

        [HttpPost]
        public bool ModifyFileCatalog(int ID, string newName)
        {
            return (new guide_Cat_manager()).ModifyFileCatalog(ID, newName);
        }

        [HttpPost]
        public JsonResult GetFilesLIst(int cataID)
        {
            List<object> file_list = new List<object>();
            if (cataID == -1)
            {
                ;
            }
            else
            {
                List<guiditem> fs = (new guide_Cat_manager()).GetFilesInCatalog(cataID);
                foreach (var fi in fs)
                {
                    object o = new
                    {
                        fID = fi.id,
                        fName = fi.fileName,
                        fTime = fi.updateTime,
                        fRName = fi.fileNamePresent,
                        fPath = fi.path,
                        ext = fi.ext
                    };
                    file_list.Add(o);
                }
            }

            return Json(new { data = file_list.ToArray() });
        }

        [HttpPost]
        public bool DelFileById(int fileID)
        {
            return (new guide_Cat_manager()).DeleteFile(fileID);
        }














    }
}
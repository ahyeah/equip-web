using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipModel.Entities;
using EquipDAL.Implement;
using EquipDAL.Interface;
using EquipModel.Context;
using System.Collections;
using EquipBLL.FileManagment;

namespace EquipBLL.AdminManagment
{
   public  class A6dot2Managment
    {
       public class WDTTJ_CellModal
       {
           public string cellName;
           public string cellId;
           public int cellNum;
           public int cellSpanNum;
       }

       public  class WDTTJ_RowModal
       {
           public string starttime;
           public string endtime;
           public int Id;
           public WDTTJ_CellModal row_PqInfo=new WDTTJ_CellModal();
           public List<WDTTJ_CellModal> row_CjInfo=new  List<WDTTJ_CellModal>();
           public List<WDTTJ_CellModal> row_ZzInfo=new List<WDTTJ_CellModal>();
       }

       public class WDTHistoy_ListModal
       {
           public int ID;
           public string uploadTime;
           public string userName;
           public string wdtName;
           public string pqName;
           public string url;

       }

       public class WDT_detailModal
       {
           public int Id;
           public string uploadDesc;
           public string uploadTime;
           public string userName;
           public string uploadName;
           public List<WDTTJ_RowModal> TjContent=new List<WDTTJ_RowModal>();

       }
       public class CQDetailModal
       {
           public string equipCode;
           public string equipDesc;

           public string lastOilTime;

           public string NextOilTime;

           public string oilCode_desc;

           public string equip_PqName;
           public string equip_CjName;
           public string equip_ZzName;
           public string CQ_demo;
       }

       //临时任务添加
       public List<A6dot2LsTaskTab> getLsTask(string wfe_id)
           {
           A6dot2LsTaskTabs t = new A6dot2LsTaskTabs();
           return t.getAllLsTaskTabs(wfe_id);

           }
       public A6dot2LsTaskTab UpdateA6dot2LsTask(int id, List<string> propertis, List<object> vals)
           {
           A6dot2LsTaskTabs t = new A6dot2LsTaskTabs();
           return t.UpdateA6dot2LsTask(id, propertis, vals);

           }
       public A6dot2LsTaskTab AddA6dot2LsTask(A6dot2LsTaskTab new_A6dot2LsTask)
           {
           A6dot2LsTaskTabs t = new A6dot2LsTaskTabs();
           return t.AddA6dot2LsTask(new_A6dot2LsTask);
           }


       public bool RemoveA6dot2LsTask(int id)
           {
           A6dot2LsTaskTabs t = new A6dot2LsTaskTabs();
           return t.remove(id);
           }

       //end 
        private A6dot2Tabs WTDs = new A6dot2Tabs();
        private Files XM = new Files();
       public List<CQDetailModal> getCQdetail(string pqName,string starttime,string endtime)
        {
            Depart_Archis DA = new Depart_Archis();
           //string pqName;
           //pqName = DA.getDepart_info(pqId).Depart_Name;
           List<A6dot2Tab2> tab2 = WTDs.getCQList(pqName,starttime, endtime);
           List<CQDetailModal> r = new List<CQDetailModal>();
           foreach(var item in tab2)
           {
               CQDetailModal cq = new CQDetailModal();
               cq.equip_PqName = item.equip_PqName;
               cq.equip_CjName = item.equip_CjName;
               cq.equip_ZzName = item.equip_ZzName;
               cq.equipCode = item.equipCode;
               cq.equipDesc = item.equipDesc;
               cq.lastOilTime = item.lastOilTime;
               cq.NextOilTime = item.NextOilTime;
               cq.CQ_demo = "超期未换油";
               cq.oilCode_desc = item.oilCode_desc;
               r.Add(cq);

           }
           return r;

       }

      public List<CQDetailModal>  getCQdetailId (int Id,string pqName)
       {
          
           //string pqName;
          
           List<A6dot2Tab2> tab2 = WTDs.getCQListId(Id,pqName);
           List<CQDetailModal> r = new List<CQDetailModal>();
           foreach (var item in tab2)
           {
               CQDetailModal cq = new CQDetailModal();
               cq.equip_PqName = item.equip_PqName;
               cq.equip_CjName = item.equip_CjName;
               cq.equip_ZzName = item.equip_ZzName;
               cq.equipCode = item.equipCode;
               cq.equipDesc = item.equipDesc;
               cq.lastOilTime = item.lastOilTime;
               cq.NextOilTime = item.NextOilTime;
               cq.CQ_demo = "超期未换油";
               cq.oilCode_desc = item.oilCode_desc;
               r.Add(cq);

           }
           return r;
       }
       public WDT_detailModal getWDT_detail(int WDT_Id)
        {
           WDT_detailModal r=new WDT_detailModal();
            A6dot2Tab1 t1 = WTDs.get_WDTInfo(WDT_Id);
            r.Id = t1.Id;
            r.uploadDesc = t1.uploadDesc;
            r.uploadTime = t1.uploadtime;
            r.userName = t1.userName;
            r.uploadName = t1.uuploadFileName;
            r.TjContent = WDTTJ(r.Id);
            return r;

        }
       public List<WDTTJ_RowModal> WDTTJ(int Id)
       {
            Depart_Archis DA = new Depart_Archis();
            List<WDTTJ_RowModal> r = new List<WDTTJ_RowModal>();
           String[] pqNameList=new String[7]{"联合一片区","联合二片区","联合三片区","联合四片区","化工片区","综合片区","其他"};
           List<EquipDAL.Implement.A6dot2Tabs.WDT_PqTj> pqGroup_1 = new List<EquipDAL.Implement.A6dot2Tabs.WDT_PqTj>();
           List<EquipDAL.Implement.A6dot2Tabs.WDT_PqTj> pqGroup= WTDs.PqGroup(Id);
           List<EquipDAL.Implement.A6dot2Tabs.WDT_CjTj> cjGroup = WTDs.CjGroup(Id);
           List<EquipDAL.Implement.A6dot2Tabs.WDT_ZzTj> zzGroup = WTDs.ZzGroup(Id);
           int i=0;
           while(i<7)
           {
               if (pqGroup.Where(x => x.pqName == pqNameList[i]).Count() > 0)
                   pqGroup_1.Add(pqGroup.Where(x => x.pqName == pqNameList[i]).ToList().First());
               i = i + 1;
           }
           foreach(var p in pqGroup_1)
           {
               WDTTJ_RowModal tmp = new WDTTJ_RowModal();
               tmp.Id = Id;
               tmp.row_PqInfo.cellName = p.pqName;
               tmp.row_PqInfo.cellId = p.pqName;//DA.getDepart_Id(p.pqName);
               tmp.row_PqInfo.cellNum = p.tjNum;
               
               tmp.row_PqInfo.cellSpanNum = zzGroup.Where(x => x.pqName == p.pqName).Count();
               List<EquipDAL.Implement.A6dot2Tabs.WDT_CjTj> cjGroup_1 = cjGroup.Where(x => x.pqName == p.pqName).ToList();

               foreach(var c in cjGroup_1)
               {
                   WDTTJ_CellModal cj1 = new WDTTJ_CellModal();
                   cj1.cellName = c.cjName;
                   cj1.cellNum = c.tjNum;
                   cj1.cellSpanNum = zzGroup.Where(x=>x.cjName==c.cjName).Count();
                   tmp.row_CjInfo.Add(cj1);
                   List<EquipDAL.Implement.A6dot2Tabs.WDT_ZzTj> zzGroup_1 = zzGroup.Where(x => x.pqName == p.pqName && x.cjName == c.cjName).ToList();
               foreach(var z in zzGroup_1)
               {
                  
                   WDTTJ_CellModal Zz1 = new WDTTJ_CellModal();
                   Zz1.cellName =z.zzName;
                   Zz1.cellNum = z.tjNum;
                   Zz1.cellSpanNum = 1;
                   tmp.row_ZzInfo.Add(Zz1);
               }
               }
               r.Add(tmp);

             
              
           }

           return r;

        }

       public List<WDTTJ_RowModal> WDTTJ(string starttime,string endtime)
       {
           Depart_Archis DA = new Depart_Archis();
           List<WDTTJ_RowModal> r = new List<WDTTJ_RowModal>();
           String[] pqNameList = new String[7] { "联合一片区", "联合二片区", "联合三片区", "联合四片区", "化工片区", "综合片区", "其他" };
           List<EquipDAL.Implement.A6dot2Tabs.WDT_PqTj> pqGroup_1 = new List<EquipDAL.Implement.A6dot2Tabs.WDT_PqTj>();
           List<EquipDAL.Implement.A6dot2Tabs.WDT_PqTj> pqGroup = WTDs.PqGroup(starttime, endtime);
           List<EquipDAL.Implement.A6dot2Tabs.WDT_CjTj> cjGroup = WTDs.CjGroup(starttime,endtime);
           List<EquipDAL.Implement.A6dot2Tabs.WDT_ZzTj> zzGroup = WTDs.ZzGroup(starttime,endtime);
           int i = 0;
           while (i < 7)
           {
               if (pqGroup.Where(x => x.pqName == pqNameList[i]).Count() > 0)
                   pqGroup_1.Add(pqGroup.Where(x => x.pqName == pqNameList[i]).ToList().First());
               i = i + 1;
           }
           foreach (var p in pqGroup_1)
           { 
               WDTTJ_RowModal tmp = new WDTTJ_RowModal();
               tmp.starttime = starttime.Substring(0,10);
               tmp.endtime = endtime.Substring(0,10);
               tmp.row_PqInfo.cellName = p.pqName;
               tmp.row_PqInfo.cellId = p.pqName;//DA.getDepart_Id(p.pqName);
               tmp.row_PqInfo.cellNum = p.tjNum;
               tmp.row_PqInfo.cellSpanNum = zzGroup.Where(x=>x.pqName==p.pqName).Count();
               List<EquipDAL.Implement.A6dot2Tabs.WDT_CjTj> cjGroup_1 = cjGroup.Where(x => x.pqName == p.pqName).ToList();
               foreach (var c in cjGroup_1)
               {
                   WDTTJ_CellModal cj1 = new WDTTJ_CellModal();
                   cj1.cellName = c.cjName;
                   cj1.cellNum = c.tjNum;
                   cj1.cellSpanNum = zzGroup.Where(x => x.cjName == c.cjName).Count();
                   tmp.row_CjInfo.Add(cj1);
               
                  List<EquipDAL.Implement.A6dot2Tabs.WDT_ZzTj> zzGroup_1 = zzGroup.Where(x => x.pqName == p.pqName && x.cjName == c.cjName).ToList();
               foreach (var z in zzGroup_1)
               {

                   WDTTJ_CellModal Zz1 = new WDTTJ_CellModal();
                   Zz1.cellName = z.zzName;
                   Zz1.cellNum = z.tjNum;
                   Zz1.cellSpanNum = 1;
                   tmp.row_ZzInfo.Add(Zz1);
               }

               }
               r.Add(tmp);


           }

           return r;

       }

       public List<WDTHistoy_ListModal> getHistoryList()
        {  int i=0;
            List<WDTHistoy_ListModal> r = new List<WDTHistoy_ListModal>();
            List<A6dot2Tab1> mt=WTDs.getWDTList();
           foreach(var item in mt)
           {
               WDTHistoy_ListModal ritem = new WDTHistoy_ListModal();
               ritem.ID = ++i;
               ritem.uploadTime = item.uploadtime;
               ritem.userName = item.userName;
             
               string[] savedFileName = item.uuploadFileName.Split(new char[] { '$' });
               ritem.wdtName = savedFileName[0]; ;

               ritem.pqName = item.pqName;
               ritem.url = "/A6dot2/WDT_detail?WDT_Id="+ Convert.ToString(item.Id);
               r.Add(ritem);
           }
           return r;

        }

       public int add_WDT_list(A6dot2Tab1 WDT_List)
       {
           return WTDs.Add_A6dot2Tab1(WDT_List).Id;
       }


       public int add_WDT_content(int WDT_Id,List<A6dot2Tab2> WDT_content)
       {
           return WTDs.Add_A6dot2Tab2(WDT_Id,WDT_content);
       }
       //2016.8.11以下为6dot1归档上传文件信息至File_Info
       public bool AddNewFile(int pID, FileItem fi)
       {
           File_Info nf = new File_Info();
           nf.File_NewName = fi.fileNamePresent;
           nf.File_OldName = fi.fileName;
           nf.File_UploadTime = Convert.ToDateTime(fi.updateTime);
           nf.Mission_Id = fi.Mission_Id;
           nf.WfEntity_Id = fi.WfEntity_Id;
           nf.File_SavePath = fi.path;
           nf.File_ExtType = fi.ext;
           return (new A6dot2Tabs()).AddFiletoCatalog(pID, nf);

       }
       //2016.8.11以下为6dot1通过上传文件的时间来获取File_Id，为下一个函数设备与文件的勾连做准备
       public int GetFileIdByUpdateTime(DateTime time)
       {
           return WTDs.getfileid(time);
       }
       public int GetEquipIdBySbCode(string sbcode)
       {
           return WTDs.getequipid(sbcode);
       }

       //勾连设备与文件
       public bool AttachtoEuipAndFile(int File_Id, int EquipID)
       {
           return XM.AttachtoEuip(File_Id, EquipID);
       }

       public List<EQ> caozuoguicheng()
       {
           return WTDs.getguicheng();
       }
    }
}

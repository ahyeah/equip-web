using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipModel.Context;
namespace EquipDAL.Implement
{
    public class NoticeView : BaseDAO
    {
        /// <summary>
        /// 获取待处理通知单
        /// </summary>
        /// <returns></returns>
        public List<Notice_A13dot1> GetNotice_A13dot1()
        {
            using (var db = base.NewDB()) 
            {
                List<Notice_A13dot1> e ;
                e = db.Database.SqlQuery<Notice_A13dot1>("select * from NoticeView where  Notice_A13dot1State='0'").ToList<Notice_A13dot1>();
            
                return e;
            }
        }
        /// <summary>
        /// 获取通知单状态为1的通知单（现场工程师提交的）
        /// </summary>
        /// <returns></returns>
        public List<Notice_A13dot1> GetNotice_A13dot1Uncomp()
        {
            using (var db = base.NewDB())
            {
                List<Notice_A13dot1> e;
                e = db.Database.SqlQuery<Notice_A13dot1>("select * from NoticeView where  Notice_A13dot1State='1'").ToList<Notice_A13dot1>();

                return e;
            }
        }
       /// <summary>
       /// 修改表中状态由0-->1，同时传入操作人姓名和时间
       /// </summary>
       /// <param name="nVal"></param>
       /// <returns></returns>
        public string ModifyNoticeState(string username,string opertime,string Notice_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifynotice = db.Notice_Infos.SqlQuery("select * from Notice_Info where  Notice_ID='" + Notice_Id + "'").First();
                    modifynotice.Notice_A13dot1State = 1;                  
                    modifynotice.Notice_A13dot1_DoUserName = username;
                    modifynotice.Notice_A13dot1_DoDateTime = opertime;
                    db.SaveChanges();
                    return "成功";
                }
                catch
                {
                    return "出错！";
                }
            }
        }
        /// <summary>
        /// 删除功能，只需修改状态为2,2表示改行数据舍弃
        /// </summary>        
        /// <param name="Notice_Id"></param>
        /// <returns></returns>
        public string RemoveNoticeState(string Notice_Id)
        {
            using (var db = new EquipWebContext())
            {
                try
                {
                    var modifynotice = db.Notice_Infos.SqlQuery("select * from Notice_Info where  Notice_ID='" + Notice_Id + "'").First();
                    modifynotice.Notice_A13dot1State = 2;                    
                    db.SaveChanges();
                    return "成功";
                }
                catch
                {
                    return "出错！";
                }
            }
        }

        public Notice_A13dot1 getNoticeByNI(string Notice_Id)
        {
             using (var db = base.NewDB()) 
            {
                Notice_A13dot1 e ;
                e = db.Database.SqlQuery<Notice_A13dot1>("select * from NoticeView where  Notice_Id='"+Notice_Id+"'").First<Notice_A13dot1>();
            
                return e;
            }
               
            }
    }

}


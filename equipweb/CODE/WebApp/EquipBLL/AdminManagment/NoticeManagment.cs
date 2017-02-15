using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipModel.Entities;
using EquipDAL.Implement;
namespace EquipBLL.AdminManagment
{
    public class NoticeManagement
    { //适用于流程A13.1 获取状态为0即待处理的通知单       
        public List<Notice_A13dot1> getNoticeForA13dot1()
        {
            NoticeView Notice = new NoticeView();
            return Notice.GetNotice_A13dot1();     
        }
        /// <summary>
        /// 适用于流程A13.1 获取状态为1即待处理的通知单（现场工程师提交的）
        /// </summary>
        /// <returns></returns>
        public List<Notice_A13dot1> getNoticeForA13dot1Uncomp()
        {
            NoticeView Notice = new NoticeView();
            return Notice.GetNotice_A13dot1Uncomp();
        }
        /// <summary>
        /// 生成13.1的工作流，同时修改状态为1，传入操作人和操作时间
        /// </summary>
        /// <param name="username"></param>
        /// <param name="opertime"></param>
        /// <param name="Notice_Id"></param>
        /// <returns></returns>
        public string modifyNoticeForA13dot1(string username, string opertime, string Notice_Id)
        {
            NoticeView Notice = new NoticeView();
            return Notice.ModifyNoticeState(username,opertime,Notice_Id);
        }
        /// <summary>
        /// 删除功能，即该库中状态为2
        /// </summary>
        /// <param name="Notice_Id"></param>
        /// <returns></returns>
        public string removeNoticeForA13dot1(string Notice_Id)
        {
            NoticeView Notice = new NoticeView();
            return Notice.RemoveNoticeState(Notice_Id);
        }
        /// <summary>
        /// 通过通知单号来获取对应通知单数据
        /// </summary>
        /// <param name="Notice_Id"></param>
        /// <returns></returns>
        public Notice_A13dot1 getNoticeForA13dot1ByNI(string Notice_Id)
        {
            NoticeView Notice = new NoticeView();
            return Notice.getNoticeByNI(Notice_Id);
        }
    }
}

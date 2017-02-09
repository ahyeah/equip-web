using FlowEngine.DAL;
using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowEngine.TimerManage
{
    /// <summary>
    /// 定时任务管理器
    /// </summary>
    public class CTimerManage
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timer_deviation">定时器误差</param>
        public CTimerManage(int timer_deviation = 5)
        {
            
        }
        #endregion

        #region 属性
        /// <summary>
        /// 定时任务管理的定时时间间隔, 毫秒为单位
        /// </summary>
        public static int time_interval { get; set; }

        /// <summary>
        /// 任务列表: 由于missions在多个线程中被访问， 需要加锁
        /// </summary>
        private static List<CTimerMission> missions = new List<CTimerMission>();

        /// <summary>
        /// 主定时器
        /// </summary>
        private static Timer m_mainTimer;
        #endregion

        #region 公共方法
        /// <summary>
        /// 开始工作
        /// </summary>
        public static void Start()
        {
            m_mainTimer.Change(0, time_interval);
        }

        /// <summary>
        /// 添加一个定时性任务
        /// </summary>
        /// <param name="tm">添加的任务</param>
        public static void AppendMissionToActiveList(CTimerMission tm)
        {
            //如果添加的任务状态为ACTIVE
            if (tm.status == TM_STATUS.TM_STATUS_ACTIVE)
            {
                //添加到当前任务列表
                lock (missions) //任务列表加锁
                {
                    if (missions.Find(delegate(CTimerMission mis) { return mis.ID == tm.ID; }) == null)
                        missions.Add(tm);
                }
                //tm.manage = this;
            }
        }

        /// <summary>
        /// 初始化定时任务管理器
        /// 主要工作： 从数据库中读取定时任务信息(ACTIVE)
        /// </summary>
        public static void InitTimerManager(int interval = 1000)
        {
            time_interval = 1000; //默认时间间隔为1s
            m_mainTimer = new Timer(new TimerCallback(__Worker), null, 0, Timeout.Infinite);
            CTimerMission.Timer_deviation = (time_interval / 1000);

            //暂停定时器
            m_mainTimer.Change(0, Timeout.Infinite);

            //清空任务列表
            //加锁： 加锁成功说明定时器任务执行完毕
            lock (missions)
            {
                missions.Clear();
            }

            //加载数据库中处于ACTIVE的工作任务
            Jobs timer_jobs = new Jobs();
            List<Timer_Jobs> db_jobs = timer_jobs.GetJobsByStatus(TM_STATUS.TM_STATUS_ACTIVE);
            
            foreach(var j in db_jobs)
            {
                missions.Add(CreateTimerMission(j));
            }
        }

        /// <summary>
        /// 针对具体任务， 活动列表的响应操作
        /// </summary>
        /// <param name="tm"></param>
        public static void ActiveListActionForMission(CTimerMission tm)
        {
            if (tm == null)
                return;
            lock(missions)
            {
                CTimerMission t = missions.Find(delegate(CTimerMission miss) { return miss.ID == tm.ID; });
                if (tm.status == TM_STATUS.TM_STATUS_ACTIVE && t == null) //tm的状态为激活状态， 且未添加到活动列表
                    missions.Add(tm);
                else if (tm.status == TM_STATUS.TM_STATUS_ACTIVE && t != null) //tm的状态为激活状态， 且已添加到活动列表
                {
                    missions.Remove(t);
                    missions.Add(tm);
                }
                else if (tm.status != TM_STATUS.TM_STATUS_ACTIVE && t != null)  //tm的状态为非激活， 且已添加到活动列表
                    missions.Remove(t);
            }
        }

        /// <summary>
        /// 将tm移除当前任务列表
        /// </summary>
        /// <param name="tm">待移除任务</param>
        public static CTimerMission RemoveFromActiveList(int tmID)
        {
            CTimerMission t;
            //从任务列表中移除tm
            lock(missions)
            {
                t = missions.Find(delegate(CTimerMission miss) { return miss.ID == tmID; });
                if (t != null)
                    missions.Remove(t);
            }

            return t;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 工作线程
        /// </summary>
        private static void __Worker(object sender)
        {
            Trace.WriteLine(string.Format("Entering Manage Worker \t {0}", DateTime.Now.ToString()));
            lock (missions) //任务列表加锁
            {
                foreach (var mi in missions)
                {
                    mi.DoJob();
                }
            }
        }

        #endregion

        #region 静态方法
        /// <summary>
        /// 创建一个空任务
        /// </summary>
        /// <returns></returns>
        public static CTimerMission CreateAEmptyMission()
        {
            CTimerEmptyMission empty = new CTimerEmptyMission();
            empty.CreateTime = DateTime.Now;
            //empty.Save();
            return empty;
        }
        /// <summary>
        /// 更行定时性任务的属性
        /// </summary>
        /// <param name="mId">定时性任务ID</param>
        /// <param name="property">定时性任务属性名集合</param>
        /// <param name="val">待更新的值集合</param>
        /// <returns></returns>
        public static CTimerMission UpdateTimerMission(int mId, List<string> property, List<object> val)
        {
            Jobs timer_jobs = new Jobs();
            CTimerMission tm = CreateTimerMission(timer_jobs.UpdateJob(mId, property, val));

            ActiveListActionForMission(tm);
            return tm;
        }

        /// <summary>
        /// 加载定时性任务
        /// </summary>
        /// <param name="mId"></param>
        /// <returns></returns>
        public static CTimerMission LoadTimerMission(int mId)
        {
            Jobs timer_jobs = new Jobs();

            return CreateTimerMission(timer_jobs.GetTimerJob(mId));
        }

        /// <summary>
        /// 根据Timer_Jobs创建相应的对象
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static CTimerMission CreateTimerMission(Timer_Jobs job)
        {
            if (job == null)
                return null;

            CTimerMission J_val;
            switch (job.Job_Type)
            {
                case TIMER_JOB_TYPE.CREATE_WORKFLOW:
                    CTimerCreateWF J_create = new CTimerCreateWF();
                    J_create.Load(job);
                    J_val = J_create;
                    break;

                case TIMER_JOB_TYPE.CHANGE_STATUS:
                    CTimerWFStatus J_status = new CTimerWFStatus();
                    J_status.Load(job);
                    J_val = J_status;
                    break;

                case TIMER_JOB_TYPE.TIME_OUT:
                    CTimerTimeout J_timeout = new CTimerTimeout();
                    J_timeout.Load(job);
                    J_val = J_timeout;
                    break;

                case TIMER_JOB_TYPE.EMPTY:
                    CTimerEmptyMission J_empty = new CTimerEmptyMission();
                    J_empty.Load(job);
                    J_val = J_empty;
                    break;
                case TIMER_JOB_TYPE.Normal:
                    CTimerNormalMission J_Normal = new CTimerNormalMission();
                    J_Normal.Load(job);
                    J_val = J_Normal;
                    break;
                default:
                    J_val = null;
                    break;
            }
            return J_val;
        }

        /// <summary>
        /// 获取全部定时性任务列表
        /// </summary>
        /// <returns></returns>
        public static List<CTimerMission> GetTimerJobs()
        {
            Jobs timer_jobs = new Jobs();
            List<Timer_Jobs> db_jobs = timer_jobs.GetAllTimerJob();
            List<CTimerMission> miss = new List<CTimerMission>();

            foreach (var j in db_jobs)
            {
                miss.Add(CreateTimerMission(j));
            }
            return miss;
        }

        public static List<CTimerMission> GetTimerJobsForCustom()
        {
            Jobs timer_jobs = new Jobs();
            List<Timer_Jobs> db_jobs = timer_jobs.GetJobsByUsing(TIMER_USING.FOR_CUSTOM);
            List<CTimerMission> miss = new List<CTimerMission>();

            foreach (var j in db_jobs)
            {
                miss.Add(CreateTimerMission(j));
            }
            return miss;
        }

        /// <summary>
        /// 删除一个定时任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CTimerMission DeleteTimerJob(int id)
        {
            Jobs timer_jobs = new Jobs();
            return CTimerManage.CreateTimerMission(timer_jobs.DeleteJob(id));
        }
        #endregion
    }
}

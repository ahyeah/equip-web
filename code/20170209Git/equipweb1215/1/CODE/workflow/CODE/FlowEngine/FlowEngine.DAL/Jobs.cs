using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.DAL
{
    /// <summary>
    /// 定时性任务管理
    /// </summary>
    public class Jobs : BaseDAO
    {
        /// <summary>
        /// 获取一个定时任务
        /// </summary>
        /// <param name="job_id">任务ID</param>
        /// <returns></returns>
        public Timer_Jobs GetTimerJob(int job_id)
        {
            try
            {
                using(var db = base.NewDB())
                {
                    return db.timer_jobs.Where(a => a.JOB_ID == job_id).First();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获得所有的工作
        /// </summary>
        /// <returns></returns>
        public List<Timer_Jobs> GetAllTimerJob()
        {           
            try
            {
                using(var db = base.NewDB())
                {
                    return db.timer_jobs.ToList();
                }
            }
            catch(Exception e)
            {
                return new List<Timer_Jobs>();
            }
        }

        /// <summary>
        /// 根据任务状态选择定时性任务
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<Timer_Jobs> GetJobsByStatus(TM_STATUS status)
        {
            try
            {
                using(var db = base.NewDB())
                {
                    return db.timer_jobs.Where(a => a.status == status).ToList();
                }
            }
            catch
            {
                return new List<Timer_Jobs>();
            }
        }

        /// <summary>
        /// 根据任务用途选择定时性任务
        /// </summary>
        /// <param name="useTag"></param>
        /// <returns></returns>
        public List<Timer_Jobs> GetJobsByUsing(TIMER_USING useTag)
        {
            try
            {
                using(var db = base.NewDB())
                {
                    return db.timer_jobs.Where(a => a.t_using == useTag).ToList();
                }
            }
            catch
            {
                return new List<Timer_Jobs>();
            }
        }

        /// <summary>
        /// 根绝任务的类型选择定时性任务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Timer_Jobs> GetJobsByType(TIMER_JOB_TYPE type)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.timer_jobs.Where(a => a.Job_Type == type).ToList();
                }
            }
            catch
            {
                return new List<Timer_Jobs>();
            }
        }

        /// <summary>
        /// 添加一个定时性任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns>新定时性任务的ID</returns>
        public int AppendJob(Timer_Jobs job)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    Timer_Jobs j = db.timer_jobs.Add(job);
                    if (db.SaveChanges() != 1)
                        return -1;

                    return j.JOB_ID;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 保存一个定时任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public int SaveJob(Timer_Jobs job)
        {
            try
            {
                using(var db = base.NewDB())
                {
                    //1. 查找是否存在该任务
                   var list = db.timer_jobs.Where(a => a.JOB_ID == job.JOB_ID);
                    //2. 如果存在则更新
                    if (list.ToList().Count != 0)
                    {
                        Timer_Jobs j = list.First();
                        j.copy(job);
                        if (db.SaveChanges() != 1)
                            return -1;

                        return j.JOB_ID;
                    }
                    //3. 不存在则添加一条新记录
                    else
                    {
                        job.JOB_ID = -1;
                        job = db.timer_jobs.Add(job);
                        if (db.SaveChanges() != 1)
                            return -1;
                        return job.JOB_ID;
                    }
                }
            }
            catch(Exception e)
            {
                return -1;
            }
        }

        /// <summary>
        /// 更新一个定时任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertis"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public Timer_Jobs UpdateJob(int id, List<string> propertis, List<object> vals)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var job = db.timer_jobs.Where(a => a.JOB_ID == id).First();
                    if (propertis.Count == 0)
                        return job;

                    for (int i = 0; i < propertis.Count; i++)
                    {
                        string property = propertis[i];
                        object val = vals[i];
                        switch (property)
                        {
                            case "job_name":
                                job.job_name = val as string;
                                break;

                            case "workflow":
                                job.workflow_ID = (int)val;
                                break;

                            case "job_type":
                                job.Job_Type = (TIMER_JOB_TYPE)val;
                                break;

                            case "status":
                                job.status = (TM_STATUS)val;
                                break;

                            case "workflow_ID":
                                job.workflow_ID = (int)val;
                                break;

                            case "run_params":
                                job.run_params = val as string;
                                break;

                            case "corn_express":
                                job.corn_express = val as string;
                                break;

                            case "INT_RES_1":
                                job.INT_RES_1 = (int)val;
                                break;

                            case "INT_RES_2":
                                job.INT_RES_2 = (int)val;
                                break;

                            case "INT_RES_3":
                                job.INT_RES_3 = (int)val;
                                break;

                            case "STR_RES_1":
                                job.STR_RES_1 = val as string;
                                break;

                            case "STR_RES_2":
                                job.STR_RES_2 = val as string;
                                break;

                            case "STR_RES_3":
                                job.STR_RES_3 = val as string;
                                break;

                            default: //其他字段
                                break;
                        }
                    }
                    if (db.SaveChanges() != 1)
                        return null;

                    return job;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 删除一个定时任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Timer_Jobs DeleteJob(int id)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var job = db.timer_jobs.FirstOrDefault(a => a.JOB_ID == id);
                    if (job != null)
                        db.timer_jobs.Remove(job);

                    if (db.SaveChanges() != 1)
                        return null;

                    return job;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public List<Timer_Jobs> GetDSRemind()
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var job = db.timer_jobs.Where(a => a.Job_Type == TIMER_JOB_TYPE.TIME_OUT&&a.status==0).ToList();
                  

                    return job;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public Timer_Jobs GetDSnexttime(string jobname, int wfe_id)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var job = db.timer_jobs.Where(a => a.Job_Type == TIMER_JOB_TYPE.TIME_OUT && a.job_name == jobname && a.workflow_ID == wfe_id);
                    if (job.Count() > 0)
                        return job.First();
                    else
                        return null;

                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<Timer_Jobs> GetDSbyWorkflow(int wf_id)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var job = db.timer_jobs.Where(a => a.Job_Type == TIMER_JOB_TYPE.CREATE_WORKFLOW && a.workflow_ID == wf_id && a.status == TM_STATUS.TM_STATUS_ACTIVE).ToList();

                    return job;
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }
    }
}

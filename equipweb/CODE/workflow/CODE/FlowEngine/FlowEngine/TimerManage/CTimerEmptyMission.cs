using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.TimerManage
{
    /// <summary>
    /// 空任务
    /// </summary>
    public class CTimerEmptyMission : CTimerMission
    {
        #region 构造函数
        public CTimerEmptyMission()
        {
            type = "Empty";
            status = TM_STATUS.TM_STATUS_STOP;
            
        }
        #endregion

        /// <summary>
        /// 解析运行结果
        /// </summary>
        /// <returns></returns>
        public override object ParseRunResultToJobject()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解析运行结果
        /// </summary>
        /// <param name="strResult"></param>
        public override void ParseRunResult(string strResult)
        {
            throw new NotImplementedException();
        }
        protected override void __processing()
        {
            return;
        }

        public override void ParseRunParam(string strPars)
        {
            return;
        }

        public override object ParseRunParamsToJObject()
        {
            return null;
        }
        #region 公共方法
        public override void Save(Timer_Jobs job = null)
        {
            Timer_Jobs job1 = new Timer_Jobs();
            job1.Job_Type = TIMER_JOB_TYPE.EMPTY;
            
            base.Save(job1);
        }
        #endregion
    }
}

using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.TimerManage
{
    public class CTimerNormalMission : CTimerMission
    {
         #region 构造函数
        public CTimerNormalMission()
        {
            type = "Normal";
          //  status = TM_STATUS.TM_STATUS_STOP;
            
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
            //throw new NotImplementedException();
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
            job1.Job_Type = TIMER_JOB_TYPE.Normal;
            
            base.Save(job1);
        }
        #endregion
        private void AfterCreate()
        {
            string custom_param = "";
            base.CallCustomAction(custom_param);
        }
        protected override void __processing()
        {


            AfterCreate();

     
        }

    }
}

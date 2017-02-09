using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace FlowEngine.TimerManage
{
    /// <summary>
    /// 触发事件
    /// </summary>
    public class TriggerTiming
    {
        #region 属性
        /// <summary>
        /// cron表达式对象
        /// </summary>
        private CronExpression m_cronExpression = null;

        /// <summary>
        /// 下一次触发事件
        /// </summary>        
        private DateTime? m_nextTiming = null;

        public DateTime? NextTiming
        {
            get
            {
                return m_nextTiming;
            }
            set
            {
                m_nextTiming = value;
            }
        }

        #endregion

        #region 公有方法
        /// <summary>
        /// 从字符串解析
        /// </summary>
        /// <param name="str">时间字符串</param>
        /// <returns>解析成功返回True, 否则返回False</returns>
        public bool FromString(string str)
        {
            //如果不是正确的cron表达式
            if (CronExpression.IsValidExpression(str) == false)
                return false;

            //构建新的CronExpression
            if (m_cronExpression != null)
                m_cronExpression = null;

            m_cronExpression = new CronExpression(str);
            return true;
        }

        /// <summary>
        /// 反解析到字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (m_cronExpression == null)
                return "";

            return m_cronExpression.ToString();
        }

        /// <summary>
        /// 获得下次触发事件, 以dt_source为基础
        /// </summary>
        /// <returns></returns>
        public void RefreshNextTiming(DateTime dt_source)
        {    
            if (m_cronExpression != null)
            {
                DateTimeOffset? dto = m_cronExpression.GetNextValidTimeAfter(dt_source);
                if (dto != null)
                    NextTiming = dto.Value.LocalDateTime;
                else
                    NextTiming = null;
            }

            
        }
        #endregion
    }
}

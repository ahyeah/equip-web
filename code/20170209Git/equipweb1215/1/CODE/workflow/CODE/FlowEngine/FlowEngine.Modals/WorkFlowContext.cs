///////////////////////////////////////////////////////////
//  WorkFlowContext.cs
//  Implementation of the Class WorkFlowContext
//  Generated by Enterprise Architect
//  Created on:      13-11月-2015 14:09:41
//  Original author: Chenbin
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;



namespace FlowEngine.Modals {
    public class WorkFlowContext : DbContext
    {
        public WorkFlowContext()
        {

		}

		~WorkFlowContext(){

		}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mission>().HasMany(p => p.next_Mission).WithOptional(p => p.pre_Mission);
            modelBuilder.Entity<CURR_Mission>().HasRequired(p => p.WFE_Parent).WithMany(p => p.Curr_Mission).HasForeignKey(p => p.WFE_ID);
            
            base.OnModelCreating(modelBuilder);
        }

		/// <summary>
		/// 工作流定义表
		/// </summary>
		public DbSet<WorkFlow_Define> workflow_define{
			get;
			set;
		}

		/// <summary>
		/// 工作流实体表
		/// </summary>
		public DbSet<WorkFlow_Entity> workflow_entities{
			get;
			set;
		}

        /// <summary>
        /// 工作流已完成的任务
        /// </summary>
		public DbSet<Mission> mission{
			get;
			set;
		}

        /// <summary>
        /// 过程记录的Record信息
        /// </summary>
		public DbSet<Process_Record> process_records{
			get;
			set;
		}

        /// <summary>
        /// 过程记录的变量的值
        /// </summary>

        public DbSet<Mission_Param> process_parms
        {
            get;
            set;
        }

        public DbSet<CURR_Mission> current_mission
        {
            get;
            set;
        }

        public DbSet<Timer_Jobs> timer_jobs
        {
            get;
            set;
        }

	}//end WorkFlowContext

}//end namespace FlowEngine.Modals
///////////////////////////////////////////////////////////
//  IFlow.cs
//  Implementation of the Interface IFlow
//  Generated by Enterprise Architect
//  Created on:      28-10月-2015 13:57:50
//  Original author: chenbin
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace FlowEngine.Flow {
	/// <summary>
	/// 流程接口
	/// 主要负责维护事件（Event）之间的联系——状态迁移
	/// </summary>
	public interface IFlow  {

		/// <summary>
		/// 获得/设置流程（Flow）的目的事件
		/// </summary>
		string destination_event{
			get;
			set;
		}

		/// <summary>
		/// 计算流（Flow）表达式的值
		/// 返回值：真假
		/// </summary>
		bool Evaluate();

		/// <summary>
		/// 获取/设置流（Flow）表达式
		/// </summary>
		string expression{
			get;
			set;
		}

		/// <summary>
		/// 获得/设置流程（Flow）的源事件（Event）的名称
		/// </summary>
		string source_event{
			get;
			set;
		}
	}//end IFlow

}//end namespace Flow
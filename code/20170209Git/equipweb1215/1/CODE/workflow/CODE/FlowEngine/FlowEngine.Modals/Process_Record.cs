///////////////////////////////////////////////////////////
//  Process_Record.cs
//  Implementation of the Class Process_Record
//  Generated by Enterprise Architect
//  Created on:      13-11��-2015 20:25:07
//  Original author: Chenbin
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



using FlowEngine.Modals;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FlowEngine.Modals {
	/// <summary>
	/// ������ʵ�幤����¼���磬�û�id��ʱ�䣬�� ��Ҫ˵�����ǣ�Ϊ�˱�֤����������ģ�������ҵ��Ķ����ԣ�
	/// �ڴ˲�������������Ϣ���ͣ���Ҫ�û����ݾ���ҵ��Ӧ��ָ��
	/// </summary>
	public class Process_Record {

		public Process_Record(){

		}

		~Process_Record(){

		}

		/// <summary>
		/// ��¼ID������
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Re_Id{
			get;
			set;
		}

		/// <summary>
		/// ��¼�����ƣ��磬�û�ID�� ʱ���
		/// </summary>
		public string Re_Name{
			get;
			set;
		}

		/// <summary>
		/// ��¼��ֵ�� ����� Info_Name Ϊʱ�䣬 ����ֶο���Ϊ 2015/11/2 19:30:00
		/// </summary>
		public string Re_Value{
			get;
			set;
		}

        /// <summary>
        /// ��¼������Mission
        /// </summary>
        public virtual Mission Re_Mission { get; set; }

	}//end Process_Record

}//end namespace FlowEngine.Modals
﻿///////////////////////////////////////////////////////////
//  CAuthority.cs
//  Implementation of the Class CCombEvent
//  Generated by Enterprise Architect
//  Created on:      26-10月-2015 9:46:30
//  Original author: chenbin
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace FlowEngine.Authority
{
    /// <summary>
    /// 权限认证
    /// </summary>
    public class CAuthority : IXMLEntity
    {
        //权限认证串——EntitySql

        public string auth_string { get; set; }

        /// <summary>
        /// 将参数值填充到查询语句中，使其不含参数
        /// </summary>
        /// <param name="inter_params"></param>
        /// <param name="outer_params"></param>
        /// <returns>返回填充后的字符串</returns>
        public string FillParams(IDictionary<string, object> inter_params,
                               IDictionary<string, object> outer_params)
        {
            string ret_string = "";

            ret_string = auth_string;

            Dictionary<string, List<string>> pars_sql = GetParams();

            //设置内部变量
            if (inter_params != null)
            {
                foreach (var intps in pars_sql["inter"])
                {
                    //内部变量为CParam， 其值的类型只有string, int, float, boolean等
                    switch (inter_params[intps].GetType().Name)
                    {
                        case "String":
                            ret_string = ret_string.Replace(@"@IP_" + intps, "'" + Convert.ToString(inter_params[intps]) + "'");
                            break;

                        default:
                            ret_string = ret_string.Replace(@"@IP_" + intps, Convert.ToString(inter_params[intps]));
                            break;
                    }
                }
            }

            if (outer_params != null)
            {
                //设置外部变量
                foreach (var outps in pars_sql["outer"])
                {
                    //外部变量可能比较复杂，暂时仅考虑字符串
                    switch (outer_params[outps].GetType().Name)
                    {
                        case "String":
                            ret_string = ret_string.Replace(@"@OP_" + outps, "'" + Convert.ToString(outer_params[outps]) + "'");
                            break;

                        default:
                            ret_string = ret_string.Replace(@"@OP_" + outps, Convert.ToString(outer_params[outps]));
                            break;
                    }
                }
            }

            return ret_string;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inter_params">内部参数，指以auth_string中@IP_name格式的名字为name的参数</param>
        /// <param name="outer_params">外部参数，指以auth_string中@OP_name格式的名字为name的参数</param>
        /// <returns>是否通过权限认证, 若查询记录集的个数不为0则为真， 否则为假</returns>
        public bool CheckAuth<T>(IDictionary<string, object> inter_params,
                                IDictionary<string, object> outer_params,
                                ObjectContext Context)
        {
            try
            {
                if (auth_string == null || auth_string.Trim() == "")
                    return true;

                Dictionary<string, List<string>> pars_sql = GetParams();

                //
                ObjectQuery<T> contextQuery = new ObjectQuery<T>(auth_string, Context);

                //设置内部变量
                foreach (var intps in pars_sql["inter"])
                {
                    contextQuery.Parameters.Add(new ObjectParameter("IP_" + intps, inter_params[intps]));
                }

                //设置外部变量
                foreach (var outps in pars_sql["outer"])
                {
                    contextQuery.Parameters.Add(new ObjectParameter("OP_" + outps, outer_params[outps]));
                }

                if (contextQuery.ToList().Count == 0)
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 返回权限认证串中的参数名称
        /// </summary>
        /// <returns>
        /// Key = "inter" 或 "outer"
        /// Value = inter_params 或 outer_params
        /// </returns>
        private Dictionary<string, List<string>> GetParams()
        {
            Dictionary<string, List<string>> pars = new Dictionary<string, List<string>>();
            pars["inter"] = new List<string>();
            pars["outer"] = new List<string>();

            //利用正则表达式提取sql语句中的变量名
            Regex paramReg = new Regex(@"[^@@](?<p>@\w+)");
            MatchCollection matches = paramReg.Matches(String.Concat(auth_string, " "));
            foreach (Match m in matches)
            {
                string par_sql = m.Groups["p"].Value;

                if (par_sql.IndexOf("@IP_") == 0) //含有内部变量前缀
                {
                    if (!pars["inter"].Contains(par_sql.Substring(4)))
                        pars["inter"].Add(par_sql.Substring(4));
                }
                else if (par_sql.IndexOf("@OP_") == 0)
                {
                    if (!pars["outer"].Contains(par_sql.Substring(4)))
                        pars["outer"].Add(par_sql.Substring(4));
                }
                else
                    throw new Exception("Authority Checking String Is Error!");
            }


            return pars;
        }
        public void InstFromXmlNode(System.Xml.XmlNode xmlNode)
        {
            if (xmlNode == null)
            {
                auth_string = "";
                return;
            }

            if (xmlNode.Name != "authority")
                return;

            auth_string = xmlNode.InnerText;
        }

        public System.Xml.XmlNode WriteToXmlNode()
        {

            XmlDocument doc = new XmlDocument();
            XmlElement xe = doc.CreateElement("authority");
            xe.AppendChild(doc.CreateCDataSection(auth_string));
            return xe;
        }
    }
}

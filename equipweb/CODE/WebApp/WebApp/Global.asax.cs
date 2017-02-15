using FlowEngine.TimerManage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Models;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string DummyPageUrl = @"http://localhost/Cache/Index";
        private const string DummyKey = @"ForGlobalTimer";
        protected void Application_Start()
        {
            //FileStream fs = new FileStream("D:\\web\\ZJtest.txt", FileMode.Append, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs); // 创建写入流
            //sw.WriteLine("App_Start" + "   " + DateTime.Now.ToString()); // 写入
            //sw.Close(); //关闭文件
            // Database.SetInitializer(new UserAccount());
            Database.SetInitializer(new WorkFlowInit());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EquipModel.Context.EquipWebContext>());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //CTimerManage.InitTimerManager();
            //CTimerManage.Start();

            RegisterCacheEntry();
        }
        //protected void Application_End()
        //{
        //    FileStream fs = new FileStream("D:\\web\\ZJtest.txt", FileMode.Append, FileAccess.Write);
        //    StreamWriter sw = new StreamWriter(fs); // 创建写入流
        //    sw.WriteLine("App_End" + "   " + DateTime.Now.ToString()); // 写入
        //    sw.Close(); //关闭文件
        //}
        private void RegisterCacheEntry()
        {
            //FileStream fs = new FileStream("D:\\web\\ZJtest.txt", FileMode.Append, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs); // 创建写入流
            //sw.WriteLine("RegisterCacheEntry" + "   " + DateTime.Now.ToString()); // 写入
            //sw.Close(); //关闭文件
            if (HttpContext.Current.Cache[DummyKey] != null)
                return;
            HttpContext.Current.Cache.Add(DummyKey, "Timer", null, DateTime.MaxValue, TimeSpan.FromMinutes(1), System.Web.Caching.CacheItemPriority.NotRemovable,
                new System.Web.Caching.CacheItemRemovedCallback(CacheItemRemovedCallback));
        }

        public void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            //FileStream fs = new FileStream("D:\\web\\ZJtest.txt", FileMode.Append, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs); // 创建写入流
            //sw.WriteLine("CacheItemRemovedCallback" + "   " + DateTime.Now.ToString()); // 写入
            //sw.Close(); //关闭文件
            WebClient client = new WebClient();
            client.DownloadData(DummyPageUrl);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //FileStream fs = new FileStream("D:\\web\\ZJtest.txt", FileMode.Append, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs); // 创建写入流
            //sw.WriteLine("Application_BeginRequest" + "   " + DateTime.Now.ToString()); // 写入
            //sw.Close(); //关闭文件
            if (HttpContext.Current.Request.Url.ToString() == DummyPageUrl)
                RegisterCacheEntry();
        }
    }
}


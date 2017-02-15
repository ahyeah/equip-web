using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class CacheController : Controller
    {
        //
        // GET: /Cache/
        public ActionResult Index()
        {
            FileStream fs = new FileStream("D:\\web\\ZJtestCache.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs); // 创建写入流
            sw.WriteLine("Cache" + "   " + DateTime.Now.ToString()); // 写入
            sw.Close(); //关闭文件
            return View();
        }
	}
}
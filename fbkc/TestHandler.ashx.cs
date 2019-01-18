using Model;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fbkc
{
    /// <summary>
    /// TestHandler 的摘要说明
    /// </summary>
    public class TestHandler : IHttpHandler
    {
        private class Person
        {
            public string Name { get; set; }
            public string Age { get; set; }
        }
        private string hostName = "100导航";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            //用的时候考代码即可，只需改三个地方：模板所在文件夹、添加数据、设定模板
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/templates"));//模板文件所在的文件夹，例如我的模板为templates文件夹下的TestNV.html
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("hostName", hostName);//可添加多个数据，基本支持所有数据类型，包括字典、数据、datatable等  添加数据，在模板中可以通过$dataName来引用数据　
            vltContext.Put("lunboTitle", GetNewsByCId("6", "20"));//轮播标题，推荐新闻
            vltContext.Put("tuijianTitle", GetProductByCId("12", "20"));//推荐产品
            vltContext.Put("productTitle", GetProductByCId("30", "20"));//最新产品，无分类
            vltContext.Put("newsTitle", GetNewsByCId("30", "20"));//最新新闻
            Template vltTemplate = vltEngine.GetTemplate("MainPage.html");//设定模板
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);

            string html = vltWriter.GetStringBuilder().ToString();
            context.Response.Write(html);//返回渲染生成的标准html代码字符串
        }
        BLL bll = new BLL();
        public List<htmlPara> GetNewsByCId(string count, string columnId)
        {
            List<htmlPara> hList = bll.GetHtmlList(string.Format("where columnId='{0}' order by addTime desc", columnId), count);
            return hList;
        }
        public List<htmlPara> GetProductByCId(string count, string columnId)
        {
            List<htmlPara> hList = bll.GetHtmlList(string.Format("where columnId!='{0}' order by addTime desc", columnId), count);
            return hList;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
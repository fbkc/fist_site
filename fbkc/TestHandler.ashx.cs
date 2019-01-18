using Model;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace fbkc
{
    /// <summary>
    /// TestHandler 的摘要说明
    /// </summary>
    public class TestHandler : IHttpHandler
    {
        private string hostName = "100导航";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append(_strContent.Append("404.html"));
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        case "mainpage": _strContent.Append(MainPage(context)); break;
                        case "list-21": _strContent.Append(MoreProduct(context)); break;
                        case "list-22": _strContent.Append(MoreNews(context)); break;
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        /// <summary>
        /// 主页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MainPage(HttpContext context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("hostName", hostName);
            dic.Add("lunboTitle", GetNewsByCId("6", "20"));//轮播标题，推荐新闻
            dic.Add("tuijianTitle", GetNoNewsByCId("12", "20"));//推荐产品
            dic.Add("productTitle", GetNoNewsByCId("30", "20"));//最新产品，无分类
            dic.Add("newsTitle", GetNewsByCId("30", "20"));//最新新闻
            return WriteTemplate(dic, "MainPage.html");
        }
        /// <summary>
        /// 更多产品页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MoreProduct(HttpContext context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("hostName", hostName);
            dic.Add("c1Title", GetProductByCId("10", "1"));
            dic.Add("c2Title", GetProductByCId("10", "2"));
            dic.Add("c3Title", GetProductByCId("10", "3"));
            dic.Add("c4Title", GetProductByCId("10", "4"));//工具量具，不显示
            dic.Add("c5Title", GetProductByCId("10", "5"));
            dic.Add("c6Title", GetProductByCId("10", "6"));
            dic.Add("c7Title", GetProductByCId("10", "7"));
            dic.Add("c8Title", GetProductByCId("10", "8"));
            dic.Add("c9Title", GetProductByCId("10", "9"));
            dic.Add("c10Title", GetProductByCId("10", "10"));
            dic.Add("c11Title", GetProductByCId("10", "11"));
            dic.Add("c12Title", GetProductByCId("10", "12"));
            dic.Add("c13Title", GetProductByCId("10", "13"));
            dic.Add("c14Title", GetProductByCId("10", "14"));
            dic.Add("c15Title", GetProductByCId("10", "15"));
            dic.Add("c16Title", GetProductByCId("10", "16"));
            dic.Add("c17Title", GetProductByCId("10", "17"));
            dic.Add("c18Title", GetProductByCId("10", "18"));
            dic.Add("c19Title", GetProductByCId("10", "19"));
            return WriteTemplate(dic, "list-21.html");
        }
        /// <summary>
        /// 更多新闻页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MoreNews(HttpContext context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("hostName", hostName);
            dic.Add("lunboTitle", GetNewsByCId("6", "20"));//轮播标题，推荐新闻
            dic.Add("tuijianTitle", GetProductByCId("12", "20"));//推荐产品
            dic.Add("productTitle", GetProductByCId("30", "20"));//最新产品，无分类
            dic.Add("newsTitle", GetNewsByCId("30", "20"));//最新新闻
            return WriteTemplate(dic, "list-22.html");
        }
        /// <summary>
        /// 渲染模板引擎
        /// </summary>
        /// <param name="dic">需要替换的参数</param>
        /// <param name="temp">html文件名</param>
        /// <returns></returns>
        public string WriteTemplate(Dictionary<string, object> dic, string temp)
        {
            //用的时候考代码即可，只需改三个地方：模板所在文件夹、添加数据、设定模板
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/hyzx"));//模板文件所在的文件夹，例如我的模板为templates文件夹下的TestNV.html
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            foreach (KeyValuePair<string, object> kvp in dic)
            {
                vltContext.Put(kvp.Key, kvp.Value);//可添加多个数据，基本支持所有数据类型，包括字典、数据、datatable等  添加数据，在模板中可以通过$dataName来引用数据　
            }
            Template vltTemplate = vltEngine.GetTemplate(temp);//设定模板
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);

            string html = vltWriter.GetStringBuilder().ToString();
            return html;//返回渲染生成的标准html代码字符串
        }

        private BLL bll = new BLL();
        public List<htmlPara> GetNewsByCId(string count, string columnId)
        {
            List<htmlPara> hList = bll.GetHtmlList(string.Format("where columnId='{0}' order by addTime desc", columnId), count);
            return hList;
        }
        public List<htmlPara> GetNoNewsByCId(string count, string columnId)
        {
            List<htmlPara> hList = bll.GetHtmlList(string.Format("where columnId!='{0}' order by addTime desc", columnId), count);
            return hList;
        }
        public List<htmlPara> GetProductByCId(string count, string columnId)
        {
            List<htmlPara> hList = bll.GetHtmlList(string.Format("where columnId ='{0}' order by addTime desc", columnId), count);
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
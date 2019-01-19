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
        private string hostUrl = "http://39.105.196.3:8173/hyzx/";
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
                        case "list-1": _strContent.Append(GetProduct(context, "1")); break;
                        case "list-21": _strContent.Append(MoreProduct(context)); break;//更多产品
                        case "list-22": _strContent.Append(MoreNews(context)); break;//更多新闻
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
            var data = new
            {
                htmlTitle = hostName,
                hostName,
                hostUrl,
                lunboTitle=GetNewsByCId("6", "20"),//轮播标题，推荐新闻
                tuijianTitle=GetNoNewsByCId("12", "20"),//推荐产品
                productTitle=GetNoNewsByCId("30", "20"),//最新产品，无分类
                newsTitle = GetNewsByCId("30", "20")//最新新闻
            };
            return WriteTemplate(data, "MainPage.html");
        }
        /// <summary>
        /// 产品列表页
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cId"></param>
        /// <returns></returns>
        public string GetProduct(HttpContext context, string cId)
        {
            var data = new
            {
                htmlTitle = "产品栏目",
                hostName,
                hostUrl,
                cId,
                productList = GetProductByCId("10", cId),
                newsList = GetNewsByCId("20","20")
            };
            return WriteTemplate(data, "list-"+cId+".html");
        }
        /// <summary>
        /// 新闻列表页
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cId"></param>
        /// <returns></returns>
        public string GetNews(HttpContext context, string cId)
        {
            var data = new
            {
                htmlTitle = "产品栏目",
                hostName,
                hostUrl,
                cId,
                productList = GetProductByCId("10", cId)
            };
            return WriteTemplate(data, "list-" + cId + ".html");
        }
        /// <summary>
        /// 更多产品页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MoreProduct(HttpContext context)
        {
            string cId = "21";
            var data = new
            {
                htmlTitle = "产品栏目",
                hostName,
                hostUrl,
                cId,
                c1Title = GetProductByCId("10", "1"),
                c2Title = GetProductByCId("10", "2"),
                c3Title = GetProductByCId("10", "3"),
                c4Title = GetProductByCId("10", "4"),//工具量具，不显示
                c5Title = GetProductByCId("10", "5"),
                c6Title = GetProductByCId("10", "6"),
                c7Title = GetProductByCId("10", "7"),
                c8Title = GetProductByCId("10", "8"),
                c9Title = GetProductByCId("10", "9"),
                c10Title = GetProductByCId("10", "10"),
                c11Title = GetProductByCId("10", "11"),
                c12Title = GetProductByCId("10", "12"),
                c13Title = GetProductByCId("10", "13"),
                c14Title = GetProductByCId("10", "14"),
                c15Title = GetProductByCId("10", "15"),
                c16Title = GetProductByCId("10", "16"),
                c17Title = GetProductByCId("10", "17"),
                c18Title = GetProductByCId("10", "18"),
                c19Title = GetProductByCId("10", "19")
            };
            return WriteTemplate(data, "list-21.html");
        }
        /// <summary>
        /// 更多新闻页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MoreNews(HttpContext context)
        {
            string cId = "22";
            var data = new
            {
                htmlTitle = "新闻栏目",
                hostName,
                hostUrl,
                cId,
                newsTitle = GetNewsByCId("30", "20")
            };
            return WriteTemplate(data, "list-22.html");
        }
        /// <summary>
        /// 渲染模板引擎
        /// </summary>
        /// <param name="dic">需要替换的参数</param>
        /// <param name="temp">html文件名</param>
        /// <returns></returns>
        public string WriteTemplate(object data, string temp)
        {
            //用的时候考代码即可，只需改三个地方：模板所在文件夹、添加数据、设定模板
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/templates"));//模板文件所在的文件夹，例如我的模板为templates文件夹下的TestNV.html
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("data", data);//可添加多个数据，基本支持所有数据类型，包括字典、数据、datatable等  添加数据，在模板中可以通过$dataName来引用数据
            Template vltTemplate = vltEngine.GetTemplate(temp);//设定模板
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);

            string html = vltWriter.GetStringBuilder().ToString();
            return html;//返回渲染生成的标准html代码字符串
        }

        private BLL bll = new BLL();
        /// <summary>
        /// 获取最新新闻
        /// </summary>
        /// <param name="count">新闻条数</param>
        /// <param name="columnId">行业id</param>
        /// <returns></returns>
        public List<htmlPara> GetNewsByCId(string count, string columnId)
        {
            List<htmlPara> hList = bll.GetHtmlList(string.Format("where columnId='{0}' order by addTime desc", columnId), count);
            return hList;
        }
        /// <summary>
        /// 获取最新产品，无分类
        /// </summary>
        /// <param name="count">产品条数</param>
        /// <param name="columnId">行业id</param>
        /// <returns></returns>
        public List<htmlPara> GetNoNewsByCId(string count, string columnId)
        {
            List<htmlPara> hList = bll.GetHtmlList(string.Format("where columnId!='{0}' order by addTime desc", columnId), count);
            return hList;
        }
        /// <summary>
        /// 根据行业id获取最新产品
        /// </summary>
        /// <param name="count"></param>
        /// <param name="columnId"></param>
        /// <returns></returns>
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
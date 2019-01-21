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
                        case "list-1": _strContent.Append(GetProduct(context, "1")); break;//产品列表
                        case "list-20": _strContent.Append(GetNews(context, "20")); break;//新闻列表
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
                lunboTitle = GetParaByCId("20", 1,6),//轮播标题，推荐新闻
                tuijianTitle = GetNoNewsByCId("12", "20"),//推荐产品
                productTitle = GetNoNewsByCId("30", "20"),//最新产品，无分类
                newsTitle = GetParaByCId("20", 1,30)//最新新闻
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
            int pageIndex = 1;
            if (context.Request["pageIndex"] != null)
            {
                pageIndex = int.Parse(context.Request["pageIndex"]);
            }
            int pageTotal = bll.GetPageTotal(cId);//此行业总条数
            int pageCount = (int)Math.Ceiling(pageTotal/20.0);//总页数（每页20条）
            object[] pageData = new object[pageTotal];
            for(int i=0;i< pageCount; i++)
            {
                pageData[i] = new { Href= "TestHandler.ashx?action=list-"+cId+"-"+(i+1),Title=i+1 };
            }
            var data = new
            {
                htmlTitle = "产品栏目",
                hostName,
                hostUrl,
                cId,
                productList = GetParaByCId(cId,pageIndex,20),
                pageData,
                pageTotal,
                newsList = GetParaByCId("20", 1,20)
            };
            return WriteTemplate(data, "list-" + cId + ".html");
        }
        /// <summary>
        /// 新闻列表页
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cId"></param>
        /// <returns></returns>
        public string GetNews(HttpContext context, string cId)
        {
            int pageIndex = 1;
            if(context.Request["pageIndex"]!=null)
            {
                pageIndex=int.Parse(context.Request["pageIndex"]);
            }
            var data = new
            {
                htmlTitle = "新闻栏目",
                hostName,
                hostUrl,
                cId,
                newsList = GetParaByCId("20", pageIndex, 20),
                productList = GetNoNewsByCId("20", "20")
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
                c1Title = GetParaByCId("1", 1,10),
                c2Title = GetParaByCId("2", 1, 10),
                c3Title = GetParaByCId("3", 1, 10),
                c4Title = GetParaByCId("4", 1, 10),//工具量具，不显示
                c5Title = GetParaByCId("5", 1, 10),
                c6Title = GetParaByCId("6", 1, 10),
                c7Title = GetParaByCId("7", 1, 10),
                c8Title = GetParaByCId("8", 1, 10),
                c9Title = GetParaByCId("9", 1, 10),
                c10Title = GetParaByCId("10", 1, 10),
                c11Title = GetParaByCId("11", 1, 10),
                c12Title = GetParaByCId("12", 1, 10),
                c13Title = GetParaByCId("13", 1, 10),
                c14Title = GetParaByCId("14", 1, 10),
                c15Title = GetParaByCId("15", 1, 10),
                c16Title = GetParaByCId("16", 1, 10),
                c17Title = GetParaByCId("17", 1, 10),
                c18Title = GetParaByCId("18", 1, 10),
                c19Title = GetParaByCId("19", 1, 10)
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
                newsTitle = GetParaByCId("20", 1,30)
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
        /// 根据行业id获取最新产品
        /// </summary>
        /// <param name="columnId">行业Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public List<htmlPara> GetParaByCId(string columnId,int pageIndex,int pageSize )
        {
            List<htmlPara> hList = bll.GetHtmlList(columnId,pageIndex,pageSize);
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
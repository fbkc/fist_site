using AutoSend;
using HRMSys.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace fbkc
{
    /// <summary>
    /// MainHandler 的摘要说明
    /// </summary>
    public class MainHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8;";
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append(_strContent.Append(json.WriteJson(0, "禁止访问", new { })));
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        case "getmainhtmllist": _strContent.Append(GetMainHtmlInfo(context)); break;
                        case "getcidhtmllist": _strContent.Append(GetCIdHtmlInfo(context)); break;
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        /// <summary>
        /// 目录首页显示信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetMainHtmlInfo(HttpContext context)
        {
            string realmNameId = "1";//目录名
            string columnId = context.Request["columnId"];//行业id

            List<htmlInfo> hList = null;
            try
            {
                //根据行业id、目录名和添加时间降序查询
                hList = GetMainHtmlList(realmNameId);
                if (hList == null || hList.Count < 1)
                    return json.WriteJson(0, "未获取到标题信息", new { });
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "成功", new { htmlList = hList });
        }
        /// <summary>
        /// 获取目录主页内容
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<htmlInfo> GetMainHtmlList(string sqlstr)
        {
            List<htmlInfo> hList = new List<htmlInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet(@"select title,titleImg,titleURL,addTime,columnId,columnName,realmNameId from 
( select 
RANK()OVER(PARTITION BY columnInfo.Id ORDER BY htmlInfo.addTime DESC) AS
RANK2, title,titleImg,titleURL,addTime,htmlInfo.columnId,columnName,realmNameId from 
htmlInfo left join columnInfo On htmlInfo.columnId = columnInfo.Id) T
where RANK2<=10 and realmNameId='" + sqlstr + "'").Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                htmlInfo hInfo = new htmlInfo();
                hInfo.title = (string)row["title"];
                hInfo.titleImg = (string)row["titleImg"];
                hInfo.titleURL = (string)row["titleURL"];
                hInfo.columnId = (string)row["columnId"];//栏目Id
                hInfo.addTime = row["addTime"].ToString();
                hInfo.realmNameId = (string)row["realmNameId"];//目录名
                hList.Add(hInfo);
            }
            return hList;
        }
        /// <summary>
        /// 每个栏目拿到的信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetCIdHtmlInfo(HttpContext context)
        {
            string realmNameId = "1";//目录名
            string columnId = context.Request["columnId"];//行业id
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            List<htmlInfo> hList = null;
            try
            {
                //根据行业id、目录名和添加时间降序查询
                hList = GetHtmlList(string.Format("where h.userId=u.Id and h.columnId='{0}' and h.realmNameId='{1}' order by h.addTime desc", columnId, realmNameId));
                if (hList == null || hList.Count < 1)
                    return json.WriteJson(0, "未获取到标题信息", new { });
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.Message, new { }); }
            //查询分页数据
            var pageData = hList.Where(u => u.Id > 0)
                            .OrderByDescending(u => u.Id)
                            .Skip((int.Parse(pageIndex) - 1) * int.Parse(pageSize))
                            .Take(int.Parse(pageSize)).ToList();
            return json.WriteJson(1, "成功", new { total = hList.Count(), htmlList = pageData });
        }

        /// <summary>
        /// 获取栏目页网页
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<htmlInfo> GetHtmlList(string sqlstr)
        {
            List<htmlInfo> hList = new List<htmlInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from htmlInfo as h,userInfo as u " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                htmlInfo hInfo = new htmlInfo();
                hInfo.Id = (long)row["Id"];
                hInfo.userId = row["userId"].ToString();
                hInfo.title = (string)row["title"];
                hInfo.titleImg = (string)row["titleImg"];
                hInfo.titleURL = (string)row["titleURL"];
                hInfo.columnId = (string)row["columnId"];//栏目Id
                string content = (string)row["articlecontent"];
                if (content.Length > 60)
                    content = "<p>" + content.Substring(0, 60) + "...</p>";
                else
                    content = "<p>" + content.Substring(0, content.Length) + "...</p>";
                hInfo.articlecontent = content;//产品简介
                hInfo.addTime = row["addTime"].ToString();
                hInfo.city = (string)row["city"];//生产城市
                hInfo.smallCount = (string)row["smallCount"];//起订
                hInfo.realmNameId = (string)row["realmNameId"];//目录名

                hInfo.companyName = (string)row["companyName"];//公司名字
                hInfo.ten_qq = (string)row["ten_qq"];
                hInfo.companyName = (string)row["companyName"];
                hList.Add(hInfo);
            }
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
﻿using HRMSys.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fbkc
{
    public class BLL
    {
        /// <summary>
        /// 查找单个用户
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public cmUserInfo GetUser(string sqlstr)
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select * from userInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            DataRow row = dt.Rows[0];
            cmUserInfo userInfo = new cmUserInfo();
            userInfo.Id = (int)row["Id"];
            userInfo.username = (string)row["username"];
            userInfo.password = (string)row["password"];
            userInfo.userType = (int)row["userType"];
            userInfo.isStop = (bool)row["isStop"];
            userInfo.gradeId = (int)row["gradeId"];
            userInfo.canPubCount = (int)row["canPubCount"];
            userInfo.realmNameInfo = (string)row["realmNameInfo"];
            userInfo.expirationTime = ((DateTime)row["expirationTime"]).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.endPubCount = (int)row["endPubCount"];
            userInfo.endTodayPubCount = (int)row["endTodayPubCount"];
            userInfo.registerTime = ((DateTime)row["registerTime"]).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.registerIP = (string)row["registerIP"];
            userInfo.companyName = (string)row["companyName"];
            userInfo.columnInfoId = (int)row["columnInfoId"];
            userInfo.person = (string)row["person"];
            userInfo.telephone = (string)row["telephone"];
            userInfo.modile = (string)row["modile"];
            userInfo.ten_qq = (string)row["ten_qq"];
            userInfo.keyword = (string)row["keyword"];
            userInfo.pinpai = (string)row["pinpai"];
            userInfo.xinghao = (string)row["xinghao"];
            userInfo.price = (string)row["price"];
            userInfo.smallCount = (string)row["smallCount"];
            userInfo.sumCount = (string)row["sumCount"];
            userInfo.unit = (string)row["unit"];
            userInfo.city = (string)row["city"];
            userInfo.address = (string)row["address"];
            userInfo.com_web = (string)row["com_web"];
            userInfo.companyRemark = (string)row["companyRemark"];
            userInfo.yewu = (string)row["yewu"];
            userInfo.ziduan1 = (string)row["ziduan1"];
            return userInfo;
        }
        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public htmlPara GetHtmlPara(string columnId, string Id)
        {
            htmlPara hPara = new htmlPara();
            DataTable dt = SqlHelperCatalog.ExecuteDataTable(@"select * from htmlPara h left join columnInfo c on h.columnId=c.Id  where columnId =@columnId and h.Id=@Id",
               new SqlParameter("@columnId", columnId),
               new SqlParameter("@Id", Id));
            if (dt.Rows.Count != 1)
                return null;
            DataRow row = dt.Rows[0];
            hPara.Id = (long)SqlHelper.FromDBNull(row["Id"]);
            hPara.userId = SqlHelper.FromDBNull(row["userId"]).ToString();
            hPara.title = (string)SqlHelper.FromDBNull(row["title"]);
            hPara.titleImg = (string)SqlHelper.FromDBNull(row["titleImg"]);
            hPara.titleURL = (string)SqlHelper.FromDBNull(row["titleURL"]);
            hPara.columnId = (string)SqlHelper.FromDBNull(row["columnId"]);//栏目Id
            string content = (string)SqlHelper.FromDBNull(row["articlecontent"]);
            //hPara.articlecontent = ReplaceHtmlTag(content, 60);//产品简介
            hPara.articlecontent = content;
            hPara.city = (string)SqlHelper.FromDBNull(row["city"]);//生产城市
            hPara.smallCount = (string)SqlHelper.FromDBNull(row["smallCount"]);//起订
            hPara.xinghao = (string)SqlHelper.FromDBNull(row["xinghao"]);//型号
            hPara.unit = (string)SqlHelper.FromDBNull(row["unit"]);//单位
            hPara.sumCount = (string)SqlHelper.FromDBNull(row["sumCount"]);//供货总量
            hPara.price = (string)SqlHelper.FromDBNull(row["price"]);//单价
            hPara.pinpai = (string)SqlHelper.FromDBNull(row["pinpai"]);//品牌
            hPara.companyName = (string)SqlHelper.FromDBNull(row["companyName"]);//公司名字
            hPara.ten_qq = (string)SqlHelper.FromDBNull(row["ten_qq"]);
            hPara.com_web = (string)SqlHelper.FromDBNull(row["com_web"]);//网址
            hPara.addTime = ((DateTime)SqlHelper.FromDBNull(row["addTime"])).ToString("yyyy-MM-dd");
            hPara.columnName = (string)SqlHelper.FromDBNull(row["columnName"]);//栏目名
            hPara.username = (string)SqlHelper.FromDBNull(row["username"]);//用户名
            return hPara;
        }
        /// <summary>
        /// 查询上一条下一条
        /// </summary>
        /// <param name="columnId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<htmlPara> GetHtmlBAPage(string columnId, string Id)
        {
            List<htmlPara> hList = new List<htmlPara>();
            DataTable dt = SqlHelperCatalog.ExecuteDataTable(@"select top 1 -1 as [Sort],Id,title,columnId,titleURL from (select top 1 Id,title,columnId,titleURL from htmlPara where columnId=@columnId and Id<@Id order by Id desc) as tab_up 
                 union select top 1 1 as [Sort],Id,title,columnId,titleURL from (select top 1 Id,title,columnId,titleURL from htmlPara where columnId=@columnId and Id>@Id order by Id) as tab_up",
               new SqlParameter("@columnId", columnId),
               new SqlParameter("@Id",Id));
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                htmlPara hPara = new htmlPara();
                hPara.Id = (long)row["Id"];
                hPara.title = (string)row["title"];
                hPara.titleURL = row["titleURL"].ToString();
                hPara.columnId = (string)row["columnId"];
                hList.Add(hPara);
            }
            return hList;
        }
        /// <summary>
        /// 右侧浮动栏，前十条公司产品
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<htmlPara> GetProFloat(string userId)
        {
            List<htmlPara> hList = new List<htmlPara>();
            DataTable dt = SqlHelperCatalog.ExecuteDataTable(@"select top 10 * from htmlPara where userId=1 and columnId!=20 order by addTime desc",
               new SqlParameter("@userId", userId));
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                htmlPara hPara = new htmlPara();
                hPara.Id = (long)row["Id"];
                hPara.title = (string)row["title"];
                hPara.titleURL = row["titleURL"].ToString();
                hList.Add(hPara);
            }
            return hList;
        }
        /// <summary>
        /// 右侧浮动栏，前十条新闻
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<htmlPara> GetNewsFloat(string userId)
        {
            List<htmlPara> hList = new List<htmlPara>();
            DataTable dt = SqlHelperCatalog.ExecuteDataTable(@"select top 10 * from htmlPara where userId=1 and columnId=20 order by addTime desc",
               new SqlParameter("@userId", userId));
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                htmlPara hPara = new htmlPara();
                hPara.Id = (long)row["Id"];
                hPara.title = (string)row["title"];
                hPara.titleURL = row["titleURL"].ToString();
                hList.Add(hPara);
            }
            return hList;
        }
        /// <summary>
        /// 获取栏目页网页
        /// </summary>
        /// <param name="columnId">行业类别</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public List<htmlPara> GetHtmlList(string columnId, int pageIndex, int pageSize)
        {
            //分页查询
            List<htmlPara> hList = new List<htmlPara>();
            DataTable dt = SqlHelperCatalog.ExecuteDataTable(@"select * from 
                (select *, ROW_NUMBER() OVER(order by addTime desc) AS RowId from htmlPara where columnId=@columnId) as b where b.RowId between @startNum and @endNum",
               new SqlParameter("@columnId", columnId),
               new SqlParameter("@startNum", (pageIndex - 1) * pageSize + 1),
               new SqlParameter("@endNum", pageIndex * pageSize));
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                htmlPara hPara = new htmlPara();
                hPara.Id = (long)row["Id"];
                hPara.userId = row["userId"].ToString();
                hPara.title = (string)row["title"];
                hPara.titleImg = (string)row["titleImg"];
                hPara.titleURL = (string)row["titleURL"];
                hPara.columnId = (string)row["columnId"];//栏目Id
                string content = (string)row["articlecontent"];
                hPara.articlecontent = ReplaceHtmlTag(content, 60);//产品简介
                hPara.city = (string)row["city"];//生产城市
                hPara.smallCount = (string)row["smallCount"];//起订
                hPara.companyName = (string)SqlHelper.FromDBNull(row["companyName"]);//公司名字
                hPara.ten_qq = (string)SqlHelper.FromDBNull(row["ten_qq"]);
                hPara.com_web = (string)SqlHelper.FromDBNull(row["com_web"]);//网址
                hPara.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd");
                hList.Add(hPara);
            }
            return hList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<htmlPara> GetHtmlList(string count, string columnId)
        {
            //分页查询
            List<htmlPara> hList = new List<htmlPara>();
            DataTable dt = SqlHelperCatalog.ExecuteDataTable(@"select top (CONVERT(int,@count)) * from htmlPara where columnId !='@columnId' order by addTime desc",
               new SqlParameter("@columnId", columnId),
               new SqlParameter("@count", count));
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                htmlPara hPara = new htmlPara();
                hPara.Id = (long)row["Id"];
                hPara.userId = row["userId"].ToString();
                hPara.title = (string)row["title"];
                hPara.titleImg = (string)row["titleImg"];
                hPara.titleURL = (string)row["titleURL"];
                hPara.columnId = (string)row["columnId"];//栏目Id
                string content = (string)row["articlecontent"];
                hPara.articlecontent = ReplaceHtmlTag(content, 60);//产品简介
                hPara.city = (string)row["city"];//生产城市
                hPara.smallCount = (string)row["smallCount"];//起订
                hPara.companyName = (string)SqlHelper.FromDBNull(row["companyName"]);//公司名字
                hPara.ten_qq = (string)SqlHelper.FromDBNull(row["ten_qq"]);
                hPara.com_web = (string)SqlHelper.FromDBNull(row["com_web"]);//网址
                hPara.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd");
                hList.Add(hPara);
            }
            return hList;
        }
        public static string ReplaceHtmlTag(string html, int length)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        /// <summary>
        /// 获取目录主页内容
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<htmlInfo> GetMainHtmlList()
        {
            List<htmlInfo> hList = new List<htmlInfo>();
            //            DataTable dt = SqlHelper.ExecuteDataSet(@"select title,titleImg,titleURL,addTime,columnId,columnName from 
            //( select 
            //RANK()OVER(PARTITION BY columnInfo.Id ORDER BY htmlInfo.addTime DESC) AS
            //RANK2, title,titleImg,titleURL,addTime,htmlInfo.columnId,columnName from 
            //htmlInfo left join columnInfo On htmlInfo.columnId = columnInfo.Id) T
            //where RANK2<=10 ").Tables[0];
            DataTable dt = SqlHelperCatalog.ExecuteDataSet(@"select  title,titleURL,titleImg,addTime,columnId from 
( select 
RANK()OVER(PARTITION BY columnId ORDER BY addTime DESC) AS
RANK2, title,titleImg,titleURL,addTime,columnId from 
htmlPara ) T
where RANK2<=10").Tables[0];
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
                //hInfo.realmNameId = (string)row["realmNameId"];//目录名
                hList.Add(hInfo);
            }
            return hList;
        }

        /// <summary>
        /// 将html内容参数存入数据库
        /// </summary>
        /// <param name="info"></param>
        public void AddHtml(htmlPara info)
        {
            int a = SqlHelperCatalog.ExecuteNonQuery(@"INSERT INTO htmlPara
         ([title]
           ,[titleURL]
           ,[articlecontent]
           ,[columnId]
           ,[pinpai]
           ,[xinghao]
           ,[price]
           ,[smallCount]
           ,[sumCount]
           ,[unit]
           ,[city]
           ,[titleImg]
           ,[addTime]
           ,[userId]
           ,[userName])
     VALUES
           (@title
           ,@titleURL
           ,@articlecontent
           ,@columnId
           ,@pinpai
           ,@xinghao
           ,@price
           ,@smallCount
           ,@sumCount
           ,@unit
           ,@city
           ,@titleImg
           ,@addTime
           ,@userId
           ,@userName)",
               new SqlParameter("@title", SqlHelper.ToDBNull(info.title)),
               new SqlParameter("@titleURL", SqlHelper.ToDBNull(info.titleURL)),
               new SqlParameter("@articlecontent", SqlHelper.ToDBNull(info.articlecontent)),
               new SqlParameter("@columnId", SqlHelper.ToDBNull(info.columnId)),
               new SqlParameter("@pinpai", SqlHelper.ToDBNull(info.pinpai)),
               new SqlParameter("@xinghao", SqlHelper.ToDBNull(info.xinghao)),
               new SqlParameter("@price", SqlHelper.ToDBNull(info.price)),
               new SqlParameter("@smallCount", SqlHelper.ToDBNull(info.smallCount)),
               new SqlParameter("@sumCount", SqlHelper.ToDBNull(info.sumCount)),
               new SqlParameter("@unit", SqlHelper.ToDBNull(info.unit)),
               new SqlParameter("@city", SqlHelper.ToDBNull(info.city)),
               new SqlParameter("@titleImg", SqlHelper.ToDBNull(info.titleImg)),
               new SqlParameter("@addTime", SqlHelper.ToDBNull(info.addTime)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(info.userId)),
               new SqlParameter("@userName", SqlHelper.ToDBNull(info.username)));
        }

        /// <summary>
        /// 获取当前表最大Id
        /// </summary>
        /// <returns></returns>
        public int GetMaxId()
        {
            object ob = "";
            try
            {
                ob = SqlHelperCatalog.ExecuteScalar("select Id  from htmlPara order by Id desc");
                if (ob == null)
                    ob = 0;
            }
            catch (Exception ex)
            { return 1; }
            return int.Parse(ob.ToString());
        }

        /// <summary>
        /// 通过用户名找Id
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public string GetUserId(string username)
        {
            object ob = SqlHelper.ExecuteScalar("select Id from userInfo where username='" + username + "'");
            return ob.ToString();
        }
        public int GetPageTotal(string columnId)
        {
            return (int)SqlHelperCatalog.ExecuteScalar("select count(*)  from htmlPara where columnId=@columnId",
                new SqlParameter("@columnId", columnId));
        }
        /// <summary>
        /// 获取栏目信息
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<columnInfo> GetColumns(string sqlstr)
        {
            List<columnInfo> cList = new List<columnInfo>();
            DataTable dt = SqlHelperCatalog.ExecuteDataTable("select * from columnInfo " + sqlstr);
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                columnInfo cInfo = new columnInfo();
                cInfo.Id = (int)row["Id"];
                cInfo.columnName = (string)row["columnName"];
                cList.Add(cInfo);
            }
            return cList;
        }
    }
}
using HRMSys.DAL;
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
        /// 获取栏目页网页
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<htmlPara> GetHtmlList(string sqlstr,string count)
        {
            List<htmlPara> hList = new List<htmlPara>();
            DataTable dt = SqlHelperCatalog.ExecuteDataSet("select top "+count+" * from htmlPara " + sqlstr).Tables[0];
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
                if (content.Length > 60)
                    content = "<p>" + content.Substring(0, 60) + "...</p>";
                else
                    content = "<p>" + content.Substring(0, content.Length) + "...</p>";
                hPara.articlecontent = content;//产品简介
                hPara.city = (string)row["city"];//生产城市
                hPara.smallCount = (string)row["smallCount"];//起订
                hPara.companyName = (string)SqlHelper.FromDBNull(row["companyName"]);//公司名字
                hPara.ten_qq = (string)SqlHelper.FromDBNull(row["ten_qq"]);
                hPara.com_web = (string)SqlHelper.FromDBNull(row["com_web"]);//网址
                hPara.addTime = row["addTime"].ToString();
                hList.Add(hPara);
            }
            return hList;
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
           ,[userId])
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
           ,@userId)",
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
               new SqlParameter("@userId", SqlHelper.ToDBNull(info.userId)));
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
    }
}
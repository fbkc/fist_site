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
        /// 将html内容参数存入数据库
        /// </summary>
        /// <param name="info"></param>
        public void AddHtml(htmlInfo info)
        {
            int a = SqlHelper1.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[htmlInfo]
           ([title]
           ,[titleURL]
           ,[articlecontent]
           ,[columnId]
           ,[titleImg]
           ,[addTime]
           ,[userId])
     VALUES
           (@title
           ,@titleURL
           ,@articlecontent
           ,@columnId
           ,@titleImg
           ,@addTime
           ,@userId)",
               new SqlParameter("@title", SqlHelper.ToDBNull(info.title)),
               new SqlParameter("@titleURL", SqlHelper.ToDBNull(info.titleURL)),
               new SqlParameter("@articlecontent", SqlHelper.ToDBNull(info.articlecontent)),
               new SqlParameter("@columnId", SqlHelper.ToDBNull(info.columnId)),
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
                ob = SqlHelper1.ExecuteScalar("select Id  from htmlInfo order by Id desc");
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
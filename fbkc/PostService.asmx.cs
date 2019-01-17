using AutoSend;
using HRMSys.DAL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;

namespace fbkc
{
    /// <summary>
    /// PostService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PostService : System.Web.Services.WebService, IRequiresSessionState
    {
        private static string host = "http://39.105.196.3:8173/hyzx";
        private static string uname = "";
        /// <summary>
        /// post接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [WebMethod(Description = "post接口", EnableSession = true)]
        public string Post(string strJson)
        {
            BLL bll = new BLL();
            //需要做一个时间，每隔多长时间才允许访问一次
            string keyValue = NetHelper.GetMD5("liu" + "100dh888");
            string username = "";
            string url = "";
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
                //return json.WriteJson(1, "dsasdasda", new { });
                string key = jo["key"].ToString();
                if (key != keyValue)
                    return json.WriteJson(0, "参数错误", new { });
                htmlPara hPara = new htmlPara();
                username = jo["username"].ToString();
                hPara.userId = bll.GetUserId(username);//用户名
                //公司/会员信息
                cmUserInfo uInfo = bll.GetUser(string.Format("where username='{0}'", username));
                hPara.title = jo["title"].ToString();
                string cid = jo["catid"].ToString();
                if (string.IsNullOrEmpty(cid))
                    return json.WriteJson(0, "行业或栏目不能为空", new { });

                //命名规则：ip/目录/用户名/show_行业id+(五位数id)
                string showName = "show_" + cid + (bll.GetMaxId() + 1).ToString() + ".html";
                url = host + "/" + username + "/" + showName;
                hPara.titleURL = url;
                hPara.articlecontent = HttpUtility.UrlDecode(jo["content"].ToString(), Encoding.UTF8);//内容,UrlDecode解码
                hPara.columnId = cid;//行业id，行业新闻id=23
                hPara.pinpai = jo["pinpai"].ToString();
                hPara.xinghao = jo["xinghao"].ToString();
                hPara.price = jo["price"].ToString();
                hPara.smallCount = jo["qiding"].ToString();
                hPara.sumCount = jo["gonghuo"].ToString();
                hPara.unit = jo["unit"].ToString();
                hPara.city = jo["city"].ToString();
                hPara.titleImg = jo["thumb"].ToString();
                hPara.companyName = uInfo.companyName;
                hPara.com_web = uInfo.com_web;
                hPara.ten_qq = uInfo.ten_qq;
                hPara.addTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bll.AddHtml(hPara);//存入数据库
                WriteFile(hPara, uInfo, username, showName);//写模板
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "发布成功", new { url, username });
        }

        #region 定义模版页
        public static string SiteTemplate()
        {
            string str = "";
            str += "....";//模版页html代码
            return str;
        }
        #endregion

        /// <summary>
        /// 写模板
        /// </summary>
        /// <param name="hInfo"></param>
        /// <param name="uInfo"></param>
        /// <param name="username"></param>
        /// <param name="hName"></param>
        /// <returns></returns>
        public static bool WriteFile(htmlPara hInfo, cmUserInfo uInfo, string username, string hName)
        {
            //文件输出目录
            string path = HttpContext.Current.Server.MapPath("~/" + username + "/");

            // 读取模板文件
            string temp = HttpContext.Current.Server.MapPath("~/DetailPage.html");//模版文件

            //string str = SiteTemplate();//读取模版页面html代码
            string str = "";
            using (StreamReader sr = new StreamReader(temp, Encoding.GetEncoding("gb2312")))
            {
                str = sr.ReadToEnd();
                sr.Close();
            }
            string htmlfilename = hName;//静态文件名
            // 替换内容
            str = str.Replace("companyName_Str", uInfo.companyName);
            if (hInfo.title.Length > 6)
                str = str.Replace("keywords_Str", hInfo.title + "," + hInfo.title.Substring(0, 2) + "," + hInfo.title.Substring(2, 2) + "," + hInfo.title.Substring(4, 2));
            else
                str = str.Replace("keywords_Str", hInfo.title);
            str = str.Replace("description_Str", hInfo.articlecontent.Substring(0, 80));
            str = str.Replace("host_Str", host);
            str = str.Replace("catid_Str", hInfo.columnId);
            str = str.Replace("Id_Str", hInfo.Id.ToString());
            str = str.Replace("title_Str", hInfo.title);
            str = str.Replace("addTime_Str", hInfo.addTime);

            str = str.Replace("pinpai_Str", hInfo.pinpai);
            str = str.Replace("price_Str", hInfo.price);
            str = str.Replace("qiding_Str", hInfo.smallCount);
            str = str.Replace("gonghuo_Str", hInfo.sumCount);
            str = str.Replace("xinghao_Str", hInfo.xinghao);
            str = str.Replace("city_Str", hInfo.city);
            str = str.Replace("unit_Str", hInfo.unit);

            str = str.Replace("titleImg_Str", hInfo.titleImg);
            str = str.Replace("content_Str", hInfo.articlecontent);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // 写文件
            using (StreamWriter sw = new StreamWriter(path + htmlfilename, true))
            {
                sw.Write(str);
                sw.Flush();
                sw.Close();
            }
            return true;
        }
    }
}

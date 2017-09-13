using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin
{
    public partial class homepage : System.Web.UI.Page
    {
        public string webApiDomain = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.webApiDomain = ConfigHelper.GetAppSettings("webApiDomain");
            if (!Page.IsPostBack)
            {
                // login_name.InnerText = Utils.GetCookie("DTRememberName");
            }
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userName = Request.Form["login_name"].ToString();
            string userPwd = Request.Form["password"].ToString();

            if (userName.Equals("") || userPwd.Equals(""))
            {
                //msgtip.InnerHtml = "请输入用户名或密码";
                JscriptAlert("错误超过5次，关闭浏览器重新登录！");
                return;
            }
            if (Session["AdminLoginSun"] == null)
            {
                Session["AdminLoginSun"] = 1;
            }
            else
            {
                Session["AdminLoginSun"] = Convert.ToInt32(Session["AdminLoginSun"]) + 1;
            }
            //判断登录错误次数
            if (Session["AdminLoginSun"] != null && Convert.ToInt32(Session["AdminLoginSun"]) > 5)
            {
                //msgtip.InnerHtml = "错误超过5次，关闭浏览器重新登录！";
                JscriptAlert("错误超过5次，关闭浏览器重新登录！");
                return;
            }
            BLL.manager bll = new BLL.manager();
            Model.manager model = bll.GetModel(userName, userPwd, true);
            if (model == null)
            {
                //msgtip.InnerHtml = "用户名或密码有误，请重试！";
                JscriptAlert("用户名或密码有误，请重试！");
                return;
            }
            Session[DTKeys.SESSION_ADMIN_INFO] = model;
            Session.Timeout = 45;
            //写入登录日志
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig();
            if (siteConfig.logstatus > 0)
            {
                new BLL.manager_log().Add(model.id, model.user_name, DTEnums.ActionEnum.Login.ToString(), "用户登录");
            }
            //写入Cookies
            Utils.WriteCookie("DTRememberName", model.user_name, 14400);
            Utils.WriteCookie("AdminName", "DTcms", model.user_name);
            Utils.WriteCookie("AdminPwd", "DTcms", model.password);
            Response.Redirect("index.aspx");
            //Response.Redirect("../indexfile/html/function.html");
            return;
        }

        private void JscriptAlert(string msgtitle)
        {
            ClientScript.RegisterStartupScript(this.GetType(), " message", "<script language='javascript' >alert('" + msgtitle + "');</script>");
        }
        /// <summary>
        /// 获取echarts数据
        /// </summary>
        /// <param name="context"></param>
       



        public class echartsmodel
        {


        }
    }
}
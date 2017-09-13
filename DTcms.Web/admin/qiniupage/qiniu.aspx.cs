using DTcms.BLL;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace DTcms.Web.admin.qiniupage
{
    public partial class qiniu : Web.UI.ManagePage
    {
        // 初始化页面服务器空间，控件名称要和页面控件 id 保持一致
        protected TextBox url;
        //protected TextBox saveName;
        //protected TextBox ak;
        //protected TextBox sk;


        protected void Page_Init(object sender, EventArgs e)
        {
            //Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            //AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");

            //string QiNiuRootUrl = appSection.Settings["QiNiuRootUrl"].Value;
            //string QiNiuAk = appSection.Settings["QiNiuAk"].Value;
            //string QiNiuSk = appSection.Settings["QiNiuSk"].Value;
            //string QiNiuScope = appSection.Settings["QiNiuScope"].Value;


            QiNiu bll = new QiNiu();
            P_QiNiuInfo info = bll.GetQiNiuConfigInfo();

            if (info != null) {
                // 给页面服务器控件赋值
                url.Text = info.P_RootUrl;
                ak.Text = info.P_AK;
                sk.Text = info.P_SK;
                saveName.Text = info.P_Scope;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string QiNiuRootUrl = url.Text;
            string QiNiuScope = saveName.Text;
            string QiNiuAk = ak.Text;
            string QiNiuSk = sk.Text;
            int userId = GetAdminInfo().id;
            try
            {
                #region MyRegion
                //if (QiNiuRootUrl != "" && QiNiuScope != "" && QiNiuAk != "" && QiNiuSk != "")
                //{
                //    Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                //    AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
                //    appSection.Settings["QiNiuRootUrl"].Value = QiNiuRootUrl;
                //    appSection.Settings["QiNiuAk"].Value = QiNiuAk;
                //    appSection.Settings["QiNiuSk"].Value = QiNiuSk;
                //    appSection.Settings["QiNiuScope"].Value = QiNiuScope;
                //    config.Save();
                //    JscriptMsg("保存成功！", "");
                //}
                //else
                //{
                //    JscriptMsg("配置项不可为空！", "");
                //} 
                #endregion
                QiNiu bll = new QiNiu();
                Boolean result = bll.UpdateQiNiuConfigInfo(QiNiuRootUrl,QiNiuScope,QiNiuAk,QiNiuSk,userId);
                if (result) {
                    JscriptMsg("保存成功！", "");
                }
                else {
                    JscriptMsg("保存失败！", "");
                }

            }
            catch (Exception ex)
            {
                JscriptMsg("保存失败！", "");
                string error = ex.Message;

            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using DTcms.Common;
using DTcms.Model.WebApiModel;

namespace DTcms.DAL
{
    public class WeChat 
    {

        // ****初始化页面服务器控件，控件名称要和页面控件 id 保持一致*****
        protected TextBox A;
        protected TextBox B;
        protected TextBox C;
        protected TextBox D;
        protected TextBox E;
        protected TextBox F;
        private object model;



        //给页面服务器控件赋值
        //       A.Text = Model.A;
        //       B.Text = Model.B;
        //        C.Text = Model.C;
        //        D.Text = Model.C;


        /// <summary>
        /// 初始化微信配置
        /// </summary>
        /// <returns></returns>
        public PayConfigModel GetWeChat()
        {

            try
            {

                PayConfigModel result = new PayConfigModel();
                result.ALiAppId = ConfigHelper.GetAppSettings("ALiAppId");
                result.ALiPayPublicKey = ConfigHelper.GetAppSettings("ALiPayPublicKey");
                result.ALiPayPrivateKey = ConfigHelper.GetAppSettings("ALiPayPrivateKey");
                result.WeiXinAppId = ConfigHelper.GetAppSettings("WeiXinAppId");
                result.WeiXinPublicKey = ConfigHelper.GetAppSettings("WeiXinPublicKey");
                result.WeiXinPrivateKey = ConfigHelper.GetAppSettings("WeiXinPrivateKey");
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


      


        //public void btnSave_Click()
        //{
        //    WeChatModel model = new WeChatModel();

        //    ConfigHelper.SetValue(xmlDocument) = A.Text;


        //}




        ///修改保存
        //public void btnSave_Click(/*object sender, EventArgs e*/)
        //{
        //    if (A != "" && B != "" && C != "" && D != "" && E != "" && F != "")
        //    {
        //        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        //        AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
        //        appSection.Settings["A"].Value = A;
        //        appSection.Settings["B"].Value = B;
        //        appSection.Settings["C"].Value = C;
        //        appSection.Settings["D"].Value = D;
        //        config.Save();
        //        JscriptMsg("保存成功！", "");
        //    }
        //    else
        //    {
        //        JscriptMsg("配置项不可为空！", "");
        //    }

        //}




        public class WeChatModel
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string E { get; set; }
            public string F { get; set; }
        }

    }
}

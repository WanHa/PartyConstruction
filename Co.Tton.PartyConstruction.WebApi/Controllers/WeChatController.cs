using DTcms.BLL;
using DTcms.Common;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v2/wechat")]
    public class WeChatController : ApiControllerBase
    {
        WebApiApplication api = new WebApiApplication();

        /// <summary>
        /// 获取微信配置信息
        /// </summary>
        [Route("init/page")]
        [AcceptVerbs("GET")]
        public HttpResponseMessage GetWeChat()
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                //PayConfigModel result = new PayConfigModel();
                //result.ALiAppId = ConfigHelper.GetAppSettings("ALiAppId");
                //result.ALiPayPublicKey = ConfigHelper.GetAppSettings("ALiPayPublicKey");
                //result.ALiPayPrivateKey = ConfigHelper.GetAppSettings("ALiPayPrivateKey");
                //result.WeiXinAppId = ConfigHelper.GetAppSettings("WeiXinAppId");
                //result.WeiXinPublicKey = ConfigHelper.GetAppSettings("WeiXinPublicKey");
                //result.WeiXinPrivateKey = ConfigHelper.GetAppSettings("WeiXinPrivateKey");
                PayConfigModel result = new WeChatCon().GetWeChat();
                message = RenderMessage(true, "", result, 1);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }


        /// <summary>
        /// 修改微信配置信息
        /// </summary>
        [Route("Save/page")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage btnSave_Click([FromBody]PayConfigModel data)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            bool result = true;
            try
            {
              
                XmlDocument a = new XmlDocument();
                string root = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                a.Load(root + "//App_Data//Config.xml");
                ConfigHelper.Set(a, "appSettings", "ALiAppId", data.ALiAppId);
                ConfigHelper.Set(a, "appSettings", "ALiPayPublicKey", data.ALiPayPublicKey);
                ConfigHelper.Set(a, "appSettings", "ALiPayPrivateKey", data.ALiPayPrivateKey);
                ConfigHelper.Set(a, "appSettings", "WeiXinAppId", data.WeiXinAppId);
                ConfigHelper.Set(a, "appSettings", "WeiXinPublicKey", data.WeiXinPublicKey);
                ConfigHelper.Set(a, "appSettings", "WeiXinPrivateKey", data.WeiXinPrivateKey);
                a.Save(root + "//App_Data//Config.xml");
            }
            catch (Exception)
            {
                result = false;

                throw;
            }
            if (result)
            {
                message = RenderMessage(true, "保存成功", result, 1);
            }
            else
            {
                message = RenderMessage(false, "保存失败");
            }
            return message;

        }
        //public HttpResponseMessage SaveWeChatpage()
        //{
        //    HttpResponseMessage message = new HttpResponseMessage();
        //    var result = new WeChatCon().GetWeChat();

        //    if (result != null)
        //    {
        //        message = RenderListTrueMessage(result, 1);
        //    }
        //    else
        //    {
        //        message = RenderListFalseMessage();
        //    }
        //    return message;
        //}

    }
  
}

using DTcms.BLL;
using DTcms.Common;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v2/rongcloud")]
    public class RongCloudController : ApiControllerBase
    {
        WebApiApplication api = new WebApiApplication();

        /// <summary>
        /// 获取融云配置信息
        /// </summary>
        /// <returns></returns>
        [Route("init/page")]
        [AcceptVerbs("GET")]
        public HttpResponseMessage GetRongCloud()
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                RongCloudModel result = new RongCloundConfig().GetRongCloudConfig();
                message = RenderMessage(true, "", result, 1);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 修改融云配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("save/page")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage btnSave_Click([FromBody]RongCloudModel data)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            bool result = true;
            try
            {
                XmlDocument xml = new XmlDocument();
                string root = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                xml.Load(root + "//App_Data//Config.xml");
                ConfigHelper.Set(xml, "appSettings", "RongAppKey", data.AppKey);
                ConfigHelper.Set(xml, "appSettings", "RongAppSecret", data.AppSecret);
                xml.Save(root + "//App_Data//Config.xml");
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

     }
}

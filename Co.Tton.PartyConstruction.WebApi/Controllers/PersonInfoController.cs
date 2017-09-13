using DTcms.BLL;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/Cover")]
    public class PersonInfoController : ApiControllerBase
    {
        private Cover cover = new Cover();
        /// <summary>
        /// 封面接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("person/update"), AcceptVerbs("POST")]
        public HttpResponseMessage Update([FromBody]CoverModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                string data = cover.Update(model);
                if (data.Length>0)
                {
                    message = RenderMessage(true, "上传数据成功。", data, 1);
                }
                else
                {
                    message = RenderMessage(false, "上传失败。");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
    }
}

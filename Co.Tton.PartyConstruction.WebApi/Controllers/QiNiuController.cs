using DTcms.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/qiniu")]
    public class QiNiuController : ApiControllerBase
    {
        [Route("token"),AcceptVerbs("GET")]
        public HttpResponseMessage GetQiNiuToken() {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                string token = new QiNiu().GetQiNiuToken();
                if (!String.IsNullOrEmpty(token))
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("token",token);
                    message = RenderMessage(true, "获取数据成功", data, 1);
                }
                else {
                    message = RenderMessage(false, "未能获取到数据");
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

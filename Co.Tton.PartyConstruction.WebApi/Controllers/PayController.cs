using DTcms.BLL;
using DTcms.Common;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/pay")]
    public class PayController : ApiControllerBase
    {
        [Route("ali"), AcceptVerbs("POST")]
        public HttpResponseMessage ALiPayCallBack([FromBody]ALiPayCallBackModel fromBody) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = new PayBll().ALiPayCalBackFun(fromBody);
                message.StatusCode = HttpStatusCode.OK;
                message.Content = new StringContent("success", Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                message.StatusCode = HttpStatusCode.InternalServerError;
                message.Content = new StringContent("fail", Encoding.UTF8, "application/json");
            }

            return message;
        }

        /// <summary>
        /// 获取支付订单
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        [Route("order"),AcceptVerbs("POST")]
        public HttpResponseMessage GetPayOrder([FromBody]PayFromBody fromBody) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                string result = new PayBll().GetPayOrder(fromBody);
                if (!String.IsNullOrEmpty(result))
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("order",result);
                    message = RenderMessage(true, "获取数据成功", data,1);
                }
                else {
                    message = RenderMessage(false, "获取数据失败");
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

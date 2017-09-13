using DTcms.BLL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/payment")]
    public class PartyPaymentController : ApiControllerBase
    {
        private Pay payment = new Pay();

        /// <summary>
        /// 党费缴纳详情
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="rows">分页行数</param>
        /// <param name="page">分页页数</param>
        /// <returns></returns>
        [Route("payment/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartypaymentList([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Detail result = payment.GetPartypaymentList(userid);

                if (result != null)
                {
                    //message = RenderListTrueMessage(result,1);
                    message = RenderMessage(true,"获取数据成功", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "未能获取到数据");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }
        /// <summary>
        /// 缴纳党费提交接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("payment/submit"), AcceptVerbs("POST")]
        public HttpResponseMessage Submit([FromBody]PartyPaymentModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = payment.Submit(model);
                if (data)
                {
                    message = RenderMessage(true, "获取列表数据成功.");
                }
                else
                {
                    message = RenderEntityFalseMessage();
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 党费缴纳记录
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="rows">分页行数</param>
        /// <param name="page">分页页数</param>
        /// <returns></returns>
        [Route("payment/record"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartypaymentRecord([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<PartyPaymentRecordModel> result = payment.GetPartypaymentRecord(userid, rows, page);

                if (result != null && result.Count > 0)
                {
                    message = RenderListTrueMessage(result, result.Count);
                }
                else
                {
                    message = RenderListFalseMessage();
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
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
    [RoutePrefix("train/demand")]
    public class TrainController : ApiControllerBase
    {
        private Ptrain partytrain = new Ptrain();
        /// <summary>
        /// 诉求总数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [Route("userdemand/sum"), AcceptVerbs("GET")]
        public HttpResponseMessage GetDemandCount([FromUri]int year)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DemandCount data = partytrain.GetDetails(year);
                if (data != null)
                {
                    message = RenderListTrueMessage(data, 1);
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
        /// <summary>
        /// 处理中总数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [Route("userdemand/pending"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPendingDemand([FromUri]int year)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DemandCount data = partytrain.GetPending(year);
                if (data != null)
                {
                    message = RenderListTrueMessage(data, 1);
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
        [Route("userdemand/finish"), AcceptVerbs("GET")]
        public HttpResponseMessage GetFinishDemand([FromUri]int year)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DemandCount data = partytrain.GetFinishDemand(year);
                if (data != null)
                {
                    message = RenderListTrueMessage(data, 1);
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
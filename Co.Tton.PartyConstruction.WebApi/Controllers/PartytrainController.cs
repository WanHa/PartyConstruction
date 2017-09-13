using DTcms.BLL;
using DTcms.Model;
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
    [RoutePrefix("v1/partytrain")]
    public class PartytrainController : ApiControllerBase
    {
        private Train partytrain = new Train();

        /// <summary>
        /// 党员诉求列表接口
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="rows">分页行数</param>
        /// <param name="page">分页页数</param>
        /// <returns></returns>
        [Route("partytrain/user"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartytrainList([FromUri]int userid, [FromUri]int rows, [FromUri]int page, [FromUri] int asstatus)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<member> result = partytrain.GetPartytrainList(userid, rows, page, asstatus);

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
        /// <summary>
        /// 书记诉求列表接口
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="rows">分页行数</param>
        /// <param name="page">分页页数</param>
        /// <returns></returns>
        [Route("partytrain/lead"), AcceptVerbs("GET")]
        public HttpResponseMessage GetLeadList([FromUri]int userid,[FromUri]int rows, [FromUri]int page, [FromUri] int asstatus)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<Trainlist> result = partytrain.GetLeadList(userid,rows, page, asstatus);

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
        /// <summary>
        /// 党员有意见接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("partytrain/add"), AcceptVerbs("POST")]
        public HttpResponseMessage Add([FromBody]PartytrainModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = partytrain.Add(model);
                if (data)
                {
                    message = RenderMessage(true, "数据创建成功", "数据创建成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "数据创建失败", "数据创建失败", 1);
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 回复内容接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("partytrain/reply"), AcceptVerbs("POST")]
        public HttpResponseMessage Reply([FromBody]PartytrainModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = partytrain.Reply(model);
                if (data)
                {
                    message = RenderMessage(true, "回复成功", "回复成功",1);
                }
                else
                {
                    message = RenderMessage(false, "回复失败", "回复失败",1);
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 加急接口
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="id">用户加急内容的id</param>
        /// <returns></returns>
        [Route("partytrain/urgent"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartyUrgent([FromUri]int userid, [FromUri]string id)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                PartyUrgent result = partytrain.GetPartyUrgent(userid,id);
                if (result != null )
                {
                    message = RenderListTrueMessage(result,1);
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
        /// 满意状态接口
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="id">用户加急内容的id</param>
        /// <returns></returns>
        [Route("partytrain/satisfaction"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartySatisfaction([FromUri]int userid, [FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                PartySatisfaction result = partytrain.GetPartySatisfaction(userid, id);

                if (result != null)
                {
                    message = RenderListTrueMessage(result, 1);
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
        [Route("partytrain/count"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPendingCount([FromUri] int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                SumCount result = partytrain.GetPendingCount(userid);

                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功.", result,1);
                }
                else
                {
                    message = RenderMessage(false, "查询数据失败.");
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

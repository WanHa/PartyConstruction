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
    [RoutePrefix("rva/view")]
    public class ReviewActivityController : ApiControllerBase
    {
        private ReviewActivity bll = new ReviewActivity();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        /// <summary>
        /// 评选活动的列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("crea/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetReViewList([FromUri]int userid, [FromUri] int rows, [FromUri] int page,[FromUri] int asstatus)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<ReviewModel> result = bll.GetReView(userid,rows,page,asstatus);
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
        /// 创建评选活动接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add/tivi"), AcceptVerbs("POST")]
        public HttpResponseMessage getAddReView([FromBody]ActivityModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.getAddReView(model);
                if (data)
                {
                    message = RenderMessage(true, "插入数据成功.", "插入数据成功.", 1);
                }
                else
                {
                    message = RenderMessage(false, "插入数据失败.");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 正在进行投票的接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("deta/view"), AcceptVerbs("GET")]
        public HttpResponseMessage DetailReViewModel([FromUri]string id,[FromUri] int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                ReviewActivionModel data = bll.GetReviewModel(id,userid);
                if (data != null)
                {
                    message = RenderMessage(true, "查询数据成功.", data, 1);
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
        /// 结束的详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("end/view"), AcceptVerbs("GET")]
        public HttpResponseMessage EndReviewModel([FromUri]string id, [FromUri] int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                ModelToAction data = bll.EndReviewModel(id, userid);
                if (data != null)
                {
                    message = RenderMessage(true, "查询数据成功.", data, 1);
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
        /// <summary>
        /// 投票接口
        /// </summary>
        /// <param name="froum"></param>
        /// <returns></returns>
        [Route("ick/frum"), AcceptVerbs("POST")]
        public HttpResponseMessage GetFroumCount([FromBody]FroumCount froum)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.GetFroumCount(froum);
                if (data)
                {
                    message = RenderMessage(true, "插入数据成功.", data, 1);
                }
                else
                {
                    message = RenderMessage(false, "插入数据失败.");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 任何人可见 投完票
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("vity/deta"), AcceptVerbs("Get")]
        public HttpResponseMessage DetailReviewModel([FromUri]string id,[FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                ReviewActivionToModel data = bll.DetailReviewModel(id,userid);
                if (data!=null)
                {
                    message = RenderMessage(true, "查询数据成功.", data, 1);
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
        /// <summary>
        /// 投完票 投票后可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("grw/eie"), AcceptVerbs("Get")]
        public HttpResponseMessage GetReview([FromUri]string id, [FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                ModelReview data = bll.GetReview(id, userid);
                if (data != null)
                {
                    message = RenderMessage(true, "查询数据成功.", data, 1);
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
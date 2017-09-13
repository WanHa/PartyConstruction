using DTcms.BLL;
using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("psc/par")]
    public class PartyStyleController : ApiControllerBase
    {
        private PartysStyle ps = new PartysStyle();
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
        /// 党建风采列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("sty/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartyStyleList([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<partyStyleModel> result = ps.GetPartyStyleList(userid, rows, page);
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
        /// 党建风采详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("data/com"), AcceptVerbs("GET")]
        public HttpResponseMessage DeatailPartyStyle([FromUri] int id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DetailPartyStyleModel data = ps.DetailPartyStyle(id);
                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data, 1);
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
        /// 添加评论接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("par/men"), AcceptVerbs("POST")]
        public HttpResponseMessage InsertComment([FromBody]PartyCommentModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = ps.InsertComment(model);
                if (data)
                {
                    message = RenderMessage(true, "添加数据成功.", "添加数据成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "添加数据失败.");

                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 点赞接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("thum/up"), AcceptVerbs("GET")]
        public HttpResponseMessage GetThumbUp([FromUri]int userid, [FromUri] string id,[FromUri]int familytype)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = ps.GetThumbUp(userid,id,familytype);
                if (data)
                {
                    message = RenderMessage(true, "点赞成功.", "点赞成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "点赞失败.");

                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 删除点赞接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
         [Route("del/thum"), AcceptVerbs("GET")]
        public HttpResponseMessage DelThumbUp([FromUri]int userid, [FromUri] string id,[FromUri]int familytype)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = ps.DelThumbUp(userid, id, familytype);
                if (data)
                {
                    message = RenderMessage(true, "取消点赞成功.", "取消点赞成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "取消点赞失败.");

                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 评论列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("com/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetComment([FromUri]int userid, [FromUri] int id, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<commentModel> result = ps.GetComment(userid, id,rows,page);
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
        /// 获取前4位文章id及图片url,按时间倒序
        /// </summary>
        /// <returns></returns>
        [Route("articles-images/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetArticleImage([FromUri] string userId)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<PartyStyleImageModel> result = ps.GetArticleImage(userId);
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
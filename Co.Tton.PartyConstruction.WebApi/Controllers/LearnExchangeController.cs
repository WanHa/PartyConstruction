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
    [RoutePrefix("pnp/lrn")]
    public class LearnExchangeController : ApiControllerBase
    {
        private LearningExchange learningexchange = new LearningExchange();
        // GET: api/LearnExchange
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LearnExchange/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LearnExchange
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LearnExchange/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LearnExchange/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 获取学习交流列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("gle/xan"), AcceptVerbs("GET")]
        public HttpResponseMessage GetLearningExchange([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<LearnModel> result = learningexchange.GetList(userid, rows, page);
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
        /// 学习交流新增接口
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        //[Route("paper/xinzeng"), AcceptVerbs("GET")]
        //public HttpResponseMessage GetXinZeng([FromUri]int id, [FromUri]int user_id)
        //{
        //    HttpResponseMessage message = new HttpResponseMessage();
        //    try
        //    {
        //        DataSet data = learningexchange.GetXinZeng(id, user_id);
        //        if (data != null)
        //        {
        //            message = RenderMessage(true, "获取数据成功.", data.Tables[0], 1);
        //        }
        //        else
        //        {
        //            message = RenderMessage(false, "未能获取到数据");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        message = RenderErrorMessage(ex);
        //    }
        //    return message;
        //}

        /// <summary>
        /// 学习交流提交接口
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [Route("paper/gtj"), AcceptVerbs("POST")]
        public HttpResponseMessage GetTiJiao([FromBody]Aticle article)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = learningexchange.TiJiao(article);
                if (data)
                {
                    message = RenderMessage(true, "插入数据成功.",data,1);
                }
                else
                {
                    message = RenderMessage(false, "插入数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 学习交流详情页面接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("paper/gxq"), AcceptVerbs("GET")]
        public HttpResponseMessage GetXiangQing([FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DetailLearnModel data = learningexchange.GetXiangQing(id);
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
    }
}

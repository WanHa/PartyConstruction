using DTcms.BLL;
using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;
using static DTcms.DAL.MyQuestion;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("mqc/v1")]
    public class MyQuestionController : ApiControllerBase
    {
        private MyQuestions bll = new MyQuestions();
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
        /// 答题记录列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("que/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartyStyleList([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<MyQuestionList> result = bll.GetMyQuestionList(userid, rows, page);
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
        /// 获取用户答卷详情
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="testPaperId">试卷ID</param>
        /// <param name="rows">分页行数</param>
        /// <param name="page">分页页数</param>
        /// <returns></returns>
        [Route("que/dea"), AcceptVerbs("GET")]
        public HttpResponseMessage GetAnswerQuestionRecord([FromUri]int userid, [FromUri]string id, [FromUri]int rows, [FromUri]int page)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<AnswerQuestionRecordModel> result = bll.GetAnswerQuestionRecord(userid, id, rows, page);
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
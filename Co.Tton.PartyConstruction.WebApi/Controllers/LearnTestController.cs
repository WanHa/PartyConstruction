using DTcms.BLL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
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
    [RoutePrefix("v1/leaningtest")]
    public class LearnTestController : ApiControllerBase
    {
        private LearningTest learningtext = new LearningTest();

        /// <summary>
        /// 学习测试题库信息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="rows">分页行数</param>
        /// <param name="page">分页页数</param>
        /// <returns></returns>
        [Route("questionbank/list"),AcceptVerbs("GET")]
        public HttpResponseMessage GetQuestionBankList([FromUri]string userid, [FromUri]int rows, [FromUri]int page) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<QuestionDankModel> result = learningtext.GetQuestionBankList(userid, rows, page);

                if (result != null && result.Count > 0) {
                    message = RenderListTrueMessage(result, result.Count);
                }
                else {
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
        /// 根据题库ID获取题库下的试卷
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="questionbankid">题库ID</param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("testpagper/list"),AcceptVerbs("GET")]
        public HttpResponseMessage GetTestPaper([FromUri]string userid, [FromUri]string questionbankid, [FromUri]int rows, [FromUri]int page) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<TestPaperModel> result = learningtext.GetTestPaper(userid, questionbankid, rows, page);

                if (result != null && result.Count > 0) {
                    message = RenderListTrueMessage(result, result.Count);
                }
                else {
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
        /// 根据试卷ID获取试题ID
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="testpaperid">试卷ID</param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("testquestion/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetQuestionList([FromUri]string userid, [FromUri]string testpaperid, [FromUri]int rows, [FromUri]int page)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<TestQuestionModel> result = learningtext.GetQuestionList(userid, testpaperid, rows, page);

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

        [Route("submit"),AcceptVerbs("POST")]
        public HttpResponseMessage SubmitTestPagerAnswer([FromBody]SubmitAnswerFromBody fromBody) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                int score = new LearningTest().SubmitTestPagerAnswer(fromBody);
                Dictionary<string, int> result = new Dictionary<string, int>();
                result.Add("answer_score", score);
                message = RenderMessage(true, "提交数据成功", result, 1);
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
        [Route("answer/question/detail"),AcceptVerbs("GET")]
        public HttpResponseMessage GetAnswerQuestionRecord([FromUri]string userId, [FromUri]string testpaperid, [FromUri]int rows, [FromUri]int page) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<AnswerQuestionRecordModel> result = new LearningTest().GetAnswerQuestionRecord(userId, testpaperid, rows, page);
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
        /// 获取学习网址
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("learn/url"), AcceptVerbs("GET")]
        public HttpResponseMessage GetLearnUrl()
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                urlmodel result = new LearningTest().GetLearn();
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

    }
}

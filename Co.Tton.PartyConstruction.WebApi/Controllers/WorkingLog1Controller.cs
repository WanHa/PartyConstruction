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
    [RoutePrefix("worklog/plus")]
    public class WorkingLog1Controller : ApiControllerBase
    {
        private WorkingLog1 worklog1 = new WorkingLog1();

        /// <summary>
        /// 审核反馈
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add/feedback"), AcceptVerbs("POST")]
        public HttpResponseMessage GetWorkLog([FromBody]FeedBackModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = worklog1.AddFeedback(model.logid,model.userid,model.feedbackcount);
                if (data)
                {
                    message = RenderMessage(true, "回复成功.", "回复成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "回复失败");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 获取工作日志列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("arbeit/list"), AcceptVerbs("GET")]
        public HttpResponseMessage Getworkdiary([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<WorkLogsModel> data = worklog1.GetUserLogList(userid, rows, page);

                if (data != null)
                {
                    message = RenderListTrueMessage(data, data.Count);
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
        /// 获取工作日志详情接口
        /// </summary>
        /// <param name = "userid" ></ param >
        /// < param name="id"></param>
        /// <returns></returns>
        [Route("work/detail"), AcceptVerbs("GET")]
        public HttpResponseMessage DetailWorkDiary([FromUri]int userid, [FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                WorkLogDetailModel data = worklog1.GetWorkLogDetial(userid, id);
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

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
using static DTcms.DAL.WorkLog;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("work/log")]
    public class WorkingLogController : ApiControllerBase
    {
        private WorkingLog worklog = new WorkingLog();

        private WorkingLog1 worklog1 = new WorkingLog1();

        // GET: api/WorkingLog
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/WorkingLog/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WorkingLog
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/WorkingLog/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WorkingLog/5
        public void Delete(int id)
        {
        }

        //[Route("king/list"), AcceptVerbs("GET")]
        //public HttpResponseMessage GetManager([FromUri]int userid)
        //{
        //    HttpResponseMessage message = new HttpResponseMessage();
        //    try
        //    {
        //        ManagerModel data = worklog.GetManager(userid);
           
        //        if (data != null)
        //        {
        //             message = RenderMessage(true, "获取数据成功.", data, 1);
                   
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
        [Route("add/king"), AcceptVerbs("POST")]
        public HttpResponseMessage GetWorkLog([FromBody]WorkLogModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = worklog.GetWorkLog(model);
                if (data)
                {
                    message = RenderMessage(true, "获取数据成功.", "获取数据成功", 1);
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
        [Route("type/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetType([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DataSet data = worklog.GetType(userid);
                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data.Tables[0], 1);
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
        public HttpResponseMessage DetailWorkDiary([FromUri]int userid,[FromUri]string id)
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
        /// <summary>
        /// 获得当月/周/日工作日志
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("work/list"), AcceptVerbs("GET")]
        public HttpResponseMessage Getmonthdiary([FromUri]int userid, [FromUri]int rows, [FromUri]int page,int type)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<WorkUpdateModel> data = worklog.Monthupdate(userid, rows, page,type);
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
        /// 标题本月/周/日更新多少篇
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        [Route("title/list"), AcceptVerbs("GET")]
        public HttpResponseMessage Gettitle([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<Workcount> data = worklog.GetlogCont(userid);
                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data, 1);
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
        /// 审核反馈
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("audit/feedback"), AcceptVerbs("POST")]
        public HttpResponseMessage GetWorkLog([FromBody]FeedBackModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = worklog1.AddFeedback(model.logid, model.userid, model.feedbackcount);
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

    }
}

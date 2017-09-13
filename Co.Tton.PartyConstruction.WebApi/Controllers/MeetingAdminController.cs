using DTcms.BLL;
using DTcms.DAL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("mac/mee")]
    public class MeetingAdminController : ApiControllerBase
    {
        private MeeteAdmin bll = new MeeteAdmin();
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
        /// 会议管理列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("ting/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetMetingAdminList([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<meeteAdmin> result = bll.GetMetingAdminList(userid, rows, page);
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
        /// 会议详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("deta/adm"), AcceptVerbs("GET")]
        public HttpResponseMessage DeatilAdmin([FromUri]string id, [FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DetailMeeting data = bll.DeatilAdmin(id, userid);
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
        /// 用户报名的接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("sub/adm"), AcceptVerbs("GET")]
        public HttpResponseMessage SelAdminSubmit([FromUri]string id, [FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.SelAdminSubmit(id, userid);
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
        /// 会议签到列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("end/list"), AcceptVerbs("GET")]
        public HttpResponseMessage StartEndMeeting([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<StartEndAdmin> result = bll.StartEndMeeting(userid, rows, page);
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
        /// 会议签到人员接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("mee/adm"), AcceptVerbs("GET")]
        public HttpResponseMessage GetMeetingAdmin([FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                MeetingCount data = bll.GetMeetingAdmin(id);
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
        /// 别人的状态 0 点击参会人员 / 1 已签到 / 2 未签到
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("info/stat"), AcceptVerbs("GET")]
        public HttpResponseMessage GetInfoStatus([FromUri]int type, [FromUri]int userid, [FromUri]string id, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<InfoModel> result = bll.GetInfoStatus(type, userid, id, rows, page);
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
        /// 用户签到接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("up/info"), AcceptVerbs("GET")]
        public HttpResponseMessage UpdataInfo([FromUri]int userid, [FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.UpdataInfo(userid, id);
                if (data)
                {
                    message = RenderMessage(true, "修改数据成功.", data, 1);
                }
                else
                {
                    message = RenderMessage(false, "修改数据失败.");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 用户的会议签到的列表
        /// </summary>
        /// <param name="useridid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("ess/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetUserMeetingList([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<UserMeetingModel> result = bll.GetUserMeetingList(userid, rows, page);
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

        #region ==== Web页面会议管理接口
        [Route("web/detail"),AcceptVerbs("GET")]
        public HttpResponseMessage WebMeetingDetail([FromUri]string id) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                WebMeetingDetailModel result = bll.WebMeetingDetail(id);
                if (result != null) {
                    message = RenderMessage(true, "获取数据成功", result, 1);
                }
                else {
                    message = RenderMessage(false, "获取数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        [Route("web/add"),AcceptVerbs("POST")]
        public HttpResponseMessage WebMeetingAdd([FromBody]WebMeetingFromBody fromBody) {
            
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = bll.WebMeetingAdd(fromBody);
                if (result)
                {
                    message = RenderMessage(true, "新建成功", result, 1);
                }
                else {
                    message = RenderMessage(false, "新建数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        [Route("web/edit"), AcceptVerbs("POST")]
        public HttpResponseMessage WebMeetingEdit([FromBody]WebMeetingFromBody fromBody)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = bll.WebMeetingEdit(fromBody);
                if (result)
                {
                    message = RenderMessage(true, "编辑数据成功", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "编辑数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }
        #endregion
    }
}
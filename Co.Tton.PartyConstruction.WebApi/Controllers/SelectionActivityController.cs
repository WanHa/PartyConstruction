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
    [RoutePrefix("v1/view")]
    public class SelectionActivityController : ApiControllerBase
    {
        private SelectAty bll = new SelectAty();
        /// <summary>
        /// 评选活动的列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("select/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetViewList([FromUri]int userid, [FromUri] int rows, [FromUri] int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<ViewModel> result = bll.GetView(userid, rows, page);
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
        /// 任何人可见 投完票
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("data/everyone"), AcceptVerbs("Get")]
        public HttpResponseMessage DetailReviewModel([FromUri]string id, [FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                ViewActivionToModel data = bll.DetailReviewModel(id, userid);
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
        /// 投完票 投票后可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("done/user"), AcceptVerbs("Get")]
        public HttpResponseMessage GetReview([FromUri]string id, [FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                ModelView data = bll.GetView(id, userid);
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
        /// 投票已完成页面接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("end/view"), AcceptVerbs("GET")]
        public HttpResponseMessage EndViewModel([FromUri]string id, [FromUri] int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                EndToAction data = bll.EndViewModel(id, userid);
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

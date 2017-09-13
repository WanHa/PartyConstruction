using DTcms.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using WebApi.Controllers;
using static DTcms.DAL.PartyBranchManagement;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("share/comment")]
    public class CommentListController : ApiControllerBase
    {
        /// <summary>
        /// 发送评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("send/comment"), AcceptVerbs("POST")]
        public HttpResponseMessage GetWorkLog([FromBody]discuss model)
        {
            ShareComment SCT = new ShareComment();
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = SCT.sendcomment(model);
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
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="shareid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("comment/list"), AcceptVerbs("GET")]
        public HttpResponseMessage Getcommentlist([FromUri]int userid,[FromUri]string shareid, [FromUri]int rows, [FromUri]int page )
        {
            ShareComment SC = new ShareComment();
               HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<DTcms.DAL.PartyBranchManagement.commentlist> data = SC.GetList(userid,shareid, page, rows);
                if (data != null && data.Count > 0)
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
        /// 分享列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("share/list"), AcceptVerbs("GET")]
        public HttpResponseMessage Getlist([FromUri]int userid, [FromUri]int groupid, [FromUri] int page, [FromUri]int rows)
        {
            ShareComment SC = new ShareComment();
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<DTcms.DAL.PartyBranchManagement.sharelist> data = SC.GetContentList(userid, groupid,page,rows);
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

    }
}

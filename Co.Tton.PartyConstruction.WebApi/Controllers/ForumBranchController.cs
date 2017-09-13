using DTcms.BLL;
using DTcms.Model;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/list")]
    public class ForumBranchController : ApiControllerBase
    {
        /// <summary>
        /// 获取支部或论坛发布信息时,@人员列表信息
        /// </summary>
        /// <param name="groupid">支部或论坛ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="type">区分是支部还是论坛发布信息0-->支部，1-->论坛</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [Route("at/personnel"),AcceptVerbs("GET")]
        public HttpResponseMessage GetNewInfoAtPersonnels([FromUri]string groupid, [FromUri]string userid, [FromUri]int type, 
            [FromUri]int page, [FromUri]int rows) {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<AtPersonnelModel> result = new ForumBranchBll().GetNewInfoAtPersonnels(groupid, userid, type, page, rows);
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
                throw;
            }

            return message;
        }


        [Route("forum"),AcceptVerbs("GET")]
        public HttpResponseMessage GetForumList([FromUri]string forumid, [FromUri]string userid, [FromUri]int page, [FromUri]int rows) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ForumBranchModel> result = new ForumBranchBll().GetForumList(forumid, userid, page, rows);
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


        [Route("branch"),AcceptVerbs("GET")]
        public HttpResponseMessage GetBranchList([FromUri]string branchid, [FromUri]string userid, [FromUri]int page, [FromUri]int rows) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ForumBranchModel> result = new ForumBranchBll().GetBranchList(branchid, userid, page, rows);
                if (result != null && result.Count > 0)
                {
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

        [Route("personal/center"),AcceptVerbs("GET")]
        public HttpResponseMessage GetPersonalCenter([FromUri]string userid, [FromUri]int page, [FromUri]int rows) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ForumBranchModel> result = new ForumBranchBll().GetPersonalCenter(userid, page, rows);
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

        [Route("mycollect"),AcceptVerbs("GET")]
        public HttpResponseMessage GetPersonalCenterCollect([FromUri]string userid, [FromUri]int page, [FromUri]int rows) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ForumBranchModel> result = new ForumBranchBll().GetPersonalCenterCollect(userid, page, rows);
                if (result != null && result.Count > 0)
                {
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

        [Route("at/my"),AcceptVerbs("GET")]
        public HttpResponseMessage GetAtMyInfoList([FromUri]string userid,[FromUri]int page, [FromUri]int rows) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ForumBranchModel> result = new ForumBranchBll().GetAtMyInfoList(userid, page, rows);
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

        [Route("common"),AcceptVerbs("GET")]
        public HttpResponseMessage GetCommentsList([FromUri]string userid, [FromUri]int page, [FromUri]int rows) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ForumBranchModel> result = new ForumBranchBll().GetCommentsList(userid, page, rows);
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

        [Route("thumb"), AcceptVerbs("GET")]
        public HttpResponseMessage GetThumbUpList([FromUri]string userid, [FromUri]int page, [FromUri]int rows)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ForumBranchModel> result = new ForumBranchBll().GetThumbUpList(userid, page, rows);
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

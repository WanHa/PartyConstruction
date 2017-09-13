using DTcms.BLL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("f1/partygroupforum")]
    public class PartyGroupForumController : ApiControllerBase
    {
        private PartyGroupForum bll = new PartyGroupForum();

        /// <summary>
        /// 新建党小组论坛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add/forum"), AcceptVerbs("POST")]
        public HttpResponseMessage AddPartyGroupForum([FromBody]PartyGroupForumModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.AddForum(model);
                if (data)
                {
                    message = RenderMessage(true, "新建论坛成功.", "新建论坛成功.", 1);
                }
                else
                {
                    message = RenderMessage(false, "新建论坛失败.");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 获取自己所在的/所有的党小组论坛
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("list/mygroup"), AcceptVerbs("GET")]
        public HttpResponseMessage GetGroupList([FromUri]int userid, [FromUri]int type, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<GroupList> result = bll.GetGroupList(userid, type, rows, page);
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
        /// 获取论坛简介
        /// </summary>
        /// <param name="groupforumId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("detail/foruminfo"), AcceptVerbs("GET")]
        public HttpResponseMessage GetForumInfo([FromUri]string groupforumId)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                ForumInfo result = bll.GetForumInfo(groupforumId);
                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功.", result, 1);
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
        /// 党小组论坛申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add/apply"), AcceptVerbs("POST")]
        public HttpResponseMessage CommitApply([FromBody]CommitApply model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.CommitApply(model);
                if (data)
                {
                    message = RenderMessage(true, "申请成功.", "申请成功.", 1);
                }
                else
                {
                    message = RenderMessage(false, "申请失败.");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 申请人列表
        /// </summary>
        /// <param name="groupforumId"></param>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("list/apply"),AcceptVerbs("GET")]
        public HttpResponseMessage ApplyList([FromUri]string userid,[FromUri]int rows,[FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<ApplyList> result = bll.GetApplyList(userid,rows,page);
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
        /// 党小组论坛申请审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("check/apply"),AcceptVerbs("POST")]
        public HttpResponseMessage CheckApply([FromBody]CheckApply model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.CheckApply(model);
                if (data)
                {
                    message = RenderMessage(true, "审核成功.", "审核成功.", 1);
                }
                else
                {
                    message = RenderMessage(false, "审核失败.");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 退出党小组论坛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("del/groupmembers"),AcceptVerbs("POST")]
        public HttpResponseMessage DelGroupForum([FromBody]DelGroupForum model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.DelGroupForum(model);
                if (data)
                {
                    message = RenderMessage(true, "退出成功.", "退出成功.", 1);
                }
                else
                {
                    message = RenderMessage(false, "退出失败.");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 举报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add/report"), AcceptVerbs("POST")]
        public HttpResponseMessage Report([FromBody]Report model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.Report(model);
                if (data)
                {
                    message = RenderMessage(true, "举报成功.", "举报成功.", 1);
                }
                else
                {
                    message = RenderMessage(false, "举报失败.");
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
using DTcms.BLL;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/branchmanagement")]
    public class BranchManagementController : ApiControllerBase
    {
        private Branch partybranch = new Branch();
        /// <summary>
        /// 获取党组织列表
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Route("branchmanagement/list"), AcceptVerbs("POST")]
        public HttpResponseMessage GetBranchList(GroupManagement group)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<Banch> result = partybranch.GetBranchList(group);
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
        /// 个人介绍接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("person/details"), AcceptVerbs("GET")]
        public HttpResponseMessage GetDetails([FromUri]int userid,[FromUri] int groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Details data = partybranch.GetDetails(userid, groupid);
                if (data != null)
                {
                    message = RenderListTrueMessage(data, 1);
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
        /// 发布内容接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("share/release"), AcceptVerbs("POST")]
        public HttpResponseMessage Add([FromBody]Release model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                if (model.types == 0)//发布文字
                {
                    Boolean data = partybranch.Addchar(model);
                    if (data)
                    {
                        message = RenderMessage(true, "分享成功.", "分享成功.", 1);
                    }
                    else
                    {
                        message = RenderMessage(false, "分享失败.");
                    }
                }
                if (model.types == 1)//发布文字图片
                {
                    Boolean data = partybranch.AddImage(model);
                    if (data)
                    {
                        message = RenderMessage(true, "分享成功.", "分享成功.", 1);
                    }
                    else
                    {
                        message = RenderMessage(false, "分享失败.");
                    }
                }
                if (model.types == 2)//发布文字视频
                {
                    Boolean data = partybranch.AddVideo(model);
                    if (data)
                    {
                        message = RenderMessage(true, "分享成功.", "分享成功.", 1);
                    }
                    else
                    {
                        message = RenderMessage(false, "分享失败.");
                    }
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 获取组员列表
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("person/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetMembersList([FromUri]string groupid, [FromUri] int rows, [FromUri] int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<PersonList> result = partybranch.GetMembersList(groupid, rows, page);
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
        /// 获取党组织简介接口
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("detail/branch"), AcceptVerbs("GET")]
        public HttpResponseMessage GetList([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                BranchLists result = partybranch.GetBranch(groupid);
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
        /// 搜索党支部接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("branch/seach"), AcceptVerbs("POST")]
        public HttpResponseMessage SeachBranch([FromBody]Where model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<BLists> result = partybranch.SeachBranch(model);
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

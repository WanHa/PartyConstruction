using DTcms.BLL;
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
    [RoutePrefix("ztree")]
    public class GroupZTreeController : ApiControllerBase
    {
        [Route("groups"),AcceptVerbs("GET")]
        public HttpResponseMessage GetGroupZTreeData() {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ZTreeModel> result = new GroupZTreeBll().GetGroupZTreeData();
                message = RenderListTrueMessage(result, result.Count);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        [Route("edit-groups"), AcceptVerbs("GET")]
        public HttpResponseMessage GetEditGroupZTreeData([FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ZTreeModel> result = new GroupZTreeBll().GetEditGroupZTreeData(id);
                message = RenderListTrueMessage(result, result.Count);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        [Route("activity/edit-groups"), AcceptVerbs("GET")]
        public HttpResponseMessage GetActivityEditGroupZTreeData([FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ZTreeModel> result = new GroupZTreeBll().GetActivityEditGroupZTreeData(id);
                message = RenderListTrueMessage(result, result.Count);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 根据人员ID获取组织列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("user/groups"), AcceptVerbs("GET")]
        public HttpResponseMessage GetUserGroups([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<int> result = new GroupZTreeBll().GetUserGroups(userid);
                message = RenderListTrueMessage(result, result.Count);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

    }
}

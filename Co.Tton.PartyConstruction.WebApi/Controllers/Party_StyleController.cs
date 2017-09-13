using DTcms.BLL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;
using static DTcms.DAL.Partystyle_logic;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("party_style/sty")]
    public class Party_StyleController : ApiControllerBase
    {
        Style bll = new Style();
        /// <summary>
        /// 获取活动风采详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("details"), AcceptVerbs("GET")]
        public HttpResponseMessage GetDetail([FromUri]string id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Webstyle result = bll.GetDetail(id);
                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }
        /// <summary>
        /// 活动风采新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("web/add"), AcceptVerbs("POST")]
        public HttpResponseMessage StyleAdd([FromBody]WebAdd model)
        {

            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean result = bll.StyleAdd(model);
                if (result)
                {
                    message = RenderMessage(true, "新建成功", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "新建数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("web/edit"), AcceptVerbs("POST")]
        public HttpResponseMessage WebEdit([FromBody]WebAdd model)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = bll.WebEdit(model);
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
        [Route("member"), AcceptVerbs("POST")]
        public HttpResponseMessage GetMem([FromBody]ZTreeUserFromBody fromBody)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<ZTreeModel> result = bll.GetMem(fromBody);
                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 获取组织以及下属组织所有人员信息
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("gourp/all-users"),AcceptVerbs("GET")]
        public HttpResponseMessage GetGroupAllUsers([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<ZTreeModel> result = bll.GetGroupAllUsers(groupid);
                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取数据失败");
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

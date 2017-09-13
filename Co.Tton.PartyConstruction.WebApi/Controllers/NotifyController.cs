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
    [RoutePrefix("partycomm/notify")]
    public class NotifyController : ApiControllerBase
    {
        private Notify bll = new Notify();

        [Route("web/add"), AcceptVerbs("POST")]
        public HttpResponseMessage AddNotify([FromBody]DTcms.Model.ztree model)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = bll.AddNotify(model);
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


        [Route("web/detail"), AcceptVerbs("GET")]
        public HttpResponseMessage GetNotify([FromUri]int id)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                DTcms.Model.ztree result = bll.GetNotify(id);
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


        [Route("web/edit"), AcceptVerbs("POST")]
        public HttpResponseMessage EditNotify([FromBody]DTcms.Model.ztree model)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = bll.EditNotify(model);
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

        [Route("get/group"),AcceptVerbs("GET")]
        public HttpResponseMessage GetGroup()
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<ZTreeModel> result = bll.GetGroup();
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

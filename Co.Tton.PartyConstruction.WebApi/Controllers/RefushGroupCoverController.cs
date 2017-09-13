using DTcms.BLL;
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
    [RoutePrefix("v1/groupcover")]
    public class RefushGroupCoverController : ApiControllerBase
    {
        [Route("refush"), AcceptVerbs("POST")]
        public HttpResponseMessage RefushGroupCover([FromBody] RefushCoverModel fromBody)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean issuccess = new RefushCover().RufushCover(fromBody);
                if (issuccess)
                {
                    message = RenderMessage(true, "更改封面成功", "更改封面成功",1);
                }
                else
                {
                    message = RenderMessage(false, "更改封面失败");
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

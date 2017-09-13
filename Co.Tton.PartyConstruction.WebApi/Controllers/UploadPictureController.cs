using DTcms.BLL;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("img/uploadpicture")]
    public class UploadPictureController : ApiControllerBase
    {
        private UploadPicture bll = new UploadPicture();

        [Route("add/image"),AcceptVerbs("POST")]
        public HttpResponseMessage UploadPicture([FromBody]UploadPictureModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                string data = bll.AddImage(model);
                if (data.Length>0)
                {
                    message = RenderMessage(true, "上传成功.", data, 1);
                }
                else
                {
                    message = RenderMessage(false, "上传失败.");
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

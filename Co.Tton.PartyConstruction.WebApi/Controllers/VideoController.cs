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
    [RoutePrefix("phone/video")]
    public class VideoController : ApiControllerBase
    {/// <summary>
    /// 获取观看记录列表
    /// </summary>
        VideoRecord record = new VideoRecord();
        [Route("video/list"), AcceptVerbs("GET")]
        public HttpResponseMessage StudioList([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<videomodel> data = record.VideoList(userid, rows, page);
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


        /// <summary>
        /// 获取观看视频累计时长
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("sum/time"), AcceptVerbs("GET")]
        public HttpResponseMessage GetManager([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                timemodel data = record.BllVideoTime(userid);

                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data, 1);

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


        [Route("per/photo"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPersonal([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                personalmodel data = record.BllPersonCenter(userid);

                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data, 1);

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


    }
}

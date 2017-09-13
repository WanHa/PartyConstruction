using DTcms.BLL;
using DTcms.DAL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("mps/course")]
    public class PartyVideoController : ApiControllerBase
    {
        private PartysVideo video = new PartysVideo();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("cr/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetCoure([FromUri]int userid, [FromUri] int rows, [FromUri] int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<videoModel> result = video.GetPartyNewsList(userid, rows, page);
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
        ///视频列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("video/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetVideo([FromUri]int userid, [FromUri] int id, [FromUri] int rows, [FromUri] int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                CorceDetialModel result = video.GetVideoList(userid, id, rows, page);
                if (result != null && result.videos !=null && result.videos.Count > 0)
                {
                    message = RenderListTrueMessage(result, result.videos.Count);
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
        /// 收藏课程接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("cou/coll"), AcceptVerbs("GET")]
        public HttpResponseMessage GetCouceCollect([FromUri]int userid, [FromUri] int id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                int data = video.GetCourceCollect(userid, id);
                if (data==1)
                {
                    message = RenderMessage(true, "收藏数据成功.", "收藏数据成功.",1);
                }               
                else if(data==2)
                {
                    message = RenderMessage(false, "该课程没有视频，不能收藏");
                }
                else
                {
                    message = RenderMessage(false, "收藏视频失败.");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 删除课程收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("cou/del"), AcceptVerbs("GET")]
        public HttpResponseMessage DeleteVideoCollect([FromUri]int userid, [FromUri] int id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
               Boolean data= video.DelectCource(userid,id);
                if (data)
                {
                    message = RenderMessage(true, "删除收藏成功.","删除收藏成功.",1);
                }
                else
                {
                    message = RenderMessage(false, "删除收藏失败.", "删除收藏失败.", 1);
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 观看视频接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("play/coun"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPlayCount([FromUri]int userid,[FromUri]int id, [FromUri]string videoid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                int data = video.GetPlayCount(userid,id,videoid);
                if (data!=0)
                {
                    message = RenderMessage(true, "视频播放成功", data, 1);
                }
                else
                {
                    message = RenderMessage(true, "视频播放成功.", data, 1);
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 上传视频时长接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="videoid"></param>
        /// <returns></returns>
        [Route("vio/sts"), AcceptVerbs("POST")]
        public HttpResponseMessage GetVideoStatus([FromBody]VideoPlay model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                bool data = video.GetVideoStatus(model);
                if (data)
                {
                    message = RenderMessage(true, "上传视频时长成功", data, 1);
                }
                else
                {
                    message = RenderMessage(false, "上传视频时长失败.");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 上传观看视频时长
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        [Route("upload/time"),AcceptVerbs("POST")]
        public HttpResponseMessage UploadVideoPlaybackTime([FromBody]UploadVideoPlaybackFrombody fromBody) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = new PartysVideo().UploadVideoPlaybackTime(fromBody);
                if (result)
                {
                    message = RenderMessage(true, "成功", "成功", 1);
                }
                else {
                    message = RenderMessage(false, "失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 获取视频详情
        /// </summary>
        /// <param name="videoid">视频ID</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        [Route("video/detail"), AcceptVerbs("GET")]
        public HttpResponseMessage GetVideoDetail([FromUri]string videoid, [FromUri]string userid) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                VideoDetialModel data = new PartysVideo().GetVideoDetail(videoid, userid);
                if (data != null) {
                    message = RenderMessage(true, "获取数据成功", data, 1);
                }
                else {
                    message = RenderEntityFalseMessage();
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
                throw;
            }

            return message;
        }

    }
}
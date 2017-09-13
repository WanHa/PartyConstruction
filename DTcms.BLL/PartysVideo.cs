using DTcms.DAL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public class PartysVideo
    {
        private PartyVideo video = new PartyVideo();
        /// <summary>
        /// 获取课程的接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<videoModel> GetPartyNewsList(int userid, int rows, int page)
        {
            return video.GetPartyNewsList(userid, rows, page);
        }
        public CorceDetialModel GetVideoList(int userid, int id, int rows, int page)
        {
            return video.GetVideoList(userid, id, rows, page);
        }
        public int GetCourceCollect(int userid, int id)
        {
            return video.GetCourceCollect(userid, id);
        }
        public Boolean DelectCource(int userid, int id)
        {
            return video.DelectCource(userid,id);
        }
        public int GetPlayCount(int userid,int id,string videoid)
        {
            return video.GetPlayCount(userid,id, videoid);

        }
        public Boolean GetVideoStatus(VideoPlay model)
        {
            return video.GetVideoStatus(model);
        }

        public Boolean UploadVideoPlaybackTime(UploadVideoPlaybackFrombody fromBody) {
            return video.UploadVideoPlaybackTime(fromBody);
        }

        /// <summary>
        /// 获取视频详情
        /// </summary>
        /// <param name="videoId">视频ID</param>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        public VideoDetialModel GetVideoDetail(string videoId, string userId) {
            return video.GetVideoDetail(videoId, userId);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    public class UploadVideoPlaybackFrombody
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public int course_id { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        public string video_id { get; set; }

        /// <summary>
        /// 观看时长
        /// </summary>
        public int playback_time { get; set; }
    }
}

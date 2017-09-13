using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class CorceDetialModel
    {
        /// <summary>
        /// 总时长
        /// </summary>
        public int total_time { get; set; }

        /// <summary>
        /// 学习时间
        /// </summary>
        public int learning_time { get; set; }

        /// <summary>
        /// 视频信息
        /// </summary>
        public List<VideoDetialModel> videos { get; set; }
    }

}

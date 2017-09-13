using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class VideoDetialModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 视频Uri
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 播放次数
        /// </summary>
        public int playcount { get; set; }

        /// <summary>
        /// 收藏次数
        /// </summary>
        public int collectcount { get; set; }

        /// <summary>
        /// 转发次数
        /// </summary>
        public int trankcount { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public int collect { get; set; }

        /// <summary>
        /// 视频图片
        /// </summary>
        public string videopic { get; set; }
        //public int playtime { get; set; }

        /// <summary>
        /// 最近一次播放时间
        /// </summary>
        public int lastplaytime { get; set; }

        /// <summary>
        /// 最大播放时间
        /// </summary>
        public int maxplaytime { get; set; }
        
        /// <summary>
        /// 视频时长
        /// </summary>
        public int videolength { get; set; }
    }
}

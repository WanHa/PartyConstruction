using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// 七牛云视频信息
    /// </summary>
    public class QiNiuVideoInfoModel
    {
        public VideoLengthModel format { get; set; }
    }

    public class VideoLengthModel {
        /// <summary>
        ///  视频时长
        /// </summary>
        public double duration { get; set; }
    }
}
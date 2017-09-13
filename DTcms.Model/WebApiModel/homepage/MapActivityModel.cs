using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.homepage
{
    public class MapActivityModel
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        public string activity_name { get; set; }

        /// <summary>
        /// 活动地址
        /// </summary>
        public string activity_address { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public string start_time { get; set; }
        
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// 主办单位
        /// </summary>
        public string organizer { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int personnel_count { get; set; }

        /// <summary>
        /// 活动封面图片
        /// </summary>
        public string cover_pic { get; set; }

        /// <summary>
        /// 活动详情
        /// </summary>
        public string activity_detail { get; set; }
    }
}

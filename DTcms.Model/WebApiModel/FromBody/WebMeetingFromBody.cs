using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    public class WebMeetingFromBody
    {
        /// <summary>
        /// 编辑时会议ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 会议标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string statrtime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string endtime { get; set; }

        /// <summary>
        /// 会议地点
        /// </summary>
        public string site { get; set; }

        /// <summary>
        /// 与会人数
        /// </summary>
        public int count { get; set; } 

        /// <summary>
        /// 会议内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 与会人员ID列表字符串
        /// </summary>
        public string people { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string userid { get; set; }  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// Web页面会议详情
    /// </summary>
    public class WebMeetingDetailModel
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string starttime { get; set; }

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
        public int peoplecount { get; set; }

        public List<MeetingPeople> people { get; set; }
    }

    public class MeetingPeople {

        /// <summary>
        /// 人员ID
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string username { get; set; }
    }
}

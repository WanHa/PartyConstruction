using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class WorkLogPlusModel
    {
    }


    /// <summary>
    /// 反馈模型
    /// </summary>
    public class FeedBackModel
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public string logid { get; set; }


        /// <summary>
        /// 提交反馈人id
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// //反馈内容
        /// </summary>
        public string feedbackcount { get; set; }
    }


    public class WorkLogsModel
    {
        private string _time;
        public string time
        {
            get
            {
                return _time == null ? "" : DateTime.Parse(_time).ToString("yyyy年MM月dd日");
            }
            set
            {
                _time = value;
            }
        }

        /// <summary>
        /// 工作日志ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 工作日志标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int feedback { get; set; }
    }

    public class WorkLogDetailModel
    {
        public List<ImageModel> image { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        private string _time;
        public string time
        {
            get
            {
                return _time == null ? "" : DateTime.Parse(_time).ToString("yyyy年MM月dd日");
            }
            set
            {
                _time = value;
            }
        }

        public string auditcontent { get; set; }

        private string _feedbacktime;
        public string feedbacktime 
        {
            get
            {
                return _feedbacktime == null ? "" : DateTime.Parse(_feedbacktime).ToString("yyyy年MM月dd日");
            }
            set
            {
                _feedbacktime = value;
            }
        }

    }
}

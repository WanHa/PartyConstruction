using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class GeTuiPushModel
    {
        /// <summary>
        /// 推送类型
        /// </summary>
        public int push_type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string push_title { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public string push_time { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 信息数据ID
        /// </summary>
        public string message_id { get; set; }
    }
}

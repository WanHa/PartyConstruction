using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// API题库model
    /// </summary>
    public class QuestionDankModel
    {
        /// <summary>
        /// 题库ID
        /// </summary>
        public string p_id { get; set; }

        /// <summary>
        /// 题库标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 图片URL路径
        /// </summary>
        public string image_url { get; set; }

        /// <summary>
        /// 试卷数量
        /// </summary>
        public int test_paper_number { get; set; }

        /// <summary>
        /// 点击数量
        /// </summary>
        public int click_count { get; set; }
    }
}

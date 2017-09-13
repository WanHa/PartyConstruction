using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// 试题答案
    /// </summary>
    public class AnswerModel
    {
        /// <summary>
        /// 答案ID
        /// </summary>
        public string p_id { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public string p_sequence { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 是否是正确答案
        /// </summary>
        public int is_answer { get; set; }
    }
}

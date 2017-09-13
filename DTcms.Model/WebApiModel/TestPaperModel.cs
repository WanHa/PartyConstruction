using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// 手机API试卷Model
    /// </summary>
    public class TestPaperModel
    {
        /// <summary>
        /// 试卷ID
        /// </summary>
        public string p_id { get; set; }
        
        /// <summary>
        /// 试卷名称
        /// </summary>
        public string p_testpapername { get; set; }

        /// <summary>
        /// 答题时间
        /// </summary>
        public int p_answertime { get; set; }

        /// <summary>
        /// 是否可以答题 0 不能答 1 能答
        /// </summary>
        public int answer { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public int score { get; set; }
    }
}

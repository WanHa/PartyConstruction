using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// Api试题model
    /// </summary>
    public class TestQuestionModel
    {
        /// <summary>
        /// 试题ID
        /// </summary>
        public string p_id { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        public string p_questionstem { get; set; }

        /// <summary>
        /// 答案列表
        /// </summary>
        public List<AnswerModel> answers { get; set; }
    }
}

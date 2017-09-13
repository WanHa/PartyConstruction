using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// 用户答卷详情model
    /// </summary>
    public class AnswerQuestionRecordModel
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

        /// <summary>
        /// 用户选择答案
        /// </summary>
        public List<SelectedAnswerModel> selected_answers { get; set; }
    }

    public class SelectedAnswerModel {

        /// <summary>
        /// 选中答案ID
        /// </summary>
        public string selected_id { get; set; }

        /// <summary>
        /// 选中答案序列
        /// </summary>
        public string selected_sequence { get; set; }
    }

    /// <summary>
    /// 在线学习的网址
    /// </summary>
    public class urlmodel
    {
        public string learnurl { get; set; }

    }

}

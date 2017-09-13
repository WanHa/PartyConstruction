using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    public class SubmitAnswerFromBody
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 试卷ID
        /// </summary>
        public string testpager_id { get; set; }

        /// <summary>
        /// 试题列表
        /// </summary>
        public List<Question> questions { get; set; }
    }

    public class Question {

        /// <summary>
        /// 试题ID
        /// </summary>
        public string question_id { get; set; }

        /// <summary>
        /// 答案列表
        /// </summary>
        public List<string> answers { get; set; }
    }
}

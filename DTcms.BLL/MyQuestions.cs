using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.MyQuestion;

namespace DTcms.BLL
{
   public class MyQuestions
    {
        private MyQuestion dal = new MyQuestion();
        /// <summary>
        /// 答题的列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<MyQuestionList> GetMyQuestionList(int userid, int rows, int page)
        {
            return dal.GetMyQuestionList(userid,rows,page);
        }
        public List<AnswerQuestionRecordModel> GetAnswerQuestionRecord(int userId, string id, int rows, int page)
        {
            return dal.GetAnswerQuestionRecord(userId,id,rows,page);
        }
    }
}

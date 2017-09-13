using DTcms.DAL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public class LearningTest
    {
        private LearnTest learn = new LearnTest();

        /// <summary>
        /// 获取题库列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<QuestionDankModel> GetQuestionBankList(string userId, int pageSize, int pageIndex) {
            return learn.GetQuestionBankList(userId, pageSize, pageIndex);
        }

        /// <summary>
        /// 根据题库ID获取试卷列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="questionBankId">题库ID</param>
        /// <param name="pageSize">分页行数</param>
        /// <param name="pageIndex">分页页数</param>
        /// <returns></returns>
        public List<TestPaperModel> GetTestPaper(string userId, string questionBankId, int pageSize, int pageIndex) {
            return learn.GetTestPaper(userId, questionBankId, pageSize, pageIndex);
        }

        /// <summary>
        /// 根据试卷ID获取试题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="testPaperId">试卷ID</param>
        /// <param name="pageSize">分页行数</param>
        /// <param name="pageIndex">分页页数</param>
        /// <returns></returns>
        public List<TestQuestionModel> GetQuestionList(string userId, string testPaperId, int pageSize, int pageIndex) {
            return learn.GetQuestionList(userId, testPaperId, pageSize, pageIndex);
        }

        /// <summary>
        /// 提交试卷
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public int SubmitTestPagerAnswer(SubmitAnswerFromBody fromBody) {
            return learn.SubmitTestPagerAnswer(fromBody);
        }

        /// <summary>
        /// 获取用户答卷详情
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="testPaperId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<AnswerQuestionRecordModel> GetAnswerQuestionRecord(string userId, string testPaperId, int pageSize, int pageIndex) {
            return learn.GetAnswerQuestionRecord(userId, testPaperId, pageSize, pageIndex);
        }
        /// <summary>
        /// 获取学习网址
        /// </summary>
        /// <returns></returns>
        public urlmodel GetLearn()
        {
            return learn.GetUrl();

        }


    }
}

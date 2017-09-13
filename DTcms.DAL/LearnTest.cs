using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class LearnTest
    {
        #region 基本方法
        /// <summary>
        /// 获取学习测试题库信息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<QuestionDankModel> GetQuestionBankList(string userId, int pageSize, int pageIndex)
        {

            string sqlStr = String.Format(@"select 
                        P_Id as p_id,
                        P_QuestionBankName as title,
                        (select P_Image.P_ImageUrl from P_Image where P_Image.P_ImageId = P_QuestionBank.P_Id and P_Image.P_ImageType = '{1}' ) as image_url,
                        P_CreateTime,
                        (select count(P_TestPaper.P_Id) from P_TestPaper where P_QuestionBankId = P_QuestionBank.P_Id and P_TestPaper.P_Status = 0) as test_paper_number
                        from P_QuestionBank
                        where P_QuestionBank.P_Status != 1", userId, (int)ImageTypeEnum.学习测试);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(pageSize, pageIndex, sqlStr.ToString(), " P_CreateTime desc"));

            DataSetToModelHelper<QuestionDankModel> helper = new DataSetToModelHelper<QuestionDankModel>();

            List<QuestionDankModel> result = helper.FillModel(ds);
            // 获取题库点击数量
            if (result != null && result.Count > 0) {
                foreach (var item in result)
                {
                    item.click_count = GetQuestionBankClickCount(item.p_id);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取题库点击次数
        /// </summary>
        /// <param name="questtionId"></param>
        /// <returns></returns>
        private int GetQuestionBankClickCount(string questtionId) {
            int result = 0;

            string sqlStr = String.Format(@"select COUNT(t.P_CreateUser) from (
                        select P_AnswerSheetRecord.P_CreateUser from P_AnswerSheetRecord
                        where P_AnswerSheetRecord.P_QuestionBankId = '{0}'
                        group by P_AnswerSheetRecord.P_CreateUser
                        ) t", questtionId);

            string clickCount = Convert.ToString( DbHelperSQL.GetSingle(sqlStr.ToString()));

            if (!String.IsNullOrEmpty(clickCount)) {
                result = Int32.Parse(clickCount);
            }
            return result;
        }

        /// <summary>
        /// 根据题库获取试卷信息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="questionBankId">题库ID</param>
        /// <param name="pageSize">分页行数</param>
        /// <param name="pageIndex">分页页数</param>
        /// <returns></returns>
        public List<TestPaperModel> GetTestPaper(string userId, string questionBankId, int pageSize, int pageIndex)
        {

            string sqlStr = String.Format(@"select 
                        t.p_id,
                        t.p_testpapername,
                        t.p_answertime,
                        t.P_CreateTime,
                        case when t.p_isrepeat = 0 and t.last_score is NOT NULL then 0 else 1 END as answer,
                        case when t.last_score is null then -1 ELSE t.last_score END as score
                        from (
	                        select 
	                        P_Id as p_id,
	                        P_TestPaperName as p_testpapername,
	                        P_AnswerTime as p_answertime,
	                        P_IsRepeat as p_isrepeat,
	                        P_CreateTime,
	                        (select TOP 1 P_AnswerSheetRecord.P_Score from P_AnswerSheetRecord 
	                        where P_AnswerSheetRecord.P_TestPaperId = P_TestPaper.P_Id and P_AnswerSheetRecord.P_UserId = {0}
	                        ORDER BY P_AnswerSheetRecord.P_CreateTime DESC) as last_score
	                        from P_TestPaper
	                        where P_Status != 1 and P_QuestionBankId = '{1}'
                        ) t", userId, questionBankId);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(pageSize, pageIndex, sqlStr.ToString(), " P_CreateTime asc"));

            DataSetToModelHelper<TestPaperModel> helper = new DataSetToModelHelper<TestPaperModel>();

            List<TestPaperModel> model = helper.FillModel(ds);

            return model;
        }

        /// <summary>
        /// 获取试题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="testPaperId">试卷ID</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<TestQuestionModel> GetQuestionList(string userId, string testPaperId, int pageSize, int pageIndex)
        {

            string sqlStr = String.Format(@"select 
                        P_Id as p_id,
                        P_QuestionStem as p_questionstem,
                        P_CreateTime
                        from P_TestQuestion
                        where P_Status !=1 and P_TestPaperId = '{0}'", testPaperId);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(pageSize, pageIndex, sqlStr.ToString(), " P_CreateTime asc"));

            DataSetToModelHelper<TestQuestionModel> helper = new DataSetToModelHelper<TestQuestionModel>();

            List<TestQuestionModel> list = helper.FillModel(ds);

            if (list != null)
            {
                foreach (TestQuestionModel item in list)
                {
                    List<AnswerModel>  answers = GetQuestionAnswer(item.p_id);

                    item.answers = answers == null ? new List<AnswerModel>() : answers;
                }
            }

            return list;
        }

        /// <summary>
        /// 获取试题答案
        /// </summary>
        /// <param name="question"></param>
        private List<AnswerModel> GetQuestionAnswer(string questionId)
        {

            string sqlStr = String.Format(@"select 
                        P_Id as p_id,
                        P_Sequence as p_sequence,
                        P_Choices as content,
                        P_Correct as is_answer
                        from P_TestList
                        where  P_TestQuestionId = '{0}'
                        order by P_sequence asc", questionId);

            DataSet ds = DbHelperSQL.Query(sqlStr.ToString());

            DataSetToModelHelper<AnswerModel> helper = new DataSetToModelHelper<AnswerModel>();

            return helper.FillModel(ds);
        }

        /// <summary>
        /// 提交试卷
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public int SubmitTestPagerAnswer(SubmitAnswerFromBody fromBody)
        {
            int score = 0;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string questionBankStr = String.Format(@"select P_TestPaper.P_QuestionBankId from P_TestPaper
                        where P_TestPaper.P_Id = '{0}'", fromBody.testpager_id);
                        // 获取试卷所属题库ID
                        string questionBankId = Convert.ToString(DbHelperSQL.GetSingle(questionBankStr.ToString()));
                        string mianId = Guid.NewGuid().ToString();
                        // 判断是否存在试题列表 不存在试题列表则不需要计算分数。分数为0
                        if (fromBody != null && fromBody.questions != null)
                        {
                            Task task1 = new Task(()=> {
                                // 获取分数
                                score = CalculateScore(fromBody.questions);
                            });

                            task1.Start();

                            // 遍历试题生成提交试题答案详情记录
                            foreach (Question item in fromBody.questions)
                            {
                                //  判断提交答案是否存在 存在则记录答案数据，不存在则不记录
                                if (item.answers != null && item.answers.Count > 0)
                                {
                                    // 循环记录提交的试题答案
                                    foreach (string answerItem in item.answers)
                                    {
                                        // 插入提交试题答案详情
                                        StringBuilder inserSql = new StringBuilder();
                                        inserSql.Append("insert into P_AnswerDetails (");
                                        inserSql.Append("P_Id,P_RecordId,P_TestQuestionId,P_UserAnswerId,P_CreateTime,P_CreateUser,P_Status");
                                        inserSql.Append(")");
                                        inserSql.Append(" values(");
                                        inserSql.Append("@P_Id,@P_RecordId,@P_TestQuestionId,@P_UserAnswerId,@P_CreateTime,@P_CreateUser,@P_Status)");
                                        SqlParameter[] parameters1 = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_RecordId", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_TestQuestionId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_UserAnswerId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,4),
                                        new SqlParameter("@P_Status", SqlDbType.Int,4),
                                        };
                                        parameters1[0].Value = Guid.NewGuid().ToString();
                                        parameters1[1].Value = mianId;
                                        parameters1[2].Value = item.question_id;
                                        parameters1[3].Value = answerItem;
                                        parameters1[4].Value = DateTime.Now;
                                        parameters1[5].Value = fromBody.userid;
                                        parameters1[6].Value = 0;
                                        DbHelperSQL.GetSingle(conn, trans, inserSql.ToString(), parameters1); //带事务
                                    }
                                }
                            }
                            Task.WaitAll(task1);
                        }
                        else
                        {
                            score = 0;
                        }

                        

                        // 插入提交试题答案详情
                        StringBuilder inserMainSql = new StringBuilder();
                        inserMainSql.Append("insert into P_AnswerSheetRecord (");
                        inserMainSql.Append("P_Id,P_UserId,P_QuestionBankId,P_TestPaperId,P_Score,P_Status,P_CreateTime,P_CreateUser");
                        inserMainSql.Append(")");
                        inserMainSql.Append(" values(");
                        inserMainSql.Append("@P_Id,@P_UserId,@P_QuestionBankId,@P_TestPaperId,@P_Score,@P_Status,@P_CreateTime,@P_CreateUser)");
                        SqlParameter[] parameters = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_UserId", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_QuestionBankId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_TestPaperId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_Score", SqlDbType.Int,100),
                                        new SqlParameter("@P_Status", SqlDbType.NVarChar,4),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime,4),
                                        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,4),
                                        };
                        parameters[0].Value = mianId;
                        parameters[1].Value = fromBody.userid;
                        parameters[2].Value = questionBankId;
                        parameters[3].Value = fromBody.testpager_id;
                        parameters[4].Value = score;
                        parameters[5].Value = 0;
                        parameters[6].Value = DateTime.Now;
                        parameters[7].Value = fromBody.userid;
                        DbHelperSQL.GetSingle(conn, trans, inserMainSql.ToString(), parameters); //带事务

                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return score;
        }

        /// <summary>
        /// 计算分数
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        private int CalculateScore(List<Question> questions)
        {
            int score = 0;
            foreach (Question item in questions)
            {
                if (item.answers != null && item.answers.Count > 0)
                {
                    string correctAnswerCountStr = String.Format(@"select count(P_Id) from P_TestList
                        where P_TestList.P_TestQuestionId = '{0}' and P_TestList.P_Correct = 1", item.question_id);
                    // 获取正确答案数量
                    int count = Convert.ToInt32(DbHelperSQL.GetSingle(correctAnswerCountStr));
                    // 判断正确答案与提交答案数量是否一致，相同继续判断提交答案是否是正确答案，不相同直接跳出验证下一道题
                    if (count != item.answers.Count)
                    {
                        continue;
                    }
                    // 提交答案是否正确的标识
                    Boolean isCorrect = true;
                    // 循环判断答案是否正确
                    foreach (string answerid in item.answers)
                    {
                        string answerExistStr = String.Format(@"select count(p_id) from P_TestList
                        where P_TestList.P_TestQuestionId = '{0}' 
                        and P_TestList.P_Id = '{1}'
                        and P_TestList.P_Correct = 1", item.question_id, answerid);
                        // 获取提交答案是否正确
                        Boolean isExists = DbHelperSQL.Exists(answerExistStr);
                        // 判断提交答案是否正确，不正确跳出。
                        if (!isExists)
                        {
                            // 修改提交答案是否正确的标识为 false
                            isCorrect = false;
                            break;
                        }
                    }
                    //  判断提交答案是否正确 正确累加分数
                    if (isCorrect)
                    {
                        string scoreSqlStr = String.Format(@"select P_TestQuestion.P_Score from P_TestQuestion where P_Id = '{0}'", item.question_id);
                        object scoreObject = DbHelperSQL.GetSingle(scoreSqlStr.ToString());
                        int questonScore = Convert.ToInt32(scoreObject == null ? "0" : scoreObject);
                        score += questonScore;
                    }

                }
                else
                {
                    continue;
                }

            }

            return score;
        }

        /// <summary>
        /// 获取用户答卷记录详情
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="testPaperId">试卷ID</param>
        /// <param name="pageSize">分页行数</param>
        /// <param name="pageIndex">分页页数</param>
        /// <returns></returns>
        public List<AnswerQuestionRecordModel> GetAnswerQuestionRecord(string userId, string testPaperId, int pageSize, int pageIndex) {

            string sqlStr = String.Format(@"select 
                        P_Id as p_id,
                        P_QuestionStem as p_questionstem,
                        P_CreateTime
                        from P_TestQuestion
                        where P_Status !=1 and P_TestPaperId = '{0}'", testPaperId);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(pageSize, pageIndex, sqlStr.ToString(), " P_CreateTime asc"));

            DataSetToModelHelper<AnswerQuestionRecordModel> helper = new DataSetToModelHelper<AnswerQuestionRecordModel>();

            List<AnswerQuestionRecordModel> list = helper.FillModel(ds);

            if (list != null)
            {
                foreach (AnswerQuestionRecordModel item in list)
                {
                    List<AnswerModel> answers = GetQuestionAnswer(item.p_id);

                    item.answers = answers == null ? new List<AnswerModel>() : answers;

                    List<SelectedAnswerModel> selectedAnswers = GetUserSelectedAnswers(userId, testPaperId, item.p_id);

                    item.selected_answers = selectedAnswers == null ? new List<SelectedAnswerModel>() : selectedAnswers;
                }
            }
            return list;
        }

        private List<SelectedAnswerModel> GetUserSelectedAnswers(string userId, string testPaperId, string questionId) {

            string sql = String.Format(@"select top 1 P_AnswerSheetRecord.P_Id from P_AnswerSheetRecord
                        where P_AnswerSheetRecord.P_TestPaperId = '{0}' and P_UserId = {1}
                        order by P_AnswerSheetRecord.P_CreateTime DESC", testPaperId, userId);

            string mainRecordId = Convert.ToString(DbHelperSQL.GetSingle(sql));

            if (!String.IsNullOrEmpty(mainRecordId))
            {
                string selectedStr = String.Format(@"select P_AnswerDetails.P_UserAnswerId as selected_id,
                        P_TestList.P_Sequence as selected_sequence from P_AnswerDetails
                        left join P_TestList on P_TestList.P_Id = P_AnswerDetails.P_UserAnswerId
                        where P_AnswerDetails.P_RecordId = '{0}'
                        and P_AnswerDetails.P_TestQuestionId = '{1}'", mainRecordId, questionId);

                DataSet ds =  DbHelperSQL.Query(selectedStr);

                DataSetToModelHelper<SelectedAnswerModel> helper = new DataSetToModelHelper<SelectedAnswerModel>();

                return helper.FillModel(ds);
            }
            else {
                return null;
            }

        }
        #endregion
        /// <summary>
        /// 在线学习的网址
        /// </summary>
        /// <returns></returns>
        public urlmodel GetUrl()
        {
            urlmodel um = new urlmodel();
            StringBuilder st = new StringBuilder();
            st.Append("SELECT P_LearnUrl from P_OnlineLearn ");
            DataSet ds = DbHelperSQL.Query(st.ToString());
            DataTable tb = ds.Tables[0];
            DataRow row = tb.Rows[0];
            um.learnurl = ds.Tables[0].Rows[0][0].ToString();

            return um;
        }


    }
}

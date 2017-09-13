using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class MyQuestion
    {
        /// <summary>
        /// 以答题的列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<MyQuestionList> GetMyQuestionList(int userid, int rows, int page)
        {
            List<MyQuestionList> model = new List<MyQuestionList>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" SELECT P_QuestionBank.P_QuestionBankName as questionname,P_TestPaper.P_TestPaperName as papername,P_Score as score, ");
            strsql.Append(" CONVERT(varchar(16),P_AnswerSheetRecord.P_CreateTime,120) as createtime,P_AnswerSheetRecord.P_Id as id ");
            strsql.Append(" from P_AnswerSheetRecord ");
            strsql.Append(" LEFT JOIN P_QuestionBank on P_AnswerSheetRecord.P_QuestionBankId=P_QuestionBank.P_Id ");
            strsql.Append(" LEFT JOIN P_TestPaper on P_TestPaper.P_Id=P_AnswerSheetRecord.P_TestPaperId ");
            strsql.Append(" where P_AnswerSheetRecord.P_UserId=" + userid + @" ");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_AnswerSheetRecord.P_CreateTime"));
            DataSetToModelHelper<MyQuestionList> mm = new DataSetToModelHelper<MyQuestionList>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                model = mm.FillModel(dt);
            }
            else
            {
                model = null;
            }
            return model;
        }
        public List<AnswerQuestionRecordModel> GetAnswerQuestionRecord(int userId, string id, int rows, int page)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_Id AS p_id, P_QuestionStem as p_questionstem,  P_CreateTime");
            strsql.Append("  from P_TestQuestion ");
            strsql.Append("  where P_Status !=1 and P_TestPaperId = ( select P_TestPaperId from P_AnswerSheetRecord  ");
            strsql.Append(" where P_AnswerSheetRecord.P_Id='" + id + @"') ");
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), " P_CreateTime asc"));
            DataSetToModelHelper<AnswerQuestionRecordModel> helper = new DataSetToModelHelper<AnswerQuestionRecordModel>();
            List<AnswerQuestionRecordModel> list = helper.FillModel(ds);
            if (list != null)
            {
                foreach (AnswerQuestionRecordModel item in list)
                {
                    List<AnswerModel> answers = GetQuestionAnswer(item.p_id);

                    item.answers = answers == null ? new List<AnswerModel>() : answers;

                    List<SelectedAnswerModel> selectedAnswers = GetUserSelectedAnswers(userId, id,item.p_id);

                    item.selected_answers = selectedAnswers == null ? new List<SelectedAnswerModel>() : selectedAnswers;
                }
            }
            return list;
        }
        private List<SelectedAnswerModel> GetUserSelectedAnswers(int userId,string id,string questionid)
        {
                string selectedStr = String.Format(@"select P_AnswerDetails.P_UserAnswerId as selected_id,
                        P_TestList.P_Sequence as selected_sequence from P_AnswerDetails
                        left join P_TestList on P_TestList.P_Id = P_AnswerDetails.P_UserAnswerId
                        where P_AnswerDetails.P_RecordId = '{0}'
                        and P_AnswerDetails.P_TestQuestionId = '{1}'", id, questionid);

                DataSet ds = DbHelperSQL.Query(selectedStr);

                DataSetToModelHelper<SelectedAnswerModel> helper = new DataSetToModelHelper<SelectedAnswerModel>();

                return helper.FillModel(ds);
        

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
    }
    public class MyQuestionList
    {
        /// <summary>
        /// 答题记录的id
        /// </summary>
        public string id { get; set; }

        public string questionname { get; set; }
        public string papername { get; set; }
        public string createtime { get; set; }
        public int score { get; set; }

    }
}

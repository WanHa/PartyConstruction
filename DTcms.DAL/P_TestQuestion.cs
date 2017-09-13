using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{/// <summary>
 /// 数据访问类:试题
 /// </summary>
    public partial class P_TestQuestion
    {
        //private enum Options
        //{
        //    A = 0,
        //    B = 1,
        //    C = 2,
        //    D = 3,
        //    E = 4,
        //    F = 5,
        //    G = 6,
        //    H = 7,
        //    I = 8,
        //};




        public P_TestQuestion() { }
        #region 基本方法================================

        /// <summary>
        /// 是否存在该记录（根据ID）
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_TestQuestion");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Model.P_TestQuestion model, string id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_TestQuestion(");
                        strSql.Append("P_Id,P_TestPaperId,P_QuestionStem,P_Type,P_Score,P_CreateTime,P_CreateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_TestPaperId,@P_QuestionStem,@P_Type,@P_Score,@P_CreateTime,@P_CreateUser,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_TestPaperId", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_QuestionStem", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_Type", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Score", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4)
                               };
                        parameters[0].Value = model.P_Id;
                        parameters[1].Value = model.P_TestPaperId;
                        parameters[2].Value = model.P_QuestionStem;
                        parameters[3].Value = model.P_Type;
                        parameters[4].Value = model.P_Score;
                        parameters[5].Value = model.P_CreateTime;
                        parameters[6].Value = model.P_CreateUser;
                        parameters[7].Value = model.P_Status;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务

                        if (model.questions_list != null && model.questions_list.Count > 0)
                        {
                            for (int i = 0; i < model.questions_list.Count; i++)
                            {
                                Model.P_TestList item = model.questions_list[i];
                                StringBuilder strSql2 = new StringBuilder();
                                strSql2.Append("insert into P_TestList(");
                                strSql2.Append("P_Id,P_TestQuestionId,P_Choices,P_Correct,P_CreateTime,P_CreateUser,P_Status,P_Sequence)");
                                strSql2.Append(" values (");
                                strSql2.Append("@P_Id,@P_TestQuestionId,@P_Choices,@P_Correct,@P_CreateTime,@P_CreateUser,@P_Status,@P_Sequence)");
                                strSql2.Append(";select @@IDENTITY");
                                SqlParameter[] parameters2 = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_TestQuestionId", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_Choices", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Correct", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Sequence", SqlDbType.NVarChar,10),
                               };
                                parameters2[0].Value = item.P_Id;
                                parameters2[1].Value = item.P_TestQuestionId;
                                parameters2[2].Value = item.P_Choices;
                                parameters2[3].Value = item.P_Correct;
                                parameters2[4].Value = item.P_CreateTime;
                                parameters2[5].Value = item.P_CreateUser;
                                parameters2[6].Value = item.P_Status;
                                parameters2[7].Value = item.P_Sequence;
                                object obj2 = DbHelperSQL.GetSingle(conn, trans, strSql2.ToString(), parameters2);
                            }
                        }

                        trans.Commit();
                    }

                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return model.P_Id;
        }
        #endregion

        /// <summary>
        ///  更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Update(Model.P_TestQuestion model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_TestQuestion set ");
                        strSql.Append("P_QuestionStem=@P_QuestionStem,");
                        strSql.Append("P_Type=@P_Type,");
                        strSql.Append("P_Score=@P_Score,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_QuestionStem", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Type", SqlDbType.NVarChar,300),
                                new SqlParameter("@P_Score", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50)};
                        parameters[0].Value = model.P_QuestionStem;
                        parameters[1].Value = model.P_Type;
                        parameters[2].Value = model.P_Score;
                        parameters[3].Value = model.P_UpdateTime;
                        parameters[4].Value = model.P_UpdateUser;
                        parameters[5].Value = model.P_Id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        //删除答案
                        StringBuilder strSql6 = new StringBuilder();
                        strSql6.Append("delete from P_TestList");
                        strSql6.Append(" where P_TestQuestionId=@P_TestQuestionId ");
                        SqlParameter[] parameters6 = {
                        new SqlParameter("@P_TestQuestionId", SqlDbType.NVarChar,50)};
                        parameters6[0].Value = model.P_Id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql6.ToString(), parameters6);

                        //更新答案
                        if (model.questions_list != null && model.questions_list.Count > 0)
                        {
                            for (int i = 0; i < model.questions_list.Count; i++)
                            {
                                Model.P_TestList item = model.questions_list[i];
                                StringBuilder strSql2 = new StringBuilder();
                                strSql2.Append("insert into P_TestList(");
                                strSql2.Append("P_Id,P_TestQuestionId,P_Choices,P_Correct,P_CreateTime,P_CreateUser,P_Status,P_Sequence)");
                                strSql2.Append(" values (");
                                strSql2.Append("@P_Id,@P_TestQuestionId,@P_Choices,@P_Correct,@P_CreateTime,@P_CreateUser,@P_Status,@P_Sequence)");
                                strSql2.Append(";select @@IDENTITY");
                                SqlParameter[] parameters2 = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_TestQuestionId", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_Choices", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Correct", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Sequence", SqlDbType.NVarChar,10),
                               };
                                parameters2[0].Value = item.P_Id;
                                parameters2[1].Value = item.P_TestQuestionId;
                                parameters2[2].Value = item.P_Choices;
                                parameters2[3].Value = item.P_Correct;
                                parameters2[4].Value = item.P_CreateTime;
                                parameters2[5].Value = item.P_CreateUser;
                                parameters2[6].Value = item.P_Status;
                                parameters2[7].Value = item.P_Sequence;
                                object obj2 = DbHelperSQL.GetSingle(conn, trans, strSql2.ToString(), parameters2);
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }


        #region 删除一条数据================================
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string userid)
        {

            Model.P_TestQuestion oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_TestQuestion set ");
                        strSql.Append("P_Status=@P_Status,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = 1;
                        parameters[1].Value = DateTime.Now;
                        parameters[2].Value = userid;
                        parameters[3].Value = id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append("update P_TestList set ");
                        strSql2.Append("P_Status=@P_Status,");
                        strSql2.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql2.Append("P_UpdateUser=@P_UpdateUser");
                        strSql2.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters2 = {
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters2[0].Value = 1;
                        parameters2[1].Value = DateTime.Now;
                        parameters2[2].Value = userid;
                        parameters2[3].Value = id;

                        DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                        trans.Commit();
                    }
                    catch (Exception esss)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region 得到对象实体================================
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.P_TestQuestion GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT P_Id, P_TestPaperId, P_QuestionStem,P_Type,P_Score,P_CreateTime, P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status FROM P_TestQuestion");
            strSql.Append(" where P_Id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = id;

            Model.channel model = new Model.channel();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("select P_Id,P_TestQuestionId,P_Choices,P_Correct,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_Sequence from P_TestList");
            strSql1.Append(" where P_TestQuestionId = @id order by P_Sequence asc");
            SqlParameter[] parameters1 = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters1[0].Value = id;

            DataSet questions = DbHelperSQL.Query(strSql1.ToString(), parameters1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0], questions.Tables[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion


        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM P_TestQuestion  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where P_Status = 0 and " + strWhere);
            }
            else
            {
                strSql.Append(" where P_Status = 0");
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        




        #region 扩展方法================================
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.P_TestQuestion DataRowToModel(DataRow row, DataTable questions)
        {
            Model.P_TestQuestion model = new Model.P_TestQuestion();
            if (row != null)
            {
                #region 主表信息======================
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_TestPaperId"] != null && row["P_TestPaperId"].ToString() != "")
                {
                    model.P_TestPaperId = row["P_TestPaperId"].ToString();
                }
                if (row["P_QuestionStem"] != null && row["P_QuestionStem"].ToString() != "")
                {
                    model.P_QuestionStem = row["P_QuestionStem"].ToString();
                }
                if (row["P_Type"] != null && row["P_Type"].ToString() != "")
                {
                    model.P_Type = row["P_Type"].ToString();
                }
                if (row["P_Score"] != null && row["P_Score"].ToString() != "")
                {
                    model.P_Score = int.Parse(row["P_Score"].ToString());
                }
                if (row["P_CreateTime"] != null)
                {
                    model.P_CreateTime = Convert.ToDateTime(row["P_CreateTime"].ToString());
                }
                if (row["P_CreateUser"] != null)
                {
                    model.P_CreateUser = row["P_CreateUser"].ToString();
                }
                if (row["P_UpdateTime"] != null && row["P_UpdateTime"].ToString() != "")
                {
                    model.P_UpdateTime = Convert.ToDateTime(row["P_UpdateTime"].ToString());
                }
                if (row["P_UpdateUser"] != null && row["P_UpdateUser"].ToString() != "")
                {
                    model.P_UpdateUser = row["P_UpdateUser"].ToString();
                }
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = Convert.ToInt32(row["P_Status"]);
                }

                if (questions != null &questions.Rows.Count > 0) {
                    List<P_TestList> data = new List<P_TestList>();
                    for (int i = 0; i < questions.Rows.Count; i++)
                    {
                        DataRow dr = questions.Rows[i];
                        P_TestList item = new P_TestList();
                        if (dr["P_Id"] != null && dr["P_Id"].ToString() != "") {
                            item.P_Id = dr["P_Id"].ToString();
                        }
                        if (dr["P_TestQuestionId"] != null && dr["P_TestQuestionId"].ToString() != "")
                        {
                            item.P_TestQuestionId = dr["P_TestQuestionId"].ToString();
                        }
                        if (dr["P_Choices"] != null && dr["P_Choices"].ToString() != "")
                        {
                            item.P_Choices = dr["P_Choices"].ToString();
                        }
                        if (dr["P_Correct"] != null && dr["P_Correct"].ToString() != "")
                        {
                            item.P_Correct = int.Parse(dr["P_Correct"].ToString());
                        }
                        if (dr["P_CreateTime"] != null && dr["P_CreateTime"].ToString() != "")
                        {
                            item.P_CreateTime = DateTime.Parse(dr["P_CreateTime"].ToString());
                        }
                        if (dr["P_CreateUser"] != null && dr["P_CreateUser"].ToString() != "")
                        {
                            item.P_CreateUser =  dr["P_CreateUser"].ToString();
                        }
                        if (dr["P_UpdateTime"] != null && dr["P_UpdateTime"].ToString() != "")
                        {
                            item.P_UpdateTime = DateTime.Parse(dr["P_UpdateTime"].ToString());
                        }
                        if (dr["P_UpdateUser"] != null && dr["P_UpdateUser"].ToString() != "")
                        {
                            item.P_UpdateUser = dr["P_UpdateUser"].ToString();
                        }
                        if (dr["P_Status"] != null && dr["P_Status"].ToString() != "")
                        {
                            item.P_Status = int.Parse(dr["P_Status"].ToString());
                        }
                        if (dr["P_Sequence"] != null && dr["P_Sequence"].ToString() != "")
                        {
                            item.P_Sequence =  dr["P_Sequence"].ToString();
                        }
                        data.Add(item);
                    }
                    model.questions_list = data;
                }
                #endregion
            }
            return model;
        }

        #endregion




















    }
}

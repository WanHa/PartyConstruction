using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// 数据访问类:试卷
    /// </summary>
    public partial class P_TestPaper
    {
        public P_TestPaper()
        {  }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_TestPaper");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Model.P_TestPaper model,string parentid)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_TestPaper(");
                        strSql.Append("P_Id,P_QuestionBankId,P_TestPaperName,P_Description,P_AnswerTime,P_CreateTime,P_CreateUser,P_Status,P_IsRepeat)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_QuestionBankId,@P_TestPaperName,@P_Description,@P_AnswerTime,@P_CreateTime,@P_CreateUser,@P_Status,@P_IsRepeat)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_QuestionBankId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_TestPaperName", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Description", SqlDbType.NVarChar,300),
                                new SqlParameter("@P_AnswerTime", SqlDbType.Int,100),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status", SqlDbType.Int),
                                new SqlParameter("@P_IsRepeat", SqlDbType.Int)};
                        parameters[0].Value = model.P_Id;
                        parameters[1].Value = parentid;
                        parameters[2].Value = model.P_TestPaperName;
                        parameters[3].Value = model.P_Description;
                        parameters[4].Value = model.P_AnswerTime;
                        parameters[5].Value = model.P_CreateTime;
                        parameters[6].Value = model.P_CreateUser;
                        parameters[7].Value = model.P_Status;
                        parameters[8].Value = model.P_IsRepeat;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
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


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.P_TestPaper model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_TestPaper set ");
                        strSql.Append("P_TestPaperName=@P_TestPaperName,");
                        strSql.Append("P_Description=@P_Description,");
                        strSql.Append("P_AnswerTime=@P_AnswerTime,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser,");
                        strSql.Append("P_IsRepeat=@P_IsRepeat");
                        strSql.Append(" where P_Id=@P_Id");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_TestPaperName", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Description", SqlDbType.NVarChar,300),
                                new SqlParameter("@P_AnswerTime", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_IsRepeat", SqlDbType.Int,4),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50)};
                        parameters[0].Value = model.P_TestPaperName;
                        parameters[1].Value = model.P_Description;
                        parameters[2].Value = model.P_AnswerTime;
                        parameters[3].Value = model.P_UpdateTime;
                        parameters[4].Value = model.P_UpdateUser;
                        parameters[5].Value = model.P_IsRepeat;
                        parameters[6].Value = model.P_Id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch(Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string UserId)
        {

            Model.P_TestPaper oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_TestPaper set ");
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
                        parameters[2].Value = UserId;
                        parameters[3].Value = id;

                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.P_TestPaper GetModel(string P_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_Id,P_QuestionBankId, P_TestPaperName,P_Description,P_AnswerTime,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_IsRepeat");
            strSql.Append(" from P_TestPaper");
            strSql.Append(" where P_Id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = P_Id;

            Model.P_TestPaper model = new Model.P_TestPaper();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM P_TestPaper  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append("where " + strWhere);
            }
            else
            {
                strSql.Append("where P_Status = 0");
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }


        /// <summary>
        /// 将对象转换为实体
        /// </summary>
        private Model.P_TestPaper DataRowToModel(DataRow row)
        {
            Model.P_TestPaper model = new Model.P_TestPaper();
            if (row != null)
            {
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }

                if (row["P_QuestionBankId"] != null && row["P_QuestionBankId"].ToString() != "")
                {
                    model.P_QuestionBankId = row["P_QuestionBankId"].ToString();
                }
                if (row["P_TestPaperName"] != null)
                {
                    model.P_TestPaperName = row["P_TestPaperName"].ToString();
                }
                if (row["P_Description"] != null)
                {
                    model.P_Description = row["P_Description"].ToString();
                }
                if (row["P_AnswerTime"] != null)
                {
                    model.P_AnswerTime = int.Parse(row["P_AnswerTime"].ToString());
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
                if (row["P_UpdateUser"] != null)
                {
                    model.P_UpdateUser = row["P_UpdateUser"].ToString();
                }
                if (row["P_Status"] != null)
                {
                    model.P_Status = Convert.ToInt32(row["P_Status"]);
                }
                if (row["P_IsRepeat"] != null)
                {
                    model.P_IsRepeat = Convert.ToInt32(row["P_IsRepeat"]);
                }
            }
            return model;
        }
    }
}



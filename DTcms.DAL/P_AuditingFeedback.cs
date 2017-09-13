using DTcms.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DTcms.DAL
{
    public partial class P_AuditingFeedback
    {
        public string Add(Model.P_AuditingFeedback model, string id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("Insert into P_AuditingFeedback(");
                        strSql.Append("P_Id,P_LogId,P_AuditContent,P_CreateTime,P_CreateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_LogId,@P_AuditContent,@P_CreateTime,@P_CreateUser,@P_Status)");
                        //strSql.Append("");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_LogId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_AuditContent",SqlDbType.NText),
                                new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status",SqlDbType.Int),
                        };
                        parameters[0].Value = model.P_Id;
                        parameters[1].Value = id;
                        parameters[2].Value = model.P_AuditContent;
                        parameters[3].Value = model.P_CreateTime;
                        parameters[4].Value = model.P_CreateTime;
                        parameters[5].Value = 0;
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
    }
}
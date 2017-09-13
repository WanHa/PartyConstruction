using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{
    public partial class P_PartyPayMent
    {
        public P_PartyPayMent() { }

        #region 基本方法================================

        /// <summary>
		/// 是否存在该记录（根据ID）
		/// </summary>
		public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_PartyPayMent");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Model.P_PartyPayMent model, List<Model.P_PartyPayMentPeople> peopleList)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if(model.P_PayMentState==0)
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.Append("select P_PayMentState from P_PartyPayMent where P_Status = 0");
                            DataSet ds = DbHelperSQL.Query(sql.ToString());
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(row["P_PayMentState"]) == 0)
                                {
                                    return null;
                                }
                            }
                        }
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_PartyPayMent(");
                        strSql.Append("P_Id,P_Title,P_PayMentState,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_Title,@P_PayMentState,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Title", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_PayMentState", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,100)
                               };
                        parameters[0].Value = model.P_Id;
                        parameters[1].Value = model.P_Title;
                        parameters[2].Value = model.P_PayMentState;
                        parameters[3].Value = model.P_CreateTime;
                        parameters[4].Value = model.P_CreateUser;
                        parameters[5].Value = model.P_UpdateTime;
                        parameters[6].Value = model.P_UpdateUser;
                        parameters[7].Value = model.P_Status;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务

                        foreach (Model.P_PartyPayMentPeople item in peopleList)
                        {                            
                            string str = "select id from dt_users where mobile = '"+item.P_Tel+@"'";
                            DataSet das = DbHelperSQL.Query(str);
                            if (das.Tables[0].Rows.Count > 0)
                            {
                                string userid = das.Tables[0].Rows[0]["id"].ToString();
                                StringBuilder strSql1 = new StringBuilder();
                                strSql1.Append("insert into P_PartyPayMentPeople(");
                                strSql1.Append("P_ID,P_UserID,P_Tel,P_Money,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_PayMentId)");
                                strSql1.Append(" values (");
                                strSql1.Append("@P_ID,@P_UserID,@P_Tel,@P_Money,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_PayMentId)");
                                strSql1.Append(";select @@IDENTITY");
                                SqlParameter[] parameters1 = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Tel",SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Money", SqlDbType.Decimal),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,100),
                                new SqlParameter("@P_PayMentId", SqlDbType.NVarChar,100),
                               };
                                parameters1[0].Value = item.P_ID;
                                parameters1[1].Value = userid;
                                parameters1[2].Value = item.P_Tel;
                                parameters1[3].Value = item.P_Money;
                                parameters1[4].Value = item.P_CreateTime;
                                parameters1[5].Value = item.P_CreateUser;
                                parameters1[6].Value = item.P_UpdateTime;
                                parameters1[7].Value = item.P_UpdateUser;
                                parameters1[8].Value = item.P_Status;
                                parameters1[9].Value = model.P_Id;
                                object obj1 = DbHelperSQL.GetSingle(conn, trans, strSql1.ToString(), parameters1); //带事务
                            }
                            else
                            {
                                return null;
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

        /// <summary>
        /// 更新一条数据
        /// </summary>s
        public bool Update(Model.P_PartyPayMent model)
        {
            Model.P_PartyPayMent oldModel = GetModel(model.P_Id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if(model.P_PayMentState==0)
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.Append("select P_PayMentState from P_PartyPayMent");
                            DataSet ds = DbHelperSQL.Query(sql.ToString());
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(row["P_PayMentState"]) == 0)
                                {
                                    return false;
                                }
                            }
                        }
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_PartyPayMent set ");
                        strSql.Append("P_Title=@P_Title,");
                        strSql.Append("P_PayMentState=@P_PayMentState,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Title", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_PayMentState", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = model.P_Title;
                        parameters[1].Value = model.P_PayMentState;
                        parameters[2].Value = model.P_UpdateTime;
                        parameters[3].Value = model.P_UpdateUser;
                        parameters[4].Value = model.P_Id;
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string userid)
        {

            Model.P_PartyPayMent oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_PartyPayMent set ");
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
                        trans.Commit();
                    }
                    catch (Exception es)
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
        public Model.P_PartyPayMent GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT P_Id, P_Title, P_CreateTime, P_CreateUser,P_UpdateTime,P_UpdateUser,P_PayMentState,P_Status FROM P_PartyPayMent");
            strSql.Append(" where P_Id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = id;

            Model.channel model = new Model.channel();
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
            strSql.Append("select * FROM P_PartyPayMent  ");
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

        #endregion

        #region 扩展方法================================
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.P_PartyPayMent DataRowToModel(DataRow row)
        {
            Model.P_PartyPayMent model = new Model.P_PartyPayMent();
            if (row != null)
            {
                #region 主表信息======================
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_Title"] != null && row["P_Title"].ToString() != "")
                {
                    model.P_Title = row["P_Title"].ToString();
                }
                if (row["P_CreateTime"] != null)
                {
                    model.P_CreateTime = Convert.ToDateTime(row["P_CreateTime"].ToString());
                }
                if (row["P_CreateUser"] != null)
                {
                    model.P_CreateUser = row["P_CreateUser"].ToString();
                }
                //if (row["P_UpdateTime"] != null)
                //{
                //    model.P_UpdateTime = Convert.ToDateTime(row["P_UpdateTime"].ToString());
                //}
                //if (row["P_UpdateUser"] != null && row["P_UpdateUser"].ToString() != "")
                //{
                //    model.P_UpdateUser = row["P_UpdateUser"].ToString();
                //}
                if (row["P_PayMentState"] != null && row["P_PayMentState"].ToString() != "")
                {
                    model.P_PayMentState = Convert.ToInt32(row["P_PayMentState"]);
                }
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = Convert.ToInt32(row["P_Status"]);
                }
                #endregion
            }
            return model;
        }
        #endregion
    }
}

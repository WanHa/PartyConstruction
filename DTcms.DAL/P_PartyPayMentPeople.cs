using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public partial class P_PartyPayMentPeople
    {
        public P_PartyPayMentPeople() { }

        #region 基本方法================================

        /// <summary>
		/// 是否存在该记录（根据ID）
		/// </summary>
		public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_PartyPayMentPeople");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Model.P_PartyPayMentPeople model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_PartyPayMentPeople(");
                        strSql.Append("P_ID,P_UserID,P_Tel,P_Money,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_ID,@P_UserID,@P_Tel,@P_Money,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Tel",SqlDbType.Int,10),
                                new SqlParameter("@P_Money", SqlDbType.Money),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,100)
                               };
                        parameters[0].Value = model.P_ID;
                        parameters[1].Value = model.P_UserID;
                        parameters[2].Value = model.P_Tel;
                        parameters[3].Value = model.P_Money;
                        parameters[4].Value = model.P_CreateTime;
                        parameters[5].Value = model.P_CreateUser;
                        parameters[6].Value = model.P_UpdateTime;
                        parameters[7].Value = model.P_UpdateUser;
                        parameters[8].Value = model.P_Status;
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
            return model.P_ID;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.P_PartyPayMentPeople model)
        {
            Model.P_PartyPayMentPeople oldModel = GetModel(model.P_ID); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_Status set ");
                        strSql.Append("P_UserID=@P_UserID,");
                        strSql.Append("P_Status=@P_Status,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_UserID", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = model.P_UserID;
                        parameters[1].Value = model.P_Status;
                        parameters[2].Value = model.P_UpdateTime;
                        parameters[3].Value = model.P_UpdateUser;
                        parameters[4].Value = model.P_ID;

                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string userid)
        {

            Model.P_PartyPayMentPeople oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_PartyPayMentPeople set ");
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
                    catch (Exception esss)
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

        public Model.P_PartyPayMentPeople GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT P_Id, P_UserID,P_Tel,P_Money,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status FROM P_PartyPayMentPeople");
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
        public List<Model.P_PartyPayMentPeople> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,");
            strSql.Append("(select P_Status from P_PartyPayMentRecord ");
            strSql.Append(" where P_PartyPayMentRecord.P_OutTradeNo =P_PartyPayMentPeople.P_ID)as a ");
            strSql.Append(" FROM P_PartyPayMentPeople");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where P_Status = 0 and " + strWhere);
            }
            else
            {
                strSql.Append(" where P_Status = 0");
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            DataSet ds = DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
            return DetailListModel(ds.Tables[0]);
        }
        /// <summary>
        /// 将datatable转换为list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Model.P_PartyPayMentPeople> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<Model.P_PartyPayMentPeople> list = new List<Model.P_PartyPayMentPeople>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataRowToModel(row));
            }
            return list;
        }
        public Model.P_PartyPayMentPeople DetailModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            Model.P_PartyPayMentPeople model = new Model.P_PartyPayMentPeople();
            foreach (DataRow row in table.Rows)
            {
                model=DataRowToModel(row);
            }

            return model;
        }
        #endregion
        #region 扩展方法================================
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.P_PartyPayMentPeople DataRowToModel(DataRow row)
        {
            Model.P_PartyPayMentPeople model = new Model.P_PartyPayMentPeople();
            if (row != null)
            {
                #region 主表信息======================
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_ID = row["P_ID"].ToString();
                }
                if (row["P_UserID"] != null && row["P_UserID"].ToString() != "")
                {
                    model.P_UserID = row["P_UserId"].ToString();
                }
                if (row["P_Money"] != null && row["P_Money"].ToString() != "")
                {
                    model.P_Money = Convert.ToDecimal(row["P_Money"]);
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
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = Convert.ToInt32(row["P_Status"]);
                }
                if (string.IsNullOrEmpty((row["a"]).ToString())||Convert.ToInt32(row["a"])==0)
                {
                    model.PayStatus = "未支付";
                }
                else if (Convert.ToInt32((row["a"])) == 1)
                {
                    model.PayStatus = "已支付";
                }
                #endregion
            }
            return model;
        }

        #endregion
    }
}

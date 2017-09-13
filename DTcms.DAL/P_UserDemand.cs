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
    public partial class P_UserDemand
    {
        public P_UserDemand() { }

        /// <summary>
		/// 是否存在该记录（根据ID）
		/// </summary>
		public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_UserDemand");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        //添加
        public string Add(Model.P_UserDemand model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_UserDemand(");
                        strSql.Append("P_Id,P_Content,P_CheckStatus,P_CreateTime,P_CreateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_Content,@P_CheckStatus,@P_CreateTime,@P_CreateUser,P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_CheckStatus", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.NVarChar,100)
                               };
                        parameters[0].Value = model.P_Id;
                        parameters[1].Value = model.P_Content;
                        parameters[2].Value = model.P_CheckStatus;
                        parameters[3].Value = model.P_CreateTime;
                        parameters[4].Value = model.P_CreateUser;
                        parameters[7].Value = model.P_Status;
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
        public bool Update(Model.P_UserDemand model)
        {
            Model.P_UserDemand oldModel = GetModel(model.P_Id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_UserDemand set ");
                        strSql.Append("P_Content=@P_Content,");
                        strSql.Append("P_CheckStatus=@P_CheckStatus,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Content", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_CheckStatus", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = model.P_Content;
                        parameters[1].Value = model.P_CheckStatus;
                        parameters[2].Value = model.P_UpdateTime;
                        parameters[3].Value = model.P_UpdateUser;
                        parameters[4].Value = model.P_Id;

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

            Model.P_UserDemand oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_UserDemand set ");
                        strSql.Append("P_Status=@P_Status");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = 1;
                        parameters[1].Value = id;

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
        public Model.P_UserDemand GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_Id, P_Content, P_CreateTime, P_CreateUser,P_CheckStatus,P_Status,P_UpdateTime,P_UpdateUser,user_name as Username,");
            strSql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as sum");
            strSql.Append(" from P_UserDemand LEFT JOIN dt_users on P_UserDemand.P_CreateUser =dt_users.id");
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
        public List<Model.P_UserDemand> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            //select *,(select user_name from dt_users where dt_users.id = P_UserDemand.P_CreateUser) as username FROM P_UserDemand
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (select dt_users.user_name as Username,P_UserDemand.P_CheckStatus,P_UserDemand.P_CreateTime,P_UserDemand.P_CreateUser,P_UserDemand.P_Status,");
            strSql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as Sum,P_UserDemand.P_Id,P_UserDemand.P_Content,");
            strSql.Append(" P_UserDemand.P_UpdateTime,P_UserDemand.P_UpdateUser from P_UserDemand LEFT JOIN dt_users on P_UserDemand.P_CreateUser = dt_users.id");
            strSql.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id =(select TOP 1 t.value from F_Split(");
            strSql.Append(" (select dt_users.group_id from dt_users where dt_users.id = P_UserDemand.P_CreateUser),',') as t ");
            strSql.Append(" left join dt_user_groups on dt_user_groups.id = t.value where t.value != '' order by dt_user_groups.grade DESC)) as table1");
            strSql.Append(" where (table1.sum = 0 or table1.sum = 2) ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and table1." + strWhere);
            }
            //else
            //{
            //    strSql.Append("");
            //}
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            DataSet ds= DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
            return DetailListModel(ds.Tables[0]);
        }

        public List<Model.P_UserDemand> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<Model.P_UserDemand> list = new List<Model.P_UserDemand>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataRowToModel(row));
            }
            return list;
        }

        #region 扩展方法================================
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.P_UserDemand DataRowToModel(DataRow row)
        {
            Model.P_UserDemand model = new Model.P_UserDemand();
            if (row != null)
            {
                #region 主表信息======================
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_Content"] != null && row["P_Content"].ToString() != "")
                {
                    model.P_Content = row["P_Content"].ToString();
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
                if (row["P_CheckStatus"] != null && row["P_CheckStatus"].ToString() != "")
                {
                    model.P_CheckStatus = Convert.ToInt32(row["P_CheckStatus"]);
                }
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = Convert.ToInt32(row["P_Status"]);
                }
                if(Convert.ToInt32(row["Sum"]) != 0)
                {
                    model.Sum = Convert.ToInt32(row["Sum"]);
                }
                if (row["Username"] != null)
                {
                    model.Username = row["Username"].ToString();
                }
                //if ((Convert.ToInt32(row["P_CheckStatus"]) == -1 && Convert.ToInt32(row["Sum"])==0)|| (Convert.ToInt32(row["P_CheckStatus"]) == 1 && Convert.ToInt32(row["Sum"]) == 2))
                //{
                //    model.StatusName = "处理中";
                //}
                //if((Convert.ToInt32(row["P_CheckStatus"]) == 0 && Convert.ToInt32(row["Sum"]) == 1)|| (Convert.ToInt32(row["P_CheckStatus"]) == -1 && Convert.ToInt32(row["Sum"]) == 1)|| (Convert.ToInt32(row["P_CheckStatus"]) == 1 && Convert.ToInt32(row["Sum"]) == 3))
                //{
                //    model.StatusName = "已办结";
                //}
                //if((Convert.ToInt32(row["Sum"]) == 1)|| (Convert.ToInt32(row["Sum"]) == 3))
                //{
                //    model.ReplyStatus = "已回复";
                //}
                //else
                //{
                //    model.ReplyStatus = "回复";
                //}
                #endregion
            }
            return model;
        }

        #endregion
    }
}

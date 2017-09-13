using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public partial class signlnStatistics
    {
        public signlnStatistics()
        { }
        UserGroupHelper group = new UserGroupHelper();
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_MeetingAdminSublist");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.P_MeetingAdminSublist GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_ID,P_Title, P_MeeID,P_SigninInfo,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status");
            strSql.Append(" from P_MeetingAdminSublist");
            strSql.Append(" where P_Id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;

            Model.P_MeetingAdminSublist model = new Model.P_MeetingAdminSublist();
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
            UserGroupHelper getgroup = new UserGroupHelper();
            StringBuilder str = new StringBuilder();
            //string a = getgroup.GetUserMinimumGroupId();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_MeetingAdminSublist.*,(select USER_NAME from dt_users where dt_users.id=P_MeetingAdminSublist.P_UserId) as user_name , ");
            strSql.Append(@"(select TOP 1 dt_user_groups.title from F_Split(
                                         (select dt_users.group_id from dt_users where dt_users.id = P_MeetingAdminSublist.P_UserId), ',') as t
                                         left join dt_user_groups on dt_user_groups.id = t.value
                                         where t.value != ' '  ORDER BY dt_user_groups.grade DESC) as title ");
            strSql.Append(" from P_MeetingAdminSublist left join dt_users on dt_users.id = P_MeetingAdminSublist.P_UserId ");            

            if (strWhere.Trim() != "")
            {
                strSql.Append("where" + strWhere);
            }
            else
            {
                strSql.Append("where P_Status = 0");
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        private Model.P_MeetingAdminSublist DataRowToModel(DataRow row)
        {
            Model.P_MeetingAdminSublist model = new Model.P_MeetingAdminSublist();
            if (row != null)
            {
                if (row["P_ID"] != null && row["P_ID"].ToString() != "")
                {
                    model.P_ID = row["P_ID"].ToString();
                }

                if (row["P_Title"] != null && row["P_Title"].ToString() != "")
                {
                    model.P_Title = row["P_Title"].ToString();
                }
                if (row["P_MeeID"] != null)
                {
                    model.P_MeeID = row["P_MeeID"].ToString();
                }
                if (row["P_SigninIfo"] != null)
                {
                    model.P_SigninInfo = Convert.ToInt32(row["P_SigninIfo"]);
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
            }
            return model;
        }

        public bool UpdateType(string id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_MeetingAdminSublist set ");
                        strSql.Append("P_SigninInfo=1");
                        strSql.Append(" where P_Id='"+id+"'");

                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
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


        public bool PhoneSingIn(string P_MeeID, string userId)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                bool exist = isexist(P_MeeID, userId);

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {  
                    try
                    {
                        if (exist)
                        {
                            //更新
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("update  P_MeetingAdminSublist set ");
                            strSql.Append("P_SigninInfo=@P_SigninInfo,P_UpdateTime=@P_UpdateTime,P_UpdateUser=@P_UpdateUser ");
                            strSql.Append(" where ");
                            strSql.Append(" P_MeeID=@P_MeeID and P_UserId=@P_UserId ");
                            strSql.Append(" P_Status=0 ");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_SigninInfo", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_MeeID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,100)
                             };
                            parameters[0].Value = 1;
                            parameters[1].Value = DateTime.Now;
                            parameters[2].Value = userId;
                            parameters[3].Value = P_MeeID;
                            parameters[4].Value = userId;

                            DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                           
                        }
                        else
                        {
                            //插入
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("insert into P_MeetingAdminSublist(");
                            strSql.Append("P_ID,P_MeeID,P_UserId,P_SigninInfo,P_CreateTime,P_CreateUser,P_Status)");
                            strSql.Append(" values (");
                            strSql.Append("@P_ID,@P_MeeID,@P_UserId,@P_SigninInfo,@P_CreateTime,@P_CreateUser,@P_Status)");

                            SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_MeeID", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_SigninInfo", SqlDbType.Int),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int)
                            };

                            parameters[0].Value =Guid.NewGuid().ToString();
                            parameters[1].Value = P_MeeID;
                            parameters[2].Value = userId;
                            parameters[3].Value = 1;
                            parameters[4].Value = DateTime.Now;
                            parameters[5].Value = userId;
                            parameters[6].Value = 0;

                            DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        private bool isexist(string P_MeeID, string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_MeetingAdminSublist");
            strSql.Append(" where P_MeeID='" + P_MeeID + "' and P_Status != 1");
            strSql.Append(" and P_UserId='"+ userId + "'");
            return DbHelperSQL.Exists(strSql.ToString());
        }
    }
}

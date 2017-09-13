using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTcms.Model;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// 数据访问类：会议管理
    /// </summary>
    public partial class P_MeetingAdmin
    {
        public P_MeetingAdmin()
        { }

        /// <summary>
        /// 是否存在该纪录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_MeetingAdmin ");
            strSql.Append("where P_Id=@id ");
            SqlParameter[] parameters = {
                new SqlParameter("@id",SqlDbType.NVarChar,50)};
            parameters[0].Value = id;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(Model.P_MeetingAdmin model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_MeetingAdmin(");
                        strSql.Append("P_Id,P_Title,P_MeeContent,P_StartTime,P_EndTime,P_MeePlace,P_PeopleAmount,P_CreateTime,P_CreateUser,P_Status,UserId) ");
                        strSql.Append("values(");
                        strSql.Append("@P_Id,@P_Title,@P_MeeContent,@P_StartTime,@P_EndTime,@P_MeePlace,@P_PeopleAmount,@P_CreateTime,@P_CreateUser,@P_Status,@UserId)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Title",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_MeeContent",SqlDbType.NText),
                            new SqlParameter("@P_StartTime",SqlDbType.DateTime,100),
                            new SqlParameter("@P_EndTime",SqlDbType.DateTime,100),
                            new SqlParameter("@P_MeePlace",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_PeopleAmount",SqlDbType.Int,4),
                            new SqlParameter("@P_CreateTime",SqlDbType.DateTime,100),
                            new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Status",SqlDbType.Int,4),
                            new SqlParameter("@UserId",SqlDbType.NText),
                        };
                        parameters[0].Value = model.P_Id;
                        parameters[1].Value = model.P_Title;
                        parameters[2].Value = model.P_MeeContent;
                        parameters[3].Value = model.P_StartTime;
                        parameters[4].Value = model.P_EndTime;
                        parameters[5].Value = model.P_MeePlace;
                        parameters[6].Value = model.P_PeopleAmount;
                        parameters[7].Value = model.P_CreateTime;
                        parameters[8].Value = model.P_CreateUser;
                        parameters[9].Value = model.P_Status;
                        parameters[10].Value = model.UserId;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
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
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(Model.P_MeetingAdmin model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_MeetingAdmin set ");
                        strSql.Append("P_Title=@P_Title,");
                        strSql.Append("P_MeeContent=@P_MeeContent,");
                        strSql.Append("P_StartTime=@P_StartTime,");
                        strSql.Append("P_EndTime=@P_EndTime,");
                        strSql.Append("P_MeePlace=@P_MeePlace,");
                        strSql.Append("UserId=@UserId,");
                        strSql.Append("P_PeopleAmount=@P_PeopleAmount,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id");
                        SqlParameter[] parameters ={
                          new SqlParameter("@P_Title",SqlDbType.NVarChar,100),
                          new SqlParameter("@P_MeeContent",SqlDbType.NText),
                          new SqlParameter("@P_StartTime",SqlDbType.DateTime,100),
                          new SqlParameter("@P_EndTime",SqlDbType.DateTime,100),
                          new SqlParameter("@P_MeePlace",SqlDbType.NVarChar,100),
                          new SqlParameter("@UserId",SqlDbType.NText),
                          new SqlParameter("@P_PeopleAmount",SqlDbType.Int,4),
                          new SqlParameter("@P_UpdateTime",SqlDbType.DateTime,100),
                          new SqlParameter("@P_UpdateUser",SqlDbType.NVarChar,50),
                          new SqlParameter("@P_Id",SqlDbType.NVarChar,50)};
                        parameters[0].Value = model.P_Title;
                        parameters[1].Value = model.P_MeeContent;
                        parameters[2].Value = model.P_StartTime;
                        parameters[3].Value = model.P_EndTime;
                        parameters[4].Value = model.P_MeePlace;
                        parameters[5].Value = model.UserId;
                        parameters[6].Value = model.P_PeopleAmount;
                        parameters[7].Value = model.P_UpdateTime;
                        parameters[8].Value = model.P_UpdateUser;
                        parameters[9].Value = model.P_Id;
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
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool Delete(string id, string UserId)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_MeetingAdmin set ");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser,");
                        strSql.Append("P_Status=@P_Status");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = DateTime.Now;
                        parameters[1].Value = UserId;
                        parameters[2].Value = 1;
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
        /// 得到对象实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.P_MeetingAdmin GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_Id,P_Title,P_MeeContent,P_StartTime,P_EndTime,P_MeePlace,P_PeopleAmount,P_MAStatus,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status ");
            strSql.Append(",(select COUNT(P_Id) from P_MeetingAdminSublist where P_MeetingAdminSublist.P_MeeID=P_MeetingAdmin.P_Id) as CountUser ");
            strSql.Append("from P_MeetingAdmin ");
            strSql.Append("where P_Id=@id");
            SqlParameter[] parameters ={
                new SqlParameter("@id",SqlDbType.NVarChar,50)};
            parameters[0].Value = id;

            Model.P_MeetingAdmin model = new Model.P_MeetingAdmin();
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
        public List<Model.P_MeetingAdmin> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,(select COUNT(P_Id) from P_MeetingAdminSublist where P_MeetingAdminSublist.P_MeeID=P_MeetingAdmin.P_Id) as CountUser ");
            strSql.Append(" FROM P_MeetingAdmin ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where P_Status = 0 and " + strWhere);
            }
            else
            {
                strSql.Append(" where P_Status = 0 ");
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
        public List<Model.P_MeetingAdmin> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<Model.P_MeetingAdmin> list = new List<Model.P_MeetingAdmin>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataRowToModel(row));
            }
            return list;
        }
        /// <summary>
        /// 将对象转换为实体
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private Model.P_MeetingAdmin DataRowToModel(DataRow row)
        {
            Model.P_MeetingAdmin model = new Model.P_MeetingAdmin();
            if (row != null)
            {
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_Title"] != null && row["P_Title"].ToString() != "")
                {
                    model.P_Title = row["P_Title"].ToString();
                }
                if (row["P_MeeContent"] != null)
                {
                    model.P_MeeContent = row["P_MeeContent"].ToString();
                }
                if (row["P_StartTime"] != null && row["P_StartTime"].ToString() != "")
                {
                    model.P_StartTime = Convert.ToDateTime(row["P_StartTime"].ToString());
                }
                if (row["P_EndTime"] != null && row["P_EndTime"].ToString() != "")
                {
                    model.P_EndTime = Convert.ToDateTime(row["P_EndTime"].ToString());
                }
                if (row["P_MeePlace"] != null)
                {
                    model.P_MeePlace = row["P_MeePlace"].ToString();
                }
                if (row["P_PeopleAmount"] != null)
                {
                    model.P_PeopleAmount = Convert.ToInt32(row["P_PeopleAmount"]);
                }
                if (row["P_MAStatus"] != null && row["P_MAStatus"].ToString() != "")
                {
                    model.P_MAStatus = Convert.ToInt32(row["P_MAStatus"]);
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
                if (Convert.ToInt32(row["CountUser"]) != 0)
                {
                    model.CountUser = Convert.ToInt32(row["CountUser"]);
                }
                //if(row["Participant"] != null && row["Participant"].ToString() != "")
                //{
                //    model.Participant = row["Participant"].ToString();
                //}
                if (Convert.ToDateTime(row["P_StartTime"]) > DateTime.Now && Convert.ToInt32(row["CountUser"]) < Convert.ToInt32(row["P_PeopleAmount"]))
                {
                    model.StatusName = "报名中";
                }
                if (Convert.ToDateTime(row["P_StartTime"]) <= DateTime.Now || Convert.ToInt32(row["CountUser"]) == Convert.ToInt32(row["P_PeopleAmount"]))
                {
                    model.StatusName = "已截止";
                }
                if (Convert.ToDateTime(row["P_EndTime"]) <= DateTime.Now)
                {
                    model.StatusName = "已结束";
                }
            }
            return model;
        }
    }
}

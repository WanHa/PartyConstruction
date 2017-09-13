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
    public partial class ModelMember
    {
        public ModelMember()
        {

        }
        #region 基本方法
        public bool Exists(string id)
        {
            string sql = "select * from P_ModelPartyMember where P_Id = '" + id + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 返回数据总数
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as H ");
            strSql.Append(" from P_ModelPartyMember");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
        }

        /// <summary>
        /// 增加一条数据及其子表
        /// </summary>
        public bool Add(Model.P_ModelPartyMember model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //根据name查询id
                        //string queryUserId = "select * from Sys_User where F_RealName = " + model.name + "";
                        //Model.Sys_User user = (Model.Sys_User)DbHelperSQL.GetSingle(queryUserId);

                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_ModelPartyMember(");
                        strSql.Append("P_Id,P_UserId,P_Description,P_CreateTime,P_CreateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@id,@userid,@des,@createtime,@createuser,@status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id", SqlDbType.VarChar,50),
                                new SqlParameter("@userid", SqlDbType.VarChar,50),
                                new SqlParameter("@des", SqlDbType.VarChar,200),
                                new SqlParameter("@createtime", SqlDbType.DateTime,4),
                                new SqlParameter("@createuser", SqlDbType.VarChar,50),
                                new SqlParameter("@status", SqlDbType.TinyInt,1)
                        };
                        parameters[0].Value = Guid.NewGuid().ToString("N");
                        parameters[1].Value = model.P_UserId;
                        parameters[2].Value = model.P_Description;
                        parameters[3].Value = DateTime.Now;
                        parameters[4].Value = model.P_CreateUser;
                        parameters[5].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务


                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.P_ModelPartyMember model)
        {
            Model.P_ModelPartyMember oldModel = GetModel(model.P_Id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_ModelPartyMember set ");
                        strSql.Append("P_UserId=@userid,");
                        strSql.Append("P_Description=@des,");
                        strSql.Append("P_UpdateTime=@updatetime,");
                        strSql.Append("P_UpdateUser=@updateuser");
                        strSql.Append(" where P_Id=@id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@userid", SqlDbType.VarChar,50),
                                new SqlParameter("@des", SqlDbType.VarChar,200),
                                new SqlParameter("@updatetime", SqlDbType.DateTime,4),
                                new SqlParameter("@updateuser", SqlDbType.VarChar,50),
                                new SqlParameter("@id",SqlDbType.NVarChar,50)};
                        parameters[0].Value = model.P_UserId;
                        parameters[1].Value = model.P_Description;
                        parameters[2].Value = model.P_UpdateTime;
                        parameters[3].Value = model.P_UpdateTime;
                        parameters[4].Value = model.P_Id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);


                        trans.Commit();
                    }
                    catch
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
        public bool Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //删除导航主表
                        if (!string.IsNullOrEmpty(id))
                        {
                            DbHelperSQL.ExecuteSql(conn, trans, "update P_ModelPartyMember set P_Status = 1 where P_Id in('" + id + "')");
                        }


                        trans.Commit();
                    }
                    catch
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
        public Model.P_ModelPartyMember GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from P_ModelPartyMember ");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = id;

            Model.P_ModelPartyMember model = new Model.P_ModelPartyMember();
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
        /// 获取编辑信息
        /// </summary>
        public DataRow GetEditInfo(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 b.user_name,a.P_Description from P_ModelPartyMember a left join dt_users b on a.P_UserId = b.id ");
            strSql.Append(" where a.P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户name获取用户id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetUserId(string name)
        {
            string sql = "select id from dt_users where user_name = '" + name + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["id"].ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" a.P_Id,b.user_name,a.P_DesCription ");
            strSql.Append(" FROM P_ModelPartyMember a left join dt_users b on a.P_UserId = b.id ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.P_Id,b.user_name,a.P_DesCription FROM P_ModelPartyMember a left join dt_users b on a.P_UserId = b.id");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        public Model.P_ModelPartyMember DataRowToModel(DataRow row)
        {
            Model.P_ModelPartyMember model = new Model.P_ModelPartyMember();
            if (row != null)
            {
                #region 主表信息======================
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_UserId"] != null && row["P_UserId"].ToString() != "")
                {
                    model.P_UserId = row["P_UserId"].ToString();
                }
                if (row["P_Description"] != null)
                {
                    model.P_Description = row["P_Description"].ToString();
                }
                if (row["P_CreateUser"] != null)
                {
                    model.P_CreateUser = row["P_CreateUser"].ToString();
                }
                if (row["P_CreateTime"] != null && row["P_CreateTime"].ToString() != "")
                {
                    model.P_CreateTime = DateTime.Parse(row["P_CreateTime"].ToString());
                }
                if (row["P_UpdateTime"] != null && row["P_UpdateTime"].ToString() != "")
                {
                    model.P_UpdateTime = DateTime.Parse(row["P_UpdateTime"].ToString());
                }
                if (row["P_UpdateUser"] != null && row["P_UpdateUser"].ToString() != "")
                {
                    model.P_UpdateUser = row["P_UpdateUser"].ToString();
                }
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = int.Parse(row["P_Status"].ToString());
                }
                #endregion
            }
            return model;
        }
        #endregion

        /// <summary>
        /// 获取党员姓名列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<Name> GetUserNameList(string key)
        {
            List<Name> list = new List<Name>();
            string sql = string.Empty;
            if (key == "-1")
            {
                sql = "select ROW_NUMBER() over(order by user_name) as row_number,id as val,user_name as text from dt_users";
            }
            else
            {
                sql = "select ROW_NUMBER() over(order by user_name) as row_number,id as val,user_name as text from dt_users where user_name like '%" + key + "%'";
            }
            DataSet ds = DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Name n = new Name();
                    n.val = Convert.ToInt32(dt.Rows[i]["val"]);
                    n.text = dt.Rows[i]["text"].ToString();
                    list.Add(n);
                }
            }
            return list;
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool NewRole(string name,string userid)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string sql = "select id from dt_users where user_name = '" + name + "'";

                DataSet ds = DbHelperSQL.Query(sql);
                string id = ds.Tables[0].Rows[0]["id"].ToString();
                string query = "select * from P_ModelPartyMember where P_UserId = '" + id + "' and P_Status = 0";
                DataSet num = DbHelperSQL.Query(query);
                if (num.Tables[0].Rows.Count == 0)
                {
                    Model.P_ModelPartyMember item = new Model.P_ModelPartyMember();
                    item.P_Id = Guid.NewGuid().ToString();
                    item.P_UserId = ds.Tables[0].Rows[0]["id"].ToString();
                    //item.P_Description = ds.Tables[0].Rows[0]["F_Description"].ToString();
                    item.P_CreateTime = DateTime.Now;
                    item.P_CreateUser = userid;
                    item.P_Status = 0;
                    bool a = Add(item);
                    if (a)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 更新模范党员信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="des"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool UpdateRoleInfo(string id, string name, string des, string userid)
        {
            string sql = "select id from dt_users where user_name = '" + name + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            string infoid = ds.Tables[0].Rows[0]["id"].ToString();

            Model.P_ModelPartyMember item = new Model.P_ModelPartyMember();
            item.P_UserId = infoid;
            item.P_Description = des;
            item.P_UpdateUser = userid;
            item.P_UpdateTime = DateTime.Now;
            item.P_Id = id;
            bool a = Update(item);
            return a;
        }

        /// <summary>
        /// 查询模范党员是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckRepeat(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string sql = "select id from dt_users where user_name = '" + name + "'";
                DataSet ds = DbHelperSQL.Query(sql);
                string userid = ds.Tables[0].Rows[0]["id"].ToString();

                string query = "select * from P_ModelPartyMember where P_UserId = '" + userid + "' and P_Status = 0";
                DataSet set = DbHelperSQL.Query(query);
                if (set.Tables[0].Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 姓名列表类（下拉框）
    /// </summary>
    public class Name
    {
        public int val { get; set; }

        public string text { get; set; }
    }

    /// <summary>
    /// 模范党员
    /// </summary>
    public class MemberModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public string des { get; set; }

        public string createtime { get; set; }
    }

    public class PostBody
    {
        public string id { get; set; }
        public string name { get; set; }

        public string des { get; set; }

        public string user { get; set; }
    }
}

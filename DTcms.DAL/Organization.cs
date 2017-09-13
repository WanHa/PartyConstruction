using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{
    public partial class Organization
    {
        public Organization()
        { }
        #region 基本方法
        public bool Exists(int id)
        {
            string sql = "select * from dt_user_groups where Id = '" + id + "'";
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
            strSql.Append(" from dt_user_groups");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
        }

        /// <summary>
        /// 增加一条数据及其子表
        /// </summary>
        public bool Add(Model.user_groups model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into dt_user_groups(");
                        strSql.Append("pid,title,manager,manager_id,position)");
                        strSql.Append(" values (");
                        strSql.Append("@pid,@title,@manager,@manager_id,@position)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@pid", SqlDbType.Int,8),
                                new SqlParameter("@title", SqlDbType.VarChar,200),
                                new SqlParameter("@manager",SqlDbType.VarChar,50),
                                new SqlParameter("@manager_id",SqlDbType.Int,4),
                                new SqlParameter("@position",SqlDbType.NVarChar,50)
                        };
                        parameters[0].Value = model.pid ;
                        parameters[1].Value = model.title;
                        parameters[2].Value = model.manager;
                        parameters[3].Value = model.Manager_id;
                        parameters[4].Value = model.position;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        model.id = Convert.ToInt32(obj);

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
        public bool Update(Model.user_groups model)
        {
            Model.user_groups oldModel = GetModel(model.id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update dt_user_groups set ");
                        strSql.Append("pid=@pid,");
                        strSql.Append("title=@title,");
                        strSql.Append("manager=@manager,manager_id=@manager_id,");
                        strSql.Append("position=@position");
                        strSql.Append(" where id=@id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@pid", SqlDbType.Int,8),
                                new SqlParameter("@title", SqlDbType.VarChar,200),
                                new SqlParameter("@manager",SqlDbType.VarChar,50),
                                new SqlParameter("@manager_id",SqlDbType.Int,4),
                                new SqlParameter("@position",SqlDbType.NVarChar,50),
                                new SqlParameter("@id",SqlDbType.Int,8)};
                        parameters[0].Value = model.pid;
                        parameters[1].Value = model.title;
                        parameters[2].Value = model.manager;
                        parameters[3].Value = model.Manager_id;
                        parameters[4].Value = model.position;
                        parameters[5].Value = model.id;
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
        public bool Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //删除导航主表
                        if (!string.IsNullOrEmpty(id.ToString()))
                        {
                            DbHelperSQL.ExecuteSql(conn, trans, "delete from dt_user_groups where Id in('" + id + "')");
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
        public Model.user_groups GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from dt_user_groups ");
            strSql.Append(" where Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = id;

            Model.user_groups model = new Model.user_groups();
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

        public Model.user_groups DataRowToModel(DataRow row)
        {
            Model.user_groups model = new Model.user_groups();
            if (row != null)
            {
                #region 主表信息======================
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["pid"] != null && row["pid"].ToString() != "")
                {
                    model.pid = int.Parse(row["pid"].ToString());
                }
                if (row["title"] != null)
                {
                    model.title = row["title"].ToString();
                }
                if (row["is_lock"] != null && row["is_lock"].ToString() != "")
                {
                    model.is_lock = int.Parse(row["is_lock"].ToString());
                }
                if (row["manager"] != null && row["manager"].ToString() != "")
                {
                    model.manager = row["manager"].ToString();
                }
                if (row["position"] != null && row["position"].ToString() != "")
                {
                    model.position = row["position"].ToString();
                }
                #endregion
            }
            return model;
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
            strSql.Append(" a.id,a.title,b.title as pname,a.manager,a.position ");
            strSql.Append(" from dt_user_groups a left join dt_user_groups b on b.id = a.pid ");
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
            strSql.Append("select a.id,a.title,b.title as pname,a.manager,a.position from dt_user_groups a left join dt_user_groups b on b.id = a.pid");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        /// <summary>
        /// 获取编辑信息
        /// </summary>
        public DataRow GetEditInfo(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 a.id,a.title,b.title as pname,a.manager,a.position,a.manager_id from dt_user_groups a left join dt_user_groups b on b.id = a.pid ");
            strSql.Append(" where a.id=@id ");
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

        public bool UpdateInfo(int id, string name, string pname, string manager, string userid,string position,string managerid)
        {
            int? pid;
            if (!string.IsNullOrEmpty(pname) && pname != "请选择父级")
            {
                string sql = "select id from dt_user_groups where title = '" + pname + "'";
                DataSet ds = DbHelperSQL.Query(sql);
                pid = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
            else
            {
                pid = null;
            }

            Model.user_groups item = new Model.user_groups();
            item.id = id;
            item.title = name;
            item.pid = pid;
            item.manager = manager;
            item.position = position;
            item.Manager_id =Convert.ToInt32(managerid);
            bool a = Update(item);
            return a;
        }

        public ArrayList PnameList()
        {
            string sql = "select title from dt_user_groups where is_lock = 0";
            DataSet ds = DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            ArrayList list = new ArrayList();
            list.Add("请选择父级");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["title"].ToString());
            }
            return list;
        }

        public int? GetPid(string pname)
        {
            int? pid;
            if (!string.IsNullOrEmpty(pname) && pname != "请选择父级")
            {
                string sql = "select id from dt_user_groups where title = '" + pname + "'";
                DataSet ds = DbHelperSQL.Query(sql);
                
                pid = Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString());
            }
            else
            {
                pid = null;
            }
            return pid;
        }

        public bool CheckRepeat(string name, string pname,string managerid)
        {
            int? pid = GetPid(pname);
            string sql = string.Empty;
            if (pid == null)
            {
                sql = "select * from dt_user_groups where title = '" + name + "' and pid is null and is_lock = 0";
            }
            else
            {
                sql = "select * from dt_user_groups where title = '" + name + "' and pid = " + pid + " and is_lock = 0";
            }
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

        public string PositionList()
        {
            List<PositionItem> list = new List<PositionItem>();
            string sql = "select title,position from dt_user_groups where is_lock = 0";
            DataSet ds = DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PositionItem p = new PositionItem();
                string a = dt.Rows[i]["position"].ToString();
                if (!string.IsNullOrEmpty(a))
                {
                    string[] pos = a.Split(',');
                    p.lng = pos[0];
                    p.lat = pos[1];
                    p.title = dt.Rows[i]["title"].ToString();
                    list.Add(p);
                }
            }
            string json = JsonHelper.ObjectToJSON(list);

            return json;
        }
        #endregion

    }

    public class PositionItem
    {
        //经度
        public string lng { get; set; }
        //纬度
        public string lat { get; set; }
        //文本
        public string title { get; set; }
    }
}

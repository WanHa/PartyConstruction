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
    /// <summary>
    /// 评选活动逻辑处理
    /// </summary>
    public partial class P_ReviewActivity
    {
        public P_ReviewActivity(){}

        #region 基本方法
        /// <summary>
		/// 是否存在该记录（根据ID）
		/// </summary>
		public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_ReviewActivity");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public List<Model.P_ReviewActivity> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM P_ReviewActivity  ");
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

        
        #endregion
        /// <summary>
        /// 将datatable转换为list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Model.P_ReviewActivity> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<Model.P_ReviewActivity> list = new List<Model.P_ReviewActivity>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataComment(row));
            }

            return list;
        }
        /// <summary>
        /// 将对象转换为实体
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.P_ReviewActivity DataComment(DataRow row)        {            Model.P_ReviewActivity com = new Model.P_ReviewActivity();            if (row != null)            {
                #region 主表信息======================
                if (row["P_Id"] != null && row["P_Id"].ToString() != null)
                {
                    com.P_Id = row["P_Id"].ToString();
                }
                if(row["P_UserId"]!=null && row["P_UserId"].ToString() != null)
                {
                    com.P_UserId = row["P_UserId"].ToString();
                }
                if (row["P_Title"] != null && row["P_Title"].ToString() != "")
                {
                    com.P_Title = row["P_Title"].ToString();
                }
                if (row["P_CreateTime"] != null && row["P_CreateTime"].ToString() != "")
                {
                    com.P_CreateTime =Convert.ToDateTime(row["P_CreateTime"]);
                }
                if (row["P_EndTime"] != null && row["P_EndTime"].ToString() != "")
                {
                    com.P_EndTime = Convert.ToDateTime(row["P_EndTime"]);
                    if(Convert.ToDateTime(row["P_EndTime"]) <= DateTime.Now)
                    {
                        com.AsStatus = "已完成";
                    }
                    else
                    {
                        com.AsStatus = "进行中";
                    }
                }
                
                #endregion
            }            return com;        }
        /// <summary>
		/// 得到一个对象实体
		/// </summary>
        public Model.P_ReviewActivity GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 P_Id,P_Title,P_Content,P_OptionType,P_UserId,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_EndTime");
            strSql.Append(" from P_ReviewActivity");
            strSql.Append(" where P_Id=@P_Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;

            Model.article model = new Model.article();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataComment(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string userid)
        {

            Model.P_ReviewActivity oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_ReviewActivity set ");
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
    }
}

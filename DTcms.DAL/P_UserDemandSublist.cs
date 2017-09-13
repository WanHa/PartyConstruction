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
    public partial class P_UserDemandSublist
    {
        public P_UserDemandSublist() { }
        #region 基本方法================================

        /// <summary>
		/// 是否存在该记录（根据ID）
		/// </summary>
		public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_UserDemand");
            strSql.Append(" where P_ID=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Model.P_UserDemandSublist model,string id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_UserDemandSublist(");
                        strSql.Append("P_ID,P_UDId,P_Verifier,P_Content,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_ID,@P_UDId,@P_Verifier,@P_Content,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UDId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Verifier",SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,100)
                               };
                        parameters[0].Value = model.P_ID;
                        parameters[1].Value = id;
                        parameters[2].Value = model.P_Verifier;
                        parameters[3].Value = model.P_Content;
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
        public bool Update(Model.P_UserDemandSublist model)
        {
            Model.P_UserDemandSublist oldModel = GetModel(model.P_ID); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_Status set ");
                        strSql.Append("P_UDId=@P_UDId,");
                        strSql.Append("P_Status=@P_Status,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_ID=@P_ID ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_UDId", SqlDbType.NVarChar,1000),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100)};
                        parameters[0].Value = model.P_UDId;
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

            Model.P_UserDemandSublist oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_UserDemandSublist set ");
                        strSql.Append("P_Status=@P_Status,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_ID=@P_ID ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100)};
                        parameters[0].Value = 1;
                        parameters[1].Value = DateTime.Now;
                        parameters[2].Value = userid;
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
        public List<Ims> GetUrl(string id)
        {
            List<Ims> model = new List<Ims>();
            StringBuilder str = new StringBuilder();
            str.Append("select P_ImageUrl as imgurl from P_Image where P_ImageId='" + id+"' and P_Status=0 and P_ImageType=20006");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<Ims> helper = new DataSetToModelHelper<Ims>();
            if (ds != null)
            {
                model = helper.FillModel(ds);
            }
            else
            {
                model = null;
            }
            return model;
        }
        public Userdemand GetToModel(string id)
        {
            Userdemand result = new Userdemand();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_UserDemand.P_Content as content,");
            strSql.Append("P_UserDemand.P_CheckStatus as checkstatus,");
            strSql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as sum FROM P_UserDemand");
            strSql.Append(" where P_UserDemand.P_Id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = id;
            DataSet data = DbHelperSQL.Query(strSql.ToString(), parameters);
            DataSetToModelHelper<Userdemand> helper = new DataSetToModelHelper<Userdemand>();
            if (data.Tables[0].Rows.Count > 0)
            {
                result = helper.FillToModel(data.Tables[0].Rows[0]);
            }
            else
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>

        public Model.P_UserDemandSublist GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("SELECT P_UserDemandSublist.P_ID,P_UserDemandSublist.P_UDId,P_UserDemandSublist.P_Verifier,P_UserDemandSublist.P_Content,");
            //strSql.Append("P_UserDemandSublist.P_CreateTime,P_UserDemandSublist.P_CreateUser,P_UserDemandSublist.P_Status,P_UserDemand.P_Content as mcon,");
            //strSql.Append("P_UserDemand.P_checkStatus FROM P_UserDemandSublist");
            //strSql.Append(" left join P_UserDemand on P_UserDemand.P_Id =P_UserDemandSublist.P_UDId ");
            strSql.Append("SELECT P_ID,P_UDId,P_Verifier,P_Content,P_CreateTime,P_CreateUser,P_Status FROM P_UserDemandSublist");
            strSql.Append(" where P_UserDemandSublist.P_UDId=@id");
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
        /// </summary>s
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM P_UserDemandSublist");
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

        public List<Model.P_UserDemandSublist> Getlist(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_UserDemandSublist.P_ID,P_UserDemandSublist.P_UDId,P_UserDemandSublist.P_Verifier,P_UserDemandSublist.P_CreateUser,");
            strSql.Append("P_UserDemandSublist.P_Content,P_UserDemandSublist.P_CreateTime,P_UserDemandSublist.P_UpdateTime,P_UserDemandSublist.P_UpdateUser,");
            strSql.Append(" P_UserDemandSublist.P_Status");
            strSql.Append(" from P_UserDemandSublist ");
            strSql.Append(" LEFT JOIN P_UserDemand on P_UserDemandSublist.P_UDId = P_UserDemand.P_Id ");
            strSql.Append(" where P_UserDemand.P_Id ='"+id+ @"'and P_UserDemand.P_Status=0 and P_UserDemandSublist.P_Status=0 ");
            strSql.Append(" ORDER BY P_UserDemandSublist.P_CreateTime asc");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return DetailListModel(ds.Tables[0]);

        }
        /// <summary>
        /// 将datatable转换为list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Model.P_UserDemandSublist> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<Model.P_UserDemandSublist> list = new List<Model.P_UserDemandSublist>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataRowToModel(row));
            }

            return list;
        }
        #endregion
        #region 扩展方法================================
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.P_UserDemandSublist DataRowToModel(DataRow row)
        {
            Model.P_UserDemandSublist model = new Model.P_UserDemandSublist();
            if (row != null)
            {
                #region 主表信息======================
                if (row["P_ID"] != null && row["P_ID"].ToString() != "")
                {
                    model.P_ID = row["P_ID"].ToString();
                }
                if (row["P_UDId"] != null && row["P_UDId"].ToString() != "")
                {
                    model.P_UDId = row["P_UDId"].ToString();
                }
                if (row["P_Verifier"] != null && row["P_Verifier"].ToString() != "")
                {
                    model.P_Verifier = row["P_Verifier"].ToString();
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
                //if (row["Sum"] !=null && row["Sum"].ToString() != "")
                //{
                //    model.Sum = Convert.ToInt32(row["Sum"]);
                //}
                #endregion
            }
            return model;
        }
        #endregion
        //public Userdemand ToModel(DataRow row)
        //{
        //    Userdemand mo = new Userdemand();
        //    if (row["P_ID"] != null && row["P_ID"].ToString() != "")
        //    {
        //        mo.mcontent = row["P_ID"].ToString();
        //    }
        //    return mo;
        //}
    }
    public class Userdemand
    {
        public string content { get; set; }
        public int checkstatus { get; set; }
        public int sum { get; set; }
    }
    public class Ims
    {
        public string imgurl { get; set; }
    }
}

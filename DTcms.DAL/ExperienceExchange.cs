using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace DTcms.DAL
{
	public partial class ExperienceExchange
	{

		
		/// <summary>
		/// 查询所有心得交流
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="rows"></param>
		/// <param name="page"></param>		
		public List<Model.P_LearnExchange> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select * ,(select user_name from dt_users where id =P_LearnExchange.P_UserId )as Name FROM P_LearnExchange");
			if (strWhere.Trim() != "")
			{
				strSql.Append(" where P_Status = 0 and " + strWhere);
			}
			else
			{
				strSql.Append(" where P_Status = 0");
			}
			recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
			DataSet ds =DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
            return DetailListModel(ds.Tables[0]);
        }
        public List<Model.P_LearnExchange> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<Model.P_LearnExchange> list = new List<Model.P_LearnExchange>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataRowToModel(row));
            }
            return list;
        }

        public LearnExchangeModel SelLearnExchange(string id)
        {
            StringBuilder str = new StringBuilder();
            str.Append("select P_LearnExchange.P_Content as content,P_LearnExchange.P_Title as title,P_LearnExchange.P_CreateTime as createtime,(select title from dt_user_groups where id =(select TOP 1 t.value from F_Split(");
            str.Append("(select dt_users.group_id from dt_users where dt_users.id = P_UserId),',') as t");
            str.Append(" left join dt_user_groups on dt_user_groups.id = t.value");
            str.Append(" where t.value != '' order by dt_user_groups.grade DESC))as branch,dt_users.user_name as username"); 
            str.Append(" from P_LearnExchange");
            //str.Append(" left join P_Image on P_Image.P_ImageId = P_LearnExchange.P_Id and P_Image.P_ImageType = 20001 and P_Image.P_Status = 0");
            str.Append(" left join dt_users on dt_users.id = P_LearnExchange.P_UserId");
            str.Append(" where P_LearnExchange.P_Id = '" + id + "' and P_LearnExchange.P_Status = 0 ");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataRow row = ds.Tables[0].Rows[0];
            DataSetToModelHelper<LearnExchangeModel> helper = new DataSetToModelHelper<LearnExchangeModel>();
            return helper.FillToModel(row);
        }

        public List<LearnExchangeImage> SelLearnExchangeImg(string id)
        {
            List<LearnExchangeImage> model = new List<LearnExchangeImage>();
            StringBuilder str = new StringBuilder();
            str.Append("select P_ImageUrl as imageurl from P_Image where P_ImageId = '" + id + "' and P_ImageType = 20001 and P_Status = 0");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<LearnExchangeImage> helper = new DataSetToModelHelper<LearnExchangeImage>();
            if(ds != null)
            {
                model = helper.FillModel(ds);
            }
            else
            {
                model = null;
            }
            return model;
        }

        public int SelStatus(string id)
        {
            LearnExchangeStatus result = new LearnExchangeStatus();
            StringBuilder str = new StringBuilder();
            str.Append("select P_AuditState as status from P_LearnExchange where P_Id = '" + id + "' and P_Status = 0");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<LearnExchangeStatus> model = new DataSetToModelHelper<LearnExchangeStatus>();
            if(ds != null)
            {
                result = model.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                result = null;
            }
            return result.status;
        }

        /// <summary>
        /// 通过操作：
        ///         将字段P_AuditState修改为1（通过状态）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Pass(string id)
		{						
			using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						StringBuilder strSql = new StringBuilder();
						strSql.Append("update P_LearnExchange set ");
						strSql.Append("P_AuditState=@P_AuditState");//将审核状态改为1（通过）
						strSql.Append(" where P_Id=@P_Id ");
						SqlParameter[] parameters = {
								new SqlParameter("@P_AuditState", SqlDbType.NVarChar,36),
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
		/// 拒绝操作：
		///         将字段P_AuditState修改为2（拒绝状态）
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Refuse(string id)
		{
			using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						StringBuilder strSql = new StringBuilder();
						strSql.Append("update P_LearnExchange set ");
						strSql.Append("P_AuditState=@P_AuditState");//将审核状态改为2（拒绝）
						strSql.Append(" where P_Id=@P_Id ");
						SqlParameter[] parameters = {
								new SqlParameter("@P_AuditState", SqlDbType.NVarChar,36),
								new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
						parameters[0].Value = 2;
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
		public Model.P_LearnExchange GetModel(string id)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT *,(select user_name from dt_users where id =P_LearnExchange.P_UserId )as Name FROM P_LearnExchange");
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
		/// 将对象转换实体
		/// </summary>
		public Model.P_LearnExchange DataRowToModel(DataRow row)
		{
			Model.P_LearnExchange model = new Model.P_LearnExchange();
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
				if (row["P_AuditState"] != null && row["P_AuditState"].ToString() != "")
				{
					model.P_AuditState = Convert.ToInt32(row["P_AuditState"]);
				}
				if (row["P_Title"] != null && row["P_Title"].ToString() != "")
				{
					model.P_Title = row["P_Title"].ToString();
				}
				if (row["P_Content"] != null && row["P_Content"].ToString() != "")
				{
					model.P_Content = row["P_Content"].ToString();
				}
				if (row["P_ImageId"] != null && row["P_ImageId"].ToString() != "")
				{
					model.P_Content = row["P_ImageId"].ToString();
				}
                if(row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
				#endregion
			}
			return model;
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string id)
		{

			Model.P_LearnExchange oldModel = GetModel(id); //旧的数据
			using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						StringBuilder strSql = new StringBuilder();
						strSql.Append("update P_LearnExchange set ");
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
        public class LearnExchangeModel
        {
            public string content { get; set; }
            public string title { get; set; }
            public DateTime createtime { get; set; }
            public string branch { get; set; }

            //public string imgurl { get; set; }
            public string username { get; set; }
        }

        public class LearnExchangeStatus
        {
            public int status { get; set; }
        }

        public class LearnExchangeImage
        {
            public string imageurl { get; set; }
        }
    }
}

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
    public class P_DailyLoWord
    {
        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,(SELECT p_type from P_WorkJournalType where P_WorkJournal.P_TypeId=P_WorkJournalType.P_Id) as P_Type ,");
            strSql.Append("(select user_name from dt_users where P_WorkJournal.P_CreateUser=dt_users.id) as UserName, ");
            strSql.Append("(select user_name from dt_users where P_WorkJournal.P_SendUser=dt_users.id) as senduser ");
            strSql.Append("  from P_WorkJournal ");
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
        /// <summary>
        /// 详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailWoedJournal SelPartyNewsPeper(string id)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_WorkJournal.P_Title as title,P_WorkJournal.P_Content as content,P_WorkJournal.P_CreateTime as createtime,");
            strsql.Append("(select user_name from dt_users where P_WorkJournal.P_SendUser=dt_users.id) as senduser, ");
            strsql.Append("(SELECT p_type from P_WorkJournalType where P_WorkJournal.P_TypeId=P_WorkJournalType.P_Id) as type,");
            strsql.Append("(select user_name from dt_users where P_WorkJournal.P_CreateUser=dt_users.id) as username,");
            strsql.Append("(select P_AuditContent from P_AuditingFeedback where P_LogId ='" + id + @"')as replycontent");
            strsql.Append(" from P_WorkJournal ");
            strsql.Append(" where P_WorkJournal.P_Id='"+id+@"'");
           
            DataSet ds = DbHelperSQL.Query(strsql.ToString());

            DataRow row = ds.Tables[0].Rows[0];
            DataSetToModelHelper<DetailWoedJournal> model = new DataSetToModelHelper<DetailWoedJournal>();
            return model.FillToModel(row);
        }

        public List<DailyLogImage> SelDailyLogImage(string id)
        {
            List<DailyLogImage> model = new List<DailyLogImage>();
            StringBuilder str = new StringBuilder();
            str.Append("select P_ImageUrl as imageurl from P_Image where P_ImageId = '" + id + "' and P_ImageType = 20004 and P_Status = 0");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<DailyLogImage> helper = new DataSetToModelHelper<DailyLogImage>();
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
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string userid)
        {

            Model.DetailWoedJournal oldModel = SelPartyNewsPeper(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_WorkJournal set ");
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
        public class DailyLogImage
        {
            public string imageurl { get; set; }
        }
    }
}

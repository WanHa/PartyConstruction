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
    public class BranchDAL
    {
        private UserGroupHelper usergroup = new UserGroupHelper();

        public List<ListModel> getList(string group_id, int rows, int page)
        {
            List<ListModel> model = new List<ListModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select dt_users.id as id,dt_users.user_name as username,dt_user_groups.title as branch,dt_user_groups.manager_id as mid,");
            strSql.Append(" P_Image.P_ImageUrl as avatar from dt_users ");
            strSql.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id ='" + group_id + @"'");
            strSql.Append(" LEFT JOIN P_Image on P_ImageId = dt_users.id and P_ImageType='20011'");
            DataSet dt = DbHelperSQL.Query(strSql.ToString());
            DataSetToModelHelper<ListModel> result = new DataSetToModelHelper<ListModel>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                model = result.FillModel(dt);
            }
            else
            {
                model = null;
            }
            foreach (var item in model)
            {
                if (item.id == item.mid)
                {
                    item.role = 0;
                }
                if (item.id != item.mid)
                {
                    item.role = 1;
                }
            }
            return model;
        }
        //申请
        public Boolean Applyfor (string P_ApplyUserId,string P_ApplyContent)
            {
            using(SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder SstrSql = new StringBuilder();
                        SstrSql.Append("insert into P_Apply(");
                        SstrSql.Append("P_Id,P_ApplyUserId,P_ApplyContent,P_CreateTime,P_Status)");
                        SstrSql.Append(" values (");
                        SstrSql.Append("@P_Id,@P_ApplyUserId,@P_ApplyContent,@P_CreateTime,@P_Status)");
                        SqlParameter[] parameters2 = {
            new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
            new SqlParameter("@P_ApplyUserId", SqlDbType.NVarChar,50),
            new SqlParameter("@P_ApplyContent", SqlDbType.NText,10000),
            new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
            new SqlParameter("@P_Status", SqlDbType.Int,10)
            };
                        parameters2[0].Value = Guid.NewGuid().ToString();
                        parameters2[1].Value = P_ApplyUserId;
                        parameters2[2].Value = P_ApplyContent;
                        parameters2[3].Value = DateTime.Now;
                        parameters2[4].Value = 0;
                        object obj1 = DbHelperSQL.GetSingle(conn, trans, SstrSql.ToString(), parameters2);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }
        public class ListModel
        {
            public int role { get; set; }
            public int mid { get; set; }
            public string branch { get; set; }
            public string username { get; set; }
            public string avatar { get; set; }
            public int id { get; set; }
        }
    }
}

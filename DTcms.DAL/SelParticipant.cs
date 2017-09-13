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
    public partial class SelParticipant
    {
        public SelParticipant(){}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from users");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.users GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,group_id, user_name,salt,password,mobile,email,avatar,nick_name,sex,telphone,status");
            strSql.Append(" from users");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;

            Model.users model = new Model.users();
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM dt_users  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append("where  status = 0  " + strWhere);
            }
            else
            {
                strSql.Append("where  status = 0 ");
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            //return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
            return DbHelperSQL.Query(strSql.ToString());
        }


        public string GetOrganizeList()
        {
            List<groupItem> list = new List<groupItem>();
            string sql = "select row_number()OVER(ORDER BY id desc) as id, title from  dt_user_groups where is_lock = 0 ORDER BY id ";
            DataSet ds = DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                groupItem g = new groupItem();
                g.id = dt.Rows[i]["id"].ToString();
                g.text = dt.Rows[i]["title"].ToString();
                list.Add(g);
            }
            string json = JsonHelper.ObjectToJSON(list);
            return json;
        }



        private Model.users DataRowToModel(DataRow row)
        {
            Model.users model = new Model.users();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = Convert.ToInt32(row["id"]);
                }

                if (row["group_id"] != null && row["group_id"].ToString() != "")
                {
                    model.group_id = row["group_id"].ToString();
                }
                if (row["user_name"] != null)
                {
                    model.user_name = row["user_name"].ToString();
                }
                if (row["salt"] != null)
                {
                    model.salt = row["salt"].ToString();
                }
                if (row["password"] != null)
                {
                    model.password = row["password"].ToString();
                }
                //if (row["P_CreateTime"] != null)
                //{
                //    model.P_CreateTime = Convert.ToDateTime(row["P_CreateTime"].ToString());
                //}
                //if (row["P_CreateUser"] != null)
                //{
                //    model.P_CreateUser = row["P_CreateUser"].ToString();
                //}
                //if (row["P_UpdateTime"] != null && row["P_UpdateTime"].ToString() != "")
                //{
                //    model.P_UpdateTime = Convert.ToDateTime(row["P_UpdateTime"].ToString());
                //}
                //if (row["P_UpdateUser"] != null)
                //{
                //    model.P_UpdateUser = row["P_UpdateUser"].ToString();
                //}
                if (row["status"] != null)
                {
                    model.status = Convert.ToInt32(row["status"]);
                }
            }
            return model;
        }

   }
    public class groupItem
    {
        public string id { get; set; }

        public string text { get; set; }
    }

}

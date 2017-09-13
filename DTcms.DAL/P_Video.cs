using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{
    public partial class P_Video
    {
        public P_Video()
        {

        }

        #region 基本方法========================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_Video");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.P_Video> GetList(int article_id)
        {
            List<Model.P_Video> modelList = new List<Model.P_Video>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_Id,P_ParentId,P_VideoName,P_VideoPic,P_Url,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_Number,P_VideoLength");
            strSql.Append(" FROM P_Video");
            strSql.Append(" where P_ParentId='" + article_id);
            strSql.Append("' order by P_Number asc");
            DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];

            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.P_Video model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.P_Video();
                    if (dt.Rows[n]["P_Id"] != null && dt.Rows[n]["P_Id"].ToString() != "")
                    {
                        model.P_Id = dt.Rows[n]["P_Id"].ToString();
                    }
                    if (dt.Rows[n]["P_ParentId"] != null && dt.Rows[n]["P_ParentId"].ToString() != "")
                    {
                        model.P_ParentId = dt.Rows[n]["P_ParentId"].ToString();
                    }
                    if (dt.Rows[n]["P_VideoName"] != null && dt.Rows[n]["P_VideoName"].ToString() != "")
                    {
                        model.P_VideoName = dt.Rows[n]["P_VideoName"].ToString();
                    }
                    if (dt.Rows[n]["P_VideoPic"] != null && dt.Rows[n]["P_VideoPic"].ToString() != "")
                    {
                        model.P_VideoPic = dt.Rows[n]["P_VideoPic"].ToString();
                    }
                    if (dt.Rows[n]["P_Url"] != null && dt.Rows[n]["P_Url"].ToString() != "")
                    {
                        model.P_Url = dt.Rows[n]["P_Url"].ToString();
                    }
                    //if (dt.Rows[n]["P_PlayTime"] != null && dt.Rows[n]["P_PlayTime"].ToString() != "")
                    //{
                    //    model.P_PlayTime = int.Parse(dt.Rows[n]["P_PlayTime"].ToString());
                    //}
                    if (dt.Rows[n]["P_CreateTime"] != null && dt.Rows[n]["P_CreateTime"].ToString() != "")
                    {
                        model.P_CreateTime = DateTime.Parse(dt.Rows[n]["P_CreateTime"].ToString());
                    }
                    if (dt.Rows[n]["P_CreateUser"] != null && dt.Rows[n]["P_CreateUser"].ToString() != "")
                    {
                        model.P_CreateUser = dt.Rows[n]["P_CreateUser"].ToString();
                    }
                    if (dt.Rows[n]["P_UpdateTime"] != null && dt.Rows[n]["P_UpdateTime"].ToString() != "")
                    {
                        model.P_UpdateTime = DateTime.Parse(dt.Rows[n]["P_UpdateTime"].ToString());
                    }
                    if (dt.Rows[n]["P_UpdateUser"] != null && dt.Rows[n]["P_UpdateUser"].ToString() != "")
                    {
                        model.P_UpdateUser = dt.Rows[n]["P_UpdateUser"].ToString();
                    }
                    if (dt.Rows[n]["P_Status"] != null && dt.Rows[n]["P_Status"].ToString() != "")
                    {
                        model.P_Status = int.Parse(dt.Rows[n]["P_Status"].ToString());
                    }
                    if (dt.Rows[n]["P_Number"] != null && dt.Rows[n]["P_Number"].ToString() != "")
                    {
                        model.P_Number = int.Parse(dt.Rows[n]["P_Number"].ToString());

                    }
                    if (dt.Rows[n]["P_VideoLength"] != null && dt.Rows[n]["P_VideoLength"].ToString() != "")
                    {
                        model.P_VideoLength = int.Parse(dt.Rows[n]["P_VideoLength"].ToString());

                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        #endregion
    }
}

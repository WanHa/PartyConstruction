using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public partial class Partystyle_logic
    {
        /// <summary>
        /// 是否存在该记录（根据ID）
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_ActivityStyle");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        public Partystyle_logic() { }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string userid)
        {

            //Model.P_ActivityStyle oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_ActivityStyle set ");
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
                    catch (Exception ex)
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
        /// </summary>s
        public bool WebEdit(WebAdd model)
        {
            //Model.P_ActivityStyle oldModel = GetModel(model.P_Id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_ActivityStyle set ");
                        strSql.Append("P_ActivityName=@P_ActivityName,P_ActivityStartTime=@P_ActivityStartTime,P_Sponsor=@P_Sponsor,");
                        strSql.Append("P_ActivitySite=@P_ActivitySite,P_Particular=@P_Particular,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,P_ActivityEndTime=@P_ActivityEndTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_ActivityName", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ActivityStartTime", SqlDbType.DateTime),
                                new SqlParameter("@P_Sponsor", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ActivitySite", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Particular", SqlDbType.NText),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_ActivityEndTime", SqlDbType.DateTime),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50)};
                        parameters[0].Value = model.name;
                        parameters[1].Value = Convert.ToDateTime(model.starttime);
                        parameters[2].Value = model.sponsor;
                        parameters[3].Value = model.site;
                        parameters[4].Value = model.particular;
                        parameters[5].Value = DateTime.Now;
                        parameters[6].Value = Convert.ToDateTime(model.endtime);
                        parameters[7].Value = model.userid;
                        parameters[8].Value = model.id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        //根据活动id删除子表人员数据
                        StringBuilder ss = new StringBuilder();
                        ss.Append("delete from P_ActivityStyleSublist where P_Relation=@P_Relation");
                        SqlParameter[] par = {
                                new SqlParameter("@P_Relation", SqlDbType.NVarChar,50),
                        };
                        par[0].Value = model.id;
                        object objq = DbHelperSQL.GetSingle(conn, trans, ss.ToString(), par);
                        //去重添加
                        if (model.user != null && model.user.Count > 0)
                        {
                            string arry = string.Empty;
                            for (int i = 0; i < model.user.Count; i++)
                            {
                                if (i == model.user.Count - 1)
                                {
                                    arry += model.user[i];
                                }
                                else
                                {
                                    arry += model.user[i] + ",";
                                }
                            }
                            StringBuilder sqll = new StringBuilder();
                            sqll.Append("select id from dt_users where id in(" + arry + ")");
                            DataSet ds = DbHelperSQL.Query(sqll.ToString());
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                StringBuilder strr = new StringBuilder();
                                strr.Append("insert into P_ActivityStyleSublist(");
                                strr.Append("P_Id,P_Relation,P_Participant,P_CreateTime,P_Status)");
                                strr.Append("values(");
                                strr.Append("@P_Id,@P_Relation,@P_Participant,@P_CreateTime,@P_Status)");
                                SqlParameter[] pa =
                                {
                                    new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_Relation",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_Participant",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                    new SqlParameter("@P_Status",SqlDbType.Int),
                                    };
                                pa[0].Value = Guid.NewGuid().ToString();
                                pa[1].Value = model.id;
                                pa[2].Value = dr[0];
                                pa[3].Value = DateTime.Now;
                                pa[4].Value = 0;
                                object ob1 = DbHelperSQL.GetSingle(conn, trans, strr.ToString(), pa);
                            }
                        }
                        //添加图片

                        StringBuilder sql = new StringBuilder();
                        sql.Append("update P_Image set");
                        sql.Append(" P_ImageUrl=@P_ImageUrl,P_UpdateTime=@P_UpdateTime,P_UpdateUser=@P_UpdateUser");
                        sql.Append(" where P_ImageId=@P_ImageId");
                        SqlParameter[] parameter =
                        {
                            new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                            new SqlParameter("@P_UpdateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_UpdateUser",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36),
                            new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100),
                        };
                        parameter[0].Value = new QiNiuHelper().GetQiNiuFileUrl(model.img_name);
                        parameter[1].Value = DateTime.Now;
                        parameter[2].Value = model.userid;
                        parameter[3].Value = model.id;
                        parameter[4].Value = model.img_name;
                        object objs = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameter);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                    trans.Commit();
                }
            }
            return true;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean StyleAdd(WebAdd model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_ActivityStyle(");
                        strSql.Append("P_Id,P_ActivityName,P_ActivitySite,P_Sponsor,P_Particular,P_ActivityStartTime,P_ActivityEndTime,P_CreateTime,P_CreateUser,P_Status) ");
                        strSql.Append("values(");
                        strSql.Append("@P_Id,@P_ActivityName,@P_ActivitySite,@P_Sponsor,@P_Particular,@P_ActivityStartTime,@P_ActivityEndTime,@P_CreateTime,@P_CreateUser,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ActivityName",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_ActivitySite",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_Sponsor",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_Particular",SqlDbType.NText),
                            new SqlParameter("@P_ActivityStartTime",SqlDbType.DateTime),
                            new SqlParameter("@P_ActivityEndTime",SqlDbType.DateTime),
                            new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Status",SqlDbType.Int,4),
                        };
                        string id = Guid.NewGuid().ToString();
                        parameters[0].Value = id;
                        parameters[1].Value = model.name; 
                        parameters[2].Value = model.site;
                        parameters[3].Value = model.sponsor;
                        parameters[4].Value = model.particular;
                        parameters[5].Value = Convert.ToDateTime(model.starttime);
                        parameters[6].Value = Convert.ToDateTime(model.endtime);
                        parameters[7].Value = DateTime.Now;
                        parameters[8].Value = model.userid;
                        parameters[9].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        if(model.user != null && model.user.Count > 0)
                        {
                            string arry = string.Empty;
                            for (int i = 0; i < model.user.Count; i++)
                            {
                                if (i== model.user.Count-1)
                                {
                                    arry += model.user[i];
                                }
                                else
                                {
                                    arry += model.user[i] + ",";
                                }
                            }
                            StringBuilder sqll = new StringBuilder();
                            sqll.Append("select id from dt_users where id in("+arry+")");
                            DataSet ds = DbHelperSQL.Query(sqll.ToString());
                            foreach(DataRow dr in ds.Tables[0].Rows)
                            {
                                StringBuilder ss = new StringBuilder();
                                ss.Append("insert into P_ActivityStyleSublist(");
                                ss.Append("P_Id,P_Relation,P_Participant,P_CreateTime,P_Status)");
                                ss.Append("values(");
                                ss.Append("@P_Id,@P_Relation,@P_Participant,@P_CreateTime,@P_Status)");
                                SqlParameter[] pa =
                                {
                                    new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_Relation",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_Participant",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                    new SqlParameter("@P_Status",SqlDbType.Int),
                                    };
                                pa[0].Value = Guid.NewGuid().ToString();
                                pa[1].Value = id;
                                pa[2].Value = dr[0];
                                pa[3].Value = DateTime.Now;
                                pa[4].Value = 0;
                                object ob = DbHelperSQL.GetSingle(conn, trans, ss.ToString(), pa);
                            }
                        }
                        StringBuilder sql = new StringBuilder();
                        sql.Append("insert into P_Image(");
                        sql.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_Status,P_PictureName,P_ImageType)");
                        sql.Append("values(");
                        sql.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_Status,@P_PictureName,@P_ImageType)");
                        SqlParameter[] parameter =
                        {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,36),
                            new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36),
                            new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                            new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_Status",SqlDbType.Int),
                            new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_ImageType",SqlDbType.NVarChar,50),
                        };
                        parameter[0].Value = Guid.NewGuid().ToString();
                        parameter[1].Value = id;
                        parameter[2].Value = new QiNiuHelper().GetQiNiuFileUrl(model.img_name);
                        parameter[3].Value = DateTime.Now;
                        parameter[4].Value = 0;
                        parameter[5].Value = model.img_name;
                        parameter[6].Value = (int)ImageTypeEnum.活动风采;
                        object objs = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameter);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                    trans.Commit();
                }
            }
            return true;
        }
        public List<Model.P_ActivityStyle> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,(SELECT P_ImageUrl FROM P_Image WHERE P_ImageType='20016'AND P_Image.P_ImageId=P_ActivityStyle.P_Id)as img_url");
            strSql.Append(" FROM P_ActivityStyle");
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
        public List<Model.P_ActivityStyle> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<Model.P_ActivityStyle> list = new List<Model.P_ActivityStyle>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataRowToModel(row));
            }
            return list;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Webstyle GetDetail(string id)
        {
            Webstyle model = new Webstyle();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT CONVERT(varchar(100), P_ActivityStartTime, 20) as starttime,CONVERT(varchar(100), P_ActivityEndTime, 20) as endtime,P_ActivityName as name,P_ActivitySite as site,P_Sponsor as sponsor,P_Particular as particular,");
            strSql.Append("(SELECT P_PictureName FROM P_Image WHERE P_ImageType='20016'AND P_Image.P_ImageId=@id)as img_name ");
            strSql.Append(" FROM P_ActivityStyle where P_Id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = id;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataSetToModelHelper<Webstyle> helper = new DataSetToModelHelper<Webstyle>();
                model = helper.FillToModel(ds.Tables[0].Rows[0]);

                StringBuilder ss = new StringBuilder();
                ss.Append("select P_Participant as id from P_ActivityStyleSublist where P_Relation='"+id+"'");
                DataSet das = DbHelperSQL.Query(ss.ToString());
                List<string> users = new List<string>();
                if (das.Tables[0] != null && das.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in das.Tables[0].Rows)
                    {
                        users.Add(dr["id"].ToString());
                    }
                }
                model.users = users;
            }
            else
            {
                model= null;
            }
            return model;
        }
        /// <summary>
        /// 获取所有党员
        /// </summary>
        /// <returns></returns>
        public List<ZTreeModel> GetMem(ZTreeUserFromBody fromBody)
        {
            List<ZTreeModel> model = new List<ZTreeModel>();
            StringBuilder str = new StringBuilder();
            str.Append("select id as id,user_name as name ");
            if (fromBody.check_user !=null && fromBody.check_user.Count > 0) {
                StringBuilder checkUsersSql = new StringBuilder();
                foreach (int item in fromBody.check_user)
                {
                    checkUsersSql.Append("'");
                    checkUsersSql.Append(item);
                    checkUsersSql.Append("',");
                }

                str.Append(",CONVERT(bit,case when id in("+ checkUsersSql.Remove(checkUsersSql.Length -1,1).ToString()
                    + ") then 'true' else 'false' end) as checked");
            }
            str.Append(" from dt_users where group_id like '%," + fromBody.id + ",%'");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<ZTreeModel> helper = new DataSetToModelHelper<ZTreeModel>();
            if (ds.Tables[0] != null &&ds.Tables[0].Rows.Count > 0)
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
        /// 获取组织以及下属组织所有人员列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<ZTreeModel> GetGroupAllUsers(string groupId) {

            List<ZTreeModel> result = new List<ZTreeModel>();
            StringBuilder str = new StringBuilder();
            str.Append("select id as id,user_name as name ");
            str.Append(" from dt_users where group_id like '%," + groupId + ",%'");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<ZTreeModel> helper = new DataSetToModelHelper<ZTreeModel>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                result = helper.FillModel(ds);
                GetLowerGroupUsers(groupId, result);
            }
            else
            {
                result = null;
            }
            return result;

        }

        private void GetLowerGroupUsers(string groupId, List<ZTreeModel> result) {

            string groupSql = String.Format(@"select * from dt_user_groups where pid = '{0}' ", groupId);
            DataSet ds = DbHelperSQL.Query(groupSql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("select id as id,user_name as name ");
                    str.Append(" from dt_users where group_id like '%," + item["id"].ToString() + ",%'");
                    DataSet userDs = DbHelperSQL.Query(str.ToString());
                    DataSetToModelHelper<ZTreeModel> helper = new DataSetToModelHelper<ZTreeModel>();
                    if (userDs.Tables[0] != null && userDs.Tables[0].Rows.Count > 0)
                    {
                        result.AddRange(helper.FillModel(userDs));
                    }
                    GetLowerGroupUsers(item["id"].ToString(), result);
                }
            }
        }

        public class Webstyle
        {
            public string starttime { get; set; }
            public string endtime { get; set; }
            public string name { get; set; }
            public string site { get; set; }
            public string sponsor { get; set; }
            public string particular { get; set; }
            public string img_name { get; set; }
            public List<string> users { get; set; }
        }
        public class WebAdd
        {
            public string id { get; set; }
            public string name { get; set; }
            public string site { get; set; }
            public string starttime { get; set; }
            public string endtime { get; set; }
            /// <summary>
            /// 主办单位
            /// </summary>
            public string sponsor { get; set; }
            /// <summary>
            /// 详细内容
            /// </summary>
            public string particular { get; set; }
            public string userid { get; set; }
            public string img_name { get; set; }
            public DateTime updatetime { get; set; }
            public List<string> user { get; set; }
        }
        public class Persons
        {
            public int userid { get; set; }
        }
        public Model.P_ActivityStyle DataRowToModel(DataRow row)
        {
            Model.P_ActivityStyle model = new Model.P_ActivityStyle();
            if (row != null)
            {
                #region 主表信息======================
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_ActivityName"] != null && row["P_ActivityName"].ToString() != "")
                {
                    model.P_ActivityName = row["P_ActivityName"].ToString();
                }
                if (row["P_ActivitySite"] != null && row["P_ActivitySite"].ToString() != "")
                {
                    model.P_ActivitySite = row["P_ActivitySite"].ToString();
                }
                if (row["P_ActivityStartTime"] != null)
                {
                    model.P_ActivityStartTime = Convert.ToDateTime(row["P_ActivityStartTime"].ToString());
                }
                if (row["P_ActivityEndTime"] != null)
                {
                    model.P_ActivityEndTime = Convert.ToDateTime(row["P_ActivityEndTime"].ToString());
                }
                if (row["P_Sponsor"] != null && row["P_Sponsor"].ToString() != "")
                {
                    model.P_Sponsor = row["P_Sponsor"].ToString();
                }
                if (row["P_Particular"] != null && row["P_Particular"].ToString() != "")
                {
                    model.P_Particular = row["P_Particular"].ToString();
                }
                if (row["P_CreateTime"] != null)
                {
                    model.P_CreateTime = Convert.ToDateTime(row["P_CreateTime"].ToString());
                }
                if (row["P_CreateUser"] != null)
                {
                    model.P_CreateUser = row["P_CreateUser"].ToString();
                }
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = Convert.ToInt32(row["P_Status"]);
                }
                if(row["img_url"] !=null && row["img_url"].ToString() != "")
                {
                    model.img_url = row["img_url"].ToString();
                }
                #endregion
            }
            return model;
        }
    }
}

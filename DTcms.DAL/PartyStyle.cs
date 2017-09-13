using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DTcms.DAL
{
    public class PartyStyle
    {
        private UserGroupHelper usergroup = new UserGroupHelper();
        /// <summary>
        /// 获取党建风采列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<partyStyleModel> GetPartyStyleList(int userid, int rows, int page)
        {
            List<partyStyleModel> model = new List<partyStyleModel>();
            StringBuilder str = new StringBuilder();
            str.Append(" select * from dt_article where dt_article.category_id=56");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, str.ToString(), "dt_article.sort_id"));
            DataSetToModelHelper<DTcms.Model.article> article = new DataSetToModelHelper<DTcms.Model.article>();
            List<DTcms.Model.article> aaa = new List<Model.article>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                aaa = article.FillModel(dt);
                foreach (var item in aaa)
                {
                    int grouid = usergroup.GetUserMinimumGroupId(item.user_id);
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("select dt_article.id,dt_article.title,(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null ) as imgurl,CONVERT(varchar(100), dt_article.add_time,23) as createtime,dt_user_groups.title as organ,");
                    strsql.Append("(select count (P_Transmit.P_Id) from P_Transmit 	where P_Transmit.P_RelationId=CAST(dt_article.id as nvarchar)) as trancount,");
                    strsql.Append("(select count(dt_article_comment.id) from dt_article_comment 	where dt_article_comment.article_id=dt_article.id) as comcount,");
                    strsql.Append("(select count(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=CAST(dt_article.id as nvarchar) and  P_ThumbUp.P_FamilyType=0 ) as upcount, ");
                    strsql.Append("(select count(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=CAST(dt_article.id as nvarchar) and P_ThumbUp.P_UserId='" + userid + @"' and  P_ThumbUp.P_FamilyType=0  ) as userup ");
                    strsql.Append(" from dt_article ");
                    strsql.Append(" LEFT JOIN dt_users on dt_users.id=dt_article.user_id ");
                    strsql.Append(" left join dt_user_groups on dt_user_groups.id=" + grouid + @" ");
                    strsql.Append(" where dt_article.category_id=56 and dt_article.id=" + item.id + @"");
                    DataSet dd = DbHelperSQL.Query(strsql.ToString());
                    DataSetToModelHelper<partyStyleModel> stylemodel = new DataSetToModelHelper<partyStyleModel>();
                    partyStyleModel style = new partyStyleModel();
                    if (dt.Tables[0].Rows.Count != 0)
                    {
                        style = stylemodel.FillToModel(dd.Tables[0].Rows[0]);
                    }
                    else
                    {
                        style = null;
                    }
                    model.Add(style);
                }
            }
            else
            {
                model = null;

            }
          
            return model;

        }
        /// <summary>
        /// 获取评论接口分页
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<commentModel> GetComment(int userid, int id, int rows, int page)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select  CONVERT(varchar(100), dt_article_comment.add_time,120) as createtime,");
            strsql.Append(" dt_article_comment.id as id,dt_users.user_name as username,");
            strsql.Append(" dt_article_comment.content as content,");
            strsql.Append("(select P_ImageUrl from P_Image where P_ImageId=CAST(dt_article_comment.user_id AS VARCHAR) and P_ImageType='"+(int)ImageTypeEnum.头像+@"') as avatar,");
            strsql.Append(" (select COUNT(P_ThumbUp.P_Id) from P_ThumbUp where  P_ThumbUp.P_ArticleId=CONVERT(VARCHAR,dt_article_comment.id) and P_ThumbUp.P_FamilyType=1 ) as trumcount,");
            strsql.Append(" (select count(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=CONVERT(VARCHAR,dt_article_comment.id) and P_ThumbUp.P_UserId=" + userid + @" and P_ThumbUp.P_FamilyType=1 ) as trumuser");
            strsql.Append(" from dt_article_comment");
            strsql.Append(" left join dt_users on dt_users.id =dt_article_comment.user_id");
            strsql.Append(" where dt_article_comment.article_id=" + id + @"");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "dt_article_comment.add_time desc"));
            DataSetToModelHelper<commentModel> model = new DataSetToModelHelper<commentModel>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                return model.FillModel(dt);
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailPartyStyleModel DetailPartyStyle(int id)
        {
            DetailPartyStyleModel dps = new DetailPartyStyleModel();
            StringBuilder str = new StringBuilder();
            str.Append("select * from dt_article where dt_article.id="+id+@" ");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<DTcms.Model.article> article = new DataSetToModelHelper<DTcms.Model.article>();
            DTcms.Model.article aaa = new Model.article();
            if (ds.Tables[0].Rows.Count != 0)
            {
                aaa = article.FillToModel(ds.Tables[0].Rows[0]);
                int groupid = usergroup.GetUserMinimumGroupId(aaa.user_id);
               
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select dt_article.id as id,dt_article.title as title,dt_user_groups.title as organ,dt_article.content as content,");
                strsql.Append("(SELECT P_ImageUrl as imgurl  from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null ) as imgurl,CONVERT(varchar(100), dt_article.add_time,120) as createtime,");
                strsql.Append("(select count(dt_article_comment.id)  from dt_article_comment where dt_article_comment.article_id=dt_article.id) as comcount ");
                strsql.Append(" from dt_article");
                strsql.Append(" LEFT JOIN dt_users on dt_users.id=dt_article.user_id");
                strsql.Append(" left join dt_user_groups on dt_user_groups.id=" + groupid + @"");
                strsql.Append(" where dt_article.id=" + id + @"");
                DataSet dss = DbHelperSQL.Query(strsql.ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dps = DataRowToModel(dss.Tables[0].Rows[0]);
                    string[] reg = { @"[<].*?[>]", @"\r", @"&emsp", @"&nbsp", @"\n", @"\t" };
                    for (int i = 0; i < reg.Length; i++)
                    {
                        Regex regex = new Regex(reg[i], RegexOptions.IgnoreCase);
                        dps.content = Regex.Replace(dps.content, reg[i], "");
                    }
                    dps.content = Regex.Replace(dps.content, @";;;", @";");
                    dps.content = Regex.Replace(dps.content, @";;", ";");
                    dps.content = Regex.Replace(dps.content, @".;", ".");
                }
                else
                {
                    dps = null;
                }
            }
            else
            {
                dps = null;
            }
           

            return dps;
        }
        /// <summary>
        /// 党建风采评论接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean InsertComment(PartyCommentModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into dt_article_comment(");
                        strSql.Append("article_id,user_id,user_name,content)");
                        strSql.Append(" values (");
                        strSql.Append("@article_id,@user_id,@user_name,@content)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@article_id", SqlDbType.Int,4),
                                new SqlParameter("@user_id", SqlDbType.Int,4),
                                new SqlParameter("@user_name", SqlDbType.NVarChar,100),
                                new SqlParameter("@content", SqlDbType.NVarChar,100),
                               };

                        parameters[0].Value = model.articleid;
                        parameters[1].Value = model.userid;
                        parameters[2].Value = model.username;
                        parameters[3].Value = model.content;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 点赞接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Boolean GetThumbUp(int userid, string id, int familytype)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_ThumbUp(");
                        strSql.Append("P_Id,P_UserId,P_ArticleId,P_CreateTime,P_CreateUser,P_Status,P_FamilyType)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_UserId,@P_ArticleId,@P_CreateTime,@P_CreateUser,@P_Status,@P_FamilyType)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.Int,4),
                                new SqlParameter("@P_ArticleId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_FamilyType", SqlDbType.Int,4),
                               };

                        parameters[0].Value = Guid.NewGuid().ToString("N");
                        parameters[1].Value = userid;
                        parameters[2].Value = id;
                        parameters[3].Value = DateTime.Now;
                        parameters[4].Value = userid;
                        parameters[5].Value = 0;
                        parameters[6].Value = familytype;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 删除点赞接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DelThumbUp(int userid, string id, int familytype)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("delete from P_ThumbUp ");
                        strSql.Append("where P_UserId='" + userid + @"' and P_ArticleId='" + id + @"' and  P_FamilyType=" + familytype + @" ");

                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString()); //带事务
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取前4文章id及图片url,按时间倒序
        /// </summary>
        /// <returns></returns>
        public List<PartyStyleImageModel>  GetArticleImage( string userId)
        {
            List<PartyStyleImageModel> result;
            try
            {
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select top 4 dt_article.id as id, P_Image.P_ImageUrl as imageUrl, " +
                                        " (case when (select count(*) from P_ThumbUp" +
                                        " where P_ThumbUp.P_ArticleId=convert(nvarchar,id)and P_ThumbUp.P_UserId="+ userId + " and P_ThumbUp.P_Status=0)>0 then 1" +
                                        " else 0 end) as thumbs" +
                                         " from dt_article");
                strsql.Append(" left join P_Image on P_ImageId = convert(nvarchar,dt_article.id) and P_ImageType is NULL");
                strsql.Append(" where category_id = 56");
                strsql.Append(" order by add_time DESC");

                DataSet ds = DbHelperSQL.Query(strsql.ToString());
                DataSetToModelHelper<PartyStyleImageModel> helper = new DataSetToModelHelper<PartyStyleImageModel>();
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    result = helper.FillModel(ds);
                }
                else
                {
                    result = null;
                }

            }
            catch (Exception ex)
            {
                result = null;
                throw;
            }

            return result;
        }
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public DetailPartyStyleModel DataRowToModel(DataRow row)
        {
            DetailPartyStyleModel model = new DetailPartyStyleModel();
            if (row != null)
            {
                #region 主表信息======================
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = Convert.ToInt32(row["id"]);
                }
                if (row["title"] != null && row["title"].ToString() != "")
                {
                    model.title = row["title"].ToString();
                }
                if (row["content"] != null && row["content"].ToString() != "")
                {
                    model.content = row["content"].ToString();
                }
                if (row["imgurl"] != null && row["imgurl"].ToString() != "")
                {
                    model.imgurl = row["imgurl"].ToString();
                }
                if (row["createtime"] != null && row["createtime"].ToString() != "")
                {
                    model.createtime = row["createtime"].ToString();
                }
                if (row["organ"] != null && row["organ"].ToString() != "")
                {
                    model.organ = row["organ"].ToString();
                }
                if (row["comcount"] != null && row["comcount"].ToString() != "")
                {
                    model.comcount = Convert.ToInt32(row["comcount"]);
                }
                #endregion
            }
            return model;
        }

        /// <summary>
        /// 将datatable转换为list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<comment> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<comment> list = new List<comment>();
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
        public comment DataComment(DataRow row)        {            comment com = new comment();            if (row != null)            {
                #region 主表信息======================
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    com.id = Convert.ToInt32(row["id"]);
                }
                if (row["username"] != null && row["username"].ToString() != "")
                {
                    com.username = row["id"].ToString();
                }
                if (row["content"] != null && row["content"].ToString() != "")
                {
                    com.content = row["id"].ToString();
                }
                if (row["createtime"] != null && row["createtime"].ToString() != "")
                {
                    com.createtime = Convert.ToDateTime(row["createtime"]);
                }
                if (row["trumcount"] != null && row["trumcount"].ToString() != "")
                {
                    com.trumcount = Convert.ToInt32(row["trumcount"]);
                }
                if (row["avatar"] != null && row["avatar"].ToString() != "")
                {
                    com.avatar = row["avatar"].ToString();
                }
                if (Convert.ToInt32(row["trumuser"]) != 0)
                {
                    com.trumuser = 1;
                }
                else
                {
                    com.trumuser = 0;
                }
                #endregion
            }            return com;        }
    }
    public class partyStyleModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string imgurl { get; set; }
        public string createtime { get; set; }
        public string organ { get; set; }
        public int trancount { get; set; }
        public int comcount { get; set; }
        public int upcount { get; set; }
        public int userup { get; set; }
    }
    public class commentModel
    {
        public int id { get; set; }
        private string _createtime;
        public string createtime
        {
            get
            {
                return DateTime.Parse(_createtime == null ? "" : _createtime).ToString("MM-dd HH-mm");
            }
            set
            {
                _createtime = value;
            }
        }

        public string username { get; set; }
        public string content { get; set; }
        public string avatar { get; set; }
        public int trumcount { get; set; }
        public int trumuser { get; set; }
    }
}

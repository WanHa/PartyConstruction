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
   public class PartyBuilding
    {
        private UserGroupHelper usergroup = new UserGroupHelper();
        /// <summary>
        /// 获取cms接口
        /// </summary>
        /// <param name="buil"></param>
        /// <returns></returns>
        public List<BuildingModel> GetPartyNewsList(BuildModel buil)
        {
            StringBuilder strsql = new StringBuilder();

            switch (buil.categoryid)
            {
                case 80:
                    strsql.Append("select dt_article.id as id,dt_article.title as title,(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null ) as imgurl,CONVERT(varchar(100), dt_article.add_time,120) as createtime,dt_article.user_name as username  ");
                    strsql.Append(" from dt_article ");
                    strsql.Append(" where dt_article.category_id in (select id from dt_article_category where dt_article_category.channel_id = 16 ) and dt_article.status=0 ");
                    break;
                case 61:
                    strsql.Append("select dt_article.id as id,dt_article.title as title,(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null ) as imgurl,CONVERT(varchar(100), dt_article.add_time,120) as createtime,dt_article.user_name as username  ");
                    strsql.Append(" from dt_article ");
                    strsql.Append(" where dt_article.category_id in (select id from dt_article_category where dt_article_category.channel_id = 21 ) and dt_article.status=0 ");
                    break;
                case 64:
                    strsql.Append("select dt_article.id as id,dt_article.title as title, ");
                    strsql.Append("(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType ="+(int)ImageTypeEnum.党委通知+" ) as imgurl,");
                    strsql.Append("CONVERT(varchar(100), dt_article.add_time,120) as createtime,dt_article.user_name as username ");
                    strsql.Append("from dt_article ");
                    strsql.Append("where dt_article.id in (");
                    strsql.Append("select P_PartyCommitteeNotify.P_Relation from P_PartyCommitteeNotify where P_PartyCommitteeNotify.P_ToUser = " + buil.userid);
                    strsql.Append(") ");
                    break;
                default:
                    strsql.Append("select dt_article.id as id,dt_article.title as title,(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null ) as imgurl,CONVERT(varchar(100), dt_article.add_time,120) as createtime,dt_article.user_name as username  ");
                    strsql.Append(" from dt_article ");
                    strsql.Append(" where dt_article.category_id=" + buil.categoryid + @" and dt_article.status=0 ");
                    break;
            }
            //if (buil.categoryid != 80)
            //{
            //    strsql.Append("select dt_article.id as id,dt_article.title as title,(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null ) as imgurl,CONVERT(varchar(100), dt_article.add_time,120) as createtime,dt_article.user_name as username  ");
            //    strsql.Append(" from dt_article ");
            //    strsql.Append(" where dt_article.category_id=" + buil.categoryid + @" and dt_article.status=0 ");
            //}
            //else
            //{
            //    strsql.Append("select dt_article.id as id,dt_article.title as title,(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null ) as imgurl,CONVERT(varchar(100), dt_article.add_time,120) as createtime,dt_article.user_name as username  ");
            //    strsql.Append(" from dt_article ");
            //    strsql.Append(" where dt_article.category_id in (select id from dt_article_category where dt_article_category.channel_id = 16 ) and dt_article.status=0 ");
            //}
            
            if (buil.where != null)
            {
                strsql.Append(" and dt_article.title like  '%"+buil.where+@"%' ");
            }

            string orderSql = String.Format(@"select channel_id from dt_article_category
                where id = {0}", buil.categoryid);
            int categoryId = Convert.ToInt32(DbHelperSQL.GetSingle(orderSql)) ;
            StringBuilder orderBySql = new StringBuilder();
            if (buil.categoryid == 57 || buil.categoryid == 2113 || categoryId == 16)
            {
                orderBySql.Append("dt_article.add_time asc");
            }
            else {
                orderBySql.Append("dt_article.add_time desc");
            }

            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(buil.rows, buil.page, strsql.ToString(), orderBySql.ToString()));
            DataSetToModelHelper<BuildingModel> model = new DataSetToModelHelper<BuildingModel>();
            if (dt != null)
            {
                return model.FillModel(dt);
            }
            else
            {
                return null;
            }
          
        }
        /// <summary>
        /// 获取cms详情接口 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailPaperModel SelPartyNewsPeper(int userid, int id)
        {
            DetailPaperModel model = new DetailPaperModel();
            StringBuilder str = new StringBuilder();
            str.Append(" select * from dt_article where id="+id+@" ");
            DataSet dt = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<DTcms.Model.article> article = new DataSetToModelHelper<DTcms.Model.article>();
            DTcms.Model.article artcile = new Model.article();
            if (dt.Tables[0].Rows.Count != 0)
            {
                artcile = article.FillToModel(dt.Tables[0].Rows[0]);
                int groupid = usergroup.GetUserMinimumGroupId(artcile.user_id);
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select dt_article.id as id,(SELECT P_ImageUrl from P_Image where P_ImageId=CAST(dt_article.id as nvarchar) and P_ImageType is null) as imgurl,dt_article.content as content,dt_article.title as title,");
                strsql.Append("dt_article.user_name as username,dt_user_groups.title as groupname,CONVERT(varchar(100), dt_article.add_time,120) as createtime,");
                //strsql.Append("(select p_id from p_collect where p_collect.p_relation = dt_article.id and p_collect.p_userid ="+userid+@" ) as collectid,");
                strsql.Append(" (select count(p_collect.p_id) from p_collect where p_collect.p_relation = CAST(dt_article.id as nvarchar)) as collectcount,");
                strsql.Append("(select count(p_transmit.p_id) from p_transmit where p_transmit.p_relationid=CAST(dt_article.id as nvarchar)) as trankcount,");
                strsql.Append("(select count(p_collect.P_id) from p_collect where p_collect.p_relation = CAST(dt_article.id as nvarchar) and p_collect.p_userid =" + userid + @") as collect from dt_article ");
                strsql.Append(" left join p_collect on p_collect.p_relation=CAST(dt_article.id as nvarchar) ");
                strsql.Append(" left join p_transmit on p_transmit.p_relationid=CAST(dt_article.id as nvarchar) ");
                strsql.Append(" left join dt_users on dt_users.id=dt_article.user_id ");
                strsql.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id="+groupid+@" ");
                strsql.Append(" where dt_article.id=" + id + @" ");
                DataSet ds = DbHelperSQL.Query(strsql.ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    model= DataRowToModel(ds.Tables[0].Rows[0]);
                    if (model.content != null)
                    {
                        string[] reg = { @"[<].*?[>]", @"\r", @"&emsp", @"&nbsp", @"\n", @"\t" };
                        for (int i = 0; i < reg.Length; i++)
                        {
                            Regex regex = new Regex(reg[i], RegexOptions.IgnoreCase);
                            model.content = Regex.Replace(model.content, reg[i], "");
                        }
                        model.content = Regex.Replace(model.content, @";;;", @";");
                        model.content = Regex.Replace(model.content, @";;", ";");
                        model.content = Regex.Replace(model.content, @".;", ".");
                    }
                   

                }
                else
                {
                    model = null;
                }
            }
            else
            {
                model = null;
            }
            return model;
           
        }
        /// <summary>
        /// 添加收藏接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Boolean GetCollect(int userid,string id,string type)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into p_collect(");
                        strSql.Append("P_Id,P_userid,p_relation,p_type,p_collecttime,p_createtime,p_createuser,p_status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_userid,@p_relation,@p_type,@p_collecttime,@p_createtime,@p_createuser,@p_status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_userid", SqlDbType.Int,4),
                                new SqlParameter("@p_relation", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_type", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_collecttime", SqlDbType.Int,4),
                                new SqlParameter("@p_createtime", SqlDbType.DateTime,100),
                                new SqlParameter("@p_createuser", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_status", SqlDbType.Int,4)
                               };
       
                        parameters[0].Value = Guid.NewGuid().ToString("N");
                        parameters[1].Value =userid;
                        parameters[2].Value = id;
                        parameters[3].Value = type;
                        parameters[4].Value =0;
                        parameters[5].Value = DateTime.Now;
                        parameters[6].Value = userid;
                        parameters[7].Value = 0;
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
        /// 删除收藏接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DelectCollect(int userid,string id,string type)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("delete from p_collect  ");
                        strSql.Append("where p_collect.p_relation='"+id+ @"' and p_collect.p_userid="+userid+@" and P_Type='"+type+@"'");
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
                        trans.Commit();
                    }
                    catch (Exception )
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 账号注册加密接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="salt"></param>
        /// <param name="password1"></param>
        /// <returns></returns>
        public Boolean GetRegisterEnroll(string mobile,string salt,string password1)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (Exists(mobile))
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("update dt_users set ");
                            strSql.Append("salt=@salt,");
                            strSql.Append("password=@password,");
                            strSql.Append("status=@status ");
                            strSql.Append(" where mobile=@mobile");
                            SqlParameter[] parameters = {
                                new SqlParameter("@salt", SqlDbType.NVarChar,100),
                                new SqlParameter("@password", SqlDbType.NVarChar,100),
                                new SqlParameter("@status", SqlDbType.Int,50),
                                new SqlParameter("@mobile", SqlDbType.NVarChar,100)};
                            parameters[0].Value = salt;
                            parameters[1].Value = password1;
                            parameters[2].Value = 2;    // 0 正常 1 待验证 2 待审核
                            parameters[3].Value = mobile;
                            DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                            trans.Commit();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                       
                    }
                    catch (Exception )
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
        /// <summary>
		/// 是否存在该记录（根据ID）
		/// </summary>
		public bool Exists(string mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_users");
            strSql.Append(" where mobile='"+ mobile + @"' ");
            return DbHelperSQL.Exists(strSql.ToString());
        }
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public DetailPaperModel DataRowToModel(DataRow row)
        {
            DetailPaperModel model = new DetailPaperModel();
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
                if (row["createtime"] != null&& row["createtime"].ToString()!="")
                {
                    model.createtime = row["createtime"].ToString();
                }
                if (row["collectcount"] != null && row["collectcount"].ToString() != "")
                {
                    model.collectcount = Convert.ToInt32(row["collectcount"]);
                }
                if (row["trankcount"] != null && row["trankcount"].ToString() != "")
                {
                    model.trankcount = Convert.ToInt32(row["trankcount"]);
                }
                //1 收藏 0 未收藏
                if (Convert.ToInt32(row["collect"]) != 0)
                {
                    model.collect = 1;
                }
                else
                {
                    model.collect = 0;
                }
                if (row["username"] != null && row["username"].ToString() != "")
                {
                    model.username = row["username"].ToString();
                }
                if (row["groupname"] != null && row["groupname"].ToString() != "")
                {
                    model.groupname = row["groupname"].ToString();
                }
                if (row["imgurl"] != null && row["imgurl"].ToString() != "")
                {
                    model.imgurl = row["imgurl"].ToString();
                }
               
                #endregion
            }
            return model;
        }
        public BuildingModel DataToModel(DataRow row)
        {
            BuildingModel model = new BuildingModel();
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
                if (row["createtime"] != null && row["createtime"].ToString() != "")
                {
                    model.createtime = row["createtime"].ToString();
                }

                if (row["username"] != null && row["username"].ToString() != "")
                {
                    model.username = row["username"].ToString();
                }
                if (row["imgurl"] != null && row["imgurl"].ToString() != "")
                {
                    model.imgurl = row["imgurl"].ToString();
                }
                #endregion
            }
            return model;
        }
    }
    public class BuildingModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string imgurl { get; set; }
        public string createtime { get; set; }
        public string username { get; set; }
    }
    public class BuildModel
    {
        public int userid { get; set; }
        public int categoryid { get; set; }
        public int rows { get; set; }
        public int page { get; set; }
        public string where { get; set; }
    }
}
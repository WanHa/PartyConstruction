using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using DTcms.Model.WebApiModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{
    public class LearnExchange
    {
        private QiNiuHelper qiniu = new QiNiuHelper();

        /// <summary>
        /// 学习交流列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<LearnModel> GetLearningList(int userid, int rows, int page)
        {
            List<LearnModel> model = new List<LearnModel>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select P_Id as id,P_Title as title,CONVERT(varchar(100), P_CreateTime,120) as createtime,dt_users.user_name as username ");
            strsql.Append(" from P_LearnExchange ");
            strsql.Append(" LEFT JOIN dt_users on dt_users.id=P_LearnExchange.P_UserId ");
            strsql.Append(" where P_LearnExchange.P_Status=0 and P_LearnExchange.P_AuditState=1 ");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_CreateTime desc"));
            if (dt.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<LearnModel> learn = new DataSetToModelHelper<LearnModel>();
                model = learn.FillModel(dt);
            }
            else
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 学习交流新增接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public DataSet GetXinZeng(int id, int user_id)
        //{
        //    StringBuilder strsql = new StringBuilder();
        //    strsql.Append(@"select dt_users.mobile as mobile,dt_user_groups.title as title from dt_users left join dt_user_groups on dt_user_groups.id = dt_users.group_id where dt_users.id = " + id + @"");
        //    DataSet ds = DbHelperSQL.Query(strsql.ToString());
        //    return ds;
        //}

        /// <summary>
        /// 学习交流提交接口
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public Boolean TiJiao(Aticle article)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_LearnExchange(");
                        strSql.Append("P_Id,P_UserId,P_Title,P_Content,P_AuditState,P_CreateTime,P_CreateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_Id,@P_UserId,@P_Title,@P_Content,@P_AuditState,@P_CreateTime,@P_CreateUser,@P_Status )");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Title", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NText),
                                new SqlParameter("@P_AuditState", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4)
                               };
                        string learnid = Guid.NewGuid().ToString("N");
                        parameters[0].Value = learnid;
                        parameters[1].Value = article.userid;
                        parameters[2].Value = article.title;
                        parameters[3].Value = article.content;
                        parameters[4].Value = 0;
                        parameters[5].Value = DateTime.Now;
                        parameters[6].Value = article.userid;
                        parameters[7].Value = 0;                      
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        if (article.imgurl != null)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("insert into P_Image(");
                            str.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status,P_ImageType,P_PictureName)");
                            str.Append(" values(");
                            str.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_ImageType,@P_PictureName )");
                            str.Append(";select @@IDENTITY");
                            SqlParameter[] sql =
                            {
                                new SqlParameter("@P_Id",SqlDbType.NVarChar,36),
                                new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36),
                                new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                                new SqlParameter("@P_CreateTime",SqlDbType.NVarChar,0),
                                new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status",SqlDbType.Int,4),
                                new SqlParameter("@P_ImageType",SqlDbType.NVarChar,100),
                                new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100)
                            };
                            for (int i = 0; i < article.imgurl.Count; i++)
                            {
                                string imgname = qiniu.GetQiNiuFileUrl(article.imgurl[i].imgname);
                                sql[0].Value = Guid.NewGuid().ToString("N");
                                sql[1].Value = learnid;
                                sql[2].Value = imgname;
                                sql[3].Value = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + (DateTime.Now.Minute+i) + ":" + DateTime.Now.Second;
                                sql[4].Value = article.userid;
                                sql[5].Value = 0;
                                sql[6].Value = (int)ImageTypeEnum.学习交流;
                                sql[7].Value = article.imgurl[i].imgname;
                                object obj1 = DbHelperSQL.GetSingle(conn, trans, str.ToString(), sql); //带事务
                            }
                            //foreach (var item in article.imgurl)
                            //{
                            //    string imgname = qiniu.GetQiNiuFileUrl(item.imgname);
                            //    sql[0].Value = Guid.NewGuid().ToString("N");
                            //    sql[1].Value = learnid;
                            //    sql[2].Value = imgname;
                            //    sql[3].Value = DateTime.Now.Year+"-"+DateTime.Now.Month+"-"+DateTime.Now.Day+" "+DateTime.Now.Hour+":"+(DateTime.Now.Minute)+":"+DateTime.Now.Second;
                            //    sql[4].Value = article.userid;
                            //    sql[5].Value = 0;
                            //    sql[6].Value = (int)ImageTypeEnum.学习交流;
                            //    sql[7].Value = item.imgname;
                            //    object obj1 = DbHelperSQL.GetSingle(conn, trans, str.ToString(), sql); //带事务
                            //}
                        }
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

        /// <summary>
        /// 学习交流详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailLearnModel GetXiangQing(string id)
        {
            StringBuilder sss = new StringBuilder();
            sss.Append("select * from P_LearnExchange where P_id='"+id+@"'");
            DataSet das = DbHelperSQL.Query(sss.ToString());
            DataSetToModelHelper<P_LearnExchange> learnex = new DataSetToModelHelper<P_LearnExchange>();
            P_LearnExchange learning = new P_LearnExchange();
            if (das.Tables[0].Rows.Count != 0)
            {
                 learning = learnex.FillToModel(das.Tables[0].Rows[0]);
            }
            else
            {
                learning = null;
            }
            int qq = Convert.ToInt32(learning.P_UserId);
            UserGroupHelper usergroup = new UserGroupHelper();
            int learnuser = usergroup.GetUserMinimumGroupId(qq);
            DetailLearnModel model = new DetailLearnModel();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_LearnExchange.P_Title as title,CONVERT(varchar(100), P_CreateTime,120) as createtime,dt_users.user_name as username,dt_user_groups.title as organ");
            strsql.Append(",P_LearnExchange.P_Content as content ");
            strsql.Append(" from P_LearnExchange ");
            strsql.Append(" LEFT JOIN dt_users on dt_users.id=P_LearnExchange.P_UserId ");
            strsql.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id="+ learnuser + @" ");
            strsql.Append(" where P_LearnExchange.P_Status=0 and P_LearnExchange.P_Id='"+id+@"' ");
            DataSet ds = DbHelperSQL.Query(strsql.ToString());
            if (ds.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<DetailLearnModel> learn = new DataSetToModelHelper<DetailLearnModel>();
                model = learn.FillToModel(ds.Tables[0].Rows[0]);
                StringBuilder str = new StringBuilder();
                str.Append(" select P_Image.P_ImageUrl as imgurl from P_Image where P_Image.P_ImageId='"+id+ @"' and P_Image.P_ImageType='"+(int)ImageTypeEnum.学习交流 + @"' ");
                DataSet dt = DbHelperSQL.Query(str.ToString());
                if (dt.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<ImageModel> ima = new DataSetToModelHelper<ImageModel>();
                    model.image = ima.FillModel(dt);
                }
                else
                {
                    model.image = null;
                }
            }
            else
            {
                model = null;
            }
            return model;
        }
    }
    public class LearnModel
    {
        public string id { get; set; }
        public string title { get; set; }

        public string createtime
        {
            get;set;
           
        }
        public string username { get; set; }
    }
    public class DetailLearnModel
    {
        public string title { get; set; }
        public string createtime { get; set; }
        public string username { get; set; }
        public string organ { get; set; }
        public string content { get; set; }
        public List<ImageModel> image { get; set; }
    }
    public class ImageModel
    {
        public string imgurl { get; set; }
    }
}

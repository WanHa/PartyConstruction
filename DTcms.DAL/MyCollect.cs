using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class MyCollect
    {
        /// <summary>
        /// 文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MyCollectModel GetMyCollect(string praleid, int userid, string id)
        {
            MyCollectModel model = new MyCollectModel();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select  dt_article.title as title,dt_article.id as courceid,dt_article.content as textcontent, ");
            strsql.Append(" (select P_ImageUrl from P_Image where P_ImageId='" + userid + @"'  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
            strsql.Append(" (select dt_users.user_name from dt_users where dt_users.id=" + userid + @") as username, ");
            strsql.Append(" (select CONVERT(VARCHAR(10),P_CreateTime,120) from P_Collect where P_Id='" + id + @"')as time ");
            strsql.Append(" from dt_article where dt_article.id=" + praleid + @" ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<MyCollectModel> mm = new DataSetToModelHelper<MyCollectModel>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                model = mm.FillToModel(dt.Tables[0].Rows[0]);
                if (model.textcontent != null)
                {
                    string[] reg = { @"[<].*?[>]", @"\r", @"&emsp", @"&nbsp", @"\n", @"\t" };
                    for (int i = 0; i < reg.Length; i++)
                    {
                        Regex regex = new Regex(reg[i], RegexOptions.IgnoreCase);
                        model.textcontent = Regex.Replace(model.textcontent, reg[i], "");
                    }
                    model.textcontent = Regex.Replace(model.textcontent, @";;;", @";");
                    model.textcontent = Regex.Replace(model.textcontent, @";;", ";");
                    model.textcontent = Regex.Replace(model.textcontent, @".;", ".");
                }

            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="praleid">收藏表的关系id</param>
        /// <param name="userid">用户的id</param>
        /// <param name="id">收藏表的id</param>
        /// <returns></returns>
        public MyCollectModel GetCollectVideo(string praleid, int userid, string id)
        {
            MyCollectModel model = new MyCollectModel();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select   P_Video.P_Id as id,P_Video.P_VideoName as videoname,P_Video.P_VideoPic as videopic,CAST(P_Video.P_ParentId as INT) as courceid,");
            strsql.Append(" P_Video.P_Url as videourl,P_Video.P_VideoLength as videolength,");
            strsql.Append(" CAST((select count(a.id) from  ");
            strsql.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a)as INT ) as playcount ,");
            strsql.Append(" (select P_ImageUrl from P_Image where P_ImageId='" + userid + @"'  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
            strsql.Append(" (select dt_users.user_name from dt_users where dt_users.id=" + userid + @") as username, ");
            strsql.Append(" (select CONVERT(VARCHAR(10),P_CreateTime,120) from P_Collect where P_Id='" + id + @"')as time ");
            strsql.Append(" from P_Video ");
            strsql.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
            strsql.Append(" where P_Video.P_Id='" + praleid + @"' and P_Video.P_Source is null ");
            strsql.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_ParentId, P_Video.P_Url ,P_Video.P_VideoLength");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<MyCollectModel> mm = new DataSetToModelHelper<MyCollectModel>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                model = mm.FillToModel(dt.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 收藏总方法
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<MyCollectModel> GetMycollects(int userid, int rows, int page)
        {
            List<MyCollectModel> model = new List<MyCollectModel>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select * from P_Collect where P_UserId='" + userid + "'");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_Collect.P_CreateTime"));
            DataSetToModelHelper<P_Collect> mm = new DataSetToModelHelper<P_Collect>();
            List<P_Collect> collect = mm.FillModel(dt);
            MyCollectModel mycollect = new MyCollectModel();
            if (collect != null)
            {
                foreach (var item in collect)
                {
                    if (Convert.ToInt32(item.P_Type) == 52)
                    {
                        mycollect = GetCollectVideo(item.P_Relation, userid, item.P_Id);
                        if (mycollect != null)
                        {
                            mycollect.type = 52;
                        }
                    }
                    if (Convert.ToInt32(item.P_Type) == 53)
                    {
                        mycollect = GetMyCollect(item.P_Relation, userid, item.P_Id);
                        if (mycollect != null)
                        {
                            mycollect.type = 53;
                        }

                    }
                    if (Convert.ToInt32(item.P_Type) == 54)
                    {
                        mycollect = GetForumCollect(item.P_Relation, userid);
                        if (mycollect != null)
                        {
                            mycollect.type = 54;
                        }
                    }
                    if (Convert.ToInt32(item.P_Type) == 55)
                    {
                        mycollect = GetTranSubmitCollect(item.P_Relation, userid);
                        if (mycollect != null)
                        {
                            mycollect.type = 55;
                        }
                    }
                    if (Convert.ToInt32(item.P_Type) == 56)
                    {
                        mycollect = GetGroupsCollect(item.P_Relation, userid);
                        if (mycollect != null)
                        {
                            mycollect.type = 56;
                        }
                    }
                    if (Convert.ToInt32(item.P_Type) == 57)
                    {
                        mycollect = GetGroupTransCollect(item.P_Relation, userid);
                        if (mycollect != null)
                        {
                            mycollect.type = 57;
                        }
                    }
                    if (Convert.ToInt32(item.P_Type) == 58)
                    {
                        mycollect = GetPlatyCollect(item.P_Relation, userid);
                        if (mycollect != null)
                        {
                            mycollect.type = 58;
                        }
                    }
                    model.Add(mycollect);
                }
            }
            else
            {
                model = null;
            }

            return model;
        }

        /// <summary>
        /// 党小组论坛分享
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MyCollectModel GetForumCollect(string id, int userid)
        {
            MyCollectModel model = new MyCollectModel();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select * from P_ForumShare where P_Id='" + id + @"' and P_ForumShare.P_Source=0 ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<P_ForumShare> mm = new DataSetToModelHelper<P_ForumShare>();
            P_ForumShare share = new P_ForumShare();
            if (dt.Tables[0].Rows.Count != 0)
            {
                share = mm.FillToModel(dt.Tables[0].Rows[0]);
                if (share.P_Type == 0)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select P_id as id,P_Content as content,");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_ForumShare.P_UserId as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append("  dt_users.user_name  as username, ");
                    ss.Append("  CONVERT(VARCHAR(10),P_CreateTime,120) as time ");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_ForumShare.P_Id and P_ThumbUp.P_FamilyType=2 ) as thumbcount, ");
                    ss.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_ForumShare.P_Id and P_ThumbUp.P_UserId='" + userid + "' and P_ThumbUp.P_FamilyType=2 )>0 THEN 1 ELSE 0 END as userup, ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_ForumShare.P_Id and dt_article_comment.type=1) as comcount ");
                    ss.Append(" from P_ForumShare ");
                    ss.Append(" left join dt_users on dt_users.id= P_ForumShare.P_UserId ");
                    ss.Append("  where P_ForumShare.P_Id='" + id + @"' and P_ForumShare.P_Status=0 ");
                    DataSet dd = DbHelperSQL.Query(ss.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 0;

                    }
                    else
                    {
                        model = null;
                    }

                }
                if (share.P_Type == 1)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select P_id as id,P_Content as content, ");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_ForumShare.P_UserId as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append("  dt_users.user_name  as username, ");
                    ss.Append("  CONVERT(VARCHAR(10),P_CreateTime,120) as time ");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_ForumShare.P_Id and P_ThumbUp.P_FamilyType=2 ) as thumbcount, ");
                    ss.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_ForumShare.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=2)>0 THEN 1 ELSE 0 END as userup, ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_ForumShare.P_Id and dt_article_comment.type=1) as comcount ");
                    ss.Append(" from P_ForumShare ");
                    ss.Append(" left join dt_users on dt_users.id= P_ForumShare.P_UserId ");
                    ss.Append("  where P_ForumShare.P_Id='" + id + @"' and P_ForumShare.P_Status=0 ");
                    DataSet dd = DbHelperSQL.Query(ss.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 1;
                        StringBuilder sss = new StringBuilder();
                        sss.Append("select P_Image.P_ImageUrl as imgurl from P_Image where  P_Image.P_ImageId='" + model.id + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.党小组论坛 + @"' ");
                        DataSet aa = DbHelperSQL.Query(sss.ToString());
                        DataSetToModelHelper<Imaurl> ii = new DataSetToModelHelper<Imaurl>();
                        if (aa.Tables[0].Rows.Count != 0)
                        {
                            model.imgurl = ii.FillModel(aa);
                        }
                        else
                        {
                            model.imgurl = null;
                        }
                    }
                    else
                    {
                        model = null;
                    }
                }
                if (share.P_Type == 2)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select P_id as id,P_Content as content, ");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_ForumShare.P_UserId as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_ForumShare.P_Id and P_ThumbUp.P_FamilyType=2 ) as thumbcount, ");
                    ss.Append("  CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_ForumShare.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=2)>0 THEN 1 ELSE 0 END as userup,  ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_ForumShare.P_Id and dt_article_comment.type=1) as comcount  ");
                    ss.Append(" from P_ForumShare ");
                    ss.Append(" left join dt_users on dt_users.id= P_ForumShare.P_UserId ");
                    ss.Append("  where P_ForumShare.P_Id='" + id + @"' and P_ForumShare.P_Status=0 ");
                    DataSet dd = DbHelperSQL.Query(ss.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 2;
                        StringBuilder aa = new StringBuilder();
                        aa.Append(" select P_Video.P_VideoName as videoname,P_Video.P_VideoPic as videopic, ");
                        aa.Append("  P_Video.P_Url as videourl, ");
                        aa.Append(" (select count(a.id) from ");
                        aa.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as playcount ");
                        aa.Append(" from P_Video ");
                        aa.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
                        aa.Append(" where P_Video.P_ParentId='" + id + @"'and P_Video.P_Source=1 ");
                        aa.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url ");
                        DataSet dss = DbHelperSQL.Query(aa.ToString());
                        if (dss.Tables[0].Rows.Count != 0)
                        {
                            model = tt.FillToModel(dss.Tables[0].Rows[0]);
                        }
                    }
                }
                else
                {
                    model = null;
                }
            }
            return model;
        }

        /// <summary>
        /// 党小组转发
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MyCollectModel GetTranSubmitCollect(string id, int userid)
        {
            MyCollectModel model = new MyCollectModel();
            StringBuilder s = new StringBuilder();
            s.Append(" select * from P_ForumShare where P_Id='" + id + @"' and P_ForumShare.P_Source=1 ");
            DataSet ds = DbHelperSQL.Query(s.ToString());
            DataSetToModelHelper<P_ForumShare> pp = new DataSetToModelHelper<P_ForumShare>();
            P_ForumShare share = new P_ForumShare();
            if (ds.Tables[0].Rows.Count != 0)
            {
                share = pp.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
                return model;
            }


            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select *  ");
            strsql.Append(" from P_Transmit where P_Id='" + share.P_SourceId + @"'  ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<P_Transmit> mm = new DataSetToModelHelper<P_Transmit>();
            P_Transmit tran = new P_Transmit();
            if (dt.Tables[0].Rows.Count != 0)
            {
                tran = mm.FillToModel(dt.Tables[0].Rows[0]);
                if (Convert.ToInt32(tran.P_Type) == 52)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("select  P_Video.P_VideoName as videoname,P_Video.P_VideoPic as videopic,P_Video.P_ParentId as courseid,");
                    str.Append(" P_Video.P_Url as videourl,");
                    str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST('" + tran.P_CreateUser + @"' as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    str.Append(" (select  dt_users.user_name  as username from dt_users where dt_users.id=Convert('" + tran.P_CreateUser + @"' as int)) ");
                    str.Append("  (select CONVERT(VARCHAR(10),P_CreateTime,120) from P_Transmit where P_Id='" + id + @"') as time,");
                    str.Append(" (select P_Content from P_Transmit where P_Id='" + id + @"') as content ");
                    str.Append(" (select count(a.id) from  ");
                    str.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id   ) a) as videoplaycount, ");
                    str.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_Video.P_Id and P_ThumbUp.P_FamilyType=2) as thumbcount,");
                    str.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_Video.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=3 )>0 THEN 1 ELSE 0 END as userup, ");
                    str.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.article_id=P_Video.P_Id) as comcount ");
                    str.Append(" from P_Video ");
                    str.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
                    str.Append(" where P_Video.P_Id='" + tran.P_RelationId + @"' and P_Source is null  ");
                    str.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url ");
                    DataSet ddt = DbHelperSQL.Query(str.ToString());
                    DataSetToModelHelper<MyCollectModel> trn = new DataSetToModelHelper<MyCollectModel>();
                    if (ddt.Tables[0].Rows.Count != 0)
                    {
                        model = trn.FillToModel(ddt.Tables[0].Rows[0]);
                        if (model != null)
                        {
                            model.istype = 52;
                        }


                    }
                    else
                    {
                        model = null;
                    }
                }
                if (Convert.ToInt32(tran.P_Type) == 53)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select  dt_article.title as title,dt_article.id as id,dt_article.content as textcontent, ");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST('" + tran.P_CreateUser + @"' as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append(" (select  dt_users.user_name  as username from dt_users where dt_users.id=Convert('" + tran.P_CreateUser + @"' as int)) ");
                    ss.Append("  (select CONVERT(VARCHAR(10),P_CreateTime,120) from P_Transmit where P_Id='" + id + @"') as time,");
                    ss.Append(" (select P_Content from P_Transmit where P_Id='" + id + @"') as content ");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=CAST(dt_article.id AS nvarchar) and P_ThumbUp.P_FamilyType=2 ) as thumbcount, ");
                    ss.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=CAST(dt_article.id AS nvarchar) and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=3 )>0 THEN 1 ELSE 0 END as userup, ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.article_id=CAST(dt_article.id AS nvarchar)) as comcount ");
                    ss.Append(" from dt_article where dt_article.id=" + tran.P_RelationId + @" ");
                    DataSet dd = DbHelperSQL.Query(strsql.ToString());
                    DataSetToModelHelper<MyCollectModel> ww = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = ww.FillToModel(dd.Tables[0].Rows[0]);
                        if (model != null)
                        {
                            model.istype = 53;
                            if (model.textcontent != null)
                            {
                                string[] reg = { @"[<].*?[>]", @"\r", @"&emsp", @"&nbsp", @"\n", @"\t" };
                                for (int i = 0; i < reg.Length; i++)
                                {
                                    Regex regex = new Regex(reg[i], RegexOptions.IgnoreCase);
                                    model.textcontent = Regex.Replace(model.textcontent, reg[i], "");
                                }
                                model.textcontent = Regex.Replace(model.textcontent, @";;;", @";");
                                model.textcontent = Regex.Replace(model.textcontent, @";;", ";");
                                model.textcontent = Regex.Replace(model.textcontent, @".;", ".");
                            }
                        }

                    }
                    else
                    {
                        model = null;
                    }
                }
            }
            else
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 党支部转发
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MyCollectModel GetGroupTransCollect(string id, int userid)
        {
            MyCollectModel model = new MyCollectModel();
            StringBuilder s = new StringBuilder();
            s.Append(" select * from P_BranchPublish where P_Id='" + id + @"' and P_Source=1 ");
            DataSet ds = DbHelperSQL.Query(s.ToString());
            DataSetToModelHelper<P_BranchPublish> pp = new DataSetToModelHelper<P_BranchPublish>();
            P_BranchPublish share = new P_BranchPublish();
            if (ds.Tables[0].Rows.Count != 0)
            {
                share = pp.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
                return model;
            }

            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select *  ");
            strsql.Append(" from P_Transmit where P_Id='" + share.P_SourceId + @"'  ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<P_Transmit> mm = new DataSetToModelHelper<P_Transmit>();
            P_Transmit tran = new P_Transmit();
            if (dt.Tables[0].Rows.Count != 0)
            {
                tran = mm.FillToModel(dt.Tables[0].Rows[0]);
                if (Convert.ToInt32(tran.P_Type) == 52)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("select  P_Video.P_VideoName as videoname,P_Video.P_VideoPic as videopic,P_Video.P_ParentId as courseid,");
                    str.Append(" P_Video.P_Url as videourl,");
                    str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST('" + tran.P_CreateUser + @"' as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    str.Append(" (select  dt_users.user_name  as username from dt_users where dt_users.id=Convert('" + tran.P_CreateUser + @"' as int)) ");
                    str.Append("  (select CONVERT(VARCHAR(10),P_CreateTime,120) from P_Transmit where P_Id='" + id + @"') as time,");
                    str.Append(" (select P_Content from P_Transmit where P_Id='" + id + @"') as content ");
                    str.Append(" (select count(a.id) from  ");
                    str.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as videoplaycount, ");
                    str.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_Video.P_Id and P_ThumbUp.P_FamilyType=3) as thumbcount,");
                    str.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_Video.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=5 )>0 THEN 1 ELSE 0 END as userup, ");
                    str.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.article_id=P_Video.P_Id) as comcount ");
                    str.Append(" from P_Video ");
                    str.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");

                    str.Append(" where P_Video.P_Id='" + tran.P_RelationId + @"' and P_Video.P_Source is null  ");
                    str.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url ");
                    DataSet ddt = DbHelperSQL.Query(str.ToString());
                    DataSetToModelHelper<MyCollectModel> trn = new DataSetToModelHelper<MyCollectModel>();
                    if (ddt.Tables[0].Rows.Count != 0)
                    {
                        model = trn.FillToModel(ddt.Tables[0].Rows[0]);
                        if (model != null)
                        {
                            model.istype = 52;
                        }


                    }
                    else
                    {
                        model = null;
                    }
                }
                if (model.type == 53)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select  dt_article.title as title,dt_article.id as id,dt_article.content as textcontent, ");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST('" + tran.P_CreateUser + @"' as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append(" (select  dt_users.user_name  as username from dt_users where dt_users.id=Convert('" + tran.P_CreateUser + @"' as int)) ");
                    ss.Append("  (select CONVERT(VARCHAR(10),P_CreateTime,120) from P_Transmit where P_Id='" + id + @"') as time,");
                    ss.Append(" (select P_Content from P_Transmit where P_Id='" + id + @"') as content ");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=CAST(dt_article.id AS nvarchar) and P_ThumbUp.P_FamilyType=3 ) as thumbcount, ");
                    ss.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=CAST(dt_article.id AS nvarchar) and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=5 )>0 THEN 1 ELSE 0 END as userup, ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.article_id=CAST(dt_article.id AS nvarchar)) as comcount ");
                    ss.Append(" from dt_article where dt_article.id=" + tran.P_RelationId + @" ");
                    DataSet dd = DbHelperSQL.Query(strsql.ToString());
                    DataSetToModelHelper<MyCollectModel> ww = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = ww.FillToModel(dd.Tables[0].Rows[0]);
                        if (model != null)
                        {
                            model.istype = 53;
                            if (model.textcontent != null)
                            {
                                string[] reg = { @"[<].*?[>]", @"\r", @"&emsp", @"&nbsp", @"\n", @"\t" };
                                for (int i = 0; i < reg.Length; i++)
                                {
                                    Regex regex = new Regex(reg[i], RegexOptions.IgnoreCase);
                                    model.textcontent = Regex.Replace(model.textcontent, reg[i], "");
                                }
                                model.textcontent = Regex.Replace(model.textcontent, @";;;", @";");
                                model.textcontent = Regex.Replace(model.textcontent, @";;", ";");
                                model.textcontent = Regex.Replace(model.textcontent, @".;", ".");
                            }
                        }

                    }
                    else
                    {
                        model = null;
                    }
                }
            }
            else
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 党支部分享
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MyCollectModel GetGroupsCollect(string id, int userid)
        {
            MyCollectModel model = new MyCollectModel();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select * from P_BranchPublish where P_Id='" + id + @"' and P_Source=0  ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<P_BranchPublish> mm = new DataSetToModelHelper<P_BranchPublish>();
            P_BranchPublish share = new P_BranchPublish();
            if (dt.Tables[0].Rows.Count != 0)
            {
                share = mm.FillToModel(dt.Tables[0].Rows[0]);
                if (share.P_Type == 0)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select P_id as id,P_Content as content,");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_BranchPublish.P_UserId as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append("  dt_users.user_name  as username, ");
                    ss.Append("  CONVERT(VARCHAR(10),P_CreateTime,120) as time ");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id and P_ThumbUp.P_FamilyType=3 ) as thumbcount, ");
                    ss.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id and P_ThumbUp.P_UserId='" + userid + "' and P_ThumbUp.P_FamilyType=4 )>0 THEN 1 ELSE 0 END as userup, ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_BranchPublish.P_Id and dt_article_comment.type=1) as comcount ");
                    ss.Append(" from P_BranchPublish ");
                    ss.Append(" left join dt_users on dt_users.id= P_BranchPublish.P_UserId ");
                    ss.Append("  where P_BranchPublish.P_Id='" + id + @"' and P_BranchPublish.P_Status=0 ");
                    DataSet dd = DbHelperSQL.Query(ss.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 0;

                    }
                    else
                    {
                        model = null;
                    }

                }
                if (model.type == 1)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select P_id as id,P_Content as content, ");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_BranchPublish.P_UserId as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append("  dt_users.user_name  as username, ");
                    ss.Append("  CONVERT(VARCHAR(10),P_CreateTime,120) as time ");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id and P_ThumbUp.P_FamilyType=3) as thumbcount, ");
                    ss.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=4)>0 THEN 1 ELSE 0 END as userup, ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_BranchPublish.P_Id and dt_article_comment.type=1) as comcount ");
                    ss.Append(" from P_BranchPublish ");
                    ss.Append(" left join dt_users on dt_users.id= P_BranchPublish.P_UserId ");
                    ss.Append("  where P_BranchPublish.P_Id='" + id + @"' and P_BranchPublish.P_Status=0 ");
                    DataSet dd = DbHelperSQL.Query(ss.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 1;
                        StringBuilder sss = new StringBuilder();
                        sss.Append("select P_Image.P_ImageUrl as imgurl from P_Image where  P_Image.P_ImageId='" + model.id + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.支部管理分享 + @"' ");
                        DataSet aa = DbHelperSQL.Query(sss.ToString());
                        DataSetToModelHelper<Imaurl> ii = new DataSetToModelHelper<Imaurl>();
                        if (aa.Tables[0].Rows.Count != 0)
                        {
                            model.imgurl = ii.FillModel(aa);
                        }
                        else
                        {
                            model.imgurl = null;
                        }
                    }
                    else
                    {
                        model = null;
                    }
                }
                if (model.type == 2)
                {
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select P_id as id,P_Content as content, ");
                    ss.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_BranchPublish.P_UserId as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    ss.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id and P_ThumbUp.P_FamilyType=3 ) as thumbcount, ");
                    ss.Append("  CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=4)>0 THEN 1 ELSE 0 END as userup,  ");
                    ss.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_BranchPublish.P_Id and dt_article_comment.type=1) as comcount  ");
                    ss.Append(" from P_BranchPublish ");
                    ss.Append(" left join dt_users on dt_users.id= P_BranchPublish.P_UserId ");
                    ss.Append("  where P_BranchPublish.P_Id='" + id + @"' and P_BranchPublish.P_Status=0 ");
                    DataSet dd = DbHelperSQL.Query(ss.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 2;
                        StringBuilder aa = new StringBuilder();
                        aa.Append(" select P_Video.P_VideoName as videoname,P_Video.P_VideoPic as videopic, ");
                        aa.Append("  P_Video.P_Url as videourl, ");
                        aa.Append(" (select count(a.id) from ");
                        aa.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as playcount ");
                        aa.Append(" from P_Video ");
                        aa.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
                        aa.Append(" where P_Video.P_ParentId='" + id + @"'and P_Video.P_Source=2 ");
                        aa.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url ");
                        DataSet dss = DbHelperSQL.Query(aa.ToString());
                        if (dss.Tables[0].Rows.Count != 0)
                        {
                            model = tt.FillToModel(dss.Tables[0].Rows[0]);
                        }

                    }
                }
                else
                {
                    model = null;
                }
            }
            return model;
        }
        /// <summary>
        /// 党建云分享
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public MyCollectModel GetPlatyCollect(string id, int userid)
        {
            MyCollectModel model = new MyCollectModel();
            StringBuilder s = new StringBuilder();
            s.Append(" select * from P_BranchPublish where P_Id='" + id + @"' and P_Source=2 ");
            DataSet ds = DbHelperSQL.Query(s.ToString());
            DataSetToModelHelper<P_BranchPublish> pp = new DataSetToModelHelper<P_BranchPublish>();
            P_BranchPublish share = new P_BranchPublish();
            if (ds.Tables[0].Rows.Count != 0)
            {
                share = pp.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
                return model;
            }
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select * from P_PartyCloudShare where P_Id='" + share.P_SourceId + @"' ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<P_PartyCloudShare> mm = new DataSetToModelHelper<P_PartyCloudShare>();
            P_PartyCloudShare cloudshare = new P_PartyCloudShare();
            if (dt.Tables[0].Rows.Count != 0)
            {
                cloudshare = mm.FillToModel(dt.Tables[0].Rows[0]);
                StringBuilder ssss = new StringBuilder();
                ssss.Append(" select * from P_PartyCloud where P_Id='" + cloudshare.P_PartyCloudId + @"'");
                DataSet dsa = DbHelperSQL.Query(ssss.ToString());
                DataSetToModelHelper<P_PartyCloud> pc = new DataSetToModelHelper<P_PartyCloud>();
                P_PartyCloud partycloud = new P_PartyCloud();
                if (dsa.Tables[0].Rows.Count == 0)
                {
                    model = null;
                    return model;
                }
                else
                {
                    partycloud = pc.FillToModel(dsa.Tables[0].Rows[0]);
                }
                if (cloudshare.P_Type == 0)
                {
                    UserGroupHelper usergroup = new UserGroupHelper();
                    int learnuser = usergroup.GetUserMinimumGroupId(Convert.ToInt32(partycloud.P_UserId));
                    StringBuilder str = new StringBuilder();
                    str.Append(" select P_id as id,dt_users.user_name  as fruser,");
                    str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + partycloud.P_UserId + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    str.Append(" ( select dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId ) as username,");
                    str.Append(" CONVERT(VARCHAR(10),P_PartyCloud.P_CreateTime,120) as createtime,dt_user_groups.title as groupname, ");
                    str.Append(" P_PartyCloud.P_Size as size,");
                    str.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_PartyCloudShare.P_Id and P_ThumbUp.P_FamilyType=3 ) as thumbcount, ");
                    str.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_PartyCloudShare.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=6)>0 THEN 1 ELSE 0 END as userup, ");
                    str.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_PartyCloudShare.P_Id and dt_article_comment.type=2) as comcount ");
                    str.Append(" from P_PartyCloudShare ");
                    str.Append(" left join dt_users on dt_users.id=" + Convert.ToInt32(partycloud.P_UserId) + @"");
                    str.Append(" left join P_PartyCloud on P_PartyCloud.P_Id=P_PartyCloudShare.P_PartyCloudId");
                    str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + learnuser + @" ");
                    str.Append(" where P_PartyCloudShare.P_Id='" + id + @"' and P_PartyCloudShare.P_Status=0  ");
                    DataSet dd = DbHelperSQL.Query(str.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 1;
                        StringBuilder sss = new StringBuilder();
                        sss.Append("select P_Image.P_ImageUrl as imgurl,P_Image.P_PictureName as imgname from P_Image where  P_Image.P_ImageId='" + cloudshare.P_PartyCloudId + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.党建云 + @"' ");
                        DataSet aa = DbHelperSQL.Query(sss.ToString());
                        DataSetToModelHelper<Imaurl> ii = new DataSetToModelHelper<Imaurl>();
                        if (aa.Tables[0].Rows.Count != 0)
                        {
                            model.imgurl = ii.FillModel(aa);
                        }
                        else
                        {
                            model.imgurl = null;
                        }
                    }
                    else
                    {
                        model = null;
                    }
                }
                if (cloudshare.P_Type == 1)
                {
                    UserGroupHelper usergroup = new UserGroupHelper();
                    int learnuser = usergroup.GetUserMinimumGroupId(Convert.ToInt32(partycloud.P_UserId));
                    StringBuilder str = new StringBuilder();
                    str.Append(" select P_id as id,dt_users.user_name  as fruser,dt_user_groups.title as groupname, ");
                    str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + partycloud.P_UserId + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    str.Append(" CONVERT(VARCHAR(10),P_PartyCloud.P_CreateTime,120) as createtime, ");
                    str.Append(" ( select dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId ) as username,");
                    str.Append(" P_PartyCloud.P_Size as size,");
                    str.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_PartyCloudShare.P_Id and P_ThumbUp.P_FamilyType=3 ) as thumbcount, ");
                    str.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_PartyCloudShare.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=6)>0 THEN 1 ELSE 0 END as userup, ");
                    str.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_PartyCloudShare.P_Id and dt_article_comment.type=2) as comcount ");
                    str.Append(" from P_PartyCloudShare ");
                    str.Append(" left join dt_users on dt_users.id= " + Convert.ToInt32(partycloud.P_UserId) + @" ");
                    str.Append(" left join P_PartyCloud on P_PartyCloud.P_Id=P_PartyCloudShare.P_PartyCloudId");
                    str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + learnuser + @" ");
                    str.Append(" where P_PartyCloudShare.P_Id='" + id + @"' and P_PartyCloudShare.P_Status=0  ");
                    DataSet dd = DbHelperSQL.Query(str.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                        model.istype = 2;
                        StringBuilder aa = new StringBuilder();
                        aa.Append(" select P_Video.P_VideoName as videoname,P_Video.P_VideoPic as videopic, ");
                        aa.Append("  P_Video.P_Url as videourl, ");
                        aa.Append(" (select count(a.id) from ");
                        aa.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as playcount ");
                        aa.Append(" from P_Video ");
                        aa.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
                        aa.Append(" where P_Video.P_ParentId='" + id + @"'and P_Video.P_Source=0 ");
                        aa.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url ");
                        DataSet dss = DbHelperSQL.Query(aa.ToString());
                        if (dss.Tables[0].Rows.Count != 0)
                        {
                            model = tt.FillToModel(dss.Tables[0].Rows[0]);
                        }
                    }
                    else
                    {
                        model = null;
                    }
                }
                if (cloudshare.P_Type == 2)
                {
                    UserGroupHelper usergroup = new UserGroupHelper();
                    int learnuser = usergroup.GetUserMinimumGroupId(Convert.ToInt32(partycloud.P_UserId));
                    StringBuilder str = new StringBuilder();
                    str.Append(" select P_PartyCloudShare.P_id as id,dt_users.user_name  as fruser,P_Document.P_Title as title,P_Document.P_Title as videoname,P_Document.P_DocUrl as videourl, ");
                    str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + partycloud.P_UserId + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    str.Append(" CONVERT(VARCHAR(10),P_PartyCloud.P_CreateTime,120) as createtime,dt_user_groups.title as groupname,  ");
                    str.Append(" ( select dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId ) as username,");
                    str.Append(" P_PartyCloud.P_Size as size,");
                    str.Append(" (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_PartyCloudShare.P_Id and P_ThumbUp.P_FamilyType=3 ) as thumbcount, ");
                    str.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_PartyCloudShare.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_FamilyType=6)>0 THEN 1 ELSE 0 END as userup, ");
                    str.Append(" (select count(dt_article_comment.id) from dt_article_comment where dt_article_comment.forum_share_id=P_PartyCloudShare.P_Id and dt_article_comment.type=2) as comcount ");
                    str.Append(" from P_PartyCloudShare ");
                    str.Append(" left join dt_users on dt_users.id= " + Convert.ToInt32(partycloud.P_UserId) + @"");
                    str.Append(" LEFT JOIN P_Document on P_Document.P_Id=P_PartyCloudShare.P_PartyCloudId ");
                    str.Append(" left join P_PartyCloud on P_PartyCloud.P_Id=P_PartyCloudShare.P_PartyCloudId");
                    str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + learnuser + @" ");
                    str.Append(" where P_PartyCloudShare.P_Id='" + id + @"' and P_PartyCloudShare.P_Status=0  ");
                    DataSet dd = DbHelperSQL.Query(str.ToString());
                    DataSetToModelHelper<MyCollectModel> tt = new DataSetToModelHelper<MyCollectModel>();
                    if (dd.Tables[0].Rows.Count != 0)
                    {
                        model = tt.FillToModel(dd.Tables[0].Rows[0]);
                    }
                }
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="imgname"></param>
        /// <returns></returns>
        public Boolean UpAvatar(int userid, string imgname)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        QiNiuHelper helper = new QiNiuHelper();
                        string imageurl = helper.GetQiNiuFileUrl(imgname);
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update  P_Image set ");
                        strSql.Append("P_ImageUrl=@P_ImageUrl,P_UpdateTime=@P_UpdateTime,P_UpdateUser=@P_UpdateUser ");
                        strSql.Append(" where ");
                        strSql.Append(" P_ImageId=@P_ImageId and P_ImageType=@P_ImageType ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_UpdateUser", SqlDbType.Int,100),
                                new SqlParameter("@P_ImageId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ImageType", SqlDbType.Int,100)
                        };
                        parameters[0].Value = imageurl;
                        parameters[1].Value = DateTime.Now;
                        parameters[2].Value = userid;
                        parameters[3].Value = userid;
                        parameters[4].Value = (int)ImageTypeEnum.头像;
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
        /// 修改密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public Boolean UpPassWord(int userid, string pass)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string salt = Utils.GetCheckCode(6);
                        string encryptionPsw = DESEncrypt.Encrypt(pass, salt);
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update  dt_users set ");
                        strSql.Append(" salt='" + salt + @"',password='" + encryptionPsw + @"'  ");
                        strSql.Append(" where ");
                        strSql.Append(" id=" + userid + @"  ");
                        //SqlParameter[] parameters = {
                        //        new SqlParameter("@salt", SqlDbType.NVarChar),
                        //        new SqlParameter("@password", SqlDbType.NVarChar),
                        //        new SqlParameter("@id", SqlDbType.Int,100)
                        //};
                        //parameters[0].Value = salt;
                        //parameters[1].Value = encryptionPsw;
                        //parameters[2].Value = userid;
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
        /// 个人中心列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<ContentModel> GetPersonalCenter(int userid, int rows, int page)
        {
            List<ContentModel> contentmodel = new List<ContentModel>();
            List<PersonalCenterModel> model = new List<PersonalCenterModel>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select * from (select P_Id as id,P_GroupForumId as partid,P_Content as content,P_Type as type,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime, ");
            strsql.Append(" CASE WHEN P_Type=0 or P_Type=1 or P_Type=2 THEN  0 END as istype,CAST(P_UserId as int) as userid ");
            strsql.Append(" FROM P_ForumShare where P_UserId='" + userid + @"' and P_Type is NOT NULL ");
            strsql.Append(" UNION ALL ");
            strsql.Append(" select P_Id as id,P_BranchId as partid,P_Content as content, P_Type as type,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime, ");
            strsql.Append(" CASE WHEN P_Type=0 or P_Type=1 or P_Type=2 THEN  1 END as istype,CAST(P_UserId as int) as userid  ");
            strsql.Append(" from P_BranchPublish where P_UserId='" + userid + @"' and  P_Type is NOT NULL ");
            strsql.Append(" UNION ALL ");
            strsql.Append(" select P_Id as id,P_RelationId AS partid,P_Content as content ,P_Type as type ,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime , ");
            strsql.Append(" CASE WHEN P_Type=52 or P_Type=53  THEN  2 END as istype,CAST(P_UserId as int) as userid ");
            strsql.Append(" from P_Transmit where P_UserId='" + userid + @"' and  P_Type is NOT NULL ");
            strsql.Append(" UNION ALL ");
            strsql.Append(" select P_Id as id,P_PartyCloudId as partid,CASE WHEN P_Status=0 THEN '正常' ELSE '删除' END as content, P_Type as type, ");
            strsql.Append(" CONVERT(VARCHAR(100),P_CreateTime,23) as createtime, CASE WHEN P_Type=0 or P_Type=1 or P_Type=2  THEN  3 END as istype,CAST(P_CreaterId as int) as userid ");
            strsql.Append(" from P_PartyCloudShare WHERE P_CreaterId='" + userid + @"' AND P_Type IS NOT NULL)a ");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "a.createtime"));
            DataSetToModelHelper<PersonalCenterModel> mm = new DataSetToModelHelper<PersonalCenterModel>();
            if (dt.Tables[0].Rows.Count > 0)
            {
                model = mm.FillModel(dt);
                foreach (var item in model)
                {
                    ContentModel CModel = new ContentModel();
                    if (item.istype == 0)
                    {
                        if (item.type == 0)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append(" select P_Id as id,P_Content as content,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime,CAST(P_Type AS INT) as type, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" FROM P_ForumShare where P_Id='" + item.id + @"'   ");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 0;
                                contentmodel.Add(CModel);
                            }
                        }
                        if (item.type == 1)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append(" select P_Id as id,P_Content as content,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime,CAST(P_Type AS INT) as type, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" FROM P_ForumShare where P_Id='" + item.id + @"'   ");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 0;
                                StringBuilder sss = new StringBuilder();
                                sss.Append("select P_ImageUrl as imgurl,P_PictureName as imgname  from P_Image where  P_Image.P_ImageId='" + item.id + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.党小组论坛 + @"' ");
                                DataSet aa = DbHelperSQL.Query(sss.ToString());
                                DataSetToModelHelper<Imaurl> ii = new DataSetToModelHelper<Imaurl>();
                                if (aa.Tables[0].Rows.Count != 0)
                                {
                                    CModel.imgurl = ii.FillModel(aa);
                                }
                                else
                                {
                                    CModel.imgurl = new List<Imaurl>();
                                }
                                contentmodel.Add(CModel);
                            }
                        }
                        if (item.type == 2)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append(" select P_Id as id,P_Content as content,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime,CAST(P_Type AS INT) as type, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" FROM P_ForumShare where P_Id='" + item.id + @"'   ");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 0;
                                StringBuilder aa = new StringBuilder();
                                aa.Append(" select P_Video.P_VideoName as isname,P_Video.P_VideoPic as videopic, ");
                                aa.Append("  P_Video.P_Url as isurl, ");
                                aa.Append(" (select count(a.id) from ");
                                aa.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as playcount ");
                                aa.Append(" from P_Video ");
                                aa.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
                                aa.Append(" where P_Video.P_ParentId='" + item.id + @"'and P_Video.P_Source=1 ");
                                aa.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url ");
                                DataSet dd = DbHelperSQL.Query(aa.ToString());
                                if (dd.Tables[0].Rows.Count != 0)
                                {
                                    CModel = cm.FillToModel(dd.Tables[0].Rows[0]);
                                }
                                contentmodel.Add(CModel);
                            }
                        }
                    }
                    if (item.istype == 1)
                    {
                        if (item.type == 0)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("select P_id as id,P_Content as content,P_Type as type,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime, ");
                            str.Append(" select P_Id as id,P_Content as content,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime,CAST(P_Type AS INT) as type, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" from P_BranchPublish where P_Id='" + item.id + @"'");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 1;
                                contentmodel.Add(CModel);
                            }
                        }
                        if (item.type == 1)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("select P_id as id,P_Content as content,CAST(P_Type AS INT) as type,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime, ");
                            str.Append(" select P_Id as id,P_Content as content,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime,P_Type as type, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" from P_BranchPublish where P_Id='" + item.id + @"'");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 1;
                                StringBuilder sss = new StringBuilder();
                                sss.Append("select P_ImageUrl as imgurl,P_PictureName as imgname  from P_Image where  P_Image.P_ImageId='" + item.id + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.支部管理分享 + @"' ");
                                DataSet aa = DbHelperSQL.Query(sss.ToString());
                                DataSetToModelHelper<Imaurl> ii = new DataSetToModelHelper<Imaurl>();
                                if (aa.Tables[0].Rows.Count != 0)
                                {
                                    CModel.imgurl = ii.FillModel(aa);
                                }
                                else
                                {
                                    CModel.imgurl = new List<Imaurl>();
                                }
                                contentmodel.Add(CModel);
                            }
                        }
                        if (item.type == 2)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("select P_id as id,P_Content as content,CAST(P_Type AS INT) as type,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime, ");
                            str.Append(" select P_Id as id,P_Content as content,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime,P_Type as type, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" from P_BranchPublish where P_Id='" + item.id + @"'");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 1;
                                StringBuilder aa = new StringBuilder();
                                aa.Append(" select P_Video.P_VideoName as isname,P_Video.P_VideoPic as videopic, ");
                                aa.Append("  P_Video.P_Url as isurl, ");
                                aa.Append(" (select count(a.id) from ");
                                aa.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as playcount ");
                                aa.Append(" from P_Video ");
                                aa.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
                                aa.Append(" where P_Video.P_ParentId='" + item.id + @"'and P_Video.P_Source=2 ");
                                aa.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url ");
                                DataSet dd = DbHelperSQL.Query(aa.ToString());
                                if (dd.Tables[0].Rows.Count != 0)
                                {
                                    CModel = cm.FillToModel(dd.Tables[0].Rows[0]);
                                }
                                contentmodel.Add(CModel);
                            }
                        }
                    }
                    if (item.istype == 2)
                    {
                        if (item.type == 52)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append(" select P_Id as id,P_Content as textcount ,CASE WHEN CAST(P_Category AS INT)=0 THEN 2 else 4 end as istype ,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime ,");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") as username ");
                            str.Append(" from P_Transmit where P_Id='"+item.id+@"'");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.type = 2;
                                StringBuilder aa = new StringBuilder();
                                aa.Append(" select P_Video.P_VideoName as isname,P_Video.P_VideoPic as videopic, ");
                                aa.Append("  P_Video.P_Url as isurl,CAST(P_ParentId as int ) as courceid, ");
                                aa.Append(" (select count(a.id) from ");
                                aa.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as playcount ");
                                aa.Append(" from P_Video ");
                                aa.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
                                aa.Append(" where P_Video.P_Id='" + item.partid + @"'and P_Video.P_Source is null  ");
                                aa.Append(" GROUP BY P_Video.P_Id,P_Video.P_VideoName,P_Video.P_VideoPic,P_Video.P_VideoLength, P_Video.P_Url,P_ParentId ");
                                DataSet dd = DbHelperSQL.Query(aa.ToString());
                                if (dd.Tables[0].Rows.Count != 0)
                                {
                                    CModel = cm.FillToModel(dd.Tables[0].Rows[0]);
                                }
                                contentmodel.Add(CModel);
                            }

                        }
                        if (item.type == 53)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append(" select P_Id as id,P_Content as textcount ,CASE WHEN CAST(P_Category AS INT)=0 THEN 2 else 4 end as istype  ,CONVERT(VARCHAR(100),P_CreateTime,23) as createtime ,");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" from P_Transmit where P_Id='" + item.id + @"'");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.type = 1;
                                StringBuilder aa = new StringBuilder();
                                aa.Append(" select  dt_article.title as title,dt_article.content as content, ");
                                aa.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                                aa.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                                aa.Append(" from dt_article where dt_article.id=" + item.partid + @" ");
                                DataSet dd = DbHelperSQL.Query(aa.ToString());
                                if (dd.Tables[0].Rows.Count != 0)
                                {
                                    CModel = cm.FillToModel(dd.Tables[0].Rows[0]);
                                    StringBuilder sss = new StringBuilder();
                                    sss.Append("select P_ImageUrl as imgurl,P_PictureName as imgname  from P_Image where  P_Image.P_ImageId='" + item.partid + @"' and P_Image.P_ImageType is null ");
                                    DataSet dds = DbHelperSQL.Query(sss.ToString());
                                    DataSetToModelHelper<Imaurl> ii = new DataSetToModelHelper<Imaurl>();
                                    if (dds.Tables[0].Rows.Count != 0)
                                    {
                                        CModel.imgurl = ii.FillModel(dds);
                                    }
                                    else
                                    {
                                        CModel.imgurl = new List<Imaurl>();
                                    }
                                }
                                contentmodel.Add(CModel);
                            }
                        }
                    }
                    if (item.istype == 3)
                    {
                        UserGroupHelper usergroup = new UserGroupHelper();
                        int learnuser = usergroup.GetUserMinimumGroupId(Convert.ToInt32(item.userid));
                        if (item.type == 0)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("select P_PartyCloudShare.P_Id as id,CASE WHEN  CAST(P_PartyCloudShare.P_Type AS INT)=0 THEN 1 END as type,CONVERT(VARCHAR(100),P_PartyCloudShare.P_CreateTime,23) as createtime,  ");
                            str.Append(" ( select dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId ) as createuser,P_PartyCloud.P_Size as size, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @"),dt_user_groups.title as groupname ");
                            str.Append(" from P_PartyCloudShare ");
                            str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + learnuser + @" ");
                            str.Append(" left join P_PartyCloud on P_PartyCloud.P_Id=P_PartyCloudShare.P_PartyCloudId");
                            str.Append(" where P_PartyCloudShare.P_Id='" + item.id + @"' ");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 3;
                                StringBuilder sss = new StringBuilder();
                                sss.Append("select P_ImageUrl as imgurl,P_PictureName as imgname  from P_Image where  P_Image.P_ImageId='" + item.partid + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.党建云 + @"' ");
                                DataSet aa = DbHelperSQL.Query(sss.ToString());
                                DataSetToModelHelper<Imaurl> ii = new DataSetToModelHelper<Imaurl>();
                                if (aa.Tables[0].Rows.Count != 0)
                                {
                                    CModel.imgurl = ii.FillModel(aa);
                                }
                                else
                                {
                                    CModel.imgurl = new List<Imaurl>();
                                }
                            }
                        }
                        if (item.type == 1)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("select P_PartyCloudShare.P_Id as id,CASE WHEN CAST(P_PartyCloudShare.P_Type AS INT)=1 THEN 2 END as type,CONVERT(VARCHAR(100),P_PartyCloudShare.P_CreateTime,23) as createtime,dt_user_groups.title as groupname,  ");
                            str.Append(" ( select dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId ) as createuser,P_PartyCloud.P_Size as size, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" from P_PartyCloudShare ");
                            str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + learnuser + @" ");
                            str.Append(" left join P_PartyCloud on P_PartyCloud.P_Id=P_PartyCloudShare.P_PartyCloudId");
                            str.Append(" where P_PartyCloudShare.P_Id='" + item.id + @"' ");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 3;
                                StringBuilder sss = new StringBuilder();
                                sss.Append(" select P_Video.P_VideoName as isname,P_Video.P_VideoPic as videopic, ");
                                sss.Append("  P_Video.P_Url as isurl, ");
                                sss.Append(" (select count(a.id) from ");
                                sss.Append(" (select distinct P_VideoRecord.P_UserId  as id from P_VideoRecord where  P_VideoRecord.P_VideoId=P_Video.P_Id ) a) as playcount ");
                                sss.Append(" from P_Video ");
                                sss.Append(" where P_Video='"+item.partid+@"'");
                                DataSet dds = DbHelperSQL.Query(sss.ToString());
                                if (dds.Tables[0].Rows.Count > 0)
                                {
                                    CModel = cm.FillToModel(dds.Tables[0].Rows[0]);
                                }
                            }

                        }
                        if (item.type == 2)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("select P_PartyCloudShare.P_Id as id,CASE WHEN CAST(P_PartyCloudShare.P_Type AS INT)=2 THEN 4 END as type,CONVERT(VARCHAR(100),P_PartyCloudShare.P_CreateTime,23) as createtime,dt_user_groups.title as groupname , ");
                            str.Append(" ( select dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId ) as createuser,P_PartyCloud.P_Size as size, ");
                            str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(" + userid + @" as varchar)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                            str.Append(" (select user_name as username from dt_users where id=" + userid + @") ");
                            str.Append(" from P_PartyCloudShare ");
                            str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + learnuser + @" ");
                            str.Append(" left join P_PartyCloud on P_PartyCloud.P_Id=P_PartyCloudShare.P_PartyCloudId");
                            str.Append(" where P_PartyCloudShare.P_Id='" + item.id + @"' ");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            DataSetToModelHelper<ContentModel> cm = new DataSetToModelHelper<ContentModel>();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                CModel = cm.FillToModel(ds.Tables[0].Rows[0]);
                                CModel.istype = 3;
                                StringBuilder sss = new StringBuilder();
                                sss.Append(" select P_Title as title,P_DocUrl as isurl from P_Document where P_Id='"+item.partid+@"'");
                                DataSet dds = DbHelperSQL.Query(sss.ToString());
                                if (dds.Tables[0].Rows.Count > 0)
                                {
                                    CModel = cm.FillToModel(dds.Tables[0].Rows[0]);
                                }
                            }
                        }
                        
                    }
                }
            }
            else
            {
                contentmodel = null;
            }
            return contentmodel;
        }
        /// <summary>
        /// 删除个人中心主页列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool GetDeleteContent(List<DeleteCenter> fromBody)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DeleteCenter item in fromBody)
                        {

                            switch (item.type)
                            {
                                case 0:
                                    string delSql = String.Format(@"delete from P_ForumShare where P_ForumShare.P_Id = '{0}'", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql);
                                    break;
                                case 2:
                                    string delSql1 = String.Format(@"delete from P_BranchPublish where P_BranchPublish.P_Id = '{0}'", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql1);
                                    break;
                                case 1:
                                    string delSql2 = String.Format(@"delete from P_ForumShare where P_ForumShare.P_Id = '{0}'", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql2);
                                    string delSql3 = String.Format(@"delete from P_Transmit
                                            where P_Transmit.P_Id in (
                                            select P_ForumShare.P_SourceId from P_ForumShare 
                                            where P_ForumShare.P_Id = '{0}')", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql3);
                                    break;
                                case 3:
                                    string delSql4 = String.Format(@"delete from P_BranchPublish where P_BranchPublish.P_Id = '{0}'", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql4);
                                    string delSql5 = String.Format(@"delete from P_Transmit
                                            where P_Transmit.P_Id in (
                                            select P_BranchPublish.P_SourceId from P_BranchPublish 
                                            where P_BranchPublish.P_Id = '{0}')", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql5);
                                    break;
                                case 4:
                                    string delSql6 = String.Format(@"delete from P_BranchPublish where P_BranchPublish.P_Id = '{0}'", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql6);
                                    string delSql7 = String.Format(@"delete from P_PartyCloudShare
                                            where P_PartyCloudShare.P_Id in (
                                            select P_BranchPublish.P_SourceId from P_BranchPublish 
                                            where P_BranchPublish.P_Id = '{0}')", item.p_id);
                                    DbHelperSQL.ExecuteSql(conn, trans, delSql7);
                                    break;
                                default:
                                    break;
                            }
                        }
                        //if (type == 0)
                        //{
                        //    StringBuilder strSql = new StringBuilder();
                        //    strSql.Append("delete from P_ForumShare  ");
                        //    strSql.Append("where P_ForumShare.P_Id='" + id + @"'  ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
                        //}
                        //if (type == 1)
                        //{
                        //    StringBuilder strSql = new StringBuilder();
                        //    strSql.Append("delete from P_BranchPublish  ");
                        //    strSql.Append("where P_BranchPublish.P_Id='" + id + @"' ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
                        //}
                        //if (type == 2)
                        //{
                        //    StringBuilder strSql = new StringBuilder();
                        //    strSql.Append("delete from P_Transmit  ");
                        //    strSql.Append("where P_Transmit.P_Id='" + id + @"' ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
                        //    StringBuilder str = new StringBuilder();
                        //    str.Append("delete from P_ForumShare  ");
                        //    str.Append("where P_ForumShare.P_SourceId='" + id + @"' and P_Source=1 ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, str.ToString());

                        //}
                        //if (type == 3)
                        //{
                        //    StringBuilder strSql = new StringBuilder();
                        //    strSql.Append("delete from P_Transmit  ");
                        //    strSql.Append("where P_Transmit.P_Id='" + id + @"' ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
                        //    StringBuilder str = new StringBuilder();
                        //    str.Append("delete from P_BranchPublish  ");
                        //    str.Append("where P_BranchPublish.P_SourceId='" + id + @"' and P_Source=1 ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, str.ToString());
                        //}
                        //if (type == 4)
                        //{
                        //    StringBuilder strSql = new StringBuilder();
                        //    strSql.Append("delete from P_PartyCloudShare  ");
                        //    strSql.Append("where P_PartyCloudShare.P_Id='" + id + @"' ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
                        //    StringBuilder str = new StringBuilder();
                        //    str.Append("delete from P_BranchPublish  ");
                        //    str.Append("where P_BranchPublish.P_SourceId='" + id + @"' and P_Source=2 ");
                        //    DbHelperSQL.ExecuteSql(conn, trans, str.ToString());
                        //}
                       
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
    }

    /// <summary>
    /// 图片
    /// </summary>
    public class Imaurl
    {
        public string imgurl { get; set; }
        public string imgname { get; set; }
    }


    public class MyCollectModel
    {
        public int type { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
        public string title { get; set; }
        public List<Imaurl> imgurl { get; set; }
        public string id { get; set; }
        public int courceid { get; set; }
        public string videoname { get; set; }
        public string videourl { get; set; }
        public string videopic { get; set; }
        public int playcount { get; set; }
        public string content { get; set; }
        public int thumbcount { get; set; }
        public int comcount { get; set; }
        public int userup { get; set; }
        private string _time;
        public string time
        {
            get { return DateTime.Parse(String.IsNullOrEmpty(_time) ? DateTime.Now.ToString() : _time).ToString("MM-dd"); }
            set { _time = value; }
        }
        public int istype { get; set; }
        public string textcontent { get; set; }
        public Int64 size { get; set; }
        public string createtime { get; set; }
        public string groupname { get; set; }
        public string fruser { get; set; }
        public int videolength { get; set; }

    }

    public class PersonalCenterModel
    {
        public string id { get; set; }
        public string partid { get; set; }
        public string content { get; set; }
        public int type { get; set; }
        private string _createtime;
        public string createtime
        {
            get { return DateTime.Parse(_createtime == null ? "" : _createtime).ToString("MM-dd"); }
            set { _createtime = value; }
        }
        public int istype { get; set; }
        public int userid { get; set; }
    }

    public class ContentModel
    {
        public string id { get; set; }
        public string content { get; set; }
        public string textcontent { get; set; }
        public string isurl { get; set; }
        public string isname { get; set; }
        public List<Imaurl> imgurl { get; set; }
        private string _createtime;
        public string createtime
        {
            get { return DateTime.Parse(_createtime == null ? DateTime.Now.ToString(): _createtime).ToString("MM-dd"); }
            set { _createtime = value; }
        }
        public int type { get; set; }
        public string videopic { get; set; }
        public int playcount { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
        public int istype { get; set; }
        public int courceid { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string textsize { get; set; }
        public string groupname { get; set; }
        
    }
}

using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace DTcms.DAL
{
    public class PartyBranchManagement
    {
        /// <summary>
		/// 发布评论
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        /// 
        public Boolean Senddiscuss(discuss model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("insert into dt_article_comment(");
                        str.Append("user_id,add_time,relation_id,type,content)");
                        str.Append(" values(");
                        str.Append("@user_id,@add_time,@relation_id,@type,@content)");
                        str.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                            new SqlParameter("@user_id",SqlDbType.Int,5),
                            new SqlParameter("@add_time",SqlDbType.DateTime,50),
                            new SqlParameter("@relation_id",SqlDbType.NVarChar,50),
                            new SqlParameter("@type",SqlDbType.Int,5),
                            new SqlParameter("@content",SqlDbType.NVarChar,500),
                        };
                        string commentid = Guid.NewGuid().ToString();
                        parameters[0].Value = model.userid;
                        parameters[1].Value = DateTime.Now;
                        parameters[2].Value = model.shareid;
                        parameters[3].Value = 2;
                        parameters[4].Value = model.content;
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameters); //带事务
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
        /// 获取评论列表
        /// </summary>
        /// <param name="shareid"></param>
        /// <returns></returns>
        public List<commentlist> GetCommentList(int userid, string shareid, int page, int rows)
        {
            List<commentlist> result = new List<commentlist>();
            try
            {

                string sql = String.Format(@"select * from ( select
                        dt_article_comment.content,
                        CONVERT(varchar(100), dt_article_comment.add_time, 20) as time,
                        dt_users.user_name as name,
                        P_Image.P_ImageUrl as image,
                        dt_article_comment.id,
                        (select COUNT(P_ThumbUp.P_Id) from P_ThumbUp
                        where P_ThumbUp.P_ArticleId = CONVERT(nvarchar,dt_article_comment.id) and P_ThumbUp.P_FamilyType = 5) as thumbcount,
                        (
                        select case when P_ThumbUp.P_Id is null then 0 else 1 end from P_ThumbUp
                        where P_ThumbUp.P_ArticleId =  CONVERT(nvarchar,dt_article_comment.id)
                        and P_ThumbUp.P_FamilyType = 5 and P_ThumbUp.P_UserId = '{0}') as userthum
                        from dt_article_comment
                        left join dt_users on dt_users.id = dt_article_comment.user_id
                        left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_Image.P_ImageType = {1}
                        where dt_article_comment.relation_id = '{2}') table1", userid, (int)ImageTypeEnum.头像, shareid);
                DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "time desc"));
                DataSetToModelHelper<commentlist> model = new DataSetToModelHelper<commentlist>();
                result = model.FillModel(ds);
            }
            catch (Exception)
            {

                throw;
            }
            return result;

        }






        public class discuss
        {
            public int userid { set; get; }
            public string shareid { set; get; }
            public string content { set; get; }

        }

        public class commentlist
        {
            public int id { get; set; }
            public int userthum { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public string time { get; set; }
            public string content { get; set; }
            public int thumbcount { get; set; }

            public string thumarticleid { get; set; }

        }
        /// <summary>
        /// 党支部分享圈
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public List<sharelist> GetShareList(int userid, int groupid, int page, int rows)
        {
            ///P_BranchPublish表人员的基本信息
            List<sharelist> sl = new List<sharelist>();
            sharelist sharelistmodel = new sharelist();
            try
            {
                P_BranchPublish PBP = new P_BranchPublish();
                StringBuilder time = new StringBuilder();
                time.Append("SELECT P_CreateTime FROM P_BranchPublish ");
                time.Append("LEFT JOIN dt_user_groups on P_BranchPublish.P_BranchId=convert(varchar,dt_user_groups.id) ");
                time.Append("WHERE dt_user_groups.id = " + groupid + "");
                DataSet timedata = DbHelperSQL.Query(time.ToString());
                DataTable tb = timedata.Tables[0];
                DataRow row = tb.Rows[0];
                PBP.P_CreateTime = DateTime.Parse(timedata.Tables[0].Rows[0][0].ToString());
                DateTime a = Convert.ToDateTime(PBP.P_CreateTime);

                StringBuilder strsql = new StringBuilder();
                strsql.Append(" select * from(SELECT P_BranchPublish.P_Id as sharedataid,P_BranchPublish.P_CreateTime as sharetime,");
                strsql.Append("P_BranchPublish.P_Source as source,P_Image.P_ImageUrl as useravatar,dt_users.user_name as username,");
                strsql.Append(" CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id  ");
                strsql.Append("and P_ThumbUp.P_UserId='" + userid + "' and P_ThumbUp.P_FamilyType=5 )>0 THEN 1 else 0 END as userthum,");
                strsql.Append(" CASE WHEN (SELECT COUNT(P_Collect.P_Id) from P_Collect where P_Collect.P_Relation=P_BranchPublish.P_Id  ");
                strsql.Append("and P_Collect.P_UserId='" + userid + "' and (P_Collect.P_Type=56 or P_Collect.P_Type=57 or P_Collect.P_Type=58))>0 THEN 1 else 0 END as usercollect,");
                strsql.Append(" P_BranchPublish.P_UserId as shareuserid,P_BranchPublish.P_Content as content,P_BranchPublish.P_Type as type, ");
                strsql.Append("(Select Datename (month, '" + a + @"')+'-'+Datename(day,  '" + a + @"'))as createtime FROM P_BranchPublish ");
                strsql.Append(" LEFT JOIN dt_users ON CAST(P_BranchPublish.P_UserId AS INT) = dt_users.id ");
                strsql.Append(" LEFT JOIN P_Image ON P_Image.P_ImageId=P_BranchPublish.P_Id and P_Image.P_ImageType='20011'  ");
                strsql.Append(" LEFT JOIN dt_user_groups on P_BranchPublish.P_BranchId=dt_user_groups.id ");
                strsql.Append(" LEFT JOIN P_ThumbUp ON P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id ");
                strsql.Append(" LEFT JOIN P_Collect ON P_Collect.P_Relation=P_BranchPublish.P_Id ");
                strsql.Append(" where '" + groupid + "'= dt_user_groups.id )a ");

                DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), " a.sharetime DESC "));
                DataSetToModelHelper<sharelist> list = new DataSetToModelHelper<sharelist>();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    sl = list.FillModel(ds);
                }
                else
                {
                    list = null;
                }

                foreach (var item in sl)
                {
                    /// branchpulish表
                    if (item.source == 0)
                    {

                        //图片
                        if (Convert.ToInt32(item.type) == 1)
                        {

                            StringBuilder photo = new StringBuilder();
                            photo.Append("SELECT P_Image.P_ImageUrl as imageurl,P_Image.P_Picturename as imagename FROM P_Image ");
                            photo.Append("left join P_BranchPublish on P_Image.P_ImageId=P_BranchPublish.P_Id and P_Image.P_ImageType='20010' ");
                            photo.Append("left join P_PartyCloudShare on P_Image.P_ImageId=P_PartyCloudShare.P_Id and P_Image.P_ImageType='20010' ");
                            photo.Append("left join P_Transmit on P_Image.P_ImageId=P_Transmit.P_Id and P_Image.P_ImageType='20010' ");
                            photo.Append("where P_Image.P_ImageId='" + item.sharedataid + "' and '" + groupid + "'= dt_user_groups.P_Id ");
                            DataSet Daphoto = DbHelperSQL.Query(photo.ToString());
                            if (Daphoto.Tables[0].Rows.Count != 0)
                            {
                                DataSetToModelHelper<image> MH = new DataSetToModelHelper<image>();
                                List<image> imgageData = MH.FillModel(Daphoto);
                                if (imgageData != null)
                                {

                                    item.imageurl = imgageData;
                                }
                            }
                            else
                            {
                                item.imageurl = null;

                            }

                        }
                        //视频
                        else if (Convert.ToInt32(item.type) == 2)
                        {

                            StringBuilder video = new StringBuilder();
                            video.Append("SELECT P_Video.P_Url as videourl,P_Video.P_VideoLength as videolength,(SELECT COUNT(P_Video.P_ParentId) FROM P_Video where P_Video.P_ParentId='1212') as playcount FROM P_Video ");
                            video.Append("left JOIN P_BranchPublish on P_BranchPublish.P_Id=P_Video.P_ParentId ");
                            video.Append("left JOIN P_PartyCloudShare on P_PartyCloudShare.P_Id=P_Video.P_ParentId ");
                            video.Append("left JOIN P_Transmit on P_Transmit.P_Id=P_Video.P_ParentId ");
                            video.Append("where P_Video.P_ParentId='" + item.sharedataid + "' ");
                            DataSet Daphvido = DbHelperSQL.Query(video.ToString());
                            DataSetToModelHelper<sharelist> list2 = new DataSetToModelHelper<sharelist>();
                            if (Daphvido.Tables[0].Rows.Count != 0)
                            {
                                sharelistmodel = list2.FillToModel(Daphvido.Tables[0].Rows[0]);
                                item.videourl = sharelistmodel.videourl;
                                item.videolength = sharelistmodel.videolength;
                                item.playcount = sharelistmodel.playcount;
                            }
                            else
                            {
                                list = null;
                            }


                        }

                    }
                    ///transmit表
                    else if (item.source == 1)
                    {
                        StringBuilder transarticle = new StringBuilder();
                        transarticle.Append("SELECT P_Type as type FROM P_Transmit ");
                        transarticle.Append(" where P_Transmit.P_UserId='" + item.shareuserid + "'P_Transmit.P_OrganizeId='" + groupid + "' ");
                        DataSet aa = DbHelperSQL.Query(transarticle.ToString());
                        DataTable tb1 = aa.Tables[0];
                        DataRow row1 = tb1.Rows[0];
                        item.type = int.Parse(aa.Tables[0].Rows[0][0].ToString());
                        //党建风采文章
                        if (Convert.ToInt32(item.type) == 53)
                        {
                            StringBuilder article = new StringBuilder();
                            article.Append("SELECT dt_article.id as articleid,dt_article.content AS content,dt_article.title as articletitle FROM dt_article ");
                            article.Append("where dt_article.id='" + item.sharedataid + "' and dt_article.user_id='" + item.shareuserid + "' ");

                            DataSet Daphoto = DbHelperSQL.Query(article.ToString());
                            if (Daphoto.Tables[0].Rows.Count != 0)
                            {
                                DataSetToModelHelper<image> MH = new DataSetToModelHelper<image>();
                                item.imageurl = MH.FillModel(Daphoto);
                            }
                            else
                            {
                                item.imageurl = null;

                            }

                        }
                        //视频
                        else if (Convert.ToInt32(item.type) == 52)
                        {

                            StringBuilder video = new StringBuilder();
                            video.Append("SELECT convert(varchar,dt_article.id) as courseid,P_Video.P_Url as videourl,P_Video.P_VideoLength as videolength,(SELECT COUNT(P_Video.P_ParentId) FROM P_Video where P_Video.P_ParentId='1212') as playcount FROM P_Video ");
                            video.Append("left JOIN P_BranchPublish on P_BranchPublish.P_Id=P_Video.P_ParentId ");
                            video.Append("left JOIN dt_article on dt_article.id=P_Video.P_ParentId ");
                            video.Append("left JOIN P_PartyCloudShare on P_PartyCloudShare.P_Id=P_Video.P_ParentId ");
                            video.Append("left JOIN P_Transmit on P_Transmit.P_Id=P_Video.P_ParentId ");
                            video.Append("where P_Video.P_ParentId='" + item.sharedataid + "' ");
                            DataSet Daphvido = DbHelperSQL.Query(video.ToString());
                            DataSetToModelHelper<sharelist> list2 = new DataSetToModelHelper<sharelist>();
                            if (Daphvido.Tables[0].Rows.Count != 0)
                            {
                                sharelistmodel = list2.FillToModel(Daphvido.Tables[0].Rows[0]);
                                item.videourl = sharelistmodel.videourl;
                                item.videolength = sharelistmodel.videolength;
                                item.playcount = sharelistmodel.playcount;
                            }
                            else
                            {
                                list = null;
                            }


                        }


                    }
                    ///cloudshare表
                    else if (item.source == 2)
                    {
                        P_PartyCloud partycloud = new P_PartyCloud();
                        StringBuilder cloudphoto = new StringBuilder();
                        cloudphoto.Append("SELECT P_Type as type FROM P_PartyCloudShare ");
                        cloudphoto.Append(" where P_PartyCloudShare.P_CreaterId='" + item.shareuserid + "'P_PartyCloudShare.P_BranchId='" + groupid + "' ");
                        DataSet aa = DbHelperSQL.Query(cloudphoto.ToString());
                        DataTable tb2 = aa.Tables[0];
                        DataRow row2 = tb2.Rows[0];
                        item.type = int.Parse(aa.Tables[0].Rows[0][0].ToString());

                        //图片
                        if (Convert.ToInt32(item.type) == 0)
                        {
                            UserGroupHelper usergroup = new UserGroupHelper();
                            int mingroup = usergroup.GetUserMinimumGroupId(Convert.ToInt32(partycloud.P_UserId));
                            StringBuilder photo = new StringBuilder();
                            photo.Append("SELECT P_Image.P_ImageUrl as imageurl,P_Image.P_Picturename as imagename,  ");
                            photo.Append("(dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId） as creatername, ");
                            photo.Append("CONVERT(VARCHAR(10),P_PartyCloud.P_CreateTime,120) as createtime,dt_user_groups.title as groupname, ");
                            photo.Append(" P_PartyCloud.P_Size as size ");
                            photo.Append("FROM P_PartyCloudShare ");
                            photo.Append("LEFT JOIN P_Image on P_Image.P_ImageId=P_PartyCloudShare.P_Id and P_Image.P_ImageType='20010' ");
                            photo.Append("LEFT JOIN dt_users on dt_users.id=" + Convert.ToInt32(partycloud.P_UserId) + @" ");
                            photo.Append("LEFT JOIN P_PartyCloud on P_PartyCloud.P_Id=P_PartyCloudShare.P_PartyCloudId ");
                            photo.Append("LEFT JOIN dt_user_groups on dt_user_groups.id=" + mingroup + @" ");
                            photo.Append("where P_Image.P_ImageId='" + item.sharedataid + "' ");

                            DataSet Daphoto = DbHelperSQL.Query(photo.ToString());
                            if (Daphoto.Tables[0].Rows.Count != 0)
                            {
                                DataSetToModelHelper<image> MH = new DataSetToModelHelper<image>();
                                item.imageurl = MH.FillModel(Daphoto);
                            }
                            else
                            {
                                item.imageurl = null;

                            }

                        }
                        //视频
                        else if (Convert.ToInt32(item.type) == 1)
                        {

                            StringBuilder video = new StringBuilder();
                            video.Append("SELECT convert(varchar,dt_article.id) as courseid,P_Video.P_VideoName as videoname,");
                            video.Append("P_Video.P_Url as videourl,P_Video.P_VideoLength as videolength,(SELECT COUNT(P_Video.P_ParentId) ");
                            video.Append("FROM P_Video where P_Video.P_ParentId='1212') as playcount,");
                            video.Append("(dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId） as creatername,");
                            video.Append("CONVERT(VARCHAR(10),P_PartyCloud.P_CreateTime,120) as createtime,dt_user_groups.title as groupname,");
                            video.Append("P_PartyCloud.P_Size as size ");
                            video.Append("FROM P_Video ");
                            video.Append("left JOIN P_BranchPublish on P_BranchPublish.P_Id=P_Video.P_ParentId ");
                            video.Append("left JOIN dt_article on dt_article.id=P_Video.P_ParentId ");
                            video.Append("left JOIN P_PartyCloudShare on P_PartyCloudShare.P_Id=P_Video.P_ParentId ");
                            video.Append("left JOIN P_PartyCloud on P_PartyCloud.P_Id=P_Video.P_ParentId ");
                            video.Append("left JOIN dt_users on dt_users.id=P_Video.P_CreateUser ");
                            video.Append("left JOIN dt_user_groups on dt_user_groups.id=dt_users.group_id");
                            video.Append("where P_Video.P_ParentId='" + item.sharedataid + "' ");
                            DataSet Daphvido = DbHelperSQL.Query(video.ToString());
                            DataSetToModelHelper<sharelist> list2 = new DataSetToModelHelper<sharelist>();
                            if (Daphvido.Tables[0].Rows.Count != 0)
                            {
                                sharelistmodel = list2.FillToModel(Daphvido.Tables[0].Rows[0]);
                                item.videourl = sharelistmodel.videourl;
                                item.videolength = sharelistmodel.videolength;
                                item.playcount = sharelistmodel.playcount;
                            }
                            else
                            {
                                list = null;
                            }


                        }
                        //文档
                        else if (Convert.ToInt32(item.type) == 2)
                        {

                            StringBuilder document = new StringBuilder();
                            document.Append("SELECT P_Document.P_DocUrl as docurl,P_Document.P_Title as doctitle,");
                            document.Append("(dt_users.user_name from dt_users where dt_users.id =P_PartyCloudShare.P_CreaterId） as creatername, ");
                            document.Append("CONVERT(VARCHAR(10),P_PartyCloud.P_CreateTime,120) as createtime,dt_user_groups.title as groupname, ");
                            document.Append("P_PartyCloud.P_Size as size ");
                            document.Append("FROM P_Document");
                            document.Append("left join dt_users on dt_users.id=P_Document.P_CreateUser");
                            document.Append("left join P_PartyCloudShare on P_Document.P_RelationId=P_PartyCloudShare.P_Id ");
                            document.Append("left join P_PartyCloud on P_PartyCloudShare.P_PartyCloudId=P_PartyCloud.P_Id");
                            document.Append("left join dt_user_groups on dt_user_groups.id=dt_users.group_id");
                            document.Append("where P_Document.P_RelationId='" + item.sharedataid + "' ");
                            DataSet Dadocument = DbHelperSQL.Query(document.ToString());

                            DataSetToModelHelper<sharelist> list2 = new DataSetToModelHelper<sharelist>();
                            if (Dadocument.Tables[0].Rows.Count != 0)
                            {
                                sharelistmodel = list2.FillToModel(Dadocument.Tables[0].Rows[0]);
                                item.docurl = sharelistmodel.docurl;
                                item.doctitle = sharelistmodel.doctitle;
                                item.creatername = sharelistmodel.creatername;
                                item.createtime = sharelistmodel.createtime;
                                item.groupname = sharelistmodel.groupname;
                                item.size = sharelistmodel.size;
                            }
                            else
                            {
                                list = null;
                            }

                        }

                    }


                    ///评论数量
                    StringBuilder st = new StringBuilder();
                    st.Append("SELECT COUNT(dt_article_comment.relation_id) as commentcount FROM dt_article_comment ");
                    st.Append("LEFT JOIN P_BranchPublish ON dt_article_comment.relation_id=P_BranchPublish.P_Id  ");
                    st.Append("LEFT JOIN P_PartyCloudShare ON dt_article_comment.relation_id=P_PartyCloudShare.P_Id  ");
                    st.Append("LEFT JOIN P_Transmit ON dt_article_comment.relation_id=P_Transmit.P_Id  ");
                    st.Append("where dt_article_comment.relation_id='" + item.sharedataid + "'");
                    DataSet dt = DbHelperSQL.Query(st.ToString());

                    if (dt.Tables[0].Rows.Count != 0)
                    {
                        DataTable tb3 = dt.Tables[0];
                        DataRow row3 = tb3.Rows[0];
                        item.commentcount = int.Parse(dt.Tables[0].Rows[0][0].ToString());

                    }
                    else
                    {
                        item.commentcount = 0;

                    }
                    ///点赞数量
                    StringBuilder st1 = new StringBuilder();
                    st1.Append("SELECT COUNT(P_ThumbUp.P_ArticleId) as thumbcount FROM P_ThumbUp ");
                    st1.Append("LEFT JOIN P_BranchPublish ON P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id ");
                    st1.Append("LEFT JOIN P_PartyCloudShare ON P_ThumbUp.P_ArticleId=P_PartyCloudShare.P_Id ");
                    st1.Append("LEFT JOIN P_Transmit ON P_ThumbUp.P_ArticleId=P_Transmit.P_Id ");
                    st1.Append("where P_ThumbUp.P_ArticleId='" + item.sharedataid + "'");
                    DataSet dt2 = DbHelperSQL.Query(st1.ToString());
                    if (dt2.Tables[0].Rows.Count != 0)
                    {
                        DataTable tb2 = dt2.Tables[0];
                        DataRow row2 = tb2.Rows[0];
                        Console.WriteLine("Result" + row2["thumbcount"]);
                        item.thumbcount = int.Parse(dt2.Tables[0].Rows[0][0].ToString());
                    }
                    else
                    {
                        item.thumbcount = 0;

                    }
                    //return sl;
                }
                //};





            }
            catch (Exception ex)
            {

            }
            return sl;
        }

        #region MyRegion
        /// P_PartyCloudShare表的人员基本信息

        //StringBuilder strcloud = new StringBuilder();
        //strcloud.Append("SELECT P_PartyCloudShare.P_Id as branchid,P_PartyCloudShare.P_Type as type,P_Image.P_ImageUrl as headimage,dt_users.user_name as username,P_PartyCloudShare.P_CreateTime as createtime FROM P_PartyCloudShare ");
        //strcloud.Append("LEFT JOIN dt_users ON P_PartyCloudShare.P_CreaterId = dt_users.id ");
        //strcloud.Append("LEFT JOIN P_Image ON P_Image.P_ImageId=P_PartyCloudShare.P_PartyCloudId and P_Image.P_ImageType='20011' ");
        //DataSet dtcloud = DbHelperSQL.Query(strcloud.ToString());
        //DataSetToModelHelper<sharelist> list1 = new DataSetToModelHelper<sharelist>();
        //if (dtcloud.Tables[0].Rows.Count != 0)
        //{
        //    sh = list1.FillModel(dtcloud);

        //}
        //else
        //{

        //    list1 = null;

        //}
        //List<sharelist> sh = new List<sharelist>();
        //foreach (var item in sl)
        //{
        //    //多张图片
        //    if (sharelistmodel.type == 1)
        //    {
        //        StringBuilder photocloud = new StringBuilder();
        //        photocloud.Append("SELECT P_Image.P_ImageUrl as imageurl FROM P_Image ");
        //        photocloud.Append("left join P_Image on P_Image.P_ImageId=P_PartyCloudShare.P_Id and P_Image.P_ImageType='20010' ");
        //        photocloud.Append("where P_Image.P_ImageId='" + item.sharedataid + "' ");

        //        DataSet Daphotocloud = DbHelperSQL.Query(photocloud.ToString());
        //        if (Daphotocloud.Tables[0].Rows.Count != 0)
        //        {
        //            DataSetToModelHelper<imageurl> MH = new DataSetToModelHelper<imageurl>();
        //            item.mg = MH.FillModel(Daphotocloud);
        //        }
        //        else
        //        {
        //            item.mg = null;

        //        }

        //    }
        //    //视频
        //    else if (sharelistmodel.type == 2)
        //    {

        //        StringBuilder videocloud = new StringBuilder();
        //        videocloud.Append("SELECT P_Video.P_Url as url FROM P_Video ");
        //        videocloud.Append("left JOIN P_PartyCloudShare on P_PartyCloudShare.P_Id=P_Video.P_ParentId ");
        //        videocloud.Append("where P_Video.P_ParentId='" + item.sharedataid + "' ");
        //        DataSet Daphvidocloud = DbHelperSQL.Query(videocloud.ToString());
        //        if (Daphvidocloud.Tables[0].Rows.Count != 0)
        //        {
        //            DataTable tb = Daphvidocloud.Tables[0];
        //            DataRow row = tb.Rows[0];
        //            item.video = Daphvidocloud.Tables[0].Rows[0][0].ToString();


        //        }
        //        else
        //        {
        //            item.video = null;

        //        }
        //    }


        //    //评论数量
        //    StringBuilder st = new StringBuilder();
        //    st.Append("SELECT COUNT(dt_article_comment.forum_share_id) as commentcount FROM dt_article_comment ");
        //    st.Append("LEFT JOIN P_PartyCloudShare ON dt_article_comment.forum_share_id=P_PartyCloudShare.P_Id  ");
        //    st.Append("where dt_article_comment.forum_share_id='" + item.sharedataid + "'");
        //    DataSet dt = DbHelperSQL.Query(st.ToString());

        //    if (dt.Tables[0].Rows.Count != 0)
        //    {
        //        DataTable tbl = dt.Tables[0];
        //        DataRow row = tbl.Rows[0];
        //        //Console.WriteLine(row["commentcount"]);
        //        item.commentcount = int.Parse(dt.Tables[0].Rows[0][0].ToString());

        //    }
        //    else
        //    {
        //        item.commentcount = 0;

        //    }
        //    ///点赞数量
        //    StringBuilder st1 = new StringBuilder();
        //    st1.Append("SELECT COUNT(P_ThumbUp.P_ArticleId) as thumbcount FROM P_ThumbUp ");
        //    st1.Append("LEFT JOIN P_BranchPublish ON P_ThumbUp.P_ArticleId=P_BranchPublish.P_Id ");
        //    st1.Append("where P_ThumbUp.P_ArticleId='" + item.sharedataid + "'");
        //    DataSet dt2 = DbHelperSQL.Query(st1.ToString());
        //    if (dt2.Tables[0].Rows.Count != 0)
        //    {
        //        DataTable tb2 = dt2.Tables[0];
        //        DataRow row2 = tb2.Rows[0];
        //        Console.WriteLine("Result" + row2["thumbcount"]);
        //        item.thumbcount = int.Parse(dt2.Tables[0].Rows[0][0].ToString());
        //    }
        //    else
        //    {
        //        item.thumbcount = 0;

        //    }

        //}


        /////P_Transmit表人员的基本信息
        ////List<sharelist> sa = new List<sharelist>();
        ////StringBuilder strsq2 = new StringBuilder();
        ////strsql.Append("SELECT P_Transmit.P_Id as branchid,P_Image.P_ImageUrl as headimage,dt_users.user_name as username,P_Transmit.P_CreateTime as createtime,P_Transmit.P_Content FROM P_Transmit");
        ////strsql.Append(" LEFT JOIN dt_users ON P_Transmit.P_UserId = dt_users.id");
        ////strsql.Append(" LEFT JOIN P_Image ON P_Image.P_ImageId=P_Transmit.P_Id and P_Image.P_ImageType='20011' ");
        ////DataSet dstrans = DbHelperSQL.Query(strsq2.ToString());
        ////DataSetToModelHelper<sharelist> list2 = new DataSetToModelHelper<sharelist>();
        ////if (dstrans.Tables[0].Rows.Count != 0)
        ////{
        ////    sa = list.FillModel(dstrans);
        ////}
        ////else
        ////{
        ////    list = null;
        ////}
        //foreach (var item in sl)
        //{
        //    //图片
        //    if (sharelistmodel.type == 53)
        //    {
        //        StringBuilder phototrans = new StringBuilder();
        //        phototrans.Append("SELECT P_Image.P_ImageUrl as imageurl FROM P_Image ");
        //        phototrans.Append("left join P_Transmit on P_Image.P_ImageId=P_Transmit.P_Id and P_Image.P_ImageType='20010' ");
        //        phototrans.Append("where P_Image.P_ImageId='" + item.sharedataid + "' ");

        //        DataSet Daphototrans = DbHelperSQL.Query(phototrans.ToString());
        //        if (Daphototrans.Tables[0].Rows.Count != 0)
        //        {
        //            DataSetToModelHelper<imageurl> MH = new DataSetToModelHelper<imageurl>();
        //            item.mg = MH.FillModel(Daphototrans);
        //        }
        //        else
        //        {
        //            item.mg = null;

        //        }

        //    }
        //    //视频
        //    else if (sharelistmodel.type == 52)
        //    {

        //        StringBuilder videotrans = new StringBuilder();
        //        videotrans.Append("SELECT P_Video.P_Url as url FROM P_Video ");
        //        videotrans.Append("left JOIN P_Transmit on P_Transmit.P_Id=P_Video.P_ParentId ");
        //        videotrans.Append("where P_Video.P_ParentId='" + item.sharedataid + "' ");
        //        DataSet Daphvidocloud = DbHelperSQL.Query(videotrans.ToString());
        //        if (Daphvidocloud.Tables[0].Rows.Count != 0)
        //        {
        //            DataTable tb = Daphvidocloud.Tables[0];
        //            DataRow row = tb.Rows[0];
        //            item.video = Daphvidocloud.Tables[0].Rows[0][0].ToString();


        //        }
        //        else
        //        {
        //            item.video = null;

        //        }
        //    }
        //    ///评论数量
        //    StringBuilder st = new StringBuilder();
        //    st.Append("SELECT COUNT(dt_article_comment.forum_share_id) as commentcount FROM dt_article_comment ");
        //    st.Append("LEFT JOIN P_Transmit ON dt_article_comment.forum_share_id=P_Transmit.P_Id  ");
        //    st.Append("where dt_article_comment.forum_share_id='" + item.sharedataid + "'");
        //    DataSet dt = DbHelperSQL.Query(st.ToString());

        //    if (dt.Tables[0].Rows.Count != 0)
        //    {
        //        DataTable tbl = dt.Tables[0];
        //        DataRow row = tbl.Rows[0];                   
        //        item.commentcount = int.Parse(dt.Tables[0].Rows[0][0].ToString());

        //    }
        //    else
        //    {
        //        item.commentcount = 0;

        //    }
        //    ///点赞数量
        //    StringBuilder st1 = new StringBuilder();
        //    st1.Append("SELECT COUNT(P_ThumbUp.P_ArticleId) as thumbcount FROM P_ThumbUp ");
        //    st1.Append("LEFT JOIN P_Transmit ON P_ThumbUp.P_ArticleId=P_Transmit.P_Id ");
        //    st1.Append("where P_ThumbUp.P_ArticleId='" + item.sharedataid + "'");
        //    DataSet dt2 = DbHelperSQL.Query(st1.ToString());
        //    if (dt2.Tables[0].Rows.Count != 0)
        //    {
        //        DataTable tb2 = dt2.Tables[0];
        //        DataRow row2 = tb2.Rows[0];
        //        Console.WriteLine("Result" + row2["thumbcount"]);
        //        item.thumbcount = int.Parse(dt2.Tables[0].Rows[0][0].ToString());
        //    }
        //    else
        //    {
        //        item.thumbcount = 0;

        //    }
        //} 
        #endregion



        /// <summary>
        /// 支部的分享，类似于朋友圈，微博
        /// </summary>
        public class sharelist
        {
            public string useravatar { get; set; }
            public string sharedataid { get; set; }
            public List<image> imageurl { get; set; }
            public string username { get; set; }
            public string createtime { get; set; }
            public DateTime sharetime { get; set; }
            public string content { get; set; }
            public int commentcount { get; set; }
            public int thumbcount { get; set; }
            public int type { get; set; }
            public string videourl { get; set; }
            public string videoname { get; set; }
            public int videolength { get; set; }
            public int playcount { get; set; }
            public int source { get; set; }
            public string courseid { get; set; }
            public string shareuserid { get; set; }
            public int userthum { get; set; }
            public int usercollect { get; set; }
            public string docurl { get; set; }
            public string doctitle { get; set; }
            public int articleid { get; set; }
            public string articletitle { get; set; }
            public string creatername { get; set; }
            public string groupname { get; set; }
            public string size { get; set; }

        }


        public class image
        {
            public string url { get; set; }

            public string imagename { get; set; }

        }

    }

}










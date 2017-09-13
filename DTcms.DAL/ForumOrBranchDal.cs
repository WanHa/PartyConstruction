using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using DTcms.Model.WebApiModel;
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
    /// <summary>
    /// 论坛、支部、个人中心、收藏列表
    /// </summary>
    public class ForumOrBranchDal
    {
        /// <summary>
        /// 支部和论坛发布信息时,获取需要@的人员 列表
        /// </summary>
        /// <param name="groupId">支部或论坛ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="type">区分是支部还是论坛发布信息0-->支部，1-->论坛</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<AtPersonnelModel> GetNewInfoAtPersonnels(string groupId, string userId, int type, int page, int rows) {

            string sql = String.Empty;

            if (type == 0) {
                // 获取支部下@人员列表
                sql = String.Format(@"select dt_users.id as user_id, dt_users.user_name, P_Image.P_ImageUrl as avatar from dt_users
                        left join P_Image on P_Image.P_ImageId = convert(nvarchar,dt_users.id) and P_Image.P_ImageType = {0}
                        where dt_users.group_id like '%,{1},%'
                        and dt_users.id != {2}", (int)ImageTypeEnum.头像, groupId, userId);
            }
            else {
                // 获取论坛下@人员列表
                sql = String.Format(@"select dt_users.id as user_id, dt_users.user_name,P_Image.P_ImageUrl as avatar from P_PersonGroupRelation
                        left join dt_users on convert(nvarchar,dt_users.id) = P_PersonGroupRelation.P_UserId
                        left join P_Image on P_Image.P_ImageId = P_PersonGroupRelation.P_UserId and P_Image.P_ImageType = {0}
                        where P_PersonGroupRelation.P_Approval = 0 
	                    and P_PersonGroupRelation.P_Status = 0 and
                        P_PersonGroupRelation.P_PartyGroupId = '{1}' 
                        and P_PersonGroupRelation.P_UserId != '{2}'", (int)ImageTypeEnum.头像, groupId, userId);
            }

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "dt_users.id"));
            DataSetToModelHelper<AtPersonnelModel> helper = new DataSetToModelHelper<AtPersonnelModel>();

            return helper.FillModel(ds);
        }

        #region ==================================== 论坛列表
        /// <summary>
        /// 获取论坛信息列表
        /// </summary>
        /// <param name="forumId">论坛ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="page">分页页数</param>
        /// <param name="rows">分页条数</param>
        /// <returns></returns>
        public List<ForumBranchModel> GetForumList(string forumId, string userId, int page, int rows)
        {

            StringBuilder querySql = new StringBuilder();
            querySql.Append("select * from (");
            querySql.Append("select P_ForumShare.P_Id as p_id, P_ForumShare.P_Content as content,P_Image.P_ImageUrl as user_avatar,dt_users.user_name as user_name,dt_users.id as user_id,");
            querySql.Append(" P_ForumShare.P_Source as content_source,P_ForumShare.P_Type as content_type, ");
            querySql.Append("CONVERT(varchar(100), P_ForumShare.P_CreateTime, 23) as create_time,P_ForumShare.P_CreateTime,");
            // 评论数量
            querySql.Append("(select COUNT(dt_article_comment.id) from dt_article_comment where dt_article_comment.relation_id = P_ForumShare.P_Id and dt_article_comment.type = 1) as comment_count,");
            // 点赞数量
            querySql.Append("(select count(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId = P_ForumShare.P_Id and P_ThumbUp.P_FamilyType = 2) as thumb_count");
            querySql.Append(" from P_ForumShare");
            querySql.Append(" left join dt_users on CONVERT(nvarchar,dt_users.id) = P_ForumShare.P_UserId");
            querySql.Append(" left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_ImageType =" + (int)ImageTypeEnum.头像);
            querySql.Append(" where dt_users.id is not null and P_ForumShare.P_GroupForumId = '" + forumId + "'");
            querySql.Append(" and P_UserId not in");
            querySql.Append(" (select P_ShieldUserId from P_Shield where P_Status = 0 and P_Shield.P_UserId = '" + userId + "' and P_RelationId = '" + forumId + "' and P_Source = 1)");
            querySql.Append(" ) as table1");
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, querySql.ToString(), "P_CreateTime desc"));

            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {

                List<ForumBranchModel> result = helper.FillModel(ds);

                foreach (ForumBranchModel item in result)
                {
                    //item.user_group_name = GetUserGroupName(userId);
                    GetCollectAndThumb(item, userId);
                    //// 是否点赞
                    //string thumbSql = String.Format(@"select top 1 P_ThumbUp.P_Id from P_ThumbUp
                    //    where P_ThumbUp.P_ArticleId = '{1}' and P_ThumbUp.P_UserId = '{1}'  and P_ThumbUp.P_FamilyType = 2", item.p_id, userId);
                    //string thumbId = Convert.ToString(DbHelperSQL.GetSingle(thumbSql.ToString()));
                    //item.is_thumb = String.IsNullOrEmpty(thumbId) ? 0 : 1;
                    //// 是否收藏
                    //string collectSql = String.Format(@"select TOP 1 P_Collect.P_Id from P_Collect 
                    //    where P_Collect.P_Relation= '{0}' 
                    //    and P_Collect.P_UserId = '{1}' and (P_Collect.P_Type=54 or P_Collect.P_Type=55)", item.p_id, userId);
                    //string collectId = Convert.ToString(DbHelperSQL.GetSingle(collectSql.ToString()));
                    //item.is_collect = String.IsNullOrEmpty(collectId) ? 0 : 1;
                    // 获取新建的数据
                    if (item.content_source == 0)
                    {
                        item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛发布);
                        switch (item.content_type)
                        {
                            case 1:
                                item.pictures = GetForumContentPics(item.p_id);
                                break;
                            case 2:
                                Model.P_Video videoData = GetForumVideo(item.p_id);
                                item.video_id = videoData.P_Id;
                                item.vodeo_name = videoData.P_VideoName;
                                item.video_pic = videoData.P_VideoPic;
                                item.video_url = videoData.P_Url;
                                item.video_lenght = videoData.P_VideoLength;
                                item.vodeo_play_count = GetFornmVideoPlayCount(videoData.P_Id);
                                item.course_id = "";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛转发);

                        string sql = String.Format(@"select P_Transmit.P_RelationId,P_Transmit.P_Type,P_Transmit.P_Content from P_Transmit
                        left join P_ForumShare on P_ForumShare.P_SourceId = P_Transmit.P_Id
                        where P_ForumShare.P_Id = '{0}'
                        and P_Transmit.P_OrganizeId = '{1}'", item.p_id, forumId);

                        DataSet forwardingData = DbHelperSQL.Query(sql);
                        if (forwardingData.Tables[0] != null && forwardingData.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = forwardingData.Tables[0].Rows[0];
                            item.forwarding_id = dr["P_RelationId"].ToString();
                            item.content_type = Convert.ToInt32(dr["P_Type"].ToString());
                            item.content = dr["P_Content"].ToString();
                        }

                        switch (item.content_type)
                        {
                            case 52:
                                Model.P_Video data = GetCourseVideo(item.forwarding_id);
                                item.video_id = data.P_Id;
                                item.vodeo_name = data.P_VideoName;
                                item.video_pic = data.P_VideoPic;
                                item.video_url = data.P_Url;
                                item.video_lenght = data.P_VideoLength;
                                item.course_id = data.P_ParentId;
                                item.vodeo_play_count = GetFornmVideoPlayCount(data.P_Id);
                                break;
                            case 53:
                            case 56:
                                ForwardingArticleModel articleData = GetForwardingArticle(item.forwarding_id);
                                item.forwarding_id = articleData.forwarding_id;
                                item.forwarding_title = articleData.forwarding_title;
                                item.forwarding_content = articleData.forwarding_content;
                                item.forwarding_pic = articleData.forwarding_pic;
                                break;
                            default:
                                break;
                        }
                    }

                }
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.forwarding_content))
                    {
                        item.forwarding_content = ReplaceStringHtml(item.forwarding_content);
                    }
                }
                return result;
            }
            return null;
        }

        private void GetCollectAndThumb(ForumBranchModel item, string userId) {
            item.user_group_name = GetUserGroupName(item.user_id);
            // 是否点赞
            string thumbSql = String.Format(@"select top 1 P_ThumbUp.P_Id from P_ThumbUp
                        where P_ThumbUp.P_ArticleId = '{0}' and P_ThumbUp.P_UserId = '{1}'  and P_ThumbUp.P_FamilyType = 2", item.p_id, userId);
            string thumbId = Convert.ToString(DbHelperSQL.GetSingle(thumbSql.ToString()));
            item.is_thumb = String.IsNullOrEmpty(thumbId) ? 0 : 1;
            // 是否收藏
            string collectSql = String.Format(@"select TOP 1 P_Collect.P_Id from P_Collect 
                        where P_Collect.P_Relation= '{0}' 
                        and P_Collect.P_UserId = '{1}' and (P_Collect.P_Type=54 or P_Collect.P_Type=55)", item.p_id, userId);
            string collectId = Convert.ToString(DbHelperSQL.GetSingle(collectSql.ToString()));
            item.is_collect = String.IsNullOrEmpty(collectId) ? 0 : 1;
        }

        private List<ContentPicture> GetForumContentPics(string id)
        {

            StringBuilder querySql = new StringBuilder();
            querySql.Append("select P_Image.P_ImageUrl as picture_url from P_Image where P_Image.P_ImageId = @P_ImageId and P_Image.P_ImageType = @P_ImageType");
            SqlParameter[] parameter = {
                new SqlParameter("@P_ImageId", SqlDbType.NVarChar, 50),
                new SqlParameter("@P_ImageType", SqlDbType.Int, 4)
            };
            parameter[0].Value = id;
            parameter[1].Value = (int)ImageTypeEnum.党小组论坛;
            DataSet ds = DbHelperSQL.Query(querySql.ToString(), parameter);
            DataSetToModelHelper<ContentPicture> helper = new DataSetToModelHelper<ContentPicture>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return helper.FillModel(ds);
            }
            return new List<ContentPicture>();
        }

        /// <summary>
        /// 获取论坛下新建的视频信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Model.P_Video GetForumVideo(string id)
        {

            StringBuilder querySql = new StringBuilder();
            querySql.Append("select * from P_Video where P_Video.P_ParentId = @P_ParentId and P_Video.P_Source = @P_Source");

            SqlParameter[] parameter = {
                new SqlParameter("@P_ParentId",SqlDbType.NVarChar, 50),
                new SqlParameter("@P_Source",SqlDbType.Int, 4)
            };
            parameter[0].Value = id;
            parameter[1].Value = (int)VideoSourceEnum.党小组论坛;
            DataSet ds = DbHelperSQL.Query(querySql.ToString(), parameter);
            DataSetToModelHelper<Model.P_Video> helper = new DataSetToModelHelper<Model.P_Video>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            return new Model.P_Video();
        }

        private string GetElegantEemeanour(string id) {

            return "";
        }

        /// <summary>
        /// 获取论坛转发的视频信息
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private Model.P_Video GetCourseVideo(string videoId)
        {

            StringBuilder querySql = new StringBuilder();
            querySql.Append("select * from P_Video where P_Video.P_Id = @P_Id");

            SqlParameter[] parameter = {
                new SqlParameter("@P_Id",SqlDbType.NVarChar, 50),
            };
            parameter[0].Value = videoId;
            DataSet ds = DbHelperSQL.Query(querySql.ToString(), parameter);
            DataSetToModelHelper<Model.P_Video> helper = new DataSetToModelHelper<Model.P_Video>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            return new Model.P_Video();
        }

        /// <summary>
        /// 视频点击次数
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private int GetFornmVideoPlayCount(string videoId)
        {

            string querySql = String.Format(@"select COUNT(P_VideoRecord.P_Id) from P_VideoRecord
                        where P_VideoRecord.P_VideoId = '{0}'", videoId);

            return Convert.ToInt32(DbHelperSQL.GetSingle(querySql));
        }

        /// <summary>
        /// 获取用户最小党组织名称
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GetUserGroupName(int userId)
        {

            string sql = String.Format(@"select TOP 1 dt_user_groups.title from F_Split(
                    (select dt_users.group_id from dt_users where dt_users.id = {0}),',') as t
                    left join dt_user_groups on dt_user_groups.id = t.value
                    where t.value != ''
                    order by dt_user_groups.grade DESC", userId);

            return Convert.ToString(DbHelperSQL.GetSingle(sql));
        }

        /// <summary>
        /// 获取转发article表中的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ForwardingArticleModel GetForwardingArticle(string id) {

            string sql = String.Format(@"select dt_article.title as forwarding_title,
                        dt_article.content as forwarding_content,
                        CONVERT(nvarchar,dt_article.id) as forwarding_id,
                        P_Image.P_ImageUrl as forwarding_pic from dt_article
                        left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_article.id)
                        where dt_article.id = {0}", id);

            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<ForwardingArticleModel> helper = new DataSetToModelHelper<ForwardingArticleModel>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                return helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            return new ForwardingArticleModel();
        }

        #endregion

        #region ============================ 支部列表

        public List<ForumBranchModel> GetbranchList(string branchId, string userId, int page, int rows) {

            StringBuilder querySql = new StringBuilder();
            querySql.Append("select * from (");
            querySql.Append("select P_BranchPublish.P_Id as p_id,P_BranchPublish.P_Content as content,P_BranchPublish.P_Source as content_source,");
            querySql.Append("P_BranchPublish.P_Type as content_type,P_Image.P_ImageUrl as user_avatar,dt_users.user_name as user_name,dt_users.id as user_id,");
            querySql.Append("CONVERT(varchar(100), P_BranchPublish.P_CreateTime, 23) as create_time,P_BranchPublish.P_CreateTime,");
            // 评论数量
            querySql.Append("(select COUNT(dt_article_comment.id) from dt_article_comment where dt_article_comment.relation_id = P_BranchPublish.P_Id and dt_article_comment.type = 2) as comment_count,");
            // 点赞数量
            querySql.Append("(select count(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId = P_BranchPublish.P_Id and P_ThumbUp.P_FamilyType = 3) as thumb_count");
            querySql.Append(" from P_BranchPublish");
            querySql.Append(" left join dt_users on CONVERT(nvarchar,dt_users.id) = P_BranchPublish.P_UserId");
            querySql.Append(" left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_ImageType =" + +(int)ImageTypeEnum.头像);
            querySql.Append(" where dt_users.id is not null and P_BranchPublish.P_BranchId = '" + branchId + "'");
            querySql.Append(" and P_BranchPublish.P_UserId not in (");
            querySql.Append(" select P_ShieldUserId from P_Shield where P_Status = 0 and P_Shield.P_UserId = '"
                + userId + "' and P_RelationId = '" + branchId + "' and P_Source = 0)");
            querySql.Append(" ) as table1");
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, querySql.ToString(), "P_CreateTime desc"));

            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                List<ForumBranchModel> result = helper.FillModel(ds);
                foreach (ForumBranchModel item in result)
                {
                    //item.user_group_name = GetUserGroupName(userId);
                    //// 是否点赞
                    //string thumbSql = String.Format(@"select top 1 P_ThumbUp.P_Id from P_ThumbUp
                    //    where P_ThumbUp.P_ArticleId = '{1}' and P_ThumbUp.P_UserId = '{1}'  and P_ThumbUp.P_FamilyType = 3", item.p_id, userId);
                    //string thumbId = Convert.ToString(DbHelperSQL.GetSingle(thumbSql.ToString()));
                    //item.is_thumb = String.IsNullOrEmpty(thumbId) ? 0 : 1;
                    //// 是否收藏
                    //string collectSql = String.Format(@"select TOP 1 P_Collect.P_Id from P_Collect 
                    //    where P_Collect.P_Relation= '{0}' 
                    //    and P_Collect.P_UserId = '{1}' and (P_Collect.P_Type=56 or P_Collect.P_Type=57 or P_Collect.P_Type= 58)", item.p_id, userId);
                    //string collectId = Convert.ToString(DbHelperSQL.GetSingle(collectSql.ToString()));
                    //item.is_collect = String.IsNullOrEmpty(collectId) ? 0 : 1;

                    GetBranchCollectAndThumb(item, userId);
                    switch (item.content_source)
                    {
                        case 0:
                            GetBranchData(item);
                            break;
                        case 1:
                            GetBranchForwarding(item);
                            break;
                        default:
                            GetBranchShare(item);
                            break;
                    }

                }
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.forwarding_content))
                    {
                        item.forwarding_content = ReplaceStringHtml(item.forwarding_content);
                    }
                }
                return result;
            }


            return new List<ForumBranchModel>();
        }

        private void GetBranchCollectAndThumb(ForumBranchModel item, string userId) {
            item.user_group_name = GetUserGroupName(item.user_id);
            // 是否点赞
            string thumbSql = String.Format(@"select top 1 P_ThumbUp.P_Id from P_ThumbUp
                        where P_ThumbUp.P_ArticleId = '{0}' and P_ThumbUp.P_UserId = '{1}'  and P_ThumbUp.P_FamilyType = 3", item.p_id, userId);
            string thumbId = Convert.ToString(DbHelperSQL.GetSingle(thumbSql.ToString()));
            item.is_thumb = String.IsNullOrEmpty(thumbId) ? 0 : 1;
            // 是否收藏
            string collectSql = String.Format(@"select TOP 1 P_Collect.P_Id from P_Collect 
                        where P_Collect.P_Relation= '{0}' 
                        and P_Collect.P_UserId = '{1}' and (P_Collect.P_Type=56 or P_Collect.P_Type=57 or P_Collect.P_Type= 58)", item.p_id, userId);
            string collectId = Convert.ToString(DbHelperSQL.GetSingle(collectSql.ToString()));
            item.is_collect = String.IsNullOrEmpty(collectId) ? 0 : 1;
        }

        /// <summary>
        /// 获取支部新建的数据
        /// </summary>
        /// <param name="item"></param>
        private void GetBranchData(ForumBranchModel item) {
            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.支部发布);
            switch (item.content_type)
            {
                case 1:
                    item.pictures = GetBranchContentPics(item.p_id);
                    break;
                case 2:
                    Model.P_Video video = GetBranchVideo(item.p_id);
                    item.video_id = video.P_Id;
                    item.vodeo_name = video.P_VideoName;
                    item.video_pic = video.P_VideoPic;
                    item.video_url = video.P_Url;
                    item.video_lenght = video.P_VideoLength;
                    item.vodeo_play_count = GetFornmVideoPlayCount(video.P_Id);
                    item.course_id = "";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取支部转发信息
        /// </summary>
        /// <param name="item"></param>
        private void GetBranchForwarding(ForumBranchModel item) {

            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.支部转发);

            string sql = String.Format(@"select P_Transmit.P_RelationId,P_Transmit.P_Type,P_Transmit.P_Content from P_Transmit
                        left join P_BranchPublish on P_BranchPublish.P_SourceId = P_Transmit.P_Id
						and P_Transmit.P_OrganizeId = P_BranchPublish.P_BranchId
                        where P_BranchPublish.P_Id = '{0}'", item.p_id);

            DataSet forwardingData = DbHelperSQL.Query(sql);
            if (forwardingData.Tables[0] != null && forwardingData.Tables[0].Rows.Count > 0)
            {
                DataRow dr = forwardingData.Tables[0].Rows[0];
                item.forwarding_id = dr["P_RelationId"].ToString();
                item.content_type = Convert.ToInt32(dr["P_Type"].ToString());
                item.content = dr["P_Content"].ToString();
            }

            switch (item.content_type)
            {
                case 52:
                    Model.P_Video data = GetCourseVideo(item.forwarding_id);
                    item.video_id = data.P_Id;
                    item.vodeo_name = data.P_VideoName;
                    item.video_pic = data.P_VideoPic;
                    item.video_url = data.P_Url;
                    item.video_lenght = data.P_VideoLength;
                    item.course_id = data.P_ParentId;
                    item.vodeo_play_count = GetFornmVideoPlayCount(data.P_Id);
                    break;
                case 53:
                case 56:
                    ForwardingArticleModel articleData = GetForwardingArticle(item.forwarding_id);
                    item.forwarding_id = articleData.forwarding_id;
                    item.forwarding_title = articleData.forwarding_title;
                    item.forwarding_content = articleData.forwarding_content;
                    item.forwarding_pic = articleData.forwarding_pic;
                    break;
                default:
                    break;
            }

        }

        public void GetBranchShare(ForumBranchModel item) {

            string querySql = String.Format(@"select 
                        P_PartyCloudShare.P_PartyCloudId,
                        P_PartyCloudShare.P_Type
                        from P_PartyCloudShare
                        left join P_BranchPublish on P_BranchPublish.P_SourceId = P_PartyCloudShare.P_Id
                        AND P_PartyCloudShare.P_BranchId = P_BranchPublish.P_BranchId
                        where P_BranchPublish.P_Id = '{0}'", item.p_id);

            DataSet ds = DbHelperSQL.Query(querySql);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                DataRow dr = ds.Tables[0].Rows[0];
                item.forwarding_id = dr["P_PartyCloudId"].ToString();
                item.content_type = Convert.ToInt32(dr["P_Type"].ToString());
            }

            switch (item.content_type)
            {
                case 0:
                    GetCloudPic(item);
                    break;
                case 1:
                    GetCloudVideo(item);
                    //item.video_id = videoData.P_Id;
                    //item.video_lenght = videoData.P_VideoLength;
                    //item.video_pic = videoData.P_VideoPic;
                    //item.vodeo_name = videoData.P_VideoName;
                    //item.forwarding_title = videoData.P_VideoName;
                    //item.video_url = videoData.P_Url;
                    //item.vodeo_play_count = GetFornmVideoPlayCount(item.forwarding_id);
                    break;
                case 2:
                     GetCloudDocument(item);
                    //item.forwarding_title = documentData.P_Title;
                    break;
                default:
                    break;
            }

        }

        private void GetCloudPic(ForumBranchModel item) {

            string sql = String.Format(@"select P_PartyCloud.P_Size as share_file_size,
                    P_Image.P_ImageUrl as share_file_url,
                    P_Image.P_PictureName as share_file_title,
                    dt_users.id,
                    dt_users.user_name as share_user_name,
                    CONVERT(varchar(100), P_PartyCloud.P_CreateTime, 23) as share_create_time
                    from P_Image
                    left join P_PartyCloud on P_PartyCloud.P_Id = P_Image.P_ImageId
                    left join dt_users on CONVERT(nvarchar,dt_users.id) = P_PartyCloud.P_UserId
                    where P_Image.P_ImageId = '{0}' and P_Image.P_ImageType = {1}",
                    item.forwarding_id, (int)ImageTypeEnum.党建云);

            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                DataRow dr = ds.Tables[0].Rows[0];
                item.share_file_pic = dr["share_file_url"].ToString();
                item.share_file_size = Convert.ToInt64(String.IsNullOrEmpty(dr["share_file_size"].ToString()) ? "0" : dr["share_file_size"].ToString());
                item.share_create_time = dr["share_create_time"].ToString();
                item.share_file_title = dr["share_file_title"].ToString();
                item.share_file_url = dr["share_file_url"].ToString();
                item.share_user_name = dr["share_user_name"].ToString();
                int userid = String.IsNullOrEmpty(dr["id"].ToString()) ? 0 : Convert.ToInt32(dr["id"].ToString());
                item.share_user_group = GetUserGroupName(userid);
            }
        }

        private void GetCloudDocument(ForumBranchModel item) {

            string sql = String.Format(@"select 
                        P_Document.P_Title as share_file_title,
                        P_Document.P_DocUrl as share_file_url,
                        dt_users.user_name as share_user_name,
                        dt_users.id,
                        P_PartyCloud.P_Size as share_file_size,
                        CONVERT(varchar(100), P_PartyCloud.P_CreateTime, 23) as share_create_time
                        from P_Document 
                        left join P_PartyCloud on P_PartyCloud.P_Id = P_Document.P_RelationId
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_PartyCloud.P_UserId
                        where P_Document.P_Id = '{0}'", item.forwarding_id);

            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<Model.P_Document> helper = new DataSetToModelHelper<P_Document>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                DataRow dr = ds.Tables[0].Rows[0];
                item.share_create_time = dr["share_create_time"].ToString();
                item.share_file_size = Convert.ToInt64(String.IsNullOrEmpty(dr["share_file_size"].ToString()) ? "0" : dr["share_file_size"].ToString());
                item.share_file_title = dr["share_file_title"].ToString();
                item.share_file_url = dr["share_file_url"].ToString();
                item.share_user_name = dr["share_user_name"].ToString();
                int userid = String.IsNullOrEmpty(dr["id"].ToString()) ? 0 : Convert.ToInt32(dr["id"].ToString());
                item.share_user_group = GetUserGroupName(userid);
            }
        }

        private void GetCloudVideo(ForumBranchModel item) {

            string sql = String.Format(@"select P_PartyCloud.P_Size as share_file_size,
                        P_Video.P_Url as share_file_url,
                        P_Video.P_VideoName as share_file_title,
                        P_Video.P_VideoPic as share_file_pic,
                        dt_users.user_name as share_user_name,
                        dt_users.id,
                        CONVERT(varchar(100), P_PartyCloud.P_CreateTime, 23) as share_create_time
                        from P_Video
                        left join P_PartyCloud on P_PartyCloud.P_Id = P_Video.P_ParentId
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_PartyCloud.P_UserId
                        where P_Video.P_Id = '{0}'",
                        item.forwarding_id);

            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                DataRow dr = ds.Tables[0].Rows[0];
                item.share_create_time = dr["share_create_time"].ToString();
                item.share_file_pic = dr["share_file_pic"].ToString();
                item.share_file_size = Convert.ToInt64(String.IsNullOrEmpty(dr["share_file_size"].ToString()) ? "0" : dr["share_file_size"].ToString());
                item.share_file_title = dr["share_file_title"].ToString();
                item.share_file_url = dr["share_file_url"].ToString();
                item.share_user_name = dr["share_user_name"].ToString();
                int userid = String.IsNullOrEmpty(dr["id"].ToString()) ? 0 : Convert.ToInt32(dr["id"].ToString());
                item.share_user_group = GetUserGroupName(userid);
            }
        }

        /// <summary>
        /// 获取支部新建图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<ContentPicture> GetBranchContentPics(string id)
        {

            StringBuilder querySql = new StringBuilder();
            querySql.Append("select P_Image.P_ImageUrl as picture_url from P_Image where P_Image.P_ImageId = @P_ImageId and P_Image.P_ImageType = @P_ImageType");
            SqlParameter[] parameter = {
                new SqlParameter("@P_ImageId", SqlDbType.NVarChar, 50),
                new SqlParameter("@P_ImageType", SqlDbType.Int, 4)
            };
            parameter[0].Value = id;
            parameter[1].Value = (int)ImageTypeEnum.支部管理分享;
            DataSet ds = DbHelperSQL.Query(querySql.ToString(), parameter);
            DataSetToModelHelper<ContentPicture> helper = new DataSetToModelHelper<ContentPicture>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return helper.FillModel(ds);
            }
            return new List<ContentPicture>();
        }

        /// <summary>
        /// 获取支部新建视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Model.P_Video GetBranchVideo(string id)
        {

            StringBuilder querySql = new StringBuilder();
            querySql.Append("select * from P_Video where P_Video.P_ParentId = @P_ParentId and P_Video.P_Source = @P_Source");

            SqlParameter[] parameter = {
                new SqlParameter("@P_ParentId",SqlDbType.NVarChar, 50),
                new SqlParameter("@P_Source",SqlDbType.Int, 4)
            };
            parameter[0].Value = id;
            parameter[1].Value = (int)VideoSourceEnum.支部;
            DataSet ds = DbHelperSQL.Query(querySql.ToString(), parameter);
            DataSetToModelHelper<Model.P_Video> helper = new DataSetToModelHelper<Model.P_Video>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            return new Model.P_Video();
        }
        #endregion

        #region ======================个人中心首页列表

        public List<ForumBranchModel> GetPersonalCenter(string userId, int page, int rows) {

            string querySql = String.Format(@"select * from (
                        select P_BranchPublish.P_Id as p_id,
                        P_BranchPublish.P_Content as content,
                        P_BranchPublish.P_Source + 2 as content_source,
                        P_BranchPublish.P_Type as content_type,P_Image.P_ImageUrl as user_avatar,dt_users.user_name as user_name,dt_users.id as user_id,
                        CONVERT(varchar(100), P_BranchPublish.P_CreateTime, 23) as create_time,P_BranchPublish.P_CreateTime,
                        (select COUNT(dt_article_comment.id) from dt_article_comment 
                        where dt_article_comment.relation_id = P_BranchPublish.P_Id and dt_article_comment.type = 2) as comment_count,
                        (select count(P_ThumbUp.P_Id) from P_ThumbUp 
                        where P_ThumbUp.P_ArticleId = P_BranchPublish.P_Id and P_ThumbUp.P_FamilyType = 3) as thumb_count
                        from P_BranchPublish
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_BranchPublish.P_UserId
                        left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_ImageType = {1}
                        where P_UserId = '{0}'
                        UNION ALL
                        select P_ForumShare.P_Id as p_id, 
                        P_ForumShare.P_Content as content,
                        P_ForumShare.P_Source as content_source,
                        P_ForumShare.P_Type as content_type, 
                        P_Image.P_ImageUrl as user_avatar,dt_users.user_name as user_name,dt_users.id as user_id,
                        CONVERT(varchar(100), P_ForumShare.P_CreateTime, 23) as create_time,P_ForumShare.P_CreateTime,
                        (select COUNT(dt_article_comment.id) from dt_article_comment 
                        where dt_article_comment.relation_id = P_ForumShare.P_Id and dt_article_comment.type = 1) as comment_count,
                        (select count(P_ThumbUp.P_Id) from P_ThumbUp 
                        where P_ThumbUp.P_ArticleId = P_ForumShare.P_Id and P_ThumbUp.P_FamilyType = 2) as thumb_count
                        from P_ForumShare
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_ForumShare.P_UserId
                        left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_ImageType = {1}
                        where P_UserId = '{0}'
                        ) as table1", userId, (int)ImageTypeEnum.头像);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, querySql.ToString(), "P_CreateTime desc"));

            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {

                List<ForumBranchModel> result = helper.FillModel(ds);

                foreach (ForumBranchModel item in result)
                {
                    switch (item.content_source)
                    {
                        case 0: // 论坛新建
                            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛发布);
                            GetCollectAndThumb(item, userId);
                            switch (item.content_type)
                            {
                                case 1:
                                    item.pictures = GetForumContentPics(item.p_id);
                                    break;
                                case 2:
                                    Model.P_Video videoData = GetForumVideo(item.p_id);
                                    item.video_id = videoData.P_Id;
                                    item.vodeo_name = videoData.P_VideoName;
                                    item.video_pic = videoData.P_VideoPic;
                                    item.video_url = videoData.P_Url;
                                    item.video_lenght = videoData.P_VideoLength;
                                    item.vodeo_play_count = GetFornmVideoPlayCount(videoData.P_Id);
                                    item.course_id = "";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 1: // 论坛转发
                            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛转发);
                            GetCollectAndThumb(item, userId);
                            string sql = String.Format(@"select P_Transmit.P_RelationId,P_Transmit.P_Type,P_Transmit.P_Content from P_Transmit
                                left join P_ForumShare on P_ForumShare.P_SourceId = P_Transmit.P_Id
                                and P_ForumShare.P_GroupForumId = P_Transmit.P_OrganizeId
                                where P_ForumShare.P_Id = '{0}'", item.p_id);

                            DataSet forwardingData = DbHelperSQL.Query(sql);
                            if (forwardingData.Tables[0] != null && forwardingData.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = forwardingData.Tables[0].Rows[0];
                                item.forwarding_id = dr["P_RelationId"].ToString();
                                item.content_type = Convert.ToInt32(dr["P_Type"].ToString());
                                item.content = dr["P_Content"].ToString();
                            }

                            switch (item.content_type)
                            {
                                case 52:
                                    Model.P_Video data = GetCourseVideo(item.forwarding_id);
                                    item.video_id = data.P_Id;
                                    item.vodeo_name = data.P_VideoName;
                                    item.video_pic = data.P_VideoPic;
                                    item.video_url = data.P_Url;
                                    item.video_lenght = data.P_VideoLength;
                                    item.course_id = data.P_ParentId;
                                    item.vodeo_play_count = GetFornmVideoPlayCount(data.P_Id);
                                    break;
                                case 53:
                                case 56:
                                    ForwardingArticleModel articleData = GetForwardingArticle(item.forwarding_id);
                                    item.forwarding_id = articleData.forwarding_id;
                                    item.forwarding_title = articleData.forwarding_title;
                                    item.forwarding_content = articleData.forwarding_content;
                                    item.forwarding_pic = articleData.forwarding_pic;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 2: // 支部新建
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchData(item);
                            break;
                        case 3: // 支部转发
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchForwarding(item);
                            break;
                        case 4: // 支部分享
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchShare(item);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.forwarding_content))
                    {
                        item.forwarding_content = ReplaceStringHtml(item.forwarding_content);
                    }
                }
                return result;
            }
            return new List<ForumBranchModel>();
        }

        #endregion

        #region ======================= 个人中心我的收藏列表
        public List<ForumBranchModel> GetPersonalCenterCollect(string userId, int page, int rows)
        {

            string collectQuerySql = String.Format(@"select P_Collect.P_Relation as p_id,
                        CONVERT( int,P_Collect.P_Type) as content_source,P_Collect.P_CreateTime
                        from P_Collect where P_Collect.P_UserId = '{0}'", userId);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, collectQuerySql, "P_CreateTime desc"));
            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                List<ForumBranchModel> data = helper.FillModel(ds); ;
                List<ForumBranchModel> result = new List<ForumBranchModel>();
                foreach (ForumBranchModel item in data)
                {
                    switch (item.content_source)
                    {
                        case 52: // 视频
                            ForumBranchModel videoItem = GetCollectVideo(item, userId);
                            result.Add(videoItem);
                            break;
                        case 53: // 要论
                            ForumBranchModel articleItem = GetCollectArticle(item, userId);
                            result.Add(articleItem);
                            break;
                        case 54: // 论坛新建
                        case 55: // 论坛转发
                            ForumBranchModel forumItem = GetCollectForum(item, userId);
                            result.Add(forumItem);
                            break;
                        case 56: // 支部新建
                        case 57: // 支部转发
                        case 58: // 支部分享
                            ForumBranchModel branchItem = GetCollectBranch(item, userId);
                            result.Add(branchItem);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.forwarding_content))
                    {
                        item.forwarding_content = ReplaceStringHtml(item.forwarding_content);
                    }
                }
                return result;
            }

            return new List<ForumBranchModel>();
        }

        private ForumBranchModel GetCollectForum(ForumBranchModel item, string userId) {
            string querySql = String.Format(@"select P_ForumShare.P_Id as p_id, 
                        P_ForumShare.P_Content as content,
                        P_ForumShare.P_Source + 54 as content_source,
                        P_ForumShare.P_Type as content_type, 
                        P_Image.P_ImageUrl as user_avatar,dt_users.user_name as user_name,dt_users.id as user_id,
                        CONVERT(varchar(100), P_ForumShare.P_CreateTime, 23) as create_time,
                        (select COUNT(dt_article_comment.id) from dt_article_comment 
                        where dt_article_comment.relation_id = P_ForumShare.P_Id and dt_article_comment.type = 1) as comment_count,
                        (select count(P_ThumbUp.P_Id) from P_ThumbUp 
                        where P_ThumbUp.P_ArticleId = P_ForumShare.P_Id and P_ThumbUp.P_FamilyType = 2) as thumb_count
                        from P_ForumShare
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_ForumShare.P_UserId
                        left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_ImageType = {0}
                        where P_ForumShare.P_Id = '{1}'", (int)ImageTypeEnum.头像, item.p_id);

            DataSet ds = DbHelperSQL.Query(querySql);
            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                item = helper.FillToModel(ds.Tables[0].Rows[0]);

                GetCollectAndThumb(item, userId);

                switch (item.content_source)
                {
                    case 54: // 论坛新建
                        item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛发布);
                        switch (item.content_type)
                        {
                            case 1:
                                item.pictures = GetForumContentPics(item.p_id);
                                break;
                            case 2:
                                Model.P_Video videoData = GetForumVideo(item.p_id);
                                item.video_id = videoData.P_Id;
                                item.vodeo_name = videoData.P_VideoName;
                                item.video_pic = videoData.P_VideoPic;
                                item.video_url = videoData.P_Url;
                                item.video_lenght = videoData.P_VideoLength;
                                item.vodeo_play_count = GetFornmVideoPlayCount(videoData.P_Id);
                                item.course_id = "";
                                break;
                            default:
                                break;
                        }
                        break;
                    case 55: // 论坛转发
                        item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛转发);
                        string sql = String.Format(@"select P_Transmit.P_RelationId,P_Transmit.P_Type,P_Transmit.P_Content from P_Transmit
                                left join P_ForumShare on P_ForumShare.P_SourceId = P_Transmit.P_Id
                                and P_ForumShare.P_GroupForumId = P_Transmit.P_OrganizeId
                                where P_ForumShare.P_Id = '{0}'", item.p_id);

                        DataSet forwardingData = DbHelperSQL.Query(sql);
                        if (forwardingData.Tables[0] != null && forwardingData.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = forwardingData.Tables[0].Rows[0];
                            item.forwarding_id = dr["P_RelationId"].ToString();
                            item.content_type = Convert.ToInt32(dr["P_Type"].ToString());
                            item.content = dr["P_Content"].ToString();
                        }

                        switch (item.content_type)
                        {
                            case 52:
                                Model.P_Video data = GetCourseVideo(item.forwarding_id);
                                item.video_id = data.P_Id;
                                item.vodeo_name = data.P_VideoName;
                                item.video_pic = data.P_VideoPic;
                                item.video_url = data.P_Url;
                                item.video_lenght = data.P_VideoLength;
                                item.course_id = data.P_ParentId;
                                item.vodeo_play_count = GetFornmVideoPlayCount(data.P_Id);
                                break;
                            case 53:
                            case 56:
                                ForwardingArticleModel articleData = GetForwardingArticle(item.forwarding_id);
                                item.forwarding_id = articleData.forwarding_id;
                                item.forwarding_title = articleData.forwarding_title;
                                item.forwarding_content = articleData.forwarding_content;
                                item.forwarding_pic = articleData.forwarding_pic;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            return item;
        }

        private ForumBranchModel GetCollectBranch(ForumBranchModel item, string userId) {

            string querySql = String.Format(@"select P_BranchPublish.P_Id as p_id,
                        P_BranchPublish.P_Content as content,
                        P_BranchPublish.P_Source +56 as content_source,
                        P_BranchPublish.P_Type as content_type,P_Image.P_ImageUrl as user_avatar,dt_users.user_name as user_name,dt_users.id as user_id,
                        CONVERT(varchar(100), P_BranchPublish.P_CreateTime, 23) as create_time,
                        (select COUNT(dt_article_comment.id) from dt_article_comment 
                        where dt_article_comment.relation_id = P_BranchPublish.P_Id and dt_article_comment.type = 2) as comment_count,
                        (select count(P_ThumbUp.P_Id) from P_ThumbUp 
                        where P_ThumbUp.P_ArticleId = P_BranchPublish.P_Id and P_ThumbUp.P_FamilyType = 3) as thumb_count
                        from P_BranchPublish
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_BranchPublish.P_UserId
                        left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_ImageType = {1}
                        where P_BranchPublish.P_Id = '{0}'", item.p_id, (int)ImageTypeEnum.头像);

            DataSet ds = DbHelperSQL.Query(querySql);
            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                item = helper.FillToModel(ds.Tables[0].Rows[0]);

                GetBranchCollectAndThumb(item, userId);

                switch (item.content_source)
                {
                    case 56:
                        GetBranchData(item);
                        break;
                    case 57:
                        GetBranchForwarding(item);
                        break;
                    default:
                        GetBranchShare(item);
                        break;
                }
            }
            return item;
        }

        public ForumBranchModel GetCollectVideo(ForumBranchModel item, string userId) {

            string sql = String.Format(@"select dt_users.id as user_id, dt_users.user_name as user_name,
                        CONVERT(varchar(100), P_Collect.P_CreateTime, 23) as create_time,
                        P_Image.P_ImageUrl as user_avatar
                         from P_Collect
                        left join dt_users on convert(nvarchar,dt_users.id) = P_Collect.P_UserId
                        left join P_Image on P_ImageId = P_Collect.P_UserId and P_ImageType = {0}
                        where P_Collect.P_Relation = '{1}' 
                        and P_Collect.P_UserId = {2} and P_Collect.P_Type = 52", (int)ImageTypeEnum.头像, item.p_id, userId);

            DataSet ds = DbHelperSQL.Query(sql);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                DataRow dr = ds.Tables[0].Rows[0];
                item.user_id = int.Parse(userId);
                item.user_name = dr["user_name"].ToString();
                item.user_avatar = dr["user_avatar"].ToString();
                item.create_time = dr["create_time"].ToString();
            }

            item.user_group_name = GetUserGroupName(int.Parse(userId));

            Model.P_Video data = GetCourseVideo(item.p_id);
            item.video_id = data.P_Id;
            item.vodeo_name = data.P_VideoName;
            item.video_pic = data.P_VideoPic;
            item.video_url = data.P_Url;
            item.video_lenght = data.P_VideoLength;
            item.course_id = data.P_ParentId;
            item.vodeo_play_count = GetFornmVideoPlayCount(data.P_Id);
            return item;
        }

        public ForumBranchModel GetCollectArticle(ForumBranchModel item, string userId) {

            string sql = String.Format(@"select dt_users.id as user_id, dt_users.user_name as user_name,
                        CONVERT(varchar(100), P_Collect.P_CreateTime, 23) as create_time,
                        P_Image.P_ImageUrl as user_avatar
                         from P_Collect
                        left join dt_users on convert(nvarchar,dt_users.id) = P_Collect.P_UserId
                        left join P_Image on P_ImageId = P_Collect.P_UserId and P_ImageType = {0}
                        where P_Collect.P_Relation = '{1}' 
                        and P_Collect.P_UserId = {2} and P_Collect.P_Type = 53", (int)ImageTypeEnum.头像, item.p_id, userId);

            DataSet ds = DbHelperSQL.Query(sql);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                item.user_id = int.Parse(userId);
                item.user_name = dr["user_name"].ToString();
                item.user_avatar = dr["user_avatar"].ToString();
                item.create_time = dr["create_time"].ToString();
            }

            item.user_group_name = GetUserGroupName(int.Parse(userId));

            ForwardingArticleModel articleData = GetForwardingArticle(item.p_id);
            item.forwarding_id = articleData.forwarding_id;
            item.forwarding_title = articleData.forwarding_title;
            item.forwarding_content = articleData.forwarding_content;
            item.forwarding_pic = articleData.forwarding_pic;
            return item;
        }
        #endregion

        #region ================ 去除字符串中的html标签
        private string ReplaceStringHtml(string dataStr) {
            //return Utils.DropHTML(dataStr);
            string[] reg = { @"[<].*?[>]", @"\r", @"\n", @"\t", @"&nbsp;", @"&emsp;", @"&emsp", @"&nbsp" };
            string result = dataStr;
            for (int i = 0; i < reg.Length; i++)
            {
                Regex regex = new Regex(reg[i], RegexOptions.IgnoreCase);
                result = Regex.Replace(result, reg[i], "");
            }
            result = result.Replace("&lt;", "<");
            result = result.Replace("&gt;",">");
            result = result.Replace("&amp;","&");
            return result;
        }

        #endregion

        /// <summary>
        /// 获取去论坛或支部信息中@的人员名称
        /// </summary>
        /// <param name="infoId">信息ID</param>
        /// <param name="type">区分论坛新建、论坛转发、支部新建、支部转发</param>
        /// <returns></returns>
        private List<AtPersonnel> GetInfoAtPersonnel(string infoId, int type) {

            string sql = String.Format(@"select dt_users.user_name from P_AtPerson
                        left join dt_users on convert(nvarchar,dt_users.id) = P_AtPerson.P_UserId
                        where P_AtPerson.P_Relation = '{0}' and P_AtPerson.P_Type = {1}
                        and dt_users.id is not null", infoId, type);
            DataSet ds = DbHelperSQL.Query(sql);
            return new DataSetToModelHelper<AtPersonnel>().FillModel(ds);

        }

        public List<ForumBranchModel> GetAtMyInfoList(string userId, int page, int rows) {

            string sql = String.Format(@"select P_AtPerson.P_Relation as p_id,
                        CONVERT(int,P_AtPerson.P_CreateUser) as user_id,
                        dt_users.user_name,
                        P_Image.P_ImageUrl as user_avatar,
                        CONVERT(int,P_AtPerson.P_Type) as content_source,
                        CONVERT(varchar(100), P_AtPerson.P_CreateTime, 23) as create_time
                        from P_AtPerson
                        left join dt_users on convert(nvarchar,dt_users.id) = P_AtPerson.P_CreateUser
                        left join P_Image on P_Image.P_ImageId = P_AtPerson.P_CreateUser and P_Image.P_ImageType = {0}
                        where dt_users.id is not null and P_AtPerson.P_UserId = '{1}' and P_AtPerson.P_Status = 0", (int)ImageTypeEnum.头像, userId);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_AtPerson.P_CreateTime desc"));
            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();
            List<ForumBranchModel> result = helper.FillModel(ds);

            if (result != null && result.Count > 0) {

                foreach (ForumBranchModel item in result)
                {
                    switch (item.content_source)
                    {
                        case 0:
                            // 获取@我中信息类型为论坛中发布的信息数据详情
                            GetAtMyForumNew(item, userId);
                            break;
                        case 1:
                            // 获取@我中信息类型为论坛中转发的信息数据详情
                            GetAtMyForumTransmit(item, userId);
                            break;
                        case 2:
                            // 获取@我中信息类型为支部中发布的信息数据详情
                            GetAtMyBranchNew(item, userId);
                            break;
                        case 3:
                            // 获取@我中信息类型为支部中转发的信息数据详情
                            GetAtMyBranchTransmit(item,userId);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.forwarding_content))
                    {
                        item.forwarding_content = ReplaceStringHtml(item.forwarding_content);
                    }
                }
            }
            return result;
        }

        private void GetAtMyForumNew(ForumBranchModel data, string userId) {
            // 获取评论数量
            data.comment_count = GetCommentCount(data.p_id, 1);
            // 获取点赞数量
            data.thumb_count = GetThumbCount(data.p_id, 2);
            // 获取用户是否点赞
            GetCollectAndThumb(data, userId);

            string querlSql = String.Format(@"select * from P_ForumShare
                        where P_ForumShare.P_Id = '{0}'", data.p_id);
            DataSet ds = DbHelperSQL.Query(querlSql);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                data.content = Convert.ToString(ds.Tables[0].Rows[0]["P_Content"].ToString());
                data.content_type = Convert.ToInt32(ds.Tables[0].Rows[0]["P_Type"].ToString());
            }

            data.at_personnel = GetInfoAtPersonnel(data.p_id, (int)AtTypeEnum.论坛发布);
            switch (data.content_type)
            {
                case 1:
                    data.pictures = GetForumContentPics(data.p_id);
                    break;
                case 2:
                    Model.P_Video videoData = GetForumVideo(data.p_id);
                    data.video_id = videoData.P_Id;
                    data.vodeo_name = videoData.P_VideoName;
                    data.video_pic = videoData.P_VideoPic;
                    data.video_url = videoData.P_Url;
                    data.video_lenght = videoData.P_VideoLength;
                    data.vodeo_play_count = GetFornmVideoPlayCount(videoData.P_Id);
                    data.course_id = "";
                    break;
                default:
                    break;
            }
        }

        private void GetAtMyForumTransmit(ForumBranchModel data, string userId) {
            // 获取评论数量
            data.comment_count = GetCommentCount(data.p_id, 1);
            // 获取点赞数量
            data.thumb_count = GetThumbCount(data.p_id, 2);
            // 获取用户是否点赞
            GetCollectAndThumb(data, userId);
            string querlSql = String.Format(@"select * from P_ForumShare
                        where P_ForumShare.P_Id = '{0}'", data.p_id);
            DataSet ds = DbHelperSQL.Query(querlSql);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                data.content = Convert.ToString(ds.Tables[0].Rows[0]["P_Content"].ToString());
            }
            data.at_personnel = GetInfoAtPersonnel(data.p_id, (int)AtTypeEnum.论坛转发);

            string sql = String.Format(@"select P_Transmit.P_RelationId,P_Transmit.P_Type,P_Transmit.P_Content from P_Transmit
                        left join P_ForumShare on P_ForumShare.P_SourceId = P_Transmit.P_Id
                        where P_ForumShare.P_Id = '{0}'", data.p_id);

            DataSet forwardingData = DbHelperSQL.Query(sql);
            if (forwardingData.Tables[0] != null && forwardingData.Tables[0].Rows.Count > 0)
            {
                DataRow dr = forwardingData.Tables[0].Rows[0];
                data.forwarding_id = dr["P_RelationId"].ToString();
                data.content_type = Convert.ToInt32(dr["P_Type"].ToString());
                data.content = dr["P_Content"].ToString();
            }

            switch (data.content_type)
            {
                case 52:
                    Model.P_Video video = GetCourseVideo(data.forwarding_id);
                    data.video_id = video.P_Id;
                    data.vodeo_name = video.P_VideoName;
                    data.video_pic = video.P_VideoPic;
                    data.video_url = video.P_Url;
                    data.video_lenght = video.P_VideoLength;
                    data.course_id = video.P_ParentId;
                    data.vodeo_play_count = GetFornmVideoPlayCount(video.P_Id);
                    break;
                case 53:
                case 56:
                    ForwardingArticleModel articleData = GetForwardingArticle(data.forwarding_id);
                    data.forwarding_id = articleData.forwarding_id;
                    data.forwarding_title = articleData.forwarding_title;
                    data.forwarding_content = articleData.forwarding_content;
                    data.forwarding_pic = articleData.forwarding_pic;
                    break;
                default:
                    break;
            }
        }

        private void GetAtMyBranchNew(ForumBranchModel data, string userid) {
            // 获取评论数量
            data.comment_count = GetCommentCount(data.p_id, 2);
            // 获取点赞数量
            data.thumb_count = GetThumbCount(data.p_id, 3);
            // 获取用户是否点赞
            GetBranchCollectAndThumb(data, userid);

            string querySql = String.Format(@"select * from P_BranchPublish
                        where P_BranchPublish.P_Id = '{0}'", data.p_id);
            
            DataSet ds = DbHelperSQL.Query(querySql);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                data.content = Convert.ToString(ds.Tables[0].Rows[0]["P_Content"].ToString());
                data.content_type = Convert.ToInt32(ds.Tables[0].Rows[0]["P_Type"].ToString());
            }
            GetBranchData(data);
        }

        private void GetAtMyBranchTransmit(ForumBranchModel data, string userid) {
            // 获取评论数量
            data.comment_count = GetCommentCount(data.p_id, 2);
            // 获取点赞数量
            data.thumb_count = GetThumbCount(data.p_id, 3);
            // 获取用户是否点赞
            GetBranchCollectAndThumb(data, userid);
            string querySql = String.Format(@"select * from P_BranchPublish
                        where P_BranchPublish.P_Id = '{0}'", data.p_id);

            DataSet ds = DbHelperSQL.Query(querySql);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                data.content = Convert.ToString(ds.Tables[0].Rows[0]["P_Content"].ToString());
            }
            GetBranchForwarding(data);
        }

        private int GetCommentCount(string p_id, int type) {

            string sql = String.Format(@"select COUNT(dt_article_comment.id) from dt_article_comment 
                        where dt_article_comment.relation_id = '{0}' and dt_article_comment.type = {1}", p_id, type);
            return Convert.ToInt32(DbHelperSQL.GetSingle(sql));
        }

        private int GetThumbCount(string p_id, int type) {

            string sql = String.Format(@"select count(P_ThumbUp.P_Id) from P_ThumbUp 
                        where P_ThumbUp.P_ArticleId = '{0}' and P_ThumbUp.P_FamilyType = {1}", p_id, type);
            return Convert.ToInt32(DbHelperSQL.GetSingle(sql));
        }

        /// <summary>
        /// 获取个人中心评论列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<ForumBranchModel> GetCommentsList(string userId, int page, int rows) {

            string querySql = String.Format(@"select * from (
                        select 
                        P_BranchPublish.P_Id as p_id,
                        P_BranchPublish.P_Content as content,
                        P_BranchPublish.P_Source + 2 as content_source,
                        P_BranchPublish.P_Type as content_type,
                        P_Image.P_ImageUrl as user_avatar,
                        dt_users.user_name as user_name,dt_users.id as user_id,
                        CONVERT(varchar(100), P_BranchPublish.P_CreateTime, 23) as create_time,
                        dt_article_comment.content as comment_content,
                        dt_article_comment.add_time
                        from dt_article_comment
                        left join P_BranchPublish on P_BranchPublish.P_Id = dt_article_comment.relation_id
                        and dt_article_comment.type = 2
                        left join P_Image on P_Image.P_ImageId = convert(nvarchar,dt_article_comment.user_id)
                        and P_Image.P_ImageType = {0}
                        left join dt_users on dt_users.id = dt_article_comment.user_id
                        where P_BranchPublish.P_UserId = {1}
                        UNION ALL
                        select 
                        P_ForumShare.P_Id as p_id, 
                        P_ForumShare.P_Content as content,
                        P_ForumShare.P_Source as content_source,
                        P_ForumShare.P_Type as content_type, 
                        P_Image.P_ImageUrl as user_avatar,
                        dt_users.user_name as user_name,
                        dt_users.id as user_id,
                        CONVERT(varchar(100), P_ForumShare.P_CreateTime, 23) as create_time,
                        dt_article_comment.content as comment_content,
                        dt_article_comment.add_time
                        from dt_article_comment
                        left join P_ForumShare on P_ForumShare.P_Id = dt_article_comment.relation_id
                        and dt_article_comment.type = 1
                        left join P_Image on P_Image.P_ImageId = convert(nvarchar,dt_article_comment.user_id)
                        and P_Image.P_ImageType = {0}
                        left join dt_users on dt_users.id = dt_article_comment.user_id
                        where P_ForumShare.P_UserId = {1}
                        ) as table1", (int)ImageTypeEnum.头像, userId);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, querySql, " add_time desc"));

            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {

                List<ForumBranchModel> result = helper.FillModel(ds);

                foreach (ForumBranchModel item in result)
                {
                    switch (item.content_source)
                    {
                        case 0: // 论坛新建
                            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛发布);
                            GetCollectAndThumb(item, userId);
                            switch (item.content_type)
                            {
                                case 1:
                                    item.pictures = GetForumContentPics(item.p_id);
                                    break;
                                case 2:
                                    Model.P_Video videoData = GetForumVideo(item.p_id);
                                    item.video_id = videoData.P_Id;
                                    item.vodeo_name = videoData.P_VideoName;
                                    item.video_pic = videoData.P_VideoPic;
                                    item.video_url = videoData.P_Url;
                                    item.video_lenght = videoData.P_VideoLength;
                                    item.vodeo_play_count = GetFornmVideoPlayCount(videoData.P_Id);
                                    item.course_id = "";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 1: // 论坛转发
                            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛转发);
                            GetCollectAndThumb(item, userId);
                            string sql = String.Format(@"select P_Transmit.P_RelationId,P_Transmit.P_Type,P_Transmit.P_Content from P_Transmit
                                left join P_ForumShare on P_ForumShare.P_SourceId = P_Transmit.P_Id
                                and P_ForumShare.P_GroupForumId = P_Transmit.P_OrganizeId
                                where P_ForumShare.P_Id = '{0}'", item.p_id);

                            DataSet forwardingData = DbHelperSQL.Query(sql);
                            if (forwardingData.Tables[0] != null && forwardingData.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = forwardingData.Tables[0].Rows[0];
                                item.forwarding_id = dr["P_RelationId"].ToString();
                                item.content_type = Convert.ToInt32(dr["P_Type"].ToString());
                                item.content = dr["P_Content"].ToString();
                            }

                            switch (item.content_type)
                            {
                                case 52:
                                    Model.P_Video data = GetCourseVideo(item.forwarding_id);
                                    item.video_id = data.P_Id;
                                    item.vodeo_name = data.P_VideoName;
                                    item.video_pic = data.P_VideoPic;
                                    item.video_url = data.P_Url;
                                    item.video_lenght = data.P_VideoLength;
                                    item.course_id = data.P_ParentId;
                                    item.vodeo_play_count = GetFornmVideoPlayCount(data.P_Id);
                                    break;
                                case 53:
                                case 56:
                                    ForwardingArticleModel articleData = GetForwardingArticle(item.forwarding_id);
                                    item.forwarding_id = articleData.forwarding_id;
                                    item.forwarding_title = articleData.forwarding_title;
                                    item.forwarding_content = articleData.forwarding_content;
                                    item.forwarding_pic = articleData.forwarding_pic;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 2: // 支部新建
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchData(item);
                            break;
                        case 3: // 支部转发
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchForwarding(item);
                            break;
                        case 4: // 支部分享
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchShare(item);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.forwarding_content))
                    {
                        item.forwarding_content = ReplaceStringHtml(item.forwarding_content);
                    }
                }
                return result;
            }

            return null;

        }

        /// <summary>
        /// 获取个人中心点赞列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<ForumBranchModel> GetThumbUpList(string userId, int page, int rows) {

            string querySql = String.Format(@"select * from (
                        select 
                        P_BranchPublish.P_Id as p_id,
                        P_BranchPublish.P_Content as content,
                        P_BranchPublish.P_Source + 2 as content_source,
                        P_BranchPublish.P_Type as content_type,
                        P_Image.P_ImageUrl as user_avatar,
                        dt_users.user_name as user_name,dt_users.id as user_id,
                        CONVERT(varchar(100), P_BranchPublish.P_CreateTime, 23) as create_time,
                        P_ThumbUp.P_CreateTime
                        from P_ThumbUp
                        left join P_BranchPublish on P_BranchPublish.P_Id = P_ThumbUp.P_ArticleId
                        and P_ThumbUp.P_FamilyType = 3
                        left join P_Image on P_Image.P_ImageId = convert(nvarchar,P_ThumbUp.P_UserId) 
                        and P_Image.P_ImageType = {0}
                        left join dt_users on convert(nvarchar,dt_users.id) = P_ThumbUp.P_UserId
                        where P_BranchPublish.P_UserId = {1}
                        UNION ALL
                        select 
                        P_ForumShare.P_Id as p_id, 
                        P_ForumShare.P_Content as content,
                        P_ForumShare.P_Source as content_source,
                        P_ForumShare.P_Type as content_type, 
                        P_Image.P_ImageUrl as user_avatar,
                        dt_users.user_name as user_name,
                        dt_users.id as user_id,
                        CONVERT(varchar(100), P_ForumShare.P_CreateTime, 23) as create_time,
                        P_ThumbUp.P_CreateTime
                        from P_ThumbUp
                        left join P_ForumShare on P_ForumShare.P_Id = P_ThumbUp.P_ArticleId
                        and P_ThumbUp.P_FamilyType = 2
                        left join P_Image on P_Image.P_ImageId = P_ThumbUp.P_UserId
                        and P_Image.P_ImageType = {0}
                        left join dt_users on convert(nvarchar,dt_users.id) = P_ThumbUp.P_UserId
                        where P_ForumShare.P_UserId = {1}
                        ) as b", (int)ImageTypeEnum.头像, userId);
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, querySql, " P_CreateTime desc"));

            DataSetToModelHelper<ForumBranchModel> helper = new DataSetToModelHelper<ForumBranchModel>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {

                List<ForumBranchModel> result = helper.FillModel(ds);

                foreach (ForumBranchModel item in result)
                {
                    switch (item.content_source)
                    {
                        case 0: // 论坛新建
                            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛发布);
                            GetCollectAndThumb(item, userId);
                            switch (item.content_type)
                            {
                                case 1:
                                    item.pictures = GetForumContentPics(item.p_id);
                                    break;
                                case 2:
                                    Model.P_Video videoData = GetForumVideo(item.p_id);
                                    item.video_id = videoData.P_Id;
                                    item.vodeo_name = videoData.P_VideoName;
                                    item.video_pic = videoData.P_VideoPic;
                                    item.video_url = videoData.P_Url;
                                    item.video_lenght = videoData.P_VideoLength;
                                    item.vodeo_play_count = GetFornmVideoPlayCount(videoData.P_Id);
                                    item.course_id = "";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 1: // 论坛转发
                            item.at_personnel = GetInfoAtPersonnel(item.p_id, (int)AtTypeEnum.论坛转发);
                            GetCollectAndThumb(item, userId);
                            string sql = String.Format(@"select P_Transmit.P_RelationId,P_Transmit.P_Type,P_Transmit.P_Content from P_Transmit
                                left join P_ForumShare on P_ForumShare.P_SourceId = P_Transmit.P_Id
                                and P_ForumShare.P_GroupForumId = P_Transmit.P_OrganizeId
                                where P_ForumShare.P_Id = '{0}'", item.p_id);

                            DataSet forwardingData = DbHelperSQL.Query(sql);
                            if (forwardingData.Tables[0] != null && forwardingData.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = forwardingData.Tables[0].Rows[0];
                                item.forwarding_id = dr["P_RelationId"].ToString();
                                item.content_type = Convert.ToInt32(dr["P_Type"].ToString());
                                item.content = dr["P_Content"].ToString();
                            }

                            switch (item.content_type)
                            {
                                case 52:
                                    Model.P_Video data = GetCourseVideo(item.forwarding_id);
                                    item.video_id = data.P_Id;
                                    item.vodeo_name = data.P_VideoName;
                                    item.video_pic = data.P_VideoPic;
                                    item.video_url = data.P_Url;
                                    item.video_lenght = data.P_VideoLength;
                                    item.course_id = data.P_ParentId;
                                    item.vodeo_play_count = GetFornmVideoPlayCount(data.P_Id);
                                    break;
                                case 53:
                                case 56:
                                    ForwardingArticleModel articleData = GetForwardingArticle(item.forwarding_id);
                                    item.forwarding_id = articleData.forwarding_id;
                                    item.forwarding_title = articleData.forwarding_title;
                                    item.forwarding_content = articleData.forwarding_content;
                                    item.forwarding_pic = articleData.forwarding_pic;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 2: // 支部新建
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchData(item);
                            break;
                        case 3: // 支部转发
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchForwarding(item);
                            break;
                        case 4: // 支部分享
                            GetBranchCollectAndThumb(item, userId);
                            GetBranchShare(item);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.forwarding_content))
                    {
                        item.forwarding_content = ReplaceStringHtml(item.forwarding_content);
                    }
                }
                return result;
            }
            return null;
        }

    }
}

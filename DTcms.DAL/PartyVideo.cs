using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{
    public class PartyVideo
    {
        /// <summary>
        /// 获取课程的接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<videoModel> GetPartyNewsList(int userid, int rows, int page)
        {
            List<videoModel> list = new List<videoModel>();
            StringBuilder strsql = new StringBuilder();
            //strsql.Append("select  dt_article.id as id ,dt_article.title as title,P_Image.P_ImageUrl as imgurl,");
            //strsql.Append("(SELECT count(P_Video.p_id) from P_Video where  P_Video.P_ParentId= convert(VARCHAR,dt_article.id) and P_Video.P_Status=0 ) as hour,");
            //strsql.Append("COUNT(P_VideoRecord.P_Id) as leaningcount,count(P_Collect.P_Id) as collect");
            //strsql.Append("count(P_Collect.P_Id) as collect");
            //strsql.Append(" from dt_article");
            //strsql.Append(" LEFT JOIN P_Image on P_ImageId=CAST(dt_article.id as nvarchar) ");
            //strsql.Append(" LEFT JOIN P_Video on P_Video.P_ParentId= convert(VARCHAR,dt_article.id) and P_Video.P_Status=0 ");
            //strsql.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_QuestionBankId=dt_article.id and P_VideoRecord.P_VideoId=P_Video.P_Id  and P_VideoRecord.P_LearnStatus=1  ");
            //strsql.Append(" LEFT JOIN P_Collect on p_collect.p_relation = P_Video.P_Id and p_collect.p_userid =" + userid + @" and p_collect.P_Type='52' ");
            //strsql.Append(" where dt_article.category_id=52 and dt_article.status=0 ");
            //strsql.Append(" GROUP BY dt_article.id ,dt_article.title ,P_Image.P_ImageUrl,dt_article.sort_id ");
            strsql.Append(" select bb.id,bb.title,bb.imgurl,SUM(bb.hour) as hour,sum(bb.collect) as collect from  ");
            strsql.Append(" ( ");
            strsql.Append(" select  dt_article.id as id ,dt_article.title as title,P_Image.P_ImageUrl as imgurl,dt_article.sort_id, ");
            strsql.Append(" count(P_Video.p_id) as hour, ");
            strsql.Append(" (select COUNT(aa.collectid) from  ");
            strsql.Append(" ( select COUNT(P_Collect.P_Id) as collectid from P_Collect where p_collect.p_relation = P_Video.P_Id and p_collect.p_userid ="+userid+@" and p_collect.P_Type='52'");
            strsql.Append(" GROUP BY P_Collect.P_UserId ");
            strsql.Append(" )aa) as collect ");
            strsql.Append(" from dt_article  ");
            strsql.Append(" LEFT JOIN P_Image on P_ImageId=CAST(dt_article.id as nvarchar)   ");
            strsql.Append(" LEFT JOIN P_Video on P_Video.P_ParentId= convert(VARCHAR,dt_article.id) and P_Video.P_Status=0   ");
            strsql.Append(" where dt_article.category_id=52 and dt_article.status=0 ");
            strsql.Append(" GROUP BY dt_article.id ,dt_article.title,P_Image.P_ImageUrl,dt_article.sort_id,P_Video.P_Id)bb ");
            strsql.Append(" LEFT JOIN dt_article on dt_article.id=bb.id ");
            strsql.Append(" GROUP BY bb.id,bb.title,bb.imgurl,dt_article.sort_id,add_time ");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "dt_article.add_time desc"));
            DataSetToModelHelper<videoModel> model = new DataSetToModelHelper<videoModel>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                list = model.FillModel(dt);
                foreach (var item in list)
                {
                    if (item.collect == item.hour && item.hour != 0 )
                    {
                        item.collect = 1;
                    }
                    else
                    {
                        item.collect = 0;
                    }
                }
            }
            else
            {
                list = null;
            }

            return list;
        }
        /// <summary>
        /// 点击课程获取视频的接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public CorceDetialModel GetVideoList(int userid, int id, int rows, int page)
        {

            CorceDetialModel result = new CorceDetialModel();

            int learningTime = 0;
            int totalTime = 0;
            // 获取课程总时长和已学习时长。(在获取课程总时长时，会出现无法获取视频时长的情况)
            GetCorceTime(userid, id, out learningTime, out totalTime);

            result.learning_time = learningTime;
            result.total_time = totalTime;
            // 判断获取总时长是否成功
            if (totalTime != -1)
            {   // 获取视频列表
                List<VideoDetialModel> list = new List<VideoDetialModel>();
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select P_Video.p_id as id,P_Video.p_videoname as name,P_Video.p_url as url,");
                strsql.Append("  P_Video.P_VideoPic as videopic,P_VideoRecord.P_LastPlaybackTime as lastplaytime,P_VideoRecord.P_MaxPlaybackTime as maxplaytime,P_Video.P_VideoLength as videolength,");
                strsql.Append("(select COUNT(P_VideoRecord.P_Id) from P_VideoRecord where P_VideoRecord.P_VideoId=P_Video.P_Id  ) as playcount,");
                strsql.Append(" (select COUNT(aa.rela) from (select DISTINCT P_Collect.P_userid as uuid,P_Collect.P_Relation as rela from P_Collect ");
                strsql.Append(" where p_collect.p_relation =P_Video.p_id and p_collect.P_Type='52' )aa) as collectcount, ");
                strsql.Append("  COUNT(P_Transmit.P_id) as trankcount,");
                strsql.Append(" CASE WHEN (select count(p_collect.P_id) from p_collect where p_collect.p_relation =P_Video.p_id and p_collect.P_Type='52' and p_collect.p_userid =" + userid + @")!=0 THEN 1 ELSE 0 END as collect  ");
                strsql.Append(" from P_Video  ");
                strsql.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id and P_VideoRecord.P_UserId=" + userid + @" ");
                //strsql.Append(" LEFT JOIN p_collect on p_collect.p_relation =P_Video.p_id and p_collect.P_Type='52' ");
                strsql.Append(" LEFT JOIN P_Transmit on P_Transmit.p_relationid=P_Video.p_id and P_Transmit.P_Type='52' ");
                strsql.Append(" where P_Video.P_Status = 0 and P_Video.p_parentid='" + id + @"' ");
                strsql.Append("  GROUP BY P_Video.p_id,P_Video.p_videoname ,P_Video.p_url,P_Video.P_Number,P_Video.P_VideoPic,P_VideoRecord.P_LastPlaybackTime,P_VideoRecord.P_MaxPlaybackTime,P_Video.P_VideoLength");
                DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_Video.P_Number"));
                DataSetToModelHelper<VideoDetialModel> model = new DataSetToModelHelper<VideoDetialModel>();
                if (dt.Tables[0].Rows.Count != 0)
                {
                    List<VideoDetialModel> videoData = model.FillModel(dt);
                    foreach (var item in videoData)
                    {
                        if (item.videolength == 0) {
                            item.videolength = (int)new QiNiuHelper().GetVideoLength(item.url);
                        }
                    }
                    result.videos = videoData;
                }
                else
                {
                    result.videos = null;
                }
            }
            else {  // 获取总时长失败，接口返回获取信息失败
                return null;
            }

           
            return result;
        }

        /// <summary>
        /// 获取课程总时长和已学习时长（获取总时长时，会出现失败情况）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="videoParentId"></param>
        /// <param name="learningTime"></param>
        /// <param name="totalTime"></param>
        public void GetCorceTime(int userId, int videoParentId, out int learningTime, out int totalTime)
        {

            string playTimeSql = String.Format(@"select sum(P_MaxPlaybackTime) as playtime from P_VideoRecord
                        left join P_Video on P_Video.P_Id = P_VideoRecord.P_VideoId
                        where P_Video.P_ParentId = '{0}' and P_VideoRecord.P_UserId = '{1}'", videoParentId, userId);
            // 已学习视频时长
            double playTime = Convert.ToInt32(DbHelperSQL.GetSingle(playTimeSql.ToString()));

            string totalTimeSql = String.Format(@"select sum(P_Video.P_VideoLength) as totaltime from P_Video
                        where P_Video.P_ParentId = '{0}'", videoParentId);
            // 视频总时长
            double total = Convert.ToInt32(DbHelperSQL.GetSingle(totalTimeSql.ToString()));

            string totalSql = String.Format(@"select P_Url from P_Video
                        where P_ParentId = '{0}' and P_VideoLength = 0", videoParentId);
            
            // 需要重新获取时长的视频列表
            DataSet totalDs = DbHelperSQL.Query(totalSql);

            if (totalDs.Tables != null && totalDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in totalDs.Tables[0].Rows)
                {
                    if (new QiNiuHelper().GetVideoLength(item["P_Url"].ToString()) != -1)
                    {
                        total += new QiNiuHelper().GetVideoLength(item["P_Url"].ToString());
                    }
                    else {
                        total = -1;
                        break;
                    }
                }
            }

            learningTime = (int)playTime;
            totalTime = (int)total;
        }

        /// <summary>
        /// 收藏课程接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetCourceCollect(int userid, int id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strsql = new StringBuilder();
                        strsql.Append("select p_id as id from P_Video where P_Video.P_ParentId='" + id + @"'");
                        DataSet ds = DbHelperSQL.Query(strsql.ToString());
                        DataSetToModelHelper<VideoId> model = new DataSetToModelHelper<VideoId>();
                        List<VideoId> videomodel = model.FillModel(ds);

                        if (videomodel != null && videomodel.Count != 0)
                        {
                            foreach (var item in videomodel)
                            {
                                StringBuilder str = new StringBuilder();
                                str.Append("insert into p_collect(");
                                str.Append("P_Id,P_userid,p_relation,p_type,p_createtime,p_createuser,p_status)");
                                str.Append(" values (");
                                str.Append("@P_Id,@P_userid,@p_relation,@p_type,@p_createtime,@p_createuser,@p_status)");
                                str.Append(";select @@IDENTITY");
                                SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_userid", SqlDbType.Int,4),
                                new SqlParameter("@p_relation", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_type", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_createtime", SqlDbType.DateTime,100),
                                new SqlParameter("@p_createuser", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_status", SqlDbType.Int,4)
                               };

                                parameters[0].Value = Guid.NewGuid().ToString("N");
                                parameters[1].Value = userid;
                                parameters[2].Value = item.id;
                                parameters[3].Value = 52;
                                parameters[4].Value = DateTime.Now;
                                parameters[5].Value = userid;
                                parameters[6].Value = 0;
                                object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameters); //带事务

                            }
                        }
                        else
                        {
                            return 2;
                        }
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return 0;
                    }


                }
            }
            return 1;

        }
        /// <summary>
        /// 删除课程收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DelectCource(int userid, int id)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select p_id as id from P_Video where P_Video.P_ParentId='" + id + @"'");
            DataSet ds = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<VideoId> model = new DataSetToModelHelper<VideoId>();
            List<VideoId> videomodel = model.FillModel(ds);
            foreach (var item in videomodel)
            {
                using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("DELETE from P_Collect ");
                            str.Append("where P_Collect.P_CreateUser=" + userid + @" and P_Collect.P_Relation='" + item.id + @"' and P_Collect.P_Type='52' ");

                            object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString()); //带事务
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 视频观看接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public int GetPlayCount(int userid, int id, string videoid)
        {
            int a = 0;
            //using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            //{
            //    conn.Open();
            //    using (SqlTransaction trans = conn.BeginTransaction())
            //    {
            //        try
            //        {
            //            if (Exists(userid, id, videoid))
            //            {
            //                StringBuilder strSql = new StringBuilder();
            //                strSql.Append("select P_PlayTime,P_LearnStatus from P_VideoRecord ");
            //                strSql.Append(" where P_UserId=" + userid + @" and P_VideoId='" + videoid + @"' and P_QuestionBankId=" + id + @"");
            //                DataSet dt = DbHelperSQL.Query(strSql.ToString());
            //                DataSetToModelHelper<P_VideoRecord> model = new DataSetToModelHelper<P_VideoRecord>();
            //                P_VideoRecord aa = model.FillToModel(dt.Tables[0].Rows[0]);
            //                if (aa.P_LearnStatus == 0)
            //                {
            //                    a = Convert.ToInt32(aa.P_PlayTime);
            //                }
            //                else
            //                {
            //                    StringBuilder str = new StringBuilder();
            //                    str.Append("update  P_VideoRecord set P_PlayTime=0 ");
            //                    str.Append(" where P_UserId=" + userid + @" and P_VideoId='" + videoid + @"' and P_QuestionBankId=" + id + @" ");
            //                    object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString()); //带事务
            //                    a = 0;
            //                }
            //            }
            //            else
            //            {
            //                StringBuilder strSql = new StringBuilder();
            //                strSql.Append("insert into  P_VideoRecord(");
            //                strSql.Append("P_Id,P_UserId,P_VideoId,P_QuestionBankId,P_LearnStatus,P_PlayTime,P_CreateTime,P_CreateUser,P_Status) ");
            //                strSql.Append(" values(@P_Id,@P_UserId,@P_VideoId,@P_QuestionBankId,@P_LearnStatus,@P_PlayTime,@P_CreateTime,@P_CreateUser,@P_Status)");
            //                strSql.Append(";select @@IDENTITY");
            //                SqlParameter[] parameters = {
            //                    new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
            //                    new SqlParameter("@P_UserId", SqlDbType.Int,4),
            //                    new SqlParameter("@P_VideoId", SqlDbType.NVarChar,100),
            //                    new SqlParameter("@P_QuestionBankId", SqlDbType.Int,100),
            //                    new SqlParameter("@P_LearnStatus", SqlDbType.Int,4),
            //                    new SqlParameter("@P_PlayTime", SqlDbType.Int,4),
            //                    new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
            //                    new SqlParameter("@P_CreateUser", SqlDbType.Int,4),
            //                    new SqlParameter("@P_Status", SqlDbType.Int,4),
            //                   };
            //                parameters[0].Value = Guid.NewGuid().ToString("N");
            //                parameters[1].Value = userid;
            //                parameters[2].Value = videoid;
            //                parameters[3].Value = id;
            //                parameters[4].Value = 0;
            //                parameters[5].Value = 0;
            //                parameters[6].Value = DateTime.Now;
            //                parameters[7].Value = userid;
            //                parameters[8].Value = 0;
            //                object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
            //                a = 0;
            //            }

            //            trans.Commit();
            //        }
            //        catch (Exception esss)
            //        {
            //            trans.Rollback();
            //            return 0;
            //        }
            //    }
            //}
            return a;
        }

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public bool Exists(int userid, int id, string videoid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_VideoRecord");
            strSql.Append(" where P_UserId=@P_UserId and P_VideoId=@P_VideoId and P_QuestionBankId=@P_QuestionBankId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@P_UserId", SqlDbType.Int,4),
                     new SqlParameter("@P_VideoId", SqlDbType.NVarChar,100),
                      new SqlParameter("@P_QuestionBankId", SqlDbType.Int,4)
            };
            parameters[0].Value = userid;
            parameters[1].Value = videoid;
            parameters[2].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        public bool Exist(int userid, string videoid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_Collect");
            strSql.Append(" where P_UserId=@P_UserId and P_Relation=@P_Relation and P_Type=@P_Type ");
            SqlParameter[] parameters = {
                    new SqlParameter("@P_UserId", SqlDbType.Int,4),
                     new SqlParameter("@P_Relation", SqlDbType.NVarChar,100),
                      new SqlParameter("@P_Type", SqlDbType.NVarChar,100)
            };
            parameters[0].Value = userid;
            parameters[1].Value = videoid;
            parameters[2].Value = 52;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 上传视频时长接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean GetVideoStatus(VideoPlay model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("update  P_VideoRecord set P_PlayTime=" + model.playtime + @",P_LearnStatus=" + model.playstatus + @" ");
                        str.Append(" where P_UserId='" + model.userid + @"' and P_VideoId='" + model.videoid + @"' and P_QuestionBankId='" + model.id + @"' ");
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString()); //带事务
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
        /// 上传观看视频时长
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public Boolean UploadVideoPlaybackTime(UploadVideoPlaybackFrombody fromBody) {

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int userId = fromBody.user_id;
                        string videoId = fromBody.video_id;
                        // 判断用户是否存在视频观看记录
                        if (VideoRecordExist(userId, videoId))
                        { // 存在则修改观看记录数据

                            string querySql = String.Format(@"select P_VideoRecord.P_MaxPlaybackTime from P_VideoRecord
                                where P_VideoRecord.P_UserId = {0} and P_VideoRecord.P_VideoId = '{1}'", fromBody.user_id, fromBody.video_id);

                            int maxPlayTime = Convert.ToInt32(DbHelperSQL.GetSingle(querySql));

                            string videoLengthSql = String.Format(@"select P_VideoLength,P_Url from P_Video
                                where P_Video.P_Id = '{0}'", fromBody.video_id);
                            // 获取视频信息
                            DataSet ds = DbHelperSQL.Query(videoLengthSql);
                            // 获取视频时长
                            int videoLength = Int32.Parse(ds.Tables[0].Rows[0]["P_VideoLength"].ToString());
                            // 判断视频时长为0时重新获取视频时长
                            if (videoLength == 0)
                            {
                                videoLength = (int)new QiNiuHelper().GetVideoLength(ds.Tables[0].Rows[0]["P_Url"].ToString());
                                // 重新获取时长失败返回false
                                if (videoLength == -1)
                                {
                                    return false;
                                }
                            }

                            StringBuilder updateSql = new StringBuilder();

                            updateSql.Append("update P_VideoRecord set");
                            updateSql.Append(" P_LastPlaybackTime = @P_LastPlaybackTime,");
                            updateSql.Append(" P_MaxPlaybackTime = @P_MaxPlaybackTime");
                            updateSql.Append(" where P_UserId = @P_UserId and P_VideoId = @P_VideoId");

                            SqlParameter[] updatePar = {
                                new SqlParameter("P_LastPlaybackTime", SqlDbType.Int),
                                new SqlParameter("P_MaxPlaybackTime", SqlDbType.Int),
                                new SqlParameter("P_UserId", SqlDbType.NVarChar),
                                new SqlParameter("P_VideoId", SqlDbType.NVarChar)
                            };
                            updatePar[0].Value = fromBody.playback_time >= videoLength ? 0 : fromBody.playback_time;
                            updatePar[1].Value = fromBody.playback_time > maxPlayTime ? fromBody.playback_time : maxPlayTime;
                            updatePar[2].Value = fromBody.user_id;
                            updatePar[3].Value = fromBody.video_id;
                            DbHelperSQL.ExecuteSql(conn, trans, updateSql.ToString(), updatePar);

                        }
                        else { // 不存在观看记录，新建观看记录
                            StringBuilder insertSql = new StringBuilder();
                            insertSql.Append("insert into P_VideoRecord (");
                            insertSql.Append("P_Id,P_UserId,P_VideoId,P_QuestionBankId,P_CreateTime,P_CreateUser,P_Status,P_MaxPlaybackTime,P_LastPlaybackTime)");
                            insertSql.Append(" values (@P_Id,@P_UserId,@P_VideoId,@P_QuestionBankId,@P_CreateTime,@P_CreateUser,@P_Status,@P_MaxPlaybackTime,@P_LastPlaybackTime)");

                            SqlParameter[] parameters = {
                                new SqlParameter("P_Id",SqlDbType.NVarChar),
                                new SqlParameter("P_UserId",SqlDbType.NVarChar),
                                new SqlParameter("P_VideoId",SqlDbType.NVarChar),
                                new SqlParameter("P_QuestionBankId",SqlDbType.NVarChar),
                                new SqlParameter("P_CreateTime",SqlDbType.DateTime),
                                new SqlParameter("P_CreateUser",SqlDbType.NVarChar),
                                new SqlParameter("P_Status",SqlDbType.Int),
                                new SqlParameter("P_MaxPlaybackTime",SqlDbType.Int),
                                new SqlParameter("P_LastPlaybackTime",SqlDbType.Int)
                            };

                            parameters[0].Value = Guid.NewGuid().ToString();
                            parameters[1].Value = fromBody.user_id;
                            parameters[2].Value = fromBody.video_id;
                            parameters[3].Value = fromBody.course_id;
                            parameters[4].Value = DateTime.Now;
                            parameters[5].Value = fromBody.user_id;
                            parameters[6].Value = 0;
                            parameters[7].Value = fromBody.playback_time;
                            parameters[8].Value = fromBody.playback_time;
                            object obj = DbHelperSQL.GetSingle(conn, trans, insertSql.ToString(), parameters); //带事务
                        }
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
        ///  用户是否存在视频观看记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="videoId">视频Id</param>
        /// <returns></returns>
        private Boolean VideoRecordExist(int userId, string videoId) {

            string sql = String.Format(@"select count(1) from P_VideoRecord
                        where P_VideoRecord.P_UserId = {0} and P_VideoRecord.P_VideoId = '{1}'", userId, videoId);

            return DbHelperSQL.Exists(sql);
        }

        /// <summary>
        /// 获取视频详情
        /// </summary>
        /// <param name="videoId">视频ID</param>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        public VideoDetialModel GetVideoDetail(string videoId, string userId) {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_Video.p_id as id,P_Video.p_videoname as name,P_Video.p_url as url,");
            strsql.Append("  P_Video.P_VideoPic as videopic,P_VideoRecord.P_LastPlaybackTime as lastplaytime,P_VideoRecord.P_MaxPlaybackTime as maxplaytime,P_Video.P_VideoLength as videolength,");
            strsql.Append("(select COUNT(P_VideoRecord.P_Id) from P_VideoRecord where P_VideoRecord.P_VideoId=P_Video.P_Id  ) as playcount,");
            strsql.Append(" (select COUNT(aa.rela) from (select DISTINCT P_Collect.P_userid as uuid,P_Collect.P_Relation as rela from P_Collect ");
            strsql.Append(" where p_collect.p_relation =P_Video.p_id and p_collect.P_Type='52' )aa) as collectcount, ");
            strsql.Append("  COUNT(P_Transmit.P_id) as trankcount,");
            strsql.Append(" CASE WHEN (select count(p_collect.P_id) from p_collect where p_collect.p_relation =P_Video.p_id and p_collect.P_Type='52' and p_collect.p_userid =" + userId + @")!=0 THEN 1 ELSE 0 END as collect  ");
            strsql.Append(" from P_Video  ");
            strsql.Append(" LEFT JOIN P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id and P_VideoRecord.P_UserId=" + userId + @" ");
            //strsql.Append(" LEFT JOIN p_collect on p_collect.p_relation =P_Video.p_id and p_collect.P_Type='52' ");
            strsql.Append(" LEFT JOIN P_Transmit on P_Transmit.p_relationid=P_Video.p_id and P_Transmit.P_Type='52' ");
            strsql.Append(" where P_Video.P_Status = 0 and P_Video.P_Id='" + videoId + @"' ");
            strsql.Append("  GROUP BY P_Video.p_id,P_Video.p_videoname ,P_Video.p_url,P_Video.P_Number,P_Video.P_VideoPic,P_VideoRecord.P_LastPlaybackTime,P_VideoRecord.P_MaxPlaybackTime,P_Video.P_VideoLength");

            DataSet ds = DbHelperSQL.Query(strsql.ToString());

            if (ds.Tables[0] !=null && ds.Tables[0].Rows.Count > 0) {
                DataSetToModelHelper<VideoDetialModel> helper = new DataSetToModelHelper<VideoDetialModel>();
                return helper.FillToModel(ds.Tables[0].Rows[0]);
            }

            return null;
        }

    }
    public class videoModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string imgurl { get; set; }
        public int hour { get; set; }
        public int collect { get; set; }
        //public int leaningcount { get; set; }
    }

    public class CorceModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int playcount { get; set; }
        public int collectcount { get; set; }
        public int trankcount { get; set; }
        public int collect { get; set; }
        public string videopic { get; set; }
        //public int playtime { get; set; }
        public int lastplaytime { get; set; }
        public int maxplaytime { get; set; }
        public int videolength { get; set; }
    }
    public class VideoId
    {
        public string id { get; set; }
    }
    public class VideoPlay
    {
        public int userid { get; set; }
        public int id { get; set; }
        public string videoid { get; set; }
        public int playtime { get; set; }
        public int playstatus { get; set; }
    }
    public class Videotrue
    {
        public string voideoid { get; set; }
    }
}

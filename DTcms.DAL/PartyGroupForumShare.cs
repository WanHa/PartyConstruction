using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTcms.Model;
using DTcms.Model.WebApiModel.FromBody;

namespace DTcms.DAL
{
    public partial class PartyGroupForumShare
    {
        QiNiuHelper qiniu = new QiNiuHelper();
        string shareId = Guid.NewGuid().ToString();
        #region 分享
        /// <summary>
        /// 分享文字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int shareMessage(ForumShare model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string p_id = Guid.NewGuid().ToString();
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"insert into P_ForumShare(P_Id,P_GroupForumId,P_UserId,P_Content,P_CreateTime,P_Status,P_Type,P_Source)
                            values(@P_Id,@P_GroupForumId,@P_UserId,@P_Content,@P_CreateTime,@P_Status,@P_Type,@P_Source)");
                        SqlParameter[] parameters = {
                            new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_GroupForumId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UserId", SqlDbType.Int),
                            new SqlParameter("@P_Content", SqlDbType.NText),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_Status", SqlDbType.Int),
                            new SqlParameter("@P_Type", SqlDbType.Int),
                            new SqlParameter("@P_Source", SqlDbType.Int),
                         };

                        parameters[0].Value = p_id;
                        parameters[1].Value = model.groupname;
                        parameters[2].Value = model.createrid;
                        parameters[3].Value = model.content;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = 0;
                        parameters[6].Value = model.tpye;
                        parameters[7].Value = 0;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        AddAtData(p_id, model.at_personnels, trans, conn, model.createrid.ToString());
                        trans.Commit();
                        // 发送推送信息
                        if (model.at_personnels != null && model.at_personnels.Count > 0)
                        {
                            List<int> per = new List<int>();
                            foreach (var item in model.at_personnels)
                            {
                                per.Add(item.user_id);
                            }

                            PushMessageHelper.PushMessages(p_id, "您收到一条@信息.", per, model.createrid, (int)PushTypeEnum.AT);
                        }
                        return 1;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append(@"insert into P_ForumShare(P_Id,P_GroupForumId,P_UserId,P_Content,P_CreateTime,P_Status,P_Type,P_Source)
            //                values(@P_Id,@P_GroupForumId,@P_UserId,@P_Content,@P_CreateTime,@P_Status,@P_Type,@P_Source)");
            //SqlParameter[] parameters = {
            //        new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
            //        new SqlParameter("@P_GroupForumId", SqlDbType.NVarChar,50),
            //        new SqlParameter("@P_UserId", SqlDbType.Int),
            //        new SqlParameter("@P_Content", SqlDbType.NText),
            //        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
            //        new SqlParameter("@P_Status", SqlDbType.Int),
            //        new SqlParameter("@P_Type", SqlDbType.Int),
            //        new SqlParameter("@P_Source", SqlDbType.Int),
            //};

            //parameters[0].Value = shareId;
            //parameters[1].Value = model.groupname;
            //parameters[2].Value = model.createrid;
            //parameters[3].Value = model.content;
            //parameters[4].Value = DateTime.Now;
            //parameters[5].Value = 0;
            //parameters[6].Value = model.tpye;
            //parameters[7].Value = 0;
            //int obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            //if (obj > 0)
            //{
            //    return Convert.ToInt32(obj);
            //}
            //else
            //{
            //    return 0;
            //}
        }

        /// <summary>
        /// 论坛发布信息时,添加@人员信息数据
        /// </summary>
        /// <param name="mainId">论坛发布信息ID</param>
        /// <param name="personnels">@人员列表</param>
        /// <param name="trans"></param>
        /// <param name="conn"></param>
        /// <param name="createUser">发布信息用户ID</param>
        private void AddAtData(string mainId, List<AtPersonnelFrombody> personnels, SqlTransaction trans, SqlConnection conn, string createUser) {

            if (personnels != null && personnels.Count > 0) {
                foreach (AtPersonnelFrombody item in personnels)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into P_AtPerson (P_Id,P_Relation,P_UserId,P_Type,P_CreateTime,P_CreateUser,P_Status)");
                    sql.Append(" values (@P_Id,@P_Relation,@P_UserId,@P_Type, @P_CreateTime,@P_CreateUser,@P_Status)");
                    SqlParameter[] par = {
                        new SqlParameter("@P_Id", SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_Relation", SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_UserId", SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_Type", SqlDbType.Int, 4),
                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_Status", SqlDbType.Int, 4)
                    };
                    par[0].Value = Guid.NewGuid().ToString("N");
                    par[1].Value = mainId;
                    par[2].Value = item.user_id.ToString();
                    par[3].Value = (int)AtTypeEnum.论坛发布;
                    par[4].Value = DateTime.Now;
                    par[5].Value = createUser;
                    par[6].Value = 0;
                    DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), par);
                }
            }

        }

        /// <summary>
        /// 分享图片文字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int shareImg(ForumShare model)
        {

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string p_id = Guid.NewGuid().ToString();
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"insert into P_ForumShare(P_Id,P_GroupForumId,P_UserId,P_Content,P_CreateTime,P_Status,P_Type,P_Source)
                            values(@P_Id,@P_GroupForumId,@P_UserId,@P_Content,@P_CreateTime,@P_Status,@P_Type,@P_Source)");
                        SqlParameter[] parameters = {
                            new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_GroupForumId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UserId", SqlDbType.Int),
                            new SqlParameter("@P_Content", SqlDbType.NText),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_Status", SqlDbType.Int),
                            new SqlParameter("@P_Type", SqlDbType.Int),
                            new SqlParameter("@P_Source", SqlDbType.Int),
                         };

                        parameters[0].Value = p_id;
                        parameters[1].Value = model.groupname;
                        parameters[2].Value = model.createrid;
                        parameters[3].Value = model.content;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = 0;
                        parameters[6].Value = model.tpye;
                        parameters[7].Value = 0;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        AddImage(p_id, model, trans, conn, model.createrid.ToString());
                        AddAtData(p_id, model.at_personnels, trans, conn, model.createrid.ToString());
                        trans.Commit();
                        // 发送推送信息
                        if (model.at_personnels != null && model.at_personnels.Count > 0)
                        {
                            List<int> per = new List<int>();
                            foreach (var item in model.at_personnels)
                            {
                                per.Add(item.user_id);
                            }

                            PushMessageHelper.PushMessages(p_id, "您收到一条@信息.", per, model.createrid, (int)PushTypeEnum.AT);
                        }
                        return 1;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            //int obj = shareMessage(model);
            //int count = 0;
            //for (int i = 0; i < model.imageurl.Count; i++)
            //{
            //    string id = Guid.NewGuid().ToString();
            //    StringBuilder strSql = new StringBuilder();
            //    strSql.Append(@"insert into P_Image(P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_PictureName,P_ImageType)
            //                values(@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_PictureName,@P_ImageType)");
            //    SqlParameter[] parameters = {
            //        new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
            //        new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
            //        new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,100),
            //        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
            //        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar),
            //        new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
            //        new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
            //        new SqlParameter("@P_Status", SqlDbType.Int),
            //        new SqlParameter("@P_PictureName", SqlDbType.NVarChar),
            //        new SqlParameter("@P_ImageType", SqlDbType.NVarChar)
            //};
            //    parameters[0].Value = id;
            //    parameters[1].Value = shareId;
            //    parameters[2].Value = qiniu.GetQiNiuFileUrl(model.imageurl[i].image);
            //    parameters[3].Value = DateTime.Now;
            //    parameters[4].Value = model.createrid;
            //    parameters[5].Value = null;
            //    parameters[6].Value = null;
            //    parameters[7].Value = 0;
            //    parameters[8].Value = model.imageurl[i].image;
            //    parameters[9].Value = (int)ImageTypeEnum.党小组论坛;
            //     DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            //}
        }

        /// <summary>
        /// 论坛发布信息时,添加图片信息
        /// </summary>
        /// <param name="p_id">信息ID</param>
        /// <param name="model">图片信息</param>
        /// <param name="trans"></param>
        /// <param name="conn"></param>
        /// <param name="createUser">发布用户</param>
        private void AddImage(string p_id, ForumShare model, SqlTransaction trans, SqlConnection conn, string createUser) {

            if (model.imageurl != null && model.imageurl.Count > 0) {
                for(int i = 0;i < model.imageurl.Count; i++)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"insert into P_Image(P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_PictureName,P_ImageType)
                            values(@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_PictureName,@P_ImageType)");
                    SqlParameter[] parameters = {
                    new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                    new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,100),
                    new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@P_CreateUser", SqlDbType.NVarChar),
                    new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_Status", SqlDbType.Int),
                    new SqlParameter("@P_PictureName", SqlDbType.NVarChar),
                    new SqlParameter("@P_ImageType", SqlDbType.NVarChar)
            };
                    parameters[0].Value = Guid.NewGuid().ToString();
                    parameters[1].Value = p_id;
                    parameters[2].Value = new QiNiuHelper().GetQiNiuFileUrl(model.imageurl[i].image);
                    parameters[3].Value = DateTime.Now;
                    parameters[4].Value = model.createrid;
                    parameters[5].Value = null;
                    parameters[6].Value = null;
                    parameters[7].Value = 0;
                    parameters[8].Value = model.imageurl[i].image;
                    parameters[9].Value = (int)ImageTypeEnum.党小组论坛;
                    DbHelperSQL.ExecuteSql(conn, trans,strSql.ToString(), parameters);
                }
            }
        }

        /// <summary>
        /// 分享视频文字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int shareVideo(ForumShare model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string p_id = Guid.NewGuid().ToString();
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"insert into P_ForumShare(P_Id,P_GroupForumId,P_UserId,P_Content,P_CreateTime,P_Status,P_Type,P_Source)
                            values(@P_Id,@P_GroupForumId,@P_UserId,@P_Content,@P_CreateTime,@P_Status,@P_Type,@P_Source)");
                        SqlParameter[] parameters = {
                            new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_GroupForumId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UserId", SqlDbType.Int),
                            new SqlParameter("@P_Content", SqlDbType.NText),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_Status", SqlDbType.Int),
                            new SqlParameter("@P_Type", SqlDbType.Int),
                            new SqlParameter("@P_Source", SqlDbType.Int),
                         };

                        parameters[0].Value = p_id;
                        parameters[1].Value = model.groupname;
                        parameters[2].Value = model.createrid;
                        parameters[3].Value = model.content;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = 0;
                        parameters[6].Value = model.tpye;
                        parameters[7].Value = 0;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        AddVideo(p_id, model.videourl,trans, conn, model.createrid.ToString());
                        AddAtData(p_id, model.at_personnels, trans, conn, model.createrid.ToString());
                        trans.Commit();
                        // 发送推送信息
                        if (model.at_personnels != null && model.at_personnels.Count > 0)
                        {
                            List<int> per = new List<int>();
                            foreach (var item in model.at_personnels)
                            {
                                per.Add(item.user_id);
                            }

                            PushMessageHelper.PushMessages(p_id, "您收到一条@信息.", per, model.createrid, (int)PushTypeEnum.AT);
                        }
                        return 1;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            //int obj = shareMessage(model);
            //string id = Guid.NewGuid().ToString();
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append(@"insert into P_Video(P_Id,P_ParentId,P_VideoName,P_VideoPic,P_Url,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_Number,P_VideoLength,P_Source)
            //                values(@P_Id,@P_ParentId,@P_VideoName,@P_VideoPic,@P_Url,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_Number,@P_VideoLength,@P_Source)");
            //SqlParameter[] parameters = {
            //        new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
            //        new SqlParameter("@P_ParentId", SqlDbType.NVarChar,50),
            //        new SqlParameter("@P_VideoName", SqlDbType.NVarChar,100),
            //        new SqlParameter("@P_VideoPic", SqlDbType.NVarChar,200),
            //        new SqlParameter("@P_Url", SqlDbType.NVarChar,300),
            //        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
            //        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
            //        new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
            //        new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
            //        new SqlParameter("@P_Status", SqlDbType.Int),
            //        new SqlParameter("@P_Number", SqlDbType.Int),
            //        new SqlParameter("@P_VideoLength", SqlDbType.Int),
            //        new SqlParameter("@P_Source", SqlDbType.Int),
            //};
            //string videoUrl = qiniu.GetQiNiuFileUrl(model.videourl);
            //parameters[0].Value = id;
            //parameters[1].Value = shareId;
            //parameters[2].Value = model.videourl;
            //parameters[3].Value = qiniu.GetQiNiuVideoPicUrl(videoUrl);
            //parameters[4].Value = videoUrl;
            //parameters[5].Value = DateTime.Now;
            //parameters[6].Value = model.createrid;
            //parameters[7].Value = DateTime.Now;
            //parameters[8].Value = "";
            //parameters[9].Value = 0;
            //parameters[10].Value = 0;
            //parameters[11].Value = qiniu.GetVideoLength(videoUrl);
            //parameters[12].Value = (int)VideoSourceEnum.党小组论坛;
            //int obja = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            //if (obj > 0 && obja > 0)
            //{
            //    return Convert.ToInt32(obj);
            //}
            //else
            //{
            //    return 0;
            //}
        }

        /// <summary>
        /// 论坛发布信息时,添加视频
        /// </summary>
        /// <param name="p_id">发布信息ID</param>
        /// <param name="videoName">视频名称</param>
        /// <param name="trans"></param>
        /// <param name="conn"></param>
        /// <param name="createUser">发布信息用户</param>
        private void AddVideo(string p_id, string videoName, SqlTransaction trans, SqlConnection conn, string createUser) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"insert into P_Video(P_Id,P_ParentId,P_VideoName,P_VideoPic,P_Url,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_Number,P_VideoLength,P_Source)
                            values(@P_Id,@P_ParentId,@P_VideoName,@P_VideoPic,@P_Url,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_Number,@P_VideoLength,@P_Source)");
            SqlParameter[] parameters = {
                    new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_ParentId", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_VideoName", SqlDbType.NVarChar,100),
                    new SqlParameter("@P_VideoPic", SqlDbType.NVarChar,200),
                    new SqlParameter("@P_Url", SqlDbType.NVarChar,300),
                    new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_Status", SqlDbType.Int),
                    new SqlParameter("@P_Number", SqlDbType.Int),
                    new SqlParameter("@P_VideoLength", SqlDbType.Int),
                    new SqlParameter("@P_Source", SqlDbType.Int),
            };
            QiNiuHelper helper = new QiNiuHelper();
            string videoUrl = helper.GetQiNiuFileUrl(videoName);
            parameters[0].Value = Guid.NewGuid().ToString();
            parameters[1].Value = p_id;
            parameters[2].Value = videoName;
            parameters[3].Value = helper.GetQiNiuVideoPicUrl(videoUrl);
            parameters[4].Value = videoUrl;
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = createUser;
            parameters[7].Value = DateTime.Now;
            parameters[8].Value = "";
            parameters[9].Value = 0;
            parameters[10].Value = 0;
            parameters[11].Value = helper.GetVideoLength(videoUrl);
            parameters[12].Value = (int)VideoSourceEnum.党小组论坛;
            int obja = DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
        }

        #endregion

        #region 删除组员
        /// <summary>
        /// 删除组员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int dorpMember(deleteMember model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update P_PersonGroupRelation set p_status = @status
                            where P_PartyGroupId = @P_PartyGroupId and P_UserId = @P_UserId");
            SqlParameter[] parameters = {
                    new SqlParameter("@status", SqlDbType.NVarChar,100),
                    new SqlParameter("@P_PartyGroupId", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_UserId", SqlDbType.Int)
            };
            parameters[0].Value = 1;
            parameters[1].Value = model.forumId;
            parameters[2].Value = model.memberId;
            int obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (obj > 0)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 屏蔽党员
        /// <summary>
        /// 屏蔽党员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean shieldMember(shieldMemberModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //支部屏蔽
                        if (model.source == 0)
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(@"insert into P_Shield(P_Id,P_UserId,P_ShieldUserId,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_RelationId,P_Source)
                                                         values(@P_Id,@P_UserId,@P_ShieldUserId,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_RelationId,@P_Source)");
                            SqlParameter[] parameters = {
                            new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UserId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ShieldUserId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Status", SqlDbType.Int),
                            new SqlParameter("@P_RelationId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Source",SqlDbType.Int,4)
                            };
                            string id = Guid.NewGuid().ToString();
                            parameters[0].Value = id;
                            parameters[1].Value = model.userid;
                            parameters[2].Value = model.shieldUserId;
                            parameters[3].Value = DateTime.Now;
                            parameters[4].Value = model.userid;
                            parameters[5].Value = DateTime.Now;
                            parameters[6].Value = model.userid;
                            parameters[7].Value = 0;
                            parameters[8].Value = model.relationid;
                            parameters[9].Value = 0;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        }
                        //论坛屏蔽
                        if (model.source == 1)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append(@"insert into P_Shield(P_Id,P_UserId,P_ShieldUserId,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_RelationId,P_Source)
                                                        values(@P_Id,@P_UserId,@P_ShieldUserId,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_RelationId,@P_Source)");
                            SqlParameter[] parameters1 = {
                            new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UserId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ShieldUserId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Status", SqlDbType.Int),
                            new SqlParameter("@P_RelationId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Source",SqlDbType.Int,4)
                                 };
                            string id1 = Guid.NewGuid().ToString();
                            parameters1[0].Value = id1;
                            parameters1[1].Value = model.userid;
                            parameters1[2].Value = model.shieldUserId;
                            parameters1[3].Value = DateTime.Now;
                            parameters1[4].Value = model.userid;
                            parameters1[5].Value = DateTime.Now;
                            parameters1[6].Value = model.userid;
                            parameters1[7].Value = 0;
                            parameters1[8].Value = model.relationid;
                            parameters1[9].Value = 1;
                            object obj1 = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameters1); //带事务
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

        #endregion

        #region 组员列表
        /// <summary>
        /// 获取组员列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MembersListModel> getMembersList(string partyGroupId, int rows, int page)
        {
            
            string sql = String.Format(@"select P_PersonGroupRelation.P_UserId as userid ,
                        P_Image.P_ImageUrl as avatar,
                        dt_users.user_name as username,
                        P_PersonGroupRelation.P_CreateTime
                        from P_PersonGroupRelation
                        LEFT JOIN P_Image ON P_Image.P_ImageId = P_PersonGroupRelation.P_UserId and P_Image.P_ImageType = 20011
                        LEFT JOIN dt_users ON Convert(varchar,dt_users.id) = P_PersonGroupRelation.P_UserId
                        where P_PersonGroupRelation.P_PartyGroupId = '{0}' 
                        and P_PersonGroupRelation.P_Status = 0 and P_PersonGroupRelation.P_Approval = 0", partyGroupId);
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_PersonGroupRelation.P_CreateTime"));
            DataSetToModelHelper<MembersListModel> model = new DataSetToModelHelper<MembersListModel>();
            List<MembersListModel> result = model.FillModel(dt);
            return result;
        }
        #endregion

        #region  点赞
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int like(likeModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"insert into P_ThumbUp(P_Id,P_UserId,P_ArticleId,P_ThumbCount,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status,P_Type,P_FamilyType)
                            values(@P_Id,@P_UserId,@P_ArticleId,@P_ThumbCount,@P_CreateTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status,@P_Type,@P_FamilyType);");
            SqlParameter[] parameters = {
                    new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_UserId", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_ArticleId", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_ThumbCount", SqlDbType.Int),
                    new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_Status", SqlDbType.Int),
                    new SqlParameter("@P_Type", SqlDbType.NVarChar,50),
                    new SqlParameter("@P_FamilyType", SqlDbType.Int),
            };
            string id = Guid.NewGuid().ToString();
            parameters[0].Value = id;
            parameters[1].Value = model.userId;
            parameters[2].Value = model.shareId;
            parameters[3].Value = model.thumbCount + 1;
            parameters[4].Value = DateTime.Now;
            parameters[5].Value = model.userId;
            parameters[6].Value = "";
            parameters[7].Value = "";
            parameters[8].Value = 0;
            parameters[9].Value = "";
            parameters[10].Value = "";
            int obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (obj > 0)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 评论
        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int getFeedForCommentsModel(postComment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"insert into dt_article_comment(user_id,add_time,relation_id,type,content)
                            values (@user_id,@add_time,@relation_id,@type,@content)");
            SqlParameter[] parameters = {
                    new SqlParameter("@user_id", SqlDbType.Int),
                    new SqlParameter("@add_time", SqlDbType.DateTime),
                    new SqlParameter("@relation_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@content", SqlDbType.NText),
            };
            parameters[0].Value = model.userid;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = model.shareid;
            parameters[3].Value = 1;
            parameters[4].Value = model.content;
            int obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (obj > 0)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }




        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<acceptComment> setFeedForCommentsModel(string userId, string forumShareId, int rows, int page)
        {
            
            string sql = String.Format(@"select 
                        dt_article_comment.content,
                        CONVERT(varchar(100), dt_article_comment.add_time, 20) as add_time,
                        dt_users.user_name,
                        P_Image.P_ImageUrl as avatar,
                        dt_article_comment.id,
                        (select COUNT(P_ThumbUp.P_Id) from P_ThumbUp
                        where P_ThumbUp.P_ArticleId = CONVERT(nvarchar,dt_article_comment.id) and P_ThumbUp.P_FamilyType = 4) as thumbcount,
                        (
                        select case when P_ThumbUp.P_Id is null then 0 else 1 end from P_ThumbUp
                        where P_ThumbUp.P_ArticleId =  CONVERT(nvarchar,dt_article_comment.id)
                        and P_ThumbUp.P_FamilyType = 4 and P_ThumbUp.P_UserId = '{0}') as IfLike
                        from dt_article_comment
                        left join dt_users on dt_users.id = dt_article_comment.user_id
                        left join P_Image on P_Image.P_ImageId = CONVERT(nvarchar,dt_users.id) and P_Image.P_ImageType = {1}
                        where dt_article_comment.relation_id = '{2}'", userId, (int)ImageTypeEnum.头像, forumShareId);
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "add_time desc"));
            DataSetToModelHelper<acceptComment> model = new DataSetToModelHelper<acceptComment>();
            List<acceptComment> result = model.FillModel(dt);
            return result;
        }
        #endregion


        #region 分享列表
        public List<shareListModel> GetShareList(string forumid, string userid, int rows, int page)
        {
            List<shareListModel> result = new List<shareListModel>();

            StringBuilder ctime = new StringBuilder();
            ctime.Append(@"select * from P_ForumShare where P_ForumShare.P_GroupForumId = '" + forumid + @"' and P_ForumShare.P_Status = 0 and P_ForumShare.P_UserId not in
                                    (select P_Shield.P_ShieldUserId from P_Shield 
                                    LEFT JOIN P_ForumShare ON P_ForumShare.P_Id = P_Shield.P_RelationId 
                                    where P_Shield.P_UserId = '" + userid + "' and P_Shield.P_Status = 0)");
            DataSet gettime = DbHelperSQL.Query(ctime.ToString());
            DataSetToModelHelper<P_ForumShare> tt = new DataSetToModelHelper<P_ForumShare>();
            List<P_ForumShare> timelist = tt.FillModel(gettime);
            if (timelist != null)
            {
                foreach (var item5 in timelist)
                {
                    DateTime a = Convert.ToDateTime(item5.P_CreateTime);

                    StringBuilder str = new StringBuilder();
                    str.Append(@"select P_ForumShare.P_UserId as id,P_Image.P_ImageUrl as useravatar,dt_users.user_name as username,P_ForumShare.P_Id as sharedataid,
                                            (Select Datename (month, '" + a + @"')+'-'+Datename(day,  '" + a + @"')) as createtime,
                                            CASE WHEN (SELECT COUNT(P_ThumbUp.P_Id) from P_ThumbUp where P_ThumbUp.P_ArticleId=P_ForumShare.P_Id and P_ThumbUp.P_UserId='" + userid + @"' and P_ThumbUp.P_Type=2) > 0 THEN 1 else 0 END as userthum, 
                                            CASE WHEN (SELECT COUNT(P_Collect.P_Id) from P_Collect where P_Collect.P_Relation=P_ForumShare.P_Id and P_Collect.P_UserId='" + userid + @"' and (P_Collect.P_Type=52 or P_Collect.P_Type=53 or P_Collect.P_Type=54 or P_Collect.P_Type=55))> 0 THEN 1 else 0 END as usercollect
                                            from P_ForumShare 
                                            LEFT JOIN dt_users ON dt_users.id = P_ForumShare.P_UserId
                                            LEFT JOIN P_Image ON P_Image.P_ImageId = P_ForumShare.P_UserId and P_Image.P_ImageType = 20011 and P_Image.P_Status = 0
                                            LEFT JOIN P_ThumbUp ON P_ThumbUp.P_ArticleId=P_ForumShare.P_Id
                                            LEFT JOIN P_Collect ON P_Collect.P_Relation=P_ForumShare.P_Id 
                                            where P_ForumShare.P_GroupForumId =  '" + forumid + @"'  and P_ForumShare.P_Status = 0 and P_ForumShare.P_UserId not in
                                            (select P_Shield.P_ShieldUserId from P_Shield 
                                            LEFT JOIN P_ForumShare ON P_ForumShare.P_Id = P_Shield.P_RelationId 
                                            where P_Shield.P_UserId =  '" + userid + "' and P_Shield.P_Status = 0 and P_Shield.P_RelationId = '"+forumid+ "' and P_Shield.P_Source = 1)");
                    DataSet ds = DbHelperSQL.Query(str.ToString());
                    DataSetToModelHelper<shareListModel> helper = new DataSetToModelHelper<shareListModel>();
                    result = helper.FillModel(ds);
                    foreach (shareListModel item in result)
                    {
                        UserGroupHelper groupid = new UserGroupHelper();
                        int id = groupid.GetUserMinimumGroupId(item.id);
                        StringBuilder groupname = new StringBuilder();
                        groupname.Append(@"select title as groupname from dt_user_groups where id = '" + id + "'");
                        DataSet name = DbHelperSQL.Query(groupname.ToString());
                        DataSetToModelHelper<shareListModel> helper1 = new DataSetToModelHelper<shareListModel>();
                        shareListModel model = helper1.FillToModel(name.Tables[0].Rows[0]);
                        item.groupname = model.groupname;

                        StringBuilder forumsource = new StringBuilder();
                        forumsource.Append(@"select P_Source as source from P_ForumShare where P_UserId = '" + item.id + "' and P_Status = 0 and P_GroupForumId = '" + forumid + "'");
                        DataSet fsource = DbHelperSQL.Query(forumsource.ToString());
                        DataSetToModelHelper<shareListModel> helper2 = new DataSetToModelHelper<shareListModel>();
                        shareListModel model2 = helper2.FillToModel(fsource.Tables[0].Rows[0]);
                        //区分分享来源 -- 0自己上传
                        if (model2.source == 0)
                        {
                            StringBuilder forumtype = new StringBuilder();
                            forumtype.Append(@"select P_Type as type from P_ForumShare where P_UserId = '" + item.id + "' and P_Status = 0 and P_GroupForumId = '" + forumid + "'");
                            DataSet type = DbHelperSQL.Query(forumtype.ToString());
                            DataSetToModelHelper<shareListModel> helper3 = new DataSetToModelHelper<shareListModel>();
                            shareListModel model3 = helper3.FillToModel(type.Tables[0].Rows[0]);
                            //判断分享类型 -- 文字
                            if (model3.type == 0)
                            {
                                StringBuilder con = new StringBuilder();
                                con.Append(@"select P_ForumShare.P_Content as content from P_ForumShare 
                                                 where P_UserId = '" + item.id + "' and P_Status = 0 and P_GroupForumId = '" + forumid + "'");
                                DataSet sharecon = DbHelperSQL.Query(con.ToString());
                                DataSetToModelHelper<shareListModel> helper4 = new DataSetToModelHelper<shareListModel>();
                                shareListModel model4 = helper4.FillToModel(sharecon.Tables[0].Rows[0]);
                                item.content = model4.content;
                            }
                            //判断分享类型 -- 图片
                            if (model3.type == 1)
                            {
                                StringBuilder img = new StringBuilder();
                                img.Append(@"select P_ForumShare.P_Content as content,P_Image.P_ImageUrl as imageurl from P_ForumShare
                                                  LEFT JOIN P_Image ON P_Image.P_ImageId = P_ForumShare.P_Id
                                                  where P_ForumShare.P_UserId = '" + item.id + "' and P_ForumShare.P_Status = 0 and P_ForumShare.P_GroupForumId = '" + forumid + "'");
                                DataSet shareimg = DbHelperSQL.Query(img.ToString());
                                if (shareimg.Tables[0].Rows.Count != 0)
                                {
                                    DataSetToModelHelper<image> helper5 = new DataSetToModelHelper<image>();
                                    List<image> imagedata = helper5.FillModel(shareimg);
                                    if (imagedata != null)
                                    {
                                        item.imageurl = imagedata;
                                    }
                                }
                                else
                                {
                                    item.imageurl = null;
                                }
                            }
                            //判断分享类型 -- 视频
                            if (model3.type == 2)
                            {
                                StringBuilder vid = new StringBuilder();
                                vid.Append(@"select P_ForumShare.P_Content as content,P_Video.P_Url as videourl                                               
                                                 from P_ForumShare
                                                 LEFT JOIN P_Video ON P_Video.P_ParentId =  P_ForumShare.P_Id
                                                 where P_ForumShare.P_UserId = '" + item.id + "' and P_ForumShare.P_GroupForumId = '" + forumid + "'");
                                DataSet sharevid = DbHelperSQL.Query(vid.ToString());
                                DataSetToModelHelper<shareListModel> helper6 = new DataSetToModelHelper<shareListModel>();
                                shareListModel model6 = helper6.FillToModel(sharevid.Tables[0].Rows[0]);
                                if (model6.videourl != null)
                                {
                                    //获取视频长度
                                    QiNiuHelper length = new QiNiuHelper();
                                    item.videolength = length.GetVideoLength(model6.videourl);
                                    //视频播放次数
                                    StringBuilder count = new StringBuilder();
                                    count.Append("select count(*)  as playcount from P_VideoRecord where P_VideoId = (select P_Id from P_Video where P_Url = '" + model6.videourl + @"' and P_Status = 0  and P_Source = 1)");
                                    DataSet co = DbHelperSQL.Query(count.ToString());
                                    DataSetToModelHelper<shareListModel> helper7 = new DataSetToModelHelper<shareListModel>();
                                    shareListModel model7 = helper6.FillToModel(co.Tables[0].Rows[0]);
                                    item.playcount = model7.playcount;
                                }
                            }
                            StringBuilder thumbupcount1 = new StringBuilder();
                            thumbupcount1.Append(@"select count(*) as thumbcount from P_ThumbUp 
                                                                where P_ArticleId = '" + item.sharedataid + "' and P_Status = 0 and P_FamilyType = 2");
                            DataSet thumb1 = DbHelperSQL.Query(thumbupcount1.ToString());
                            DataSetToModelHelper<shareListModel> helper82 = new DataSetToModelHelper<shareListModel>();
                            shareListModel model82 = helper82.FillToModel(thumb1.Tables[0].Rows[0]);
                            item.thumbcount = model82.thumbcount;

                            StringBuilder comcount1 = new StringBuilder();
                            comcount1.Append(@"select count(*) as commentcount from dt_article_comment
                                                        where relation_id = '" + item.sharedataid + "'");
                            DataSet com1 = DbHelperSQL.Query(comcount1.ToString());
                            DataSetToModelHelper<shareListModel> helper92 = new DataSetToModelHelper<shareListModel>();
                            shareListModel model92 = helper92.FillToModel(com1.Tables[0].Rows[0]);
                            item.commentcount = model92.commentcount;
                        }
                        //区分分享来源 -- 1转发
                        if (model2.source == 1)
                        {
                            StringBuilder ac = new StringBuilder();
                            ac.Append(@"select CAST(P_Type AS INT) as type from P_Transmit 
                                             where P_UserId = '" + item.id + "' and P_OrganizeId = '" + forumid + "'");
                            DataSet typeid = DbHelperSQL.Query(ac.ToString());
                            DataSetToModelHelper<shareListModel> typehelper = new DataSetToModelHelper<shareListModel>();
                            shareListModel tid = typehelper.FillToModel(typeid.Tables[0].Rows[0]);
                            //课程
                            if (tid.type == 52)
                            {
                                StringBuilder co = new StringBuilder();
                                co.Append(@"select  Convert(varchar,dt_article.id) as courseid , dt_article.title as content ,P_Video.P_Id as videoid,P_Video.P_VideoName as videoname,P_Video.P_Url as videourl from dt_article
                                               LEFT JOIN P_Video ON P_Video.P_ParentId = Convert(varchar,dt_article.id)
                                               where dt_article.user_id = '" + item.id + "'");
                                DataSet que = DbHelperSQL.Query(co.ToString());
                                DataSetToModelHelper<shareListModel> quehelper = new DataSetToModelHelper<shareListModel>();
                                shareListModel quemodel = quehelper.FillToModel(que.Tables[0].Rows[0]);
                                if (quemodel.videourl != null)
                                {
                                    //获取视频长度
                                    QiNiuHelper length = new QiNiuHelper();
                                    item.videolength = length.GetVideoLength(quemodel.videourl);
                                    //视频播放次数
                                    StringBuilder count = new StringBuilder();
                                    count.Append("select count(*)  as playcount from P_VideoRecord where P_VideoId = (select P_Id from P_Video where P_Url = '" + quemodel.videourl + @"' and P_Status = 0  and P_Source = 20000)");
                                    DataSet cou = DbHelperSQL.Query(count.ToString());
                                    DataSetToModelHelper<shareListModel> couhelper = new DataSetToModelHelper<shareListModel>();
                                    shareListModel coumodel = couhelper.FillToModel(cou.Tables[0].Rows[0]);
                                    item.playcount = coumodel.playcount;
                                }
                            }
                            //党建风采
                            if (tid.type == 53)
                            {
                                StringBuilder sty = new StringBuilder();
                                sty.Append(@"select  dt_article.id as styleid,dt_article.title as styletitile ,dt_article.content as stylecontent, P_Image.P_ImageUrl as imageurl from dt_article
                                                LEFT JOIN P_Image ON P_Image.P_ImageId = dt_article.id
                                                where dt_article.user_id = '" + item.id + "' and  dt_article.category_id = 53 and P_Image.P_Status = 0 and P_Image.P_ImageType = 20003");
                                DataSet styimg = DbHelperSQL.Query(sty.ToString());
                                DataSetToModelHelper<shareListModel> styimghelper = new DataSetToModelHelper<shareListModel>();
                                shareListModel stymodel = styimghelper.FillToModel(styimg.Tables[0].Rows[0]);
                                item.styletitile = stymodel.styletitile;
                                item.stylecontent = stymodel.stylecontent;
                                item.imageurl = stymodel.imageurl;
                            }
                            StringBuilder thumbupcount = new StringBuilder();
                            thumbupcount.Append(@"select count(*) as thumbcount from P_ThumbUp 
                                                                where P_ArticleId = '" + item.sharedataid + "' and P_Status = 0 and P_FamilyType = 2");
                            DataSet thumb = DbHelperSQL.Query(thumbupcount.ToString());
                            DataSetToModelHelper<shareListModel> helper81 = new DataSetToModelHelper<shareListModel>();
                            shareListModel model81 = helper81.FillToModel(thumb.Tables[0].Rows[0]);
                            item.thumbcount = model81.thumbcount;

                            StringBuilder comcount = new StringBuilder();
                            comcount.Append(@"select count(*) as commentcount from dt_article_comment
                                                        where relation_id = '" + item.sharedataid + "'");
                            DataSet com = DbHelperSQL.Query(comcount.ToString());
                            DataSetToModelHelper<shareListModel> helper91 = new DataSetToModelHelper<shareListModel>();
                            shareListModel model91 = helper91.FillToModel(com.Tables[0].Rows[0]);
                            item.commentcount = model91.commentcount;
                        }

                    }
                }
            }
            return result;
        }

        #endregion


        #region 模型
        public class deleteMember
        {
            public string forumId { get; set; }
            public string memberId { get; set; }
        }

        public class shieldMemberModel
        {
            public string userid { get; set; }
            public string shieldUserId { get; set; }
            public string relationid { get; set; }
            //来源
            public int source { get; set; }
        }

        public class likeModel
        {
            public string userId { get; set; }
            public string shareId { get; set; }
            public int thumbCount { get; set; }
        }

        public class feedForCommentsModel
        {
            public string commentImg { get; set; }
            public string commentName { get; set; }
            public DateTime commentTime { get; set; }
            public string commentContent { get; set; }
            public string commentId { get; set; }
            public int like { get; set; }

        }

        public class postComment
        {
            public int userid { set; get; }
            public string shareid { set; get; }
            public string content { set; get; }
        }

        public class acceptComment
        {
            public string avatar { set; get; }
            public string user_name { set; get; }
            public string add_time { set; get; }
            public string content { set; get; }
            public int id { set; get; }
            public int thumbcount { set; get; }
            public int IfLike { get; set; }

        }
        public class shareListModel
        {
            //用户id
            public int id { get; set; }
            //头像
            public string useravatar { get; set; }
            //用户名
            public string username { get; set; }
            //发布时间
            public string createtime { get; set; }
            //党组织
            public string groupname { get; set; }
            //分享id
            public string sharedataid { get; set; }
            //内容类型
            public int type { get; set; }
            //风采id
            public int styleid { get; set; }
            //风采标题
            public string styletitile { get; set; }
            //风采内容
            public string stylecontent { get; set; }
            //内容
            public string content { get; set; }
            //图片
            public List<image> imageurl { get; set; }
            //视频
            public string videourl { get; set; }
            //视频名称
            public string videoname { get; set; }
            //视频长度
            public double videolength { get; set; }
            //播放次数
            public int playcount { get; set; }
            //评论数量
            public int commentcount { get; set; }
            //点赞数量
            public int thumbcount { get; set; }
            //来源
            public int source { get; set; }
            //课程id
            public string courseid { get; set; }
            //视频id
            public string videoid { get; set; }
            //是否点赞
            public int userthum { get; set; }
            //是否收藏
            public int usercollect { get; set; }

        }

        public class image
        {
            public string url { get; set; }
            public string imagename { get; set; }
        }
        public class MembersListModel
        {
            //党员Id
            public string userid { set; get; }
            //头像
            public string avatar { set; get; }
            //姓名
            public string username { set; get; }
            //类型：判断是否为党小组论坛发起人
            //public int type { get; set; }

        }
        #endregion
    }
}

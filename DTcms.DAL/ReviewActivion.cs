using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class ReviewActivion
    {
        private QiNiuHelper qiqiu = new QiNiuHelper();
        /// <summary>
        /// 评选活动接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="asstatus"></param>
        /// <returns></returns>
        public List<ReviewModel> GetReView(int userid, int rows, int page, int asstatus)
        {
            StringBuilder strsql = new StringBuilder();
            if (asstatus == 0)
            {

                strsql.Append("select P_ReviewActivity.P_Title as title,dt_users.user_name as username,");
                strsql.Append("  CONVERT(varchar(100), P_ReviewActivity.P_EndTime,120)  as endtime,P_ReviewActivity.P_Id as id,P_ReviewActivity.P_VoteResult as voteresult,");
                strsql.Append(" CASE when (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_VoteUserID='" + userid + @"' and P_VotePerson.P_ActivityId=P_ReviewActivity.P_Id )=0 ");
                strsql.Append(" then 0 ELSE 1 END as userfroum");
                strsql.Append(" from P_ReviewActivity LEFT JOIN dt_users on dt_users.id = P_ReviewActivity.P_CreateUser ");
                strsql.Append(" where P_ReviewActivity.P_EndTime>GETDATE() and P_ReviewActivity.P_status=0 and dt_users.id is not null");

            }
            if (asstatus == 1)
            {

                strsql.Append("select P_ReviewActivity.P_Title as title,dt_users.user_name as username,");
                strsql.Append("  CONVERT(varchar(100), P_ReviewActivity.P_EndTime,120)  as endtime,P_ReviewActivity.P_Id as id,P_ReviewActivity.P_VoteResult as voteresult,");
                strsql.Append(" CASE when (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_VoteUserID='" + userid + @"' and P_VotePerson.P_ActivityId=P_ReviewActivity.P_Id )=0 ");
                strsql.Append(" THEN 0 ELSE 1 END as userfroum");
                strsql.Append(" from P_ReviewActivity LEFT JOIN dt_users on dt_users.id = P_ReviewActivity.P_CreateUser");
                strsql.Append(" where (P_ReviewActivity.P_EndTime<GETDATE() or P_ReviewActivity.P_EndTime=GETDATE()) and P_ReviewActivity.P_status=0 and dt_users.id is not null");
            }
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_ReviewActivity.P_CreateTime"));
            DataSetToModelHelper<ReviewModel> model = new DataSetToModelHelper<ReviewModel>();
            return model.FillModel(dt);

        }

        /// <summary>
        /// 创建评选活动接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean getAddReView(ActivityModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_ReviewActivity(");
                        strSql.Append("p_id,P_title,P_Content,P_OptionType,P_VoteResult,P_EndTime,P_CreateTime,P_CreateUser,P_Status,P_UserId)");
                        strSql.Append(" values (");
                        strSql.Append("@p_id,@P_title,@P_Content,@P_OptionType,@P_VoteResult,@P_EndTime,@P_CreateTime,@P_CreateUser,@P_Status,@P_UserId)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@p_id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_title", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NVarChar,400),
                                new SqlParameter("@P_OptionType", SqlDbType.Int,4),
                                new SqlParameter("@P_VoteResult", SqlDbType.Int,4),
                                new SqlParameter("@P_EndTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.Int,4),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                 new SqlParameter("@P_UserId", SqlDbType.NVarChar,100),
                               };
                        string oneid = Guid.NewGuid().ToString("N");
                        parameters[0].Value = oneid;
                        parameters[1].Value = model.title;
                        parameters[2].Value = model.content;
                        parameters[3].Value = model.optiontype;
                        parameters[4].Value = model.voteresult;
                        parameters[5].Value = model.endtime;
                        parameters[6].Value = DateTime.Now;
                        parameters[7].Value = model.userid;
                        parameters[8].Value = 0;
                        parameters[9].Value = model.userid;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        List<OptionModel> oo = model.options;
                        if (oo != null)
                        {
                            foreach (var item in oo)
                            {
                                StringBuilder str = new StringBuilder();
                                str.Append("insert into P_VoteOption(");
                                str.Append("p_id,P_ActivityId,p_option,P_CreateTime,P_CreateUser,P_Status)");
                                str.Append(" values (");
                                str.Append("@p_id,@P_ActivityId,@p_option,@P_CreateTime,@P_CreateUser,@P_Status)");
                                str.Append(";select @@IDENTITY");
                                SqlParameter[] para = {
                                new SqlParameter("@p_id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ActivityId", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_option", SqlDbType.NVarChar,400),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.Int,4),
                                new SqlParameter("@P_Status", SqlDbType.Int,4)
                               };
                                string twoid = Guid.NewGuid().ToString("N");
                                para[0].Value = twoid;
                                para[1].Value = oneid;
                                para[2].Value = item.option;
                                para[3].Value = DateTime.Now;
                                para[4].Value = model.userid;
                                para[5].Value = 0;
                                object obj1 = DbHelperSQL.GetSingle(conn, trans, str.ToString(), para); //带事务
                                if (item.imgname != null)
                                {
                                    string imgurl = qiqiu.GetQiNiuFileUrl(item.imgname);
                                    StringBuilder st = new StringBuilder();
                                    st.Append("insert into P_Image(");
                                    st.Append("p_id,p_imageid,p_imageUrl,P_CreateTime,P_CreateUser,P_Status,P_ImageType,P_PictureName)");
                                    st.Append(" values (");
                                    st.Append("@p_id,@p_imageid,@p_imageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_ImageType,@P_PictureName)");
                                    st.Append(";select @@IDENTITY");
                                    SqlParameter[] param = {
                                new SqlParameter("@p_id", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_imageid", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_imageUrl", SqlDbType.NVarChar,400),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.Int,4),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                 new SqlParameter("@P_ImageType", SqlDbType.NVarChar,100),
                                 new SqlParameter("@P_PictureName", SqlDbType.NVarChar,100)
                               };

                                    param[0].Value = Guid.NewGuid().ToString("N");
                                    param[1].Value = twoid;
                                    param[2].Value = imgurl;
                                    param[3].Value = DateTime.Now;
                                    param[4].Value = model.userid;
                                    param[5].Value = 0;
                                    param[6].Value = (int)ImageTypeEnum.评选活动;
                                    param[7].Value = item.imgname;
                                    object obj2 = DbHelperSQL.GetSingle(conn, trans, st.ToString(), param); //带事务
                                }
                            }
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
        /// 评选活动详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <param name="userfroum"></param>
        /// <returns></returns>
        public ReviewActivionModel GetReviewModel(string id, int userid)
        {
            ReviewActivionModel model = new ReviewActivionModel();

            StringBuilder str = new StringBuilder();
            str.Append(" select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,P_ReviewActivity.P_VoteResult as voteresult,P_ReviewActivity.P_OptionType as optiontype, ");
            str.Append("(select user_name from dt_users where dt_users.id=P_ReviewActivity.P_CreateUser) as createuser,");
            str.Append(" (CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24) as VARCHAR)+'天'+");
            str.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/3600-");
            str.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24)*24 as VARCHAR)+'时'+");
            str.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%60))/60-");
            str.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/60 as VARCHAR)+'分') as time,");
            str.Append(" (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ActivityId=P_ReviewActivity.P_Id ) as usercount");
            str.Append(" from P_ReviewActivity");
            str.Append(" where P_ReviewActivity.P_EndTime>GETDATE() and P_ReviewActivity.P_Id='" + id + @"' and P_ReviewActivity.P_status=0 ");
            DataSet dt = DbHelperSQL.Query(str.ToString());
            if (dt.Tables[0].Rows.Count == 0)
            {
                model = null;
            }
            else
            {
                DataSetToModelHelper<ReviewActivionModel> reviewactionmodel = new DataSetToModelHelper<ReviewActivionModel>();
                model = reviewactionmodel.FillToModel(dt.Tables[0].Rows[0]);
                StringBuilder ss = new StringBuilder();
                ss.Append("select P_Image.P_Id as imgurlid,P_Image.P_ImageUrl as imgurl,P_VoteOption.P_Id as optionid,P_VoteOption.P_Option as optioncontent ");
                ss.Append(" from P_VoteOption");
                ss.Append(" LEFT JOIN P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='"+(int)ImageTypeEnum.评选活动+@"' ");
                ss.Append(" where P_VoteOption.P_ActivityId='" + id + @"' and P_VoteOption.P_status=0 ");
                DataSet dd = DbHelperSQL.Query(ss.ToString());
                if (dd.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<VoteOptionModel> voteoptionmodel = new DataSetToModelHelper<VoteOptionModel>();
                    List<VoteOptionModel> votemodel = voteoptionmodel.FillModel(dd);
                    model.options = votemodel;
                }
                else
                {
                    model.options = null;
                }
            }
            return model;
        }
        /// <summary>
        /// 已结束的投票结果接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ModelToAction EndReviewModel(string id, int userid)
        {
            ModelToAction model = new ModelToAction();
            StringBuilder str1 = new StringBuilder();
            str1.Append("select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,");
            str1.Append("(select user_name from dt_users where dt_users.id=P_ReviewActivity.P_CreateUser) as createuser,");
            str1.Append("CONVERT(varchar(100), P_ReviewActivity.P_EndTime,120) as endtime,");
            str1.Append("(select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ActivityId='" + id + @"') as usercount ");
            str1.Append(" from P_ReviewActivity where P_ReviewActivity.P_Id='" + id + @"' and P_ReviewActivity.P_status=0 ");
            DataSet dt1 = DbHelperSQL.Query(str1.ToString());
            if (dt1.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<ModelToAction> reviewactionmodel = new DataSetToModelHelper<ModelToAction>();
                model = reviewactionmodel.FillToModel(dt1.Tables[0].Rows[0]);
                StringBuilder qq = new StringBuilder();
                qq.Append("select MAX(aa.playcount) as playcount  from   ");
                qq.Append(" (select count(P_VotePerson.P_Id) as playcount,P_VoteOption.P_Option as optioncontent, ");
                qq.Append(" P_Image.P_ImageUrl as imgurl from P_VoteOption ");
                qq.Append(" LEFT JOIN P_VotePerson on P_VotePerson.P_ByVoteUserId=P_VoteOption.P_Id ");
                qq.Append(" left JOIN P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='" + (int)ImageTypeEnum.评选活动 + @"' ");
                qq.Append(" where P_VoteOption.P_ActivityId='"+id+@"' and P_VoteOption.P_status=0  ");
                qq.Append(" group by P_VoteOption.P_Id,P_VoteOption.P_Option,P_Image.P_Id,P_Image.P_ImageUrl )aa ");
                DataSet das = DbHelperSQL.Query(qq.ToString());
                DataSetToModelHelper<PlayCountModel> pc = new DataSetToModelHelper<PlayCountModel>();
                if (das.Tables[0].Rows.Count != 0)
                {
                    PlayCountModel playcount = pc.FillToModel(das.Tables[0].Rows[0]);
                    StringBuilder ss1 = new StringBuilder();
                    ss1.Append("select count(P_VotePerson.P_Id) as playcount,P_VoteOption.P_Option as optioncontent, ");
                    ss1.Append(" P_Image.P_ImageUrl as imgurl from P_VoteOption ");
                    ss1.Append(" LEFT JOIN P_VotePerson on P_VotePerson.P_ByVoteUserId=P_VoteOption.P_Id ");
                    ss1.Append(" left JOIN P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='" + (int)ImageTypeEnum.评选活动 + @"' ");
                    ss1.Append(" where P_VoteOption.P_ActivityId='" + id + @"' and P_VoteOption.P_status=0 ");
                    ss1.Append(" group by P_VoteOption.P_Id,P_VoteOption.P_Option,P_Image.P_Id,P_Image.P_ImageUrl ");
                    ss1.Append(" HAVING count(P_VotePerson.P_Id)="+ playcount .playcount+ @" ");
                    ss1.Append(" ORDER BY count(P_VotePerson.P_Id) desc ");
                    DataSet dd1 = DbHelperSQL.Query(ss1.ToString());
                    if (dd1.Tables[0].Rows.Count != 0)
                    {
                        DataSetToModelHelper<ModelAction> voteoptionmodel1 = new DataSetToModelHelper<ModelAction>();
                        List<ModelAction> vote = voteoptionmodel1.FillModel(dd1);
                        model.actionmodel = vote;
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
        /// 已投票 任何人可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ReviewActivionToModel DetailReviewModel(string id, int userid)
        {
            ReviewActivionToModel model = new ReviewActivionToModel();
            StringBuilder str = new StringBuilder();
            str.Append(" select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,");
            str.Append("(select user_name from dt_users where dt_users.id=P_ReviewActivity.P_CreateUser) as createuser,");
            str.Append(" (select top 1 P_VotePerson.P_NameStatus from P_VotePerson where P_VotePerson.P_VoteUserID='" + userid + @"' and P_VotePerson.P_ActivityId='" + id + @"') as namestatus,");
            str.Append(" (CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24) as VARCHAR)+'天'+");
            str.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/3600-");
            str.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24)*24 as VARCHAR)+'时'+");
            str.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%60))/60-");
            str.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/60 as VARCHAR)+'分') as time,");
            str.Append(" (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ActivityId='" + id + @"') as usercount");
            str.Append(" from P_ReviewActivity");
            str.Append(" where P_ReviewActivity.P_EndTime>GETDATE() and P_ReviewActivity.P_Id='" + id + @"' and P_ReviewActivity.P_status=0 ");
            DataSet dt = DbHelperSQL.Query(str.ToString());
            if (dt.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<ReviewActivionToModel> review = new DataSetToModelHelper<ReviewActivionToModel>();
                model = review.FillToModel(dt.Tables[0].Rows[0]);
                StringBuilder ss = new StringBuilder();
                ss.Append("select CONVERT(FLOAT,table1.a*(1.0) / table2.b*(1.0)) as schedcount,* from (");
                ss.Append("	SELECT ");
                ss.Append(" COUNT(P_VotePerson.P_Id) AS a,");
                ss.Append(" P_VoteOption.P_ActivityId , ");
                ss.Append("P_VoteOption.P_Option as optioncontent,P_Image.P_ImageUrl as imgurl");
                ss.Append(" from P_VoteOption");
                ss.Append(" left join P_VotePerson on P_VotePerson.P_ByVoteUserId = P_VoteOption.P_Id");
                ss.Append(" left join P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='" + (int)ImageTypeEnum.评选活动 + @"' ");
                ss.Append(" where P_VoteOption.P_ActivityId ='" + id + @"'");
                ss.Append(" group by P_VoteOption.P_Id,P_VoteOption.P_Option,P_VoteOption.P_ActivityId,P_Image.P_ImageUrl");
                ss.Append(" )table1 ");
                ss.Append(" left join (");
                ss.Append(" select P_VoteOption.P_ActivityId,COUNT(P_VotePerson.P_Id) AS b ");
                ss.Append(" from P_VoteOption ");
                ss.Append(" left join P_VotePerson on P_VotePerson.P_ByVoteUserId = P_VoteOption.P_Id ");
                ss.Append(" where P_VoteOption.P_ActivityId = '" + id + @"'");
                ss.Append(" group by P_VoteOption.P_ActivityId ) table2 on table2.P_ActivityId = table1.P_ActivityId");
                DataSet dd = DbHelperSQL.Query(ss.ToString());
                if (dd.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<VoteOptionToModel> votemodel = new DataSetToModelHelper<VoteOptionToModel>();
                    List<VoteOptionToModel> voteoption = votemodel.FillModel(dd);
                    model.options = voteoption;
                }
                else
                {
                    model.options = null;
                }
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 投票接口
        /// </summary>
        /// <param name="froum"></param>
        /// <returns></returns>
        public Boolean GetFroumCount(FroumCount froum)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in froum.options)
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("insert into P_VotePerson(");
                            strSql.Append("P_Id,P_ActivityId,P_ByVoteUserId,P_VoteUserID,P_NameStatus,P_CreateTime,P_CreateUser,P_Status)");
                            strSql.Append(" values (");
                            strSql.Append("@P_Id,@P_ActivityId,@P_ByVoteUserId,@P_VoteUserID,@P_NameStatus,@P_CreateTime,@P_CreateUser,@P_Status)");
                            strSql.Append(";select @@IDENTITY");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ActivityId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ByVoteUserId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_VoteUserID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_NameStatus", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4)
                               };

                            parameters[0].Value = Guid.NewGuid().ToString("N");
                            parameters[1].Value = froum.activionid;
                            parameters[2].Value = item.optionid;
                            parameters[3].Value = froum.userid;
                            parameters[4].Value = froum.namestatus;
                            parameters[5].Value = DateTime.Now;
                            parameters[6].Value = froum.userid;
                            parameters[7].Value = 0;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
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
        /// 投完票 投票后可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ModelReview GetReview(string id, int userid)
        {
            ModelReview model = new ModelReview();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,");
            strsql.Append("  (select top 1 P_VotePerson.P_NameStatus from P_VotePerson where P_VotePerson.P_VoteUserID='" + userid + @"' and P_VotePerson.P_ActivityId='" + id + @"') as namestatus,");
            strsql.Append(" (select user_name from dt_users where dt_users.id=P_ReviewActivity.P_CreateUser) as createuser, ");
            strsql.Append(" (CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24) as VARCHAR)+'天'+");
            strsql.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/3600- ");
            strsql.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24)*24 as VARCHAR)+'时'+ ");
            strsql.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%60))/60- ");
            strsql.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/60 as VARCHAR)+'分') as time, ");
            strsql.Append(" (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ActivityId='" + id + @"' ) as usercount  ");
            strsql.Append(" from P_ReviewActivity ");
            strsql.Append(" where P_ReviewActivity.P_EndTime>GETDATE() and P_ReviewActivity.P_Id='" + id + @"' and P_ReviewActivity.P_status=0 ");
            DataSet dd = DbHelperSQL.Query(strsql.ToString());
            if (dd.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<ModelReview> voteoptionmodel = new DataSetToModelHelper<ModelReview>();
                model = voteoptionmodel.FillToModel(dd.Tables[0].Rows[0]);
                StringBuilder str = new StringBuilder();
                str.Append("select P_Image.P_ImageUrl as imgurl,P_VoteOption.P_Option as optioncontent,");
                str.Append(" CASE WHEN (SELECT COUNT(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ByVoteUserId=P_VoteOption.P_Id ");
                str.Append("and P_VotePerson.P_VoteUserID='" + userid + @"')=0 THEN 0 ELSE 1 END as userselect ");
                str.Append(" from P_VoteOption ");
                str.Append(" LEFT JOIN P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='" + (int)ImageTypeEnum.评选活动 + @"' ");
                str.Append(" where P_VoteOption.P_ActivityId='" + id + @"' and P_VoteOption.P_status=0 ");
                DataSet dt = DbHelperSQL.Query(str.ToString());
                if (dt != null)
                {
                    DataSetToModelHelper<voteModelOpen> voteonmodel = new DataSetToModelHelper<voteModelOpen>();
                    model.options = voteonmodel.FillModel(dt);
                }
                else
                {
                    model.options = null;
                }
            }
            else
            {
                model = null;
            }
            return model;
        }

    }
    public class ReviewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string username { get; set; }
        public string endtime { get; set; }
        public int userfroum { get; set; }
        public int voteresult { get; set; }
    }
    public class ActivityModel
    {
        /// <summary>
        /// 活动的标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 活动的说明
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 选项的类型 0单选 1多选
        /// </summary>
        public int optiontype { get; set; }
        /// <summary>
        /// 选项结果 0 任何人可见 / 1 投票后可见
        /// </summary>
        public int voteresult { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endtime { get; set; }
        /// <summary>
        /// 选项集合
        /// </summary>
        public List<OptionModel> options { get; set; }
        /// <summary>
        /// 用户的id
        /// </summary>
        public int userid { get; set; }
    }
    public class OptionModel
    {
        /// <summary>
        /// 选项内容
        /// </summary>
        public string option { get; set; }
        public string imgname { get; set; }

    }
    public class ReviewActivionModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string time { get; set; }
        public int usercount { get; set; }
        public int voteresult { get; set; }
        public int optiontype { get; set; }
        public List<VoteOptionModel> options { get; set; }
    }
    public class VoteOptionModel
    {
        public string imgurlid { get; set; }
        public string imgurl { get; set; }
        public string optionid { get; set; }
        public string optioncontent { get; set; }
    }
    public class ReviewEndModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public DateTime endtime { get; set; }
        public int usercount { get; set; }
        public List<VoteOptionModel> options { get; set; }
    }
    public class ReviewActivionToModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string time { get; set; }
        public int usercount { get; set; }
        public int namestatus { get; set; }
        public List<VoteOptionToModel> options { get; set; }

    }
    public class VoteOptionToModel
    {
        public string imgurl { get; set; }
        public string optioncontent { get; set; }
        /// <summary>
        /// 投票的百分比
        /// </summary>
        public double schedcount { get; set; }
    }
    public class FroumCount
    {
        public int userid { get; set; }
        public string activionid { get; set; }
        public int namestatus { get; set; }
        public List<OptionValueModel> options { get; set; }
    }
    public class OptionValueModel
    {
        public string optionid { get; set; }
    }
    public class ModelToAction
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string endtime { get; set; }
        public int usercount { get; set; }
        public List<ModelAction> actionmodel { get; set; }
    }
    public class ModelAction
    {
        public string optioncontent { get; set; }
        public string imgurl { get; set; }
        public int playcount { get; set; }
    }
    public class ModelReview
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string time { get; set; }
        public int usercount { get; set; }
        public int namestatus { get; set; }
        public List<voteModelOpen> options { get; set; }
    }
    public class voteModelOpen
    {
        public string optioncontent { get; set; }
        public string imgurl { get; set; }
        public int userselect { get; set; }
    }

    public class PlayCountModel
    {
        public int playcount { get; set; }
    }
}

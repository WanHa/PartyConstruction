using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DTcms.DAL
{
    public class WorkLogS
    {
        /// <summary>
        /// 添加反馈
        /// </summary>
        /// <returns></returns>
        public Boolean AddFeedback(string logId, string userId, string feedback)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    

                    try
                    {
                        bool feedbackExists = FeedbackExists(logId, userId);

                        if (feedbackExists)
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(" update P_AuditingFeedback");
                            strSql.Append(" set P_AuditContent=@P_AuditContent,P_UpdateTime=@P_UpdateTime,P_UpdateUser=@P_UpdateUser");
                            strSql.Append(" where P_LogId=@P_LogId and P_Status != 1");

                            SqlParameter[] parameters = {
                                new SqlParameter("@P_AuditContent", SqlDbType.NText),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_LogId", SqlDbType.NVarChar,50)
                            };

                            parameters[0].Value = feedback;
                            parameters[1].Value = DateTime.Now;
                            parameters[2].Value = userId;
                            parameters[3].Value = logId;

                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        }
                        else
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(" insert into P_AuditingFeedback");
                            strSql.Append(" (P_Id,P_LogId,P_AuditContent,P_CreateTime,P_CreateUser,P_Status)");
                            strSql.Append(" values");
                            strSql.Append(" (@P_Id,@P_LogId,@P_AuditContent,@P_CreateTime,@P_CreateUser,@P_Status)");

                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_LogId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_AuditContent", SqlDbType.NText),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status", SqlDbType.Int)
                            };

                            string modelid = Guid.NewGuid().ToString("N");

                            parameters[0].Value = modelid;
                            parameters[1].Value = logId;
                            parameters[2].Value = feedback;
                            parameters[3].Value = DateTime.Now;
                            parameters[4].Value = userId;
                            parameters[5].Value = 0;

                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        }

                        trans.Commit();

                        string sqlPer = String.Format(@"select P_WorkJournal.P_CreateUser from P_WorkJournal
                                where P_Id = '{0}'", logId);
                        int pushUserId = Convert.ToInt32(DbHelperSQL.GetSingle(sqlPer));
                        List<int> per = new List<int>();
                        per.Add(pushUserId);
                        PushMessageHelper.PushMessages(logId, "您收到一条工作汇报审核信息。", per,
                            Convert.ToInt32(userId), (int)PushTypeEnum.工作汇报审核);
                        return true;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }

        }

        /// <summary>
        /// 是否有反馈
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool FeedbackExists(string logId, string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_AuditingFeedback");
            strSql.Append(" where P_LogId='" + logId + @"' and P_Status != 1");
            strSql.Append(" and (P_CreateUser='" + userId + "' or P_UpdateUser='"+ userId +"')");
            return DbHelperSQL.Exists(strSql.ToString());
        }

        /// <summary>
        /// 获取WorkLog列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<WorkLogsModel> GetWorkLogList(int userid, int rows, int page)
        {
            //if (ispm(userid))
            //{

            //    StringBuilder strsql = new StringBuilder();
            //    strsql.Append("select P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_CreateTime, 120) as time,");
            //    strsql.Append(" (CASE WHEN (SELECT count(1) FROM P_AuditingFeedback");
            //    strsql.Append(" WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1");
            //    strsql.Append(" ELSE 0 END ) as feedback");
            //    strsql.Append(" from P_WorkJournal ");
            //    strsql.Append(" where P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser='" + userid + "' or P_WorkJournal.P_CreateUser ='" + userid + "'");
            //    DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_WorkJournal.P_CreateTime desc"));
            //    DataSetToModelHelper<WorkLogsModel> model = new DataSetToModelHelper<WorkLogsModel>();
            //    return model.FillModel(dt);

            //}
            //else
            //{
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_CreateTime, 120) as time,");
                strsql.Append(" (CASE WHEN (SELECT count(1) FROM P_AuditingFeedback");
                strsql.Append(" WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1");
                strsql.Append(" ELSE 0 END ) as feedback");
                strsql.Append(" from P_WorkJournal ");
                strsql.Append(" where P_WorkJournal.P_Status=0 and P_CreateUser='" + userid + "'");
                DataSet du = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_WorkJournal.P_CreateTime desc"));
                DataSetToModelHelper<WorkLogsModel> model = new DataSetToModelHelper<WorkLogsModel>();
                return model.FillModel(du);
            //}

        }

        //1.查询是否为书记
        public bool ispm(int userid)
        {
            StringBuilder strsql = new StringBuilder();
            bool ispm = false;
            string sql = "select * from dt_user_groups where dt_user_groups.manager_id ='" + userid + "'";
            strsql.Append(sql);


            DataTable da = DbHelperSQL.Query(strsql.ToString()).Tables[0];
            if (da.Rows.Count > 0)
            {
                ispm = true;
            }
            return ispm;

        }

        /// <summary>
        /// 获取工作日志详情接口 plus
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public WorkLogDetailModel GetWorkLogDetail(int userid, string id)
        {

            if (ispm(userid))
            {
                WorkLogDetailModel model = new WorkLogDetailModel();

                StringBuilder strsql = new StringBuilder();
                strsql.Append("select  P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) as time,P_WorkJournal.P_Content as content ");
                strsql.Append(" ,CONVERT(varchar(100),P_AuditingFeedback.P_CreateTime, 120) as feedbacktime,isnull(P_AuditingFeedback.P_AuditContent,'') as auditcontent");
                strsql.Append(" from P_WorkJournal ");
                strsql.Append(" left join P_AuditingFeedback on P_AuditingFeedback.P_LogId = P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1");
                strsql.Append(" where P_WorkJournal.P_Status=0 and (P_WorkJournal.P_SendUser='" + userid + "'or P_WorkJournal.P_CreateUser ='" + userid + "')and P_WorkJournal.P_Id='" + id + "' ");

                DataSet ds = DbHelperSQL.Query(strsql.ToString());

                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<WorkLogDetailModel> work = new DataSetToModelHelper<WorkLogDetailModel>();
                    model = work.FillToModel(ds.Tables[0].Rows[0]);

                    StringBuilder str = new StringBuilder();
                    str.Append(" select P_Image.P_ImageUrl as imgurl from P_Image where P_Image.P_ImageId='" + id + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.工作日志 + @"' ");
                    DataSet dt = DbHelperSQL.Query(str.ToString());
                    if (dt.Tables[0].Rows.Count != 0)
                    {
                        DataSetToModelHelper<DTcms.Model.WebApiModel.ImageModel> ima = new DataSetToModelHelper<DTcms.Model.WebApiModel.ImageModel>();
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
            else
            {
                WorkLogDetailModel model = new WorkLogDetailModel();

                StringBuilder strsql = new StringBuilder();
                strsql.Append("select  P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) as time,P_WorkJournal.P_Content as content ");
                strsql.Append(" ,CONVERT(varchar(100),P_AuditingFeedback.P_CreateTime, 120) as feedbacktime,isnull(P_AuditingFeedback.P_AuditContent,'') as auditcontent");
                strsql.Append(" from P_WorkJournal ");
                strsql.Append(" left join P_AuditingFeedback on P_AuditingFeedback.P_LogId = P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1");
                strsql.Append(" where P_WorkJournal.P_Status=0 and P_WorkJournal.P_CreateUser='" + userid + "'and P_WorkJournal.P_Id='" + id + "'");

                DataSet ds = DbHelperSQL.Query(strsql.ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<WorkLogDetailModel> work = new DataSetToModelHelper<WorkLogDetailModel>();
                    model = work.FillToModel(ds.Tables[0].Rows[0]);
                    StringBuilder str = new StringBuilder();
                    str.Append(" select P_Image.P_ImageUrl as imgurl from P_Image where P_Image.P_ImageId='" + id + @"' and P_Image.P_ImageType='" + (int)ImageTypeEnum.工作日志 + @"' ");
                    DataSet dt = DbHelperSQL.Query(str.ToString());
                    if (dt.Tables[0].Rows.Count != 0)
                    {
                        DataSetToModelHelper<DTcms.Model.WebApiModel.ImageModel> ima = new DataSetToModelHelper<DTcms.Model.WebApiModel.ImageModel>();
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



    }
}

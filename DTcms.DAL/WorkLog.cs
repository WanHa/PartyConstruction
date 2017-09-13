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
    public class WorkLog
    {
        /// <summary>
        /// 通过用户id获取 组织id name 管理者ID name
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        //public ManagerModel GetManager(int userid)
        //{
        //    StringBuilder strsql = new StringBuilder();
        //    strsql.Append("select dt_user_groups.manager as manager,dt_user_groups.title as title,dt_user_groups.id as group_id, ");
        //    strsql.Append(" (select dt_users.id  from dt_users where dt_user_groups.manager=dt_users.user_name) as userid,");
        //    strsql.Append("(select dt_users.user_name  from dt_users where dt_user_groups.manager=dt_users.user_name) as username,");
        //    strsql.Append("(select dt_users.telphone  from dt_users where dt_user_groups.manager=dt_users.user_name) as telphone");
        //    strsql.Append(" from dt_user_groups");
        //    strsql.Append(" LEFT JOIN dt_users on dt_users.group_id=dt_user_groups.id ");
        //    strsql.Append(" where dt_users.id=" + userid + @"");

        //    DataSet ds = DbHelperSQL.Query(strsql.ToString());
        //    DataSetToModelHelper<ManagerModel> model = new DataSetToModelHelper<ManagerModel>();
        //    if (ds.Tables[0].Rows[0] != null)
        //    {
        //        return model.FillToModel(ds.Tables[0].Rows[0])
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}
        /// <summary>
        /// 添加工作日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean GetWorkLog(WorkLogModel model)
        {
            //1.创建人 -  创建人的组织信息 --- 组织管理者 -- 
            //2. 组织管理者 = 接收人

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        leader managerid = new leader();
                        StringBuilder strSql = new StringBuilder();
                        string modelid = Guid.NewGuid().ToString("N");
                        if (ispm(model.userid))
                        {

                            StringBuilder strSql2 = new StringBuilder();
                            strSql2.Append(" select topgroup.manager_id as managerid from dt_user_groups");
                            strSql2.Append(" left join dt_user_groups topgroup on topgroup.id = dt_user_groups.pid");
                            strSql2.Append(" where dt_user_groups.manager_id = '" + model.userid + "'");
                            DataSet ds = DbHelperSQL.Query(strSql2.ToString());
                            DataSetToModelHelper<leader> dl = new DataSetToModelHelper<leader>();
                            managerid = dl.FillToModel(ds.Tables[0].Rows[0]);
                            strSql.Append("insert into P_WorkJournal(");
                            strSql.Append("P_Id,P_Title,P_TypeId,P_Content,P_CreateTime,P_CreateUser,P_Status,P_SendUser)");
                            strSql.Append(" values (");
                            strSql.Append("@P_Id,@P_Title,@P_TypeId,@P_Content,@P_CreateTime,@P_CreateUser,@P_Status,@P_SendUser)");
                            strSql.Append(";select @@IDENTITY");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Title", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_TypeId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NText),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                 new SqlParameter("@P_SendUser", SqlDbType.NVarChar,100)
                               };

                            parameters[0].Value = modelid;
                            parameters[1].Value = model.title;
                            parameters[2].Value = model.typeid;
                            parameters[3].Value = model.content;
                            parameters[4].Value = DateTime.Now;
                            parameters[5].Value = model.userid;
                            parameters[6].Value = 0;
                            parameters[7].Value = managerid.managerid;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务


                        }
                        else
                        {
                            UserGroupHelper help = new UserGroupHelper();
                            StringBuilder str = new StringBuilder();
                            int mingroupid = help.GetUserMinimumGroupId(model.userid);
                            StringBuilder strsql = new StringBuilder();
                            strsql.Append(" select dt_user_groups.manager_id as managerid from dt_user_groups where dt_user_groups.id=" + mingroupid + "");
                            DataSet ds = DbHelperSQL.Query(strsql.ToString());
                            DataSetToModelHelper<leader> ll = new DataSetToModelHelper<leader>();
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                managerid = ll.FillToModel(ds.Tables[0].Rows[0]);
                            }
                            else
                            {
                                managerid.managerid = -1;
                            }
                            strSql.Append("insert into P_WorkJournal(");
                            //strSql.Append("select manager_id from dt_user_groups where dt_user_groups.id = (select group_id from dt_users where '" + userid + "' = dt_users.id)");
                            strSql.Append("P_Id,P_Title,P_TypeId,P_Content,P_CreateTime,P_CreateUser,P_Status,P_SendUser)");
                            strSql.Append(" values (");
                            strSql.Append("@P_Id,@P_Title,@P_TypeId,@P_Content,@P_CreateTime,@P_CreateUser,@P_Status,@P_SendUser)");
                            strSql.Append(";select @@IDENTITY");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Title", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_TypeId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NText),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                 new SqlParameter("@P_SendUser", SqlDbType.NVarChar,100)
                               };
                            parameters[0].Value = modelid;
                            parameters[1].Value = model.title;
                            parameters[2].Value = model.typeid;
                            parameters[3].Value = model.content;
                            parameters[4].Value = DateTime.Now;
                            parameters[5].Value = model.userid;
                            parameters[6].Value = 0;
                            parameters[7].Value = managerid.managerid;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务

                            //}
                            //else
                            //{
                            //    managerid = null;
                            //}
                        }
                        if (model.imageurl != null)
                        {
                            foreach (var item in model.imageurl)
                            {
                                QiNiuHelper qiniu = new QiNiuHelper();
                                StringBuilder str = new StringBuilder();
                                String picture = qiniu.GetQiNiuFileUrl(item.picname);
                                str.Append("insert into P_Image(");
                                str.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status,P_ImageType )");
                                str.Append(" values (");
                                str.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_ImageType )");
                                str.Append(";select @@IDENTITY");
                                SqlParameter[] sql = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ImageId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_ImageType",SqlDbType.NVarChar,100),
                               };


                                sql[0].Value = Guid.NewGuid().ToString("N");
                                sql[1].Value = modelid;
                                sql[2].Value = picture;
                                //sql[3].Value = item.imgurl;
                                sql[3].Value = DateTime.Now;
                                sql[4].Value = model.userid;
                                sql[5].Value = 0;
                                sql[6].Value = (int)ImageTypeEnum.工作日志;
                                object oo = DbHelperSQL.GetSingle(conn, trans, str.ToString(), sql); //带事务
                            }
                        }
                        //int userid = model.userid;
                        //// 根据userid ---组织信息 ----managerid -


                        trans.Commit();

                        if (managerid.managerid > 0) {
                            List<int> per = new List<int>();
                            per.Add(managerid.managerid);
                            PushMessageHelper.PushMessages(modelid, "您收到一条工作汇报信息。", per, model.userid, (int)PushTypeEnum.工作汇报);
                        }
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

        public class leader
        {
            public int managerid { get; set; }
            public int mingroupid { get; set; }
        }

        public DataSet GetType(int userid)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select p_id as id,p_type as type from P_WorkJournalType order by P_CreateTime asc");
            DataSet ds = DbHelperSQL.Query(strsql.ToString());
            return ds;
        }

        /// <summary>
        /// 获取WorkLog列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<WKmodel> GetWorkLogList(int userid, int rows, int page)
        {
            if (ispm(userid))
            {

                StringBuilder strsql = new StringBuilder();
                strsql.Append("select P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_CreateTime, 120) as time");
                strsql.Append(" from P_WorkJournal ");
                strsql.Append(" where P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser='" + userid + "' or P_WorkJournal.P_CreateUser ='" + userid + "'");
                DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_WorkJournal.P_CreateTime"));
                DataSetToModelHelper<WKmodel> model = new DataSetToModelHelper<WKmodel>();
                return model.FillModel(dt);

            }
            else
            {
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_CreateTime, 120) as time");
                strsql.Append(" from P_WorkJournal ");
                strsql.Append(" where P_WorkJournal.P_Status=0 and P_CreateUser='" + userid + "'");
                DataSet du = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_WorkJournal.P_CreateTime"));
                DataSetToModelHelper<WKmodel> model = new DataSetToModelHelper<WKmodel>();
                return model.FillModel(du);
            }



        }

        /// <summary>
        /// 获取工作日志详情接口 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailWorkLogModel DetailWorkLog(int userid, string id)
        {

            if (ispm(userid))
            {
                DetailWorkLogModel model = new DetailWorkLogModel();
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select  P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_CreateTime, 120) as time,P_WorkJournal.P_Content as content ");
                strsql.Append(" from P_WorkJournal ");
                strsql.Append(" where P_WorkJournal.P_Status=0 and (P_WorkJournal.P_SendUser='" + userid + "'or P_WorkJournal.P_CreateUser ='" + userid + "')and P_WorkJournal.P_Id='" + id + "' ");
                DataSet ds = DbHelperSQL.Query(strsql.ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<DetailWorkLogModel> work = new DataSetToModelHelper<DetailWorkLogModel>();
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
                DetailWorkLogModel model = new DetailWorkLogModel();
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select  P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,CONVERT(varchar(100), P_CreateTime, 120) as time,P_WorkJournal.P_Content as content ");
                strsql.Append(" from P_WorkJournal ");            
                strsql.Append(" where P_WorkJournal.P_Status=0 and P_CreateUser='" + userid + "'and P_WorkJournal.P_Id='" + id + "'");
                DataSet ds = DbHelperSQL.Query(strsql.ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<DetailWorkLogModel> work = new DataSetToModelHelper<DetailWorkLogModel>();
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



        /// <summary>
        /// 本月/周/日更新工作日志
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<WorkUpdateModel> monthupdate(int userid, int rows, int page, int type)
        {
            if (type == 0)
            {
                /// <summary>
                /// 判断日期
                /// </summary>
                /// <param name="userid"></param>
                /// <returns></returns>
                StringBuilder strsql = new StringBuilder();
                DateTime dt = DateTime.Now;

                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初  
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末  

                string startMonthSql = startMonth.ToString("yyyy/MM/dd HH:mm:ss");
                string endMonthSql= endMonth.ToString("yyyy/MM/dd HH:mm:ss");
                //是否为书记
                if (ispm(userid))
                {
                    string sql = String.Format(@"SELECT 
	                      P_WorkJournal.P_Id as id,P_WorkJournal.P_Title AS title , P_WorkJournal.P_Content AS content ,CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) AS time ,dt_users.user_name AS name , P_Image.P_ImageUrl AS avatar 
                    ,(CASE WHEN (SELECT count(1) FROM P_AuditingFeedback 
		                         WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1
                                 ELSE 0 END ) as feedback
                    FROM P_WorkJournal 
                    LEFT JOIN dt_users ON dt_users.id = P_WorkJournal.P_CreateUser 
                    LEFT JOIN P_Image ON P_Image.P_ImageId=dt_users.id and P_ImageType='20011'
                    WHERE
	                    P_WorkJournal.P_CreateTime BETWEEN '{0}' 
                    AND '{1}'
                    AND P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser = '{2}'", startMonthSql, endMonthSql, userid);
                    //AND P_WorkJournal.P_Status = 0 and(P_WorkJournal.P_SendUser = '{2}'or P_WorkJournal.P_CreateUser = '" + userid + "')", startMonthSql, endMonthSql, userid);

                    DataSet wm = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_WorkJournal.P_CreateTime desc"));
                    DataSetToModelHelper<WorkUpdateModel> model = new DataSetToModelHelper<WorkUpdateModel>();

                    return model.FillModel(wm);

                }
                else
                {
                    string sql = String.Format(@"select P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,P_WorkJournal.P_Content as content,CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) as time,dt_users.user_name as name, P_Image.P_ImageUrl as avatar
                         ,(CASE WHEN (SELECT count(1) FROM P_AuditingFeedback 
		                         WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1
                                 ELSE 0 END ) as feedback
                         from P_WorkJournal
                         LEFT JOIN dt_users on dt_users.id=P_WorkJournal.P_CreateUser
                         LEFT JOIN P_Image ON P_Image.P_ImageId=dt_users.id and P_ImageType='20011'
                         where P_WorkJournal.P_CreateTime BETWEEN '{0}' AND '{1}' and P_WorkJournal.P_Status=0 and P_WorkJournal.P_CreateUser='{2}' ", startMonthSql, endMonthSql, userid);

                    DataSet wm2 = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_WorkJournal.P_CreateTime desc"));
                    DataSetToModelHelper<WorkUpdateModel> model = new DataSetToModelHelper<WorkUpdateModel>();
                    return model.FillModel(wm2);
                }

            }

            /// <summary>
            /// 本周更新列表
            /// </summary>
            /// <param name="userid"></param>
            /// <param name="rows"></param>
            /// <param name="page"></param>
            /// <returns></returns>
            else if (type == 1)
            {
                StringBuilder strsql = new StringBuilder();
                DateTime dt = DateTime.Now;

                DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
                DateTime endWeek = startWeek.AddDays(6);  //本周周日  

                string startWeekSql = startWeek.ToString("yyyy/MM/dd HH:mm:ss");
                string endWeekSql = endWeek.ToString("yyyy/MM/dd HH:mm:ss");

                //是否为书记账户
                if (ispm(userid))
                {


                    string sql = String.Format(@"SELECT
	                        P_WorkJournal.P_Id as id,P_WorkJournal.P_Title AS title,
	                        P_WorkJournal.P_Content AS content,
	                        CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) AS time,
	                        dt_users.user_name AS name,
	                        P_Image.P_ImageUrl AS avatar
                        ,(CASE WHEN (SELECT count(1) FROM P_AuditingFeedback 
		                         WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1
                                 ELSE 0 END ) as feedback
                        FROM P_WorkJournal
                        LEFT JOIN dt_users ON dt_users.id = P_WorkJournal.P_CreateUser   
                        LEFT JOIN P_Image ON P_Image.P_ImageId=dt_users.id and P_ImageType='20011'
                        WHERE
	                        P_WorkJournal.P_CreateTime BETWEEN '{0}'
                        AND '{1}'and P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser ='{2}'", startWeekSql, endWeekSql, userid);
                    DataSet wm = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_WorkJournal.P_CreateTime desc"));
                    DataSetToModelHelper<WorkUpdateModel> model = new DataSetToModelHelper<WorkUpdateModel>();
                    return model.FillModel(wm);

                }
                else
                {
                    string sql = String.Format(@"select P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,P_WorkJournal.P_Content as content,CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) as time,dt_users.user_name as name,P_Image.P_ImageUrl as avatar
                            ,(CASE WHEN (SELECT count(1) FROM P_AuditingFeedback 
		                         WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1
                                 ELSE 0 END ) as feedback
                         from P_WorkJournal
                         LEFT JOIN dt_users on dt_users.id=P_WorkJournal.P_CreateUser
                         LEFT JOIN P_Image ON P_Image.P_ImageId=dt_users.id and P_ImageType='20011'
                         where P_WorkJournal.P_CreateTime BETWEEN '{0}' AND '{1}'  and P_WorkJournal.P_Status=0 and P_WorkJournal.P_CreateUser='{2}' ", startWeekSql, endWeekSql, userid);

                    DataSet wm2 = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_WorkJournal.P_CreateTime desc"));
                    DataSetToModelHelper<WorkUpdateModel> model = new DataSetToModelHelper<WorkUpdateModel>();
                    return model.FillModel(wm2);
                }

            }

            /// <summary>
            /// 今日更新列表
            /// </summary>
            /// <param name="userid"></param>
            /// <param name="rows"></param>
            /// <param name="page"></param>
            /// <returns></returns>
            else if (type == 2)
            {

                StringBuilder strsql = new StringBuilder();
                DateTime dt = DateTime.Now;


                DateTime start = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");//今日
                DateTime end = DateTime.Now;

                string startSql = start.ToString("yyyy/MM/dd HH:mm:ss");
                string endSql = end.ToString("yyyy/MM/dd HH:mm:ss");

                //是否为书记账户
                if (ispm(userid))
                {

                    string sql = String.Format(@"SELECT
	                        P_WorkJournal.P_Id as id,P_WorkJournal.P_Title AS title,
	                        P_WorkJournal.P_Content AS content,
	                        CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) AS TIME,
	                        dt_users.user_name AS name,
	                        P_Image.P_ImageUrl AS avatar
                        ,(CASE WHEN (SELECT count(1) FROM P_AuditingFeedback 
		                         WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1
                                 ELSE 0 END ) as feedback
                        FROM P_WorkJournal
                        LEFT JOIN dt_users ON dt_users.id = P_WorkJournal.P_CreateUser
                        LEFT JOIN P_Image ON P_Image.P_ImageId=dt_users.id and P_ImageType='20011'
                      
                        WHERE 
	                        P_WorkJournal.P_CreateTime BETWEEN '{0}'
                        AND '{1}'and P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser='{2}'", startSql, endSql, userid);

                    DataSet wm = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_WorkJournal.P_CreateTime desc"));
                    DataSetToModelHelper<WorkUpdateModel> model = new DataSetToModelHelper<WorkUpdateModel>();
                    return model.FillModel(wm);

                }
                else
                {
                    string sql = String.Format(@"select P_WorkJournal.P_Id as id,P_WorkJournal.P_Title as title,P_WorkJournal.P_Content as content,CONVERT(varchar(100), P_WorkJournal.P_CreateTime, 120) as time,dt_users.user_name as name,P_Image.P_ImageUrl as avatar
                         ,(CASE WHEN (SELECT count(1) FROM P_AuditingFeedback 
		                         WHERE P_AuditingFeedback.P_LogId=P_WorkJournal.P_Id AND P_AuditingFeedback.P_Status !=1)>0 THEN 1
                                 ELSE 0 END ) as feedback
                         from P_WorkJournal
                         LEFT JOIN dt_users on dt_users.id=P_WorkJournal.P_CreateUser
                         LEFT JOIN P_Image ON P_Image.P_ImageId=dt_users.id and P_ImageType='20011'
                         where P_WorkJournal.P_CreateTime BETWEEN '{0}' AND '{1}'  and P_WorkJournal.P_Status=0 and P_WorkJournal.P_CreateUser='{2}' ", startSql, endSql, userid);

                    DataSet wm2 = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_WorkJournal.P_CreateTime desc"));
                    DataSetToModelHelper<WorkUpdateModel> model = new DataSetToModelHelper<WorkUpdateModel>();
                    return model.FillModel(wm2);
                }



            }
            else return null;

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

        public class WKmodel
        {
            private string _time;
            public string time
            {
                get
                {
                    return DateTime.Parse(_time == null ? "" : _time).ToString("yyyy年MM月dd日");
                }
                set
                {
                    _time = value;
                }
            }
            public string id { get; set; }

            public string title { get; set; }



            //public DateTime createtime { get; set; }

        }
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public DetailWorkLogModel DataRowToModel(DataRow row)
        {
            DetailWorkLogModel model = new DetailWorkLogModel();
            if (row != null)
            {
                #region 主表信息======================
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["title"] != null && row["title"].ToString() != "")
                {
                    model.title = row["title"].ToString();
                }
                if (row["content"] != null && row["content"].ToString() != "")
                {
                    model.content = row["content"].ToString();
                }
                if (row["time"] != null && row["time"].ToString() != "")
                {
                    model.time = row["time"].ToString();
                }

                #endregion

            }
            return model;
        }
        /// <summary>
        /// 是否存在该记录（根据ID）
        /// </summary>
        public bool Exists(string mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_users");
            strSql.Append(" where mobile='" + mobile + @"' ");
            return DbHelperSQL.Exists(strSql.ToString());
        }



        /// <summary>
        /// 标题本月/周/日更新多少篇
        /// </summary>      
        public List<Workcount> GetlogCont(int userid)
        {
            List<Workcount> wc = new List<Workcount>();
           
            //1.判断角色身份

            //2.如果是书记（三种时间类型）
            //1.是书记 -- 查询接收人是userid指定日期提交的数据数量
            if (ispm(userid))
            {
                DateTime dt = DateTime.Now;
                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初  
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末  
                DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
                DateTime endWeek = startWeek.AddDays(6);  //本周周日  
                //DateTime start = DateTime.Parse(DateTime.Today.ToString("yyyy-HH-mm")+" 00:00:00");//今日
                DateTime start = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");//今日
                DateTime end = DateTime.Now;

                //本月
                string sql = String.Format(@"SELECT
	                       count(P_WorkJournal.P_id) as count
                        FROM
	                        P_WorkJournal
                        LEFT JOIN dt_users ON dt_users.id = P_WorkJournal.P_CreateUser
                        
                        WHERE
	                        P_WorkJournal.P_CreateTime BETWEEN '{0}'
                        AND '{1}'and P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser='{2}'", startMonth, endMonth, userid);
                DataSet wm = DbHelperSQL.Query(sql);
                Workcount wk1 = new Workcount();

                if (wm.Tables[0].Rows.Count != 0)
                {

                    DataTable tb = wm.Tables[0];
                    DataRow row = tb.Rows[0];
                    Console.WriteLine(row["count"]);
                    wk1.count = int.Parse(wm.Tables[0].Rows[0][0].ToString());
                    wk1.type = 0;
                    wc.Add(wk1);
                }
                else
                {
                    wk1.count = 0;
                    wk1.type = 0;
                    wc.Add(wk1);
                }
            
               
                //本周
                string sq2 = String.Format(@"SELECT
	                       count(P_WorkJournal.P_id) as count
                        FROM
	                        P_WorkJournal
                        LEFT JOIN dt_users ON dt_users.id = P_WorkJournal.P_CreateUser
                       
                        WHERE
	                        P_WorkJournal.P_CreateTime BETWEEN '{0}'
                        AND '{1}'and P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser='{2}'", startWeek, endWeek, userid);
                DataSet wm2 = DbHelperSQL.Query(sq2);
                Workcount wk2 = new Workcount();
                if (wm2.Tables[0].Rows.Count != 0)
                {
                    DataTable tb1 = wm2.Tables[0];
                    DataRow row1 = tb1.Rows[0];
                    Console.WriteLine(row1["count"]);
                    wk2.count = int.Parse(wm2.Tables[0].Rows[0][0].ToString());
                    wk2.type = 1;
                    wc.Add(wk2);
                }
                else
                {
                    wk2.count = 0;
                    wk2.type = 1;
                    wc.Add(wk2);
                }
          
               

                //今日
                string sq3 = String.Format(@"SELECT
	                       count(P_WorkJournal.P_id) as count
                        FROM
	                        P_WorkJournal
                        LEFT JOIN dt_users ON dt_users.id = P_WorkJournal.P_CreateUser
                        
                        WHERE
	                        P_WorkJournal.P_CreateTime BETWEEN '{0}'
                        AND '{1}'and P_WorkJournal.P_Status=0 and P_WorkJournal.P_SendUser='{2}'", start, end, userid);
                DataSet wm3 = DbHelperSQL.Query(sq3);
                Workcount wk3 = new Workcount();

                if (wm3.Tables[0].Rows.Count != 0)
                {
                    DataTable tb = wm3.Tables[0];
                    DataRow row = tb.Rows[0];
                    Console.WriteLine(row["count"]);
                    wk3.count = int.Parse(wm3.Tables[0].Rows[0][0].ToString());
                    wk3.type = 2;
                    wc.Add(wk3);
                }
                else
                {
                    wk3.count = 0;
                    wk3.type = 2;
                    wc.Add(wk3);
                }
            }




            //2.如果非书记 -- 查询创建人是userid创建的制定人气提交的数据数量
            else 
            {
                DateTime dt = DateTime.Now;
                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初  
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末  
                DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
                DateTime endWeek = startWeek.AddDays(6);  //本周周日  
                //DateTime start = DateTime.Parse(DateTime.Today.ToString("yyyy-HH-mm")+" 00:00:00");//今日
                DateTime start = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");//今日
                DateTime end = DateTime.Now;


                StringBuilder strsql = new StringBuilder();
                //本月
                string sql = String.Format(@"select count(P_WorkJournal.P_id) as count
                        from P_WorkJournal
                         LEFT JOIN dt_users on dt_users.id=P_WorkJournal.P_CreateUser
                            where P_WorkJournal.P_CreateTime BETWEEN '{0}' AND '{1}'  and P_WorkJournal.P_Status=0 and P_CreateUser='{2}' ", startMonth, endMonth, userid);

                DataSet wm4 = DbHelperSQL.Query(sql);
                Workcount wk4 = new Workcount();
                if (wm4.Tables[0].Rows.Count != 0)
                {
                    DataTable tb = wm4.Tables[0];
                    DataRow row = tb.Rows[0];
                    Console.WriteLine(row["count"]);
                    wk4.count = int.Parse(wm4.Tables[0].Rows[0][0].ToString());
                    wk4.type = 0;
                    wc.Add(wk4);
                }
                else
                {
                    wk4.count = 0;
                    wk4.type = 0;
                }
            

          
                
                
                //m.typeid = 0;
                //m.count = wm.Tables[0].Rows.Count;
                //list.Add(m);
                //本周
                string sql2 = String.Format(@"select  count(P_WorkJournal.P_id) as count
                        from P_WorkJournal
                         LEFT JOIN dt_users on dt_users.id=P_WorkJournal.P_CreateUser
                            where P_WorkJournal.P_CreateTime BETWEEN '{0}' AND '{1}'  and P_WorkJournal.P_Status=0 and P_CreateUser='{2}' ", startWeek, endWeek, userid);

                DataSet wm5 = DbHelperSQL.Query(sql2);
                Workcount wk5 = new Workcount();

                if (wm5.Tables[0].Rows.Count != 0)
                {
                    DataTable tb = wm5.Tables[0];
                    DataRow row = tb.Rows[0];
                    Console.WriteLine(row["count"]);
                    wk5.count = int.Parse(wm5.Tables[0].Rows[0][0].ToString());
                    wk5.type = 1;
                    wc.Add(wk5);
                }
                else
                {
                    wk5.type = 1;
                    wk5.count = 0;
                    wc.Add(wk5);
                }


                
                //今日
                string sql3 = String.Format(@"select count(P_WorkJournal.P_id) as count
                        from P_WorkJournal
                         LEFT JOIN dt_users on dt_users.id=P_WorkJournal.P_CreateUser
                         where P_WorkJournal.P_CreateTime BETWEEN '{0}' AND '{1}'  and P_WorkJournal.P_Status=0 and P_WorkJournal.P_CreateUser='{2}' ", start, end, userid);

                DataSet wm6 = DbHelperSQL.Query(sql3);
                Workcount wk6 = new Workcount();
                if (wm6.Tables[0].Rows.Count != 0)
                {
                    DataTable tb = wm6.Tables[0];
                    DataRow row = tb.Rows[0];
                    Console.WriteLine(row["count"]);
                    wk6.count = int.Parse(wm6.Tables[0].Rows[0][0].ToString());
                    wk6.type = 2;
                    wc.Add(wk6);
                }
                else
                {
                    wk6.count = 0;
                    wk6.type = 2;
                    wc.Add(wk6);
                }
            }

            //list{
            //类型 -- 今日
            //数量 --  10
            //类型 -- 本周
            //数量 --  10
            //类型 -- 本月
            //数量 --  10
            //     }
            return wc;

           

        }





    }

    public class Workcount
    {

        public int count;
        public int type;
         
    }




}
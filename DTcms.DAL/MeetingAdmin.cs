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
using System.Threading.Tasks;

namespace DTcms.DAL
{

    public class MeetingAdmin
    {
        private UserGroupHelper usergroup = new UserGroupHelper();

        #region ==================手机APP接口业务处理
        /// <summary>
        /// 会议管理的报名列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<meeteAdmin> GetMetingAdminList(int userid, int rows, int page)
        {
            List<meeteAdmin> model = new List<meeteAdmin>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_MeetingAdmin.P_Id as id,P_MeetingAdmin.P_Title as title,CONVERT(varchar(100), P_MeetingAdmin.P_StartTime,120) as starttime,");
            strsql.Append("CASE when P_MeetingAdmin.P_PeopleAmount>COUNT(P_MeetingAdminSublist.P_Id) THEN 0 ELSE 1 END as astatus");
            strsql.Append(" from P_MeetingAdmin LEFT JOIN P_MeetingAdminSublist on P_MeetingAdmin.P_Id=P_MeetingAdminSublist.P_MeeID ");
            strsql.Append(" where P_MeetingAdmin.P_StartTime>GETDATE() and P_MeetingAdmin.P_Status=0 ");
            strsql.Append(" GROUP BY P_MeetingAdmin.P_Id,P_MeetingAdmin.P_Title ,P_MeetingAdmin.P_StartTime,P_MeetingAdmin.P_PeopleAmount,P_MeetingAdmin.P_CreateTime");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_MeetingAdmin.P_StartTime desc"));
            if (dt != null)
            {
                DataSetToModelHelper<meeteAdmin> mete = new DataSetToModelHelper<meeteAdmin>();
                model = mete.FillModel(dt);
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 查看报名列表的详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DetailMeeting DeatilAdmin(string id, int userid)
        {
            DetailMeeting model = new DetailMeeting();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_MeetingAdmin.P_Id as id,P_MeetingAdmin.P_Title as title,CONVERT(varchar(100), P_MeetingAdmin.P_StartTime,120) as starttime");
            strsql.Append(",P_MeetingAdmin.P_MeePlace as place,P_MeetingAdmin.P_MeeContent as content,");
            strsql.Append("(select COUNT(P_MeetingAdminSublist.P_Id) from P_MeetingAdminSublist where P_MeetingAdminSublist.P_MeeID=P_MeetingAdmin.P_Id ) as usercount,");
            strsql.Append("CASE WHEN(select P_MeetingAdminSublist.P_Id from P_MeetingAdminSublist ");
            strsql.Append(" where P_MeetingAdminSublist.P_MeeID=P_MeetingAdmin.P_Id and P_MeetingAdminSublist.P_UserId='" + userid + "') is null THEN 0 ELSE 1 END as userstatus,");
            strsql.Append("CASE when P_MeetingAdmin.P_PeopleAmount>(select COUNT(P_MeetingAdminSublist.P_Id) ");
            strsql.Append("from P_MeetingAdminSublist where P_MeetingAdminSublist.P_MeeID=P_MeetingAdmin.P_Id ) THEN 0 ELSE 1 END as astatus ");
            strsql.Append(" from P_MeetingAdmin ");
            strsql.Append(" where  P_MeetingAdmin.P_Id='" + id + "' and  P_MeetingAdmin.P_Status=0 ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            if (dt.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<DetailMeeting> mete = new DataSetToModelHelper<DetailMeeting>();
                model = mete.FillToModel(dt.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 用户报名的接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean SelAdminSubmit(string id, int userid)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_MeetingAdminSublist(");
                        strSql.Append("P_ID,P_MeeID,P_UserId,P_SigninInfo,P_CreateTime,P_CreateUser,P_Status)");
                        strSql.Append(" values (");
                        strSql.Append("@P_ID,@P_MeeID,@P_UserId,@P_SigninInfo,@P_CreateTime,@P_CreateUser,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_MeeID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_SigninInfo", SqlDbType.Int),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4)
                               };

                        parameters[0].Value = Guid.NewGuid().ToString("N");
                        parameters[1].Value = id;
                        parameters[2].Value = userid;
                        parameters[3].Value = 0;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = userid;
                        parameters[6].Value = 0;
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
        /// 会议签到列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<StartEndAdmin> StartEndMeeting(int userid, int rows, int page)
        {
            List<StartEndAdmin> list = new List<StartEndAdmin>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_MeetingAdmin.P_Id as id,P_MeetingAdmin.P_Title as title,CONVERT(varchar(100), P_MeetingAdmin.P_EndTime,120) as endtime,P_MeetingAdmin.P_MeePlace as place ");
            strsql.Append(" from P_MeetingAdmin");
            strsql.Append(" LEFT JOIN P_MeetingAdminSublist on P_MeetingAdminSublist.P_MeeID=P_MeetingAdmin.P_Id ");
            //strsql.Append(" where (GETDATE() BETWEEN P_MeetingAdmin.P_StartTime and P_MeetingAdmin.P_EndTime ) ");
            strsql.Append("  where P_MeetingAdminSublist.P_UserId='" + userid + @"' and P_MeetingAdmin.P_Status=0 and P_MeetingAdminSublist.P_Status=0");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_MeetingAdmin.P_CreateTime"));
            DataSetToModelHelper<StartEndAdmin> mete = new DataSetToModelHelper<StartEndAdmin>();
            if (dt != null)
            {
                list = mete.FillModel(dt);
            }
            else
            {
                list = null;
            }
            return list;
        }
        /// <summary>
        /// 会议签到人数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MeetingCount GetMeetingAdmin(string id)
        {
            MeetingCount model = new MeetingCount();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select  count(P_MeetingAdminSublist.P_ID) as admincount,");
            strsql.Append(" (select COUNT(P_MeetingAdminSublist.P_ID) from P_MeetingAdminSublist ");
            strsql.Append(" where P_MeetingAdminSublist.P_SigninInfo=0 and  P_MeetingAdminSublist.P_MeeID='" + id + @"' and P_MeetingAdminSublist.P_Status=0) as notokcount, ");
            strsql.Append(" (select COUNT(P_MeetingAdminSublist.P_ID) from P_MeetingAdminSublist ");
            strsql.Append(" where   P_MeetingAdminSublist.P_MeeID='" + id + @"'  and P_MeetingAdminSublist.P_Status=0 and P_MeetingAdminSublist.P_SigninInfo=1 ) as okcount ");
            strsql.Append(" from P_MeetingAdminSublist ");
            strsql.Append(" where P_MeetingAdminSublist.P_MeeID='" + id + @"' and P_MeetingAdminSublist.P_Status=0 ");
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
            if (dt.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<MeetingCount> mete = new DataSetToModelHelper<MeetingCount>();
                model = mete.FillToModel(dt.Tables[0].Rows[0]);
            }
            return model;
        }
        /// <summary>
        /// 获得别人的状态 0 点击参会人员 / 1 已签到 / 2 未签到
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<InfoModel> GetInfoStatus(int type, int userid, string id, int rows, int page)
        {

            StringBuilder orderByStr = new StringBuilder();
            orderByStr.Append(@" case when dt_users.id = " + userid + " then 0 else 1 end,P_MeetingAdminSublist.P_UpdateTime");
            DataSetToModelHelper<InfoModel> info = new DataSetToModelHelper<InfoModel>();
            List<InfoModel> model = new List<InfoModel>();
            InfoModel infomodel = new InfoModel();
            if (type == 0)
            {
                StringBuilder sss = new StringBuilder();
                sss.Append("select * from P_MeetingAdminSublist LEFT JOIN dt_users on dt_users.id=P_MeetingAdminSublist.P_UserId where P_MeeID='" + id + @"' and P_MeetingAdminSublist.P_Status=0   ");
                DataSetToModelHelper<P_MeetingAdminSublist> sublist = new DataSetToModelHelper<P_MeetingAdminSublist>();
                DataSet dd = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sss.ToString(), orderByStr.ToString()));
                List<P_MeetingAdminSublist> sub = new List<P_MeetingAdminSublist>();
                if (dd.Tables[0].Rows.Count != 0)
                {
                    sub = sublist.FillModel(dd);
                    foreach (var item in sub)
                    {

                        int groupid = usergroup.GetUserMinimumGroupId(Convert.ToInt32(item.P_UserId));
                        StringBuilder str = new StringBuilder();
                        str.Append("select dt_users.user_name as username,CONVERT(varchar(100), P_MeetingAdminSublist.P_UpdateTime,120) as infotime,P_MeetingAdminSublist.P_MeeID as id,dt_users.id as userid,");
                        str.Append(" case WHEN P_MeetingAdminSublist.P_SigninInfo=0 then 0 ELSE 1 END as infostatus,dt_user_groups.title as organ,");
                        str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_MeetingAdminSublist.P_UserId AS VARCHAR)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar");
                        str.Append(" from P_MeetingAdminSublist ");
                        str.Append(" LEFT JOIN dt_users on dt_users.id=P_MeetingAdminSublist.P_UserId ");
                        str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + groupid + @" ");
                        str.Append(" LEFT JOIN P_MeetingAdmin ON P_MeetingAdmin.P_Id=P_MeetingAdminSublist.p_MeeId ");
                        str.Append(" where P_MeetingAdminSublist.P_Id='" + item.P_ID + @"'  ");
                        DataSet ds = DbHelperSQL.Query(str.ToString());
                        DataSetToModelHelper<InfoModel> infom = new DataSetToModelHelper<InfoModel>();
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            infomodel = infom.FillToModel(ds.Tables[0].Rows[0]);
                        }
                        else
                        {
                            infomodel = null;
                        }
                        model.Add(infomodel);
                    }
                }
                else
                {
                    sub = null;
                    model = null;
                }




            }
            if (type == 1)
            {
                StringBuilder sss = new StringBuilder();
                sss.Append("select * from P_MeetingAdminSublist LEFT JOIN dt_users on dt_users.id=P_MeetingAdminSublist.P_UserId where P_MeeID='" + id + @"' and P_MeetingAdminSublist.P_Status=0 and P_MeetingAdminSublist.P_SigninInfo=1  ");
                DataSetToModelHelper<P_MeetingAdminSublist> sublist = new DataSetToModelHelper<P_MeetingAdminSublist>();
                DataSet dd = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sss.ToString(), orderByStr.ToString()));
                List<P_MeetingAdminSublist> sub = new List<P_MeetingAdminSublist>();
                if (dd.Tables[0].Rows.Count != 0)
                {
                    sub = sublist.FillModel(dd);
                    foreach (var item in sub)
                    {
                        int groupid = usergroup.GetUserMinimumGroupId(Convert.ToInt32(item.P_UserId));
                        StringBuilder str = new StringBuilder();
                        str.Append("select dt_users.user_name as username,CONVERT(varchar(100), P_MeetingAdmin.P_EndTime,120) as infotime,P_MeetingAdminSublist.P_MeeID as id,dt_users.id as userid,");
                        str.Append(" case WHEN P_MeetingAdminSublist.P_SigninInfo=0 then 0 ELSE 1 END as infostatus,dt_user_groups.title as organ ,");
                        str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_MeetingAdminSublist.P_UserId AS VARCHAR)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar");
                        str.Append(" from P_MeetingAdminSublist ");
                        str.Append(" LEFT JOIN dt_users on dt_users.id=P_MeetingAdminSublist.P_UserId ");
                        str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + groupid + @" ");
                        str.Append(" LEFT JOIN P_MeetingAdmin ON P_MeetingAdmin.P_Id=P_MeetingAdminSublist.p_MeeId ");
                        str.Append(" where P_MeetingAdminSublist.P_Id='" + item.P_ID + @"' ");
                        DataSet ds = DbHelperSQL.Query(str.ToString());
                        DataSetToModelHelper<InfoModel> infom = new DataSetToModelHelper<InfoModel>();
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            infomodel = infom.FillToModel(ds.Tables[0].Rows[0]);
                        }
                        else
                        {
                            infomodel = null;
                        }
                        model.Add(infomodel);

                    }
                }
                else
                {
                    sub = null;
                    model = null;
                }



            }
            if (type == 2)
            {
                StringBuilder sss = new StringBuilder();
                sss.Append("select * from P_MeetingAdminSublist LEFT JOIN dt_users on dt_users.id=P_MeetingAdminSublist.P_UserId where P_MeeID='" + id + @"' and P_MeetingAdminSublist.P_Status=0 and P_MeetingAdminSublist.P_SigninInfo=0  ");
                DataSetToModelHelper<P_MeetingAdminSublist> sublist = new DataSetToModelHelper<P_MeetingAdminSublist>();
                DataSet dd = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sss.ToString(), orderByStr.ToString()));
                List<P_MeetingAdminSublist> sub = new List<P_MeetingAdminSublist>();
                if (dd.Tables[0].Rows.Count != 0)
                {
                    sub = sublist.FillModel(dd);
                    foreach (var item in sub)
                    {
                        int groupid = usergroup.GetUserMinimumGroupId(Convert.ToInt32(item.P_UserId));
                        StringBuilder str = new StringBuilder();
                        str.Append("select dt_users.user_name as username,CONVERT(varchar(100), P_MeetingAdmin.P_EndTime,120) as infotime,P_MeetingAdminSublist.P_MeeID as id,dt_users.id as userid,");
                        str.Append(" case WHEN P_MeetingAdminSublist.P_SigninInfo=0 then 0 ELSE 1 END as infostatus ,dt_user_groups.title as organ,");
                        str.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_MeetingAdminSublist.P_UserId AS VARCHAR)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar");
                        str.Append(" from P_MeetingAdminSublist ");
                        str.Append(" LEFT JOIN dt_users on dt_users.id=P_MeetingAdminSublist.P_UserId ");
                        str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id=" + groupid + @" ");
                        str.Append(" LEFT JOIN P_MeetingAdmin ON P_MeetingAdmin.P_Id=P_MeetingAdminSublist.p_MeeId ");
                        str.Append(" where P_MeetingAdminSublist.P_ID='" + item.P_ID + @"'  ");
                        DataSet ds = DbHelperSQL.Query(str.ToString());
                        DataSetToModelHelper<InfoModel> infom = new DataSetToModelHelper<InfoModel>();
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            infomodel = infom.FillToModel(ds.Tables[0].Rows[0]);
                        }
                        else
                        {
                            infomodel = null;
                        }
                        model.Add(infomodel);
                    }
                }
                else
                {
                    sub = null;
                    model = null;
                }


            }

            return model;

        }
        /// <summary>
        /// 用户签到的接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean UpdataInfo(int userid, string id)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update  P_MeetingAdminSublist set ");
                        strSql.Append("P_SigninInfo=@P_SigninInfo,P_UpdateTime=@P_UpdateTime,P_UpdateUser=@P_UpdateUser ");
                        strSql.Append(" where ");
                        strSql.Append(" P_MeeID=@P_MeeID and P_UserId=@P_UserId ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_SigninInfo", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_MeeID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,100)
                        };
                        parameters[0].Value = 1;
                        parameters[1].Value = DateTime.Now;
                        parameters[2].Value = userid;
                        parameters[3].Value = id;
                        parameters[4].Value = userid;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        trans.Commit();
                        string sqlPer = String.Format(@"select P_MeetingAdminSublist.P_UserId from P_MeetingAdminSublist
                            left join dt_user_groups on dt_user_groups.manager_id = P_MeetingAdminSublist.P_UserId
                            where P_MeetingAdminSublist.P_MeeID = '{0}'
                            and dt_user_groups.id is not NULL", id);
                        DataSet ds = DbHelperSQL.Query(sqlPer);
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                            List<int> per = new List<int>();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                per.Add(Convert.ToInt32(dr["P_UserId"].ToString()));
                            }
                            PushMessageHelper.PushMessages(id,"您收到一条会议签到信息.", per, userid, (int)PushTypeEnum.会议签到);
                        }
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
        /// 用户的会议签到列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<UserMeetingModel> GetUserMeetingList(int userid, int rows, int page)
        {
            List<UserMeetingModel> model = new List<UserMeetingModel>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("SELECT P_MeetingAdmin.P_Title as title ,CONVERT(varchar(100),P_MeetingAdmin.P_EndTime,120) as endtime,P_MeetingAdmin.P_MeePlace as place,");
            strsql.Append("P_MeetingAdminSublist.P_SigninInfo as infostatus,P_MeetingAdmin.P_ID as id");
            strsql.Append(" from P_MeetingAdmin ");
            strsql.Append(" LEFT JOIN P_MeetingAdminSublist on P_MeetingAdminSublist.P_MeeID=P_MeetingAdmin.P_Id ");
            strsql.Append(" where P_MeetingAdmin.P_Status=0 and P_MeetingAdminSublist.P_UserId='" + userid + "'");
            //strsql.Append("  (GETDATE() BETWEEN P_MeetingAdmin.P_StartTime and P_MeetingAdmin.P_EndTime) ");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_MeetingAdmin.P_EndTime desc"));
            DataSetToModelHelper<UserMeetingModel> mete = new DataSetToModelHelper<UserMeetingModel>();
            if (dt != null)
            {
                model = mete.FillModel(dt);
            }
            else
            {
                model = null;
            }
            return model;
        }

        #endregion

        #region ============= 数据转换方法
        /// <summary>
        /// 将datatable转换为list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<InfoModel> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<InfoModel> list = new List<InfoModel>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(DataRowModel(row));
            }

            return list;
        }
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public InfoToModel DataRowToModel(DataRow row)
        {
            InfoToModel model = new InfoToModel();
            if (row != null)
            {
                #region 主表信息======================
                if (row["username"] != null && row["username"].ToString() != "")
                {
                    model.username = row["username"].ToString();
                }
                if (row["infostatus"] != null && row["infostatus"].ToString() != "")
                {
                    model.infostatus = row["infostatus"].ToString();
                }
                if (row["infotime"] != null && row["infotime"].ToString() != "")
                {
                    model.infotime = Convert.ToDateTime(row["infotime"]);
                }
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                #endregion
            }
            return model;
        }
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public InfoModel DataRowModel(DataRow row)
        {
            InfoModel model = new InfoModel();
            if (row != null)
            {
                #region 主表信息======================
                if (row["username"] != null && row["username"].ToString() != "")
                {
                    model.username = row["username"].ToString();
                }
                if (row["infostatus"] != null && row["infostatus"].ToString() != "")
                {
                    model.infostatus = Convert.ToInt32(row["infostatus"]);
                }
                if (row["infotime"] != null && row["infotime"].ToString() != "")
                {
                    model.infotime = row["infotime"].ToString();
                }
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["avatar"] != null && row["avatar"].ToString() != "")
                {
                    model.avatar = row["avatar"].ToString();
                }
                if (row["userid"] != null && row["userid"].ToString() != "")
                {
                    model.userid = Convert.ToInt32(row["userid"]);
                }

                #endregion
            }
            return model;
        }
        #endregion

        #region ========== Web页面接口处理

        /// <summary>
        /// Web页面获取会议详情
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns></returns>
        public WebMeetingDetailModel WebMeetingDetail(string id)
        {

            StringBuilder sql = new StringBuilder();
            // 会议信息
            sql.Append("select P_Title as title,P_MeeContent as content,CONVERT(varchar(100), P_StartTime, 20) as starttime,");
            sql.Append("CONVERT(varchar(100), P_EndTime, 20) as endtime,P_MeePlace as site,P_PeopleAmount as peoplecount");
            sql.Append(" from P_MeetingAdmin where P_Id = @P_Id");

            SqlParameter[] parameter = {
                new SqlParameter("@P_Id",SqlDbType.NVarChar, 50)
            };
            parameter[0].Value = id;
            // 获取会议信息
            DataSet ds = DbHelperSQL.Query(sql.ToString(), parameter);
            DataSetToModelHelper<WebMeetingDetailModel> helper = new DataSetToModelHelper<WebMeetingDetailModel>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                WebMeetingDetailModel data = helper.FillToModel(ds.Tables[0].Rows[0]);
                // 获取人员数据
                StringBuilder personnelSql = new StringBuilder();
                personnelSql.Append("select convert(nvarchar,dt_users.id) as userid, dt_users.user_name as username from P_MeetingAdminSublist ");
                personnelSql.Append("left join dt_users on convert(nvarchar,dt_users.id) = P_MeetingAdminSublist.P_UserId ");
                personnelSql.Append("where P_MeetingAdminSublist.P_MeeID = @P_MeeID and  dt_users.id is not null");
                SqlParameter[] personnelPar = {
                    new SqlParameter("@P_MeeID", SqlDbType.NVarChar, 50)
                };
                personnelPar[0].Value = id;
                // 获取与会人员列表
                DataSet personnelds = DbHelperSQL.Query(personnelSql.ToString(), personnelPar);

                if (personnelds.Tables[0] != null && personnelds.Tables[0].Rows.Count > 0)
                {
                    DataSetToModelHelper<MeetingPeople> personnelHelper = new DataSetToModelHelper<MeetingPeople>();
                    data.people = personnelHelper.FillModel(personnelds);
                }
                return data;
            }

            return null;
        }

        /// <summary>
        /// Web页面添加会议
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public Boolean WebMeetingAdd(WebMeetingFromBody fromBody)
        {

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("insert into P_MeetingAdmin (");
                        sql.Append("P_Id,P_Title,P_MeeContent,P_StartTime,P_EndTime,P_MeePlace,P_CreateTime,P_CreateUser,P_Status,P_PeopleAmount)");
                        sql.Append(" values(@P_Id,@P_Title,@P_MeeContent,@P_StartTime,@P_EndTime,@P_MeePlace,@P_CreateTime,@P_CreateUser,@P_Status,@P_PeopleAmount)");
                        SqlParameter[] parameter = {
                            new SqlParameter("@P_Id", SqlDbType.NVarChar, 50),
                            new SqlParameter("@P_Title", SqlDbType.NVarChar, 100),
                            new SqlParameter("@P_MeeContent", SqlDbType.NText),
                            new SqlParameter("@P_StartTime", SqlDbType.DateTime),
                            new SqlParameter("@P_EndTime", SqlDbType.DateTime),
                            new SqlParameter("@P_MeePlace", SqlDbType.NVarChar, 100),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_CreateUser", SqlDbType.NVarChar, 100),
                            new SqlParameter("@P_Status", SqlDbType.NVarChar, 100),
                            new SqlParameter("@P_PeopleAmount", SqlDbType.Int,4)
                        };
                        parameter[0].Value = Guid.NewGuid().ToString();
                        parameter[1].Value = fromBody.title;
                        parameter[2].Value = fromBody.content;
                        parameter[3].Value = Convert.ToDateTime(fromBody.statrtime);
                        parameter[4].Value = Convert.ToDateTime(fromBody.endtime);
                        parameter[5].Value = fromBody.site;
                        parameter[6].Value = DateTime.Now;
                        parameter[7].Value = fromBody.userid;
                        parameter[8].Value = 0;
                        parameter[9].Value = fromBody.count;

                        DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameter);
                        List<int> per = new List<int>();
                        if (!String.IsNullOrEmpty(fromBody.people))
                        {
                            string[] userId = fromBody.people.Split(',');
                            foreach (string item in userId)
                            {
                                per.Add(Convert.ToInt32(item));
                                StringBuilder sql1 = new StringBuilder();
                                sql1.Append("insert into P_MeetingAdminSublist (");
                                sql1.Append("P_ID,P_MeeID,P_UserId,P_SigninInfo,P_CreateTime,P_CreateUser,P_Status)");
                                sql1.Append(" values (@P_ID,@P_MeeID,@P_UserId,@P_SigninInfo,@P_CreateTime,@P_CreateUser,@P_Status)");

                                SqlParameter[] parameter1 = {
                                    new SqlParameter("@P_ID",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_MeeID",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_UserId",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_SigninInfo",SqlDbType.Int, 4),
                                    new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                    new SqlParameter("@P_CreateUser",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_Status",SqlDbType.Int, 4)
                                };
                                parameter1[0].Value = Guid.NewGuid().ToString();
                                parameter1[1].Value = parameter[0].Value;
                                parameter1[2].Value = item;
                                parameter1[3].Value = 0;
                                parameter1[4].Value = DateTime.Now;
                                parameter1[5].Value = fromBody.userid;
                                parameter1[6].Value = 0;

                                DbHelperSQL.ExecuteSql(conn, trans, sql1.ToString(), parameter1);
                            }

                        }

                        trans.Commit();
                        PushMessageHelper.PushMessages("", "您有一条会议信息。", per,
                           Convert.ToInt32(fromBody.userid), (int)PushTypeEnum.会议通知);
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
        /// Web页面修改会议信息
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public Boolean WebMeetingEdit(WebMeetingFromBody fromBody)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 修改会议信息
                        StringBuilder updateSql = new StringBuilder();
                        updateSql.Append("update P_MeetingAdmin set");
                        updateSql.Append(" P_Title = @P_Title,");
                        updateSql.Append("P_MeeContent = @P_MeeContent,");
                        updateSql.Append("P_StartTime = @P_StartTime,");
                        updateSql.Append("P_EndTime = @P_EndTime,");
                        updateSql.Append("P_MeePlace = @P_MeePlace,");
                        updateSql.Append("P_PeopleAmount = @P_PeopleAmount,");
                        updateSql.Append("P_UpdateTime = @P_UpdateTime,");
                        updateSql.Append("P_UpdateUser = @P_UpdateUser");
                        updateSql.Append(" where P_Id = @P_Id");

                        SqlParameter[] updatePar = {
                            new SqlParameter("@P_Title",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_MeeContent",SqlDbType.NText),
                            new SqlParameter("@P_StartTime",SqlDbType.DateTime),
                            new SqlParameter("@P_EndTime",SqlDbType.DateTime),
                            new SqlParameter("@P_MeePlace",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_PeopleAmount",SqlDbType.Int,4),
                            new SqlParameter("@P_UpdateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_UpdateUser",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50)
                        };

                        updatePar[0].Value = fromBody.title;
                        updatePar[1].Value = fromBody.content;
                        updatePar[2].Value = Convert.ToDateTime(fromBody.statrtime);
                        updatePar[3].Value = Convert.ToDateTime(fromBody.endtime) ;
                        updatePar[4].Value = fromBody.site;
                        updatePar[5].Value = fromBody.count;
                        updatePar[6].Value = DateTime.Now;
                        updatePar[7].Value = fromBody.userid;
                        updatePar[8].Value = fromBody.id;

                        DbHelperSQL.ExecuteSql(conn, trans, updateSql.ToString(), updatePar);

                        // 删除与会人员信息
                        StringBuilder deleteSql = new StringBuilder();
                        deleteSql.Append("delete from P_MeetingAdminSublist where P_MeetingAdminSublist.P_MeeID = @P_MeeID");
                        SqlParameter[] deletePar = {
                            new SqlParameter("@P_MeeID", SqlDbType.NVarChar, 50)
                        };
                        deletePar[0].Value = fromBody.id;

                        DbHelperSQL.ExecuteSql(conn, trans, deleteSql.ToString(), deletePar);

                        if (!String.IsNullOrEmpty(fromBody.people))
                        {
                            string[] userId = fromBody.people.Split(',');
                            foreach (string item in userId)
                            {
                                StringBuilder sql1 = new StringBuilder();
                                sql1.Append("insert into P_MeetingAdminSublist (");
                                sql1.Append("P_ID,P_MeeID,P_UserId,P_SigninInfo,P_CreateTime,P_CreateUser,P_Status)");
                                sql1.Append(" values (@P_ID,@P_MeeID,@P_UserId,@P_SigninInfo,@P_CreateTime,@P_CreateUser,@P_Status)");

                                SqlParameter[] parameter1 = {
                                    new SqlParameter("@P_ID",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_MeeID",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_UserId",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_SigninInfo",SqlDbType.Int, 4),
                                    new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                    new SqlParameter("@P_CreateUser",SqlDbType.NVarChar, 50),
                                    new SqlParameter("@P_Status",SqlDbType.Int, 4)
                                };
                                parameter1[0].Value = Guid.NewGuid().ToString();
                                parameter1[1].Value = fromBody.id;
                                parameter1[2].Value = item;
                                parameter1[3].Value = 0;
                                parameter1[4].Value = DateTime.Now;
                                parameter1[5].Value = fromBody.userid;
                                parameter1[6].Value = 0;

                                DbHelperSQL.ExecuteSql(conn, trans, sql1.ToString(), parameter1);
                            }
                        }
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
        #endregion

    }

    #region ============ 手机App返回数据实体类
    public class meeteAdmin
    {
        /// <summary>
        /// 会议的id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 会议的名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string starttime { get; set; }
        /// <summary>
        /// 会议是否饱满 0 未满 / 1满了
        /// </summary>
        public int astatus { get; set; }
    }
    public class DetailMeeting
    {
        public string id { get; set; }
        public string title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        private string _starttime;
        public string starttime
        {
            get
            {
                return _starttime == null ? "" : DateTime.Parse( _starttime).ToString("yyyy年MM月dd日HH点");
            }
            set
            {
                _starttime = value;
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string place { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 参与人数
        /// </summary>
        public int usercount { get; set; }
        /// <summary>
        /// 用户是否报名 0没有/1 报名了
        /// </summary>
        public int userstatus { get; set; }
        public int astatus { get; set; }
    }
    public class StartEndAdmin
    {
        public string id { get; set; }
        public string title { get; set; }
        private string _endtime { get; set; }
        public string endtime
        {
            get
            {
                return _endtime == null ? "" : DateTime.Parse(_endtime).ToString("yyyy年MM月dd日 HH-mm");
            }
            set
            {
                _endtime = value;
            }
        }

        public string place { get; set; }
    }
    public class MeetingCount
    {
        public int admincount { get; set; }
        public int notokcount { get; set; }
        public int okcount { get; set; }
    }
    public class InfoToModel
    {
        public string id { get; set; }
        public string username { get; set; }
        public DateTime infotime { get; set; }
        public string infostatus { get; set; }
        public List<InfoModel> infolist { get; set; }
    }
    public class InfoModel
    {
        public int userid { get; set; }
        public string id { get; set; }
        public string username { get; set; }
        private string _infotime { get; set; }
        public string infotime
        {
            get
            {
                return _infotime == null ? "" : DateTime.Parse(_infotime).ToString("yyyy年MM月dd日 HH:mm");
            }
            set
            {
                _infotime = value;
            }
        }
        public int infostatus { get; set; }
        public string organ { get; set; }
        public string avatar { get; set; }
    }
    public class OrganModel
    {
        public string organ { get; set; }
    }
    public class UserMeetingModel
    {
        public string id { get; set; }
        public string title { get; set; }
        //public DateTime endtime { get; set; }
        private string _endtime;
        public string endtime
        {
            get
            {
                return _endtime == null ? "" : DateTime.Parse(_endtime).ToString("yyyy年MM月dd日 HH:mm");
            }
            set
            {
                _endtime = value;
            }
        }
        public string place { get; set; }
        public int infostatus { get; set; }
    }
    #endregion

}

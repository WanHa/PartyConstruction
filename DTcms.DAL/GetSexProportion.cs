using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.homepage;
using Jayrock.Json.Conversion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class GetSexProportion
    {
        /// <summary>
        /// 获取性别比例
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        //public static List<SexProportionModel> getSexPro(string groupId)
        //{
        //    StringBuilder strsql = new StringBuilder();
        //    List<SexProportionModel> models = new List<SexProportionModel>();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(groupId))
        //        {
        //            strsql.Append("SELECT dt_users.sex,COUNT(dt_users.id) FROM dt_users GROUP BY dt_users.sex");
        //        }
        //        else
        //        {
        //            strsql.Append("SELECT dt_users.sex,COUNT(dt_users.id) FROM dt_users WHERE dt_users.group_id like '%" + groupId + "%' GROUP BY dt_users.sex ");
        //        }
        //        DataSet dt = DbHelperSQL.Query(strsql.ToString());
        //        DataSetToModelHelper<SexProportionModel> result = new DataSetToModelHelper<SexProportionModel>();
        //        if (dt.Tables[0].Rows.Count != 0)
        //        {
        //            models = result.FillModel(dt);
        //        }
        //        else
        //        {
        //            models = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return models;
        //}

        //public static List<IncomeSourceModel> getIncomeTypeNum(string groupId)
        //{
        //    StringBuilder strsql = new StringBuilder();
        //    List<IncomeSourceModel> models = new List<IncomeSourceModel>();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(groupId))
        //        {
        //            strsql.Append("SELECT COUNT(dt_users.id) FROM dt_users GROUP BY dt_users.income_source_id");
        //        }
        //        else
        //        {
        //            strsql.Append("SELECT COUNT(dt_users.id) FROM dt_users WHERE dt_users.group_id like '%" + groupId + "%' GROUP BY dt_users.income_source_id");
        //        }
        //        DataSet dt = DbHelperSQL.Query(strsql.ToString());
        //        DataSetToModelHelper<IncomeSourceModel> result = new DataSetToModelHelper<IncomeSourceModel>();
        //        if (dt.Tables[0].Rows.Count != 0)
        //        {
        //            models = result.FillModel(dt);
        //        }
        //        else
        //        {
        //            models = null;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return models;
        //}

        /// <summary>
        /// 获取正式党员预备党员数量
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public PartyMemberKind getPartyMemberCount(string groupid)
        {
            PartyMemberKind model = new PartyMemberKind();
            StringBuilder str = new StringBuilder();
            str.Append("select sum(official_male_count + official_female_count) as off_count,sum(ready_male_count + ready_female_count) as ready_count from dt_user_groups ");
            if (!"all".Equals(groupid)) {
                str.Append(" where id = '" + groupid + "'");
            }
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<PartyMemberKind> helper = new DataSetToModelHelper<PartyMemberKind>();
            model = helper.FillToModel(ds.Tables[0].Rows[0]);
            return model;
        }

        /// <summary>
        /// 获取组织人员参加会议次数
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public List<int> getMeetingNumber(string groupid)
        {
            List<int> result = new List<int>();

            for (int i = 1; i <= 12; i++)
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("select count(*) from P_MeetingAdmin ");
                sql.Append("where P_MeetingAdmin.P_Id in (");
                sql.Append("select P_MeeID from P_MeetingAdminSublist ");
                sql.Append(" where P_MeetingAdminSublist.P_UserId in (");
                sql.Append(" select id from dt_users");

                if (!"all".Equals(groupid))
                {
                    sql.Append(" where dt_users.group_id like '%," + groupid + ",%'");
                }
                sql.Append(" ) ) and P_Status = 0 ");
                sql.Append(" and YEAR(P_StartTime) = " + DateTime.Now.Year + " and month(P_StartTime) = " + i);

                int item = Convert.ToInt32(DbHelperSQL.GetSingle(sql.ToString()));

                result.Add(item);
            }

            //List<MeetingNumber> list = new List<MeetingNumber>();
            //List<MeetingNumber> userc = new List<MeetingNumber>();
            //MeetingNumber model = new MeetingNumber();
            //StringBuilder str = new StringBuilder();
            //str.Append(@"select id as userid from dt_users ");
            //if (!"all".Equals(groupid)) {
            //    str.Append(" where dt_users.group_id like '%" + groupid + "%'");
            //}
            //DataSet ds = DbHelperSQL.Query(str.ToString());
            //DataSetToModelHelper<MeetingNumber> result = new DataSetToModelHelper<MeetingNumber>();
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    userc = result.FillModel(ds);
            //    StringBuilder strsql = new StringBuilder();
            //    if (userc != null)
            //    {
            //        strsql.Append(" and  ( ");
            //        for (int i = 0; i < userc.Count; i++)
            //        {
            //            if (i == userc.Count - 1)
            //            {
            //                strsql.Append(" P_UserId = " + userc[i].userid + @" ");
            //            }
            //            if (i < userc.Count - 2)
            //            {
            //                strsql.Append(" P_UserId = " + userc[i].userid + @"  or  ");
            //            }
            //        }
            //        strsql.Append(" )");
            //    }
            //    for (int j = 1; j <= 12; j++)
            //    {
            //        StringBuilder sss = new StringBuilder();
            //        sss.Append(" select count(*) as count  from  (select distinct P_MeeID,P_UserId from P_MeetingAdminSublist where MONTH(P_CreateTime)=" + j + @" " + strsql.ToString() + @" )aa ");
            //        DataSet dd = DbHelperSQL.Query(sss.ToString());
            //        model = result.FillToModel(dd.Tables[0].Rows[0]);
            //        list.Add(model);
            //    }
            //}
            //else {
            //    for (int i = 0; i < 12; i++)
            //    {
            //        MeetingNumber item = new MeetingNumber() {
            //            count = 0,
            //            userid = 0
            //        };
            //        list.Add(item);
            //    }
            //}
            return result;
        }


        /// <summary>
        /// 获取党员诉求情况
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public AppealChartModel getAppealInfo(string groupid)
        {
            //List<GetInfo> list = new List<GetInfo>();
            List<GetInfo> userc = new List<GetInfo>();
            //GetInfo model = new GetInfo();
            StringBuilder str = new StringBuilder();
            str.Append(@"select Convert(varchar,id) as userid from dt_users ");
            if (!"all".Equals(groupid)) {
                str.Append("where dt_users.group_id like '%," + groupid + ",%'");
            }
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<GetInfo> helper = new DataSetToModelHelper<GetInfo>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
            {
                userc = helper.FillModel(ds);
                StringBuilder strsql = new StringBuilder();
                strsql.Append(" and  ( ");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i == userc.Count - 1)
                    {
                        strsql.Append(" P_CreateUser = " + userc[i].userid + @" ");
                    }
                    if (i <= userc.Count - 2)
                    {
                        strsql.Append(" P_CreateUser = " + userc[i].userid + @"  or  ");
                    }
                }
                strsql.Append(" )");
                List<int> allList = new List<int>();
                List<int> processedList = new List<int>();
                List<int> untreatedList = new List<int>();

                for (int j = 1; j <= 12; j++)
                {
                    StringBuilder sss = new StringBuilder();
                    // 未处理
                    sss.Append(@"select count(*) as count from 
                                             (select P_CheckStatus as checkstatus,
                                             (select count(P_UserDemandSublist.P_ID)
                                             from P_UserDemandSublist
                                             where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb
                                             from P_UserDemand where year(P_UserDemand.P_CreateTime)= " + DateTime.Now.Year + " and MONTH(P_UserDemand.P_CreateTime)=" + j + @" " + strsql.ToString() + @") as b
                                             where(b.checkstatus = -1 and b.bbbb = 0) or(b.checkstatus = 1 and b.bbbb = 2)");
                    int dd =Convert.ToInt32(DbHelperSQL.GetSingle(sss.ToString()));
                    //model = helper.FillToModel(dd.Tables[0].Rows[0]);
                    //model.type = "未处理";
                    untreatedList.Add(dd);

                    StringBuilder sql = new StringBuilder();
                    // 已处理
                    sql.Append(@"select count(*) as count from 
                                             (select P_CheckStatus as checkstatus,
                                             (select count(P_UserDemandSublist.P_ID)
                                             from P_UserDemandSublist
                                             where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb
                                             from P_UserDemand where year(P_UserDemand.P_CreateTime)= "+ DateTime.Now.Year + " and MONTH(P_UserDemand.P_CreateTime)=" + j + @" " + strsql.ToString() + @") as b
                                             where (b.checkstatus =-1 and b.bbbb =1)or (b.checkstatus=1 and b.bbbb =3)or (b.checkstatus =0 and b.bbbb=1)");
                    int da =Convert.ToInt32(DbHelperSQL.GetSingle(sql.ToString()));
                    //model = helper.FillToModel(da.Tables[0].Rows[0]);
                    //model.type = "已处理";
                    //list.Add(model);
                    processedList.Add(da);

                    StringBuilder sb = new StringBuilder();
                    // 全部
                    sb.Append(@"select count(*) as count from P_UserDemand where year(P_UserDemand.P_CreateTime)= " + DateTime.Now.Year + " and MONTH(P_UserDemand.P_CreateTime)=" + j + @" " + strsql.ToString() + @"");
                    int bb =Convert.ToInt32(DbHelperSQL.GetSingle(sb.ToString()));
                    //model = helper.FillToModel(bb.Tables[0].Rows[0]);
                    //model.type = "提交诉求数";
                    //list.Add(model);
                    allList.Add(bb);
                }
                return new AppealChartModel() {
                    all = allList,
                    untreated = untreatedList,
                    processed = processedList
                };
            }
            List<int> zero = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                zero.Add(0);
            }
            return new AppealChartModel() {
                all = zero,
                untreated = zero,
                processed = zero
            };
        }

        /// <summary>
        /// 获取党费缴纳详情
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public PayChartModel getGroupPayInfo(string groupid)
        {
            //List<GetInfo> list = new List<GetInfo>();
            //List<GetInfo> userc = new List<GetInfo>();
            //GetInfo model = new GetInfo();
            //StringBuilder str = new StringBuilder();
            //str.Append(@"" + groupid + "%'");
            //DataSet ds = DbHelperSQL.Query(str.ToString());
            //DataSetToModelHelper<GetInfo> result = new DataSetToModelHelper<GetInfo>();
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    userc = result.FillModel(ds);
            //    StringBuilder strsql = new StringBuilder();
            //    if (userc != null)
            //    {
            //        strsql.Append(" and  ( ");
            //        for (int i = 0; i < userc.Count; i++)
            //        {
            //            if (i == userc.Count - 1)
            //            {
            //                strsql.Append(" P_CreateUser = " + userc[i].userid + @" ");
            //            }
            //            if (i < userc.Count - 2)
            //            {
            //                strsql.Append(" P_CreateUser = " + userc[i].userid + @"  or  ");
            //            }
            //        }
            //        strsql.Append(" )");
            //    }

            //    StringBuilder strs = new StringBuilder();
            //    if (userc != null)
            //    {
            //        strs.Append(" and  ( ");
            //        for (int i = 0; i < userc.Count; i++)
            //        {
            //            if (i == userc.Count - 1)
            //            {
            //                strs.Append(" P_UserID = " + userc[i].userid + @" ");
            //            }
            //            if (i < userc.Count - 2)
            //            {
            //                strs.Append(" P_UserID = " + userc[i].userid + @"  or  ");
            //            }
            //        }
            //        strs.Append(" )");
            //    }
            //    for (int j = 1; j <= 12; j++)
            //    {
            //        StringBuilder sql = new StringBuilder();
            //        sql.Append(@"select count(*) as count from (select (select count(P_Id) from P_PartyPayMentRecord 
            //                                where P_OutTradeNo =null or P_OutTradeNo ='0' 
            //                                " + strsql.ToString() + @")as count
            //                                from P_PartyPayMentPeople
            //                                where MONTH(P_CreateTime)=" + j + @" " + strs.ToString() + @")aa");
            //        DataSet dd = DbHelperSQL.Query(sql.ToString());
            //        model = result.FillToModel(dd.Tables[0].Rows[0]);
            //        model.type = "已提交";
            //        list.Add(model);


            //        StringBuilder ss = new StringBuilder();
            //        ss.Append(@"select count(*) as count from (select (select count(P_Id) from P_PartyPayMentRecord 
            //                               where P_OutTradeNo <>null or P_OutTradeNo <>'0' 
            //                               " + strsql.ToString() + @")as count
            //                               from P_PartyPayMentPeople
            //                               where MONTH(P_CreateTime)=" + j + @" " + strs.ToString() + @")aa");
            //        DataSet db = DbHelperSQL.Query(ss.ToString());
            //        model = result.FillToModel(db.Tables[0].Rows[0]);
            //        model.type = "未提交";
            //        list.Add(model);
            //    }
            //}
            //return list;
            List<int> submitList = new List<int>();
            List<int> unsubmitList = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                // 未缴费
                StringBuilder unsubmitSql = new StringBuilder();
                unsubmitSql.Append("select count(*) from P_PartyPayMentPeople");
                unsubmitSql.Append(" left join P_PartyPayMentRecord on P_PartyPayMentRecord.P_OutTradeNo = P_PartyPayMentPeople.P_ID");
                unsubmitSql.Append(" where (P_PartyPayMentRecord.P_Status is null or P_PartyPayMentRecord.P_Status = 0)");
                unsubmitSql.Append(" and MONTH(P_PartyPayMentPeople.P_CreateTime)= " + i);
                unsubmitSql.Append(" and year(P_PartyPayMentPeople.P_CreateTime) = " + DateTime.Now.Year);
                // 已缴费
                StringBuilder submitSql = new StringBuilder();
                submitSql.Append("select count(*) from P_PartyPayMentPeople");
                submitSql.Append(" left join P_PartyPayMentRecord on P_PartyPayMentRecord.P_OutTradeNo = P_PartyPayMentPeople.P_ID");
                submitSql.Append(" where P_PartyPayMentRecord.P_Status = 1");
                submitSql.Append(" and MONTH(P_PartyPayMentPeople.P_CreateTime) = " + i);
                submitSql.Append(" and year(P_PartyPayMentPeople.P_CreateTime) = " +DateTime.Now.Year);
                // 判断查询所有支部还是单个支部
                if (!"all".Equals(groupid))
                {
                    submitSql.Append(" and P_PartyPayMentPeople.P_UserID in ");
                    submitSql.Append(" (select Convert(varchar, id) as userid from dt_users where dt_users.group_id like '%," + groupid + ",%')");
                    unsubmitSql.Append(" and P_PartyPayMentPeople.P_UserID in ");
                    unsubmitSql.Append(" (select Convert(varchar, id) as userid from dt_users where dt_users.group_id like '%," + groupid + ",%')");
                }
                int subCount = Convert.ToInt32(DbHelperSQL.GetSingle(submitSql.ToString()));
                submitList.Add(subCount);
                int unsubCount = Convert.ToInt32(DbHelperSQL.GetSingle(unsubmitSql.ToString()));
                unsubmitList.Add(unsubCount);
            }
            return new PayChartModel()
            {
                submit = submitList,
                unsubmit = unsubmitList
            };
        }

        /// <summary>
        /// 获取首页地图标题信息
        /// </summary>
        /// <returns></returns>
        public MapTitleModel GetMapTitle() {
            // 党员数量
            string queryCount = String.Format(@"select count(*) from dt_users");
            // 党组织数量
            string queryCount1 = String.Format(@"select count(*) from dt_user_groups");
            // 党员服务组织数量
            string querycount2 = String.Format(@"select sum(u_company_type.service_organiz) from dt_user_groups
                        left join u_company_type on dt_user_groups.company_info_id = u_company_type.id");

            int count = Convert.ToInt32(DbHelperSQL.GetSingle(queryCount));
            int count1 = Convert.ToInt32(DbHelperSQL.GetSingle(queryCount1));
            int count2 = Convert.ToInt32(DbHelperSQL.GetSingle(querycount2));

            return new MapTitleModel() {
                party_member_count = count,
                party_organization_count = count1,
                service_organization_count = count2
            };
        }

        /// <summary>
        /// 获取首页地图上组织列表
        /// </summary>
        /// <returns></returns>
        public List<MapOrgLocationModel> GetMapOrganizationLocation() {

            string sql = String.Format(@"select id from dt_user_groups");

            DataSet ds = DbHelperSQL.Query(sql);

            DataSetToModelHelper<MapOrgLocationModel> helper = new DataSetToModelHelper<MapOrgLocationModel>();

            return helper.FillModel(ds);
        }

        /// <summary>
        /// 获取首页地图组织详情信息
        /// </summary>
        /// <param name="groupId">组织ID</param>
        /// <returns></returns>
        public MapGroupInfoModel GetGroupInfo(string groupId) {

            string sql = String.Format(@"select dt_user_groups.id as group_id, title as group_name,
                        contact_address,dt_users.user_name as secretary,
                        (select count(dt_users.id) from dt_users where group_id like '%,{0},%') as personnel_count
                        from dt_user_groups
                        left join dt_users on dt_users.id = dt_user_groups.manager_id
                        where dt_user_groups.id = {0}", groupId);

            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<MapGroupInfoModel> helper = new DataSetToModelHelper<MapGroupInfoModel>();
            MapGroupInfoModel result = helper.FillToModel(ds.Tables[0].Rows[0]);

            string userSql = String.Format(@"select dt_users.id as user_id, dt_users.user_name from dt_users
                        where dt_users.group_id like '%,{0},%'", groupId);

            DataSet ds1 = DbHelperSQL.Query(userSql);
            
            if (ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0) {
            DataSetToModelHelper<GroupUserModel> helper1 = new DataSetToModelHelper<GroupUserModel>();
                result.group_users = helper1.FillModel(ds1);
            }

            return result;
        }

        /// <summary>
        /// 获取首页地图人员信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public MapUserInfoModel GetMapUserInfo(string userId) {

            // 人员信息
            string querySql = String.Format(@"select user_name,id as user_id from dt_users where dt_users.id = {0}", userId);

            DataSet ds = DbHelperSQL.Query(querySql);

            DataSetToModelHelper<MapUserInfoModel> helper = new DataSetToModelHelper<MapUserInfoModel>();
            MapUserInfoModel result = helper.FillToModel(ds.Tables[0].Rows[0]);

            // 组织信息
            string querySql1 = String.Format(@"select TOP 1 dt_user_groups.title,
                    dt_user_groups.manager,
                    p.title as superior_organization from F_Split(
                    (select dt_users.group_id from dt_users where dt_users.id = {0}),',') as t 
                    left join dt_user_groups on dt_user_groups.id = t.value 
                    left join dt_user_groups p on p.id = dt_user_groups.pid
                    where t.value != '' order by dt_user_groups.grade DESC", userId);
            DataSet ds1 = DbHelperSQL.Query(querySql1);
            if (ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0) {
                DataRow dr = ds1.Tables[0].Rows[0];
                result.group_name = Convert.ToString(dr["title"]);
                result.secretary = Convert.ToString(dr["manager"]);
                result.superior_organization = Convert.ToString(dr["superior_organization"]);
            }

            string activitySql = String.Format(@"select 
                    P_ActivityStyle.P_ActivityName as activity_name,
                    P_ActivityStyle.P_ActivitySite as activity_address,
                    format(P_ActivityStyle.P_ActivityStartTime,'yyyy年MM月dd日 HH:mm:ss') as start_time,
                    format(P_ActivityStyle.P_ActivityEndTime,'yyyy年MM月dd日 HH:mm:ss') as end_time,
                    P_ActivityStyle.P_Particular as activity_detail,
                    P_ActivityStyle.P_Sponsor as organizer,
                    P_Image.P_ImageUrl as cover_pic,
                    (select count(*) from P_ActivityStyleSublist 
                    where P_ActivityStyleSublist.P_Relation = P_ActivityStyle.P_Id) as personnel_count
                    from P_ActivityStyle
                    left join P_Image on P_Image.P_ImageId = P_ActivityStyle.P_Id and P_Image.P_ImageType = {0}
                    where P_ActivityStyle.P_Status = 0 and  P_ActivityStyle.P_Id in (
	                    select P_ActivityStyleSublist.P_Relation from P_ActivityStyleSublist 
	                    where P_ActivityStyleSublist.P_Participant = {1}) order by P_ActivityStyle.P_CreateTime desc", (int)ImageTypeEnum.活动风采, userId);

            DataSet ds2 = DbHelperSQL.Query(activitySql);
            DataSetToModelHelper<MapActivityModel> helper2 = new DataSetToModelHelper<MapActivityModel>();
            result.activitys = helper2.FillModel(ds2);

            return result;
        }

        /// <summary>
        /// 获取首页图片组织活动信息
        /// </summary>
        /// <param name="groupId">组织ID</param>
        /// <returns></returns>
        public List<MapActivityModel> GetGroupActivity(string groupId) {

            string sql = String.Format(@"select 
                        P_ActivityStyle.P_ActivityName as activity_name,
                        P_ActivityStyle.P_ActivitySite as activity_address,
                        format(P_ActivityStyle.P_ActivityStartTime,'yyyy年MM月dd日 HH:mm:ss') as start_time,
                        format(P_ActivityStyle.P_ActivityEndTime,'yyyy年MM月dd日 HH:mm:ss') as end_time,
                        P_ActivityStyle.P_Particular as activity_detail,
                        P_ActivityStyle.P_Sponsor as organizer,
                        P_Image.P_ImageUrl as cover_pic,    -- 活动封面图片
                        (select count(*) from P_ActivityStyleSublist 
                        where P_ActivityStyleSublist.P_Relation = P_ActivityStyle.P_Id) as personnel_count -- 参与活动人数
                        from P_ActivityStyle
                        left join P_Image on P_Image.P_ImageId = P_ActivityStyle.P_Id and P_Image.P_ImageType = {0} -- 关联图片
                        where P_ActivityStyle.P_Status = 0 and  P_ActivityStyle.P_Id in (   -- 组织下的活动ID
	                        select P_ActivityStyleSublist.P_Relation from P_ActivityStyleSublist
                            where P_ActivityStyleSublist.P_Participant in 
                            (
	                            select id from dt_users where group_id like '%,{1},%'
                            )) order by P_ActivityStyle.P_CreateTime desc", (int)ImageTypeEnum.活动风采, groupId);
            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<MapActivityModel> helper = new DataSetToModelHelper<MapActivityModel>();

            List<MapActivityModel> activitys = helper.FillModel(ds);
           
            return activitys;
        }

    }
}
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
    public class PartyNewsPaperBiz
    {
        /// <summary>
        /// 获取转发时,@的人员列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<AtPersonnelModel> GetAtPersonnel(string userId, int page, int rows)
        {
            // 用户所在论坛下所有人员和用户所属支部下所有人员
            string sql = String.Format(@"select dt_users.id as user_id,dt_users.user_name,P_Image.P_ImageUrl as avatar from dt_users
                        left join P_Image on P_ImageId = CONVERT(nvarchar,dt_users.id) and P_ImageType = {1}
                        where dt_users.id in (
                        select dt_users.id from dt_users
                        left join (
                        select '%,' +t.value + ',%' as gourp_id from F_Split
                        ((select dt_users.group_id from dt_users where dt_users.id = {0} ),',') as t
                        where t.value != '' ) b on dt_users.group_id like b.gourp_id
                        where b.gourp_id is not null
                        group by dt_users.id
                        UNION 
                        select P_PersonGroupRelation.P_UserId as id from P_PersonGroupRelation
                        where P_PersonGroupRelation.P_PartyGroupId in (
	                        select P_PersonGroupRelation.P_PartyGroupId from P_PersonGroupRelation
	                        left join P_PartyGroupForum on P_PartyGroupForum.P_Id = P_PersonGroupRelation.P_PartyGroupId
	                        where P_PersonGroupRelation.P_UserId = '{0}' and P_PersonGroupRelation.P_Approval = 0 
	                        and P_PersonGroupRelation.P_Status = 0
	                        and P_PartyGroupForum.P_Status=0
                        ) and P_PersonGroupRelation.P_Approval = 0
                        group by P_PersonGroupRelation.P_UserId
                        ) and dt_users.id != {0}", userId, (int)ImageTypeEnum.头像);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "dt_users.id"));
            DataSetToModelHelper<AtPersonnelModel> helper = new DataSetToModelHelper<AtPersonnelModel>();

            List<AtPersonnelModel> result = helper.FillModel(ds);

            return result;
        }

        /// <summary>
        /// 提交转发接口
        /// </summary>
        /// <param name="transmit"></param>
        /// <returns></returns>
        public Boolean SubTranck(Trum tr)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (tr.groupid != null && tr.groupid.Count > 0)
                        {
                            foreach (var item in tr.groupid)
                            {
                                StringBuilder strSql = new StringBuilder();
                                strSql.Append("insert into P_Transmit(");
                                strSql.Append("P_Id,P_userid,P_RelationId,p_type,p_content,P_OrganizeId,p_createtime,p_createuser,p_status,P_Category)");
                                strSql.Append(" values (");
                                strSql.Append("@P_Id,@P_userid,@P_RelationId,@p_type,@p_content,@P_OrganizeId,@p_createtime,@p_createuser,@p_status,@P_Category)");
                                strSql.Append(";select @@IDENTITY");
                                SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_userid", SqlDbType.Int,4),
                                new SqlParameter("@P_RelationId", SqlDbType.NVarChar,50),
                                new SqlParameter("@p_type", SqlDbType.NVarChar,50),
                                new SqlParameter("@p_content", SqlDbType.NText),
                                new SqlParameter("@P_OrganizeId", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_createtime", SqlDbType.DateTime,100),
                                new SqlParameter("@p_createuser", SqlDbType.NVarChar,100),
                                 new SqlParameter("@p_status", SqlDbType.Int,4),
                                 new SqlParameter("@P_Category", SqlDbType.Int,4)
                               };
                                string transmitid = Guid.NewGuid().ToString("N");
                                parameters[0].Value = transmitid;
                                parameters[1].Value = tr.userid;
                                parameters[2].Value = tr.raletionid;
                                parameters[3].Value = tr.type;
                                parameters[4].Value = tr.content;
                                parameters[5].Value = item.id;
                                parameters[6].Value = DateTime.Now;
                                parameters[7].Value = tr.userid;
                                parameters[8].Value = 0;
                                parameters[9].Value = 0;
                                object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                                StringBuilder str = new StringBuilder();
                                str.Append(" insert into P_BranchPublish(");
                                str.Append(" P_Id,P_BranchId,P_UserId,P_CreateTime,P_Status,P_Source,P_SourceId) ");
                                str.Append(" values( ");
                                str.Append(" @P_Id,@P_BranchId,@P_UserId,@P_CreateTime,@P_Status,@P_Source,@P_SourceId) ");
                                str.Append(";select @@IDENTITY");
                                SqlParameter[] param = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_BranchId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_CreateTime", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Source", SqlDbType.Int,4),
                                new SqlParameter("@P_SourceId", SqlDbType.NVarChar,50)
                               };
                                string publishId = Guid.NewGuid().ToString("N");
                                param[0].Value = publishId;
                                param[1].Value = item.id;
                                param[2].Value = tr.userid;
                                param[3].Value = DateTime.Now;
                                param[4].Value = 0;
                                param[5].Value = 1; //来源 表示转发过去的
                                param[6].Value = transmitid;
                                object oo = DbHelperSQL.GetSingle(conn, trans, str.ToString(), param); //带事务

                                AddAtPersonnels(publishId, GetGroupAtPersonels(item.id.ToString(), tr.at_personnels),
                                    trans, conn, tr.userid, (int)AtTypeEnum.支部转发);
                            }
                        }


                        if (!String.IsNullOrEmpty(tr.forumsid))
                        {
                            //foreach (var item in foo)
                            //{
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("insert into P_Transmit(");
                            strSql.Append("P_Id,P_userid,P_RelationId,p_type,p_content,P_OrganizeId,p_createtime,p_createuser,p_status,P_Category)");
                            strSql.Append(" values (");
                            strSql.Append("@P_Id,@P_userid,@P_RelationId,@p_type,@p_content,@P_OrganizeId,@p_createtime,@p_createuser,@p_status,@P_Category)");
                            strSql.Append(";select @@IDENTITY");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_userid", SqlDbType.Int,4),
                                new SqlParameter("@P_RelationId", SqlDbType.NVarChar,50),
                                new SqlParameter("@p_type", SqlDbType.NVarChar,50),
                                new SqlParameter("@p_content", SqlDbType.NText,50),
                                new SqlParameter("@P_OrganizeId", SqlDbType.NVarChar,100),
                                new SqlParameter("@p_createtime", SqlDbType.DateTime,100),
                                new SqlParameter("@p_createuser", SqlDbType.NVarChar,100),
                                 new SqlParameter("@p_status", SqlDbType.Int,4),
                                 new SqlParameter("@P_Category", SqlDbType.Int,4)
                               };
                            string forumshareid = Guid.NewGuid().ToString("N");
                            parameters[0].Value = forumshareid;
                            parameters[1].Value = tr.userid;
                            parameters[2].Value = tr.raletionid;
                            parameters[3].Value = tr.type;
                            parameters[4].Value = tr.content;
                            parameters[5].Value = tr.forumsid;
                            parameters[6].Value = DateTime.Now;
                            parameters[7].Value = tr.userid;
                            parameters[8].Value = 0;
                            parameters[9].Value = 1;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                            StringBuilder str = new StringBuilder();
                            str.Append(" insert into P_ForumShare(");
                            str.Append(" P_Id,P_GroupForumId,P_UserId,P_CreateTime,P_Status,P_Source,P_SourceId) ");
                            str.Append(" values( ");
                            str.Append(" @P_Id,@P_GroupForumId,@P_UserId,@P_CreateTime,@P_Status,@P_Source,@P_SourceId) ");
                            str.Append(";select @@IDENTITY");
                            SqlParameter[] param = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_GroupForumId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.Int),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Source", SqlDbType.Int,4),
                                new SqlParameter("@P_SourceId", SqlDbType.NVarChar,50)
                               };
                            string shareId = Guid.NewGuid().ToString("N");
                            param[0].Value = shareId;
                            param[1].Value = tr.forumsid;
                            param[2].Value = tr.userid;
                            param[3].Value = DateTime.Now;
                            param[4].Value = 0;
                            param[5].Value = 1; //这个是来源 0 论坛自己分享的/ 1 转发过去的
                            param[6].Value = forumshareid;
                            object oo = DbHelperSQL.GetSingle(conn, trans, str.ToString(), param); //带事务
                            AddAtPersonnels(shareId, tr.at_personnels, trans, conn, tr.userid, (int)AtTypeEnum.论坛转发);
                            //}
                        }
                        trans.Commit();
                        if (tr.at_personnels != null && tr.at_personnels.Count > 0)
                        {
                            List<int> per = new List<int>();
                            foreach (var item in tr.at_personnels)
                            {
                                per.Add(item.user_id);
                            }

                            PushMessageHelper.PushMessages("", "您收到一条@信息.", per, int.Parse(tr.userid), (int)PushTypeEnum.AT);
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
        /// 获取组织下@人员中在改组织下的人员
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        private List<AtPersonnelFrombody> GetGroupAtPersonels(string groupid, List<AtPersonnelFrombody> all)
        {

            if (all != null && all.Count > 0)
            {

                StringBuilder per = new StringBuilder();
                foreach (AtPersonnelFrombody item in all)
                {
                    per.Append(item.user_id);
                    per.Append(",");
                }
                string sql = String.Format(@"select dt_users.id as user_id from dt_users
                        where dt_users.group_id like '%,{0},%'
                        and dt_users.id in ({1})", groupid, per.Remove(per.Length - 1, 1).ToString());
                DataSet ds = DbHelperSQL.Query(sql);
                DataSetToModelHelper<AtPersonnelFrombody> helper = new DataSetToModelHelper<AtPersonnelFrombody>();
                return helper.FillModel(ds);
            }
            return null;
        }

        /// <summary>
        /// 转发信息时,添加@人员信息
        /// </summary>
        /// <param name="mainId">支部或论坛信息ID</param>
        /// <param name="personnels">@人员列表</param>
        /// <param name="trans"></param>
        /// <param name="conn"></param>
        /// <param name="createUser">转发人</param>
        /// <param name="type">转发都支部还是论坛</param>
        private void AddAtPersonnels(string mainId, List<AtPersonnelFrombody> personnels, SqlTransaction trans,
            SqlConnection conn, string createUser, int type)
        {
            if (personnels != null && personnels.Count > 0)
            {
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
                    par[3].Value = type;
                    par[4].Value = DateTime.Now;
                    par[5].Value = createUser;
                    par[6].Value = 0;
                    DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), par);
                }
            }

        }

        /// <summary>
        /// 获取自己所在的党小组论坛 和 组织
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public GetGroups GetGroupsRelation(int userid)
        {
            GetGroups model = new GetGroups();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_PartyGroupForum.P_Id as forumid,P_PartyGroupForum.P_Title as forumname ");
            strsql.Append(" from P_PartyGroupForum ");
            strsql.Append(" LEFT JOIN P_PersonGroupRelation on P_PersonGroupRelation.P_PartyGroupId=P_PartyGroupForum.P_Id ");
            strsql.Append(" where P_PersonGroupRelation.P_UserId='" + userid + @"'and P_PartyGroupForum.P_Status =0 ");
            strsql.Append("and P_PersonGroupRelation.P_Approval=0 and P_PersonGroupRelation.P_Status = 0");
            DataSet ds = DbHelperSQL.Query(strsql.ToString());
            if (ds.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<GetGroupsRelation> rela = new DataSetToModelHelper<GetGroupsRelation>();
                model.relations = rela.FillModel(ds);
            }
            else
            {
                model.relations = null;
            }

            StringBuilder strsq = new StringBuilder();
            strsq.Append("select  dt_user_groups.id as groupid,dt_user_groups.title as grouptitle from F_Split( ");
            strsq.Append("(select dt_users.group_id from dt_users where dt_users.id = " + userid + @"),',') as t ");
            strsq.Append(" left join dt_user_groups on dt_user_groups.id = t.value ");
            strsq.Append(" where t.value != '' and dt_user_groups.id is not null");
            DataSet dt = DbHelperSQL.Query(strsq.ToString());
            DataSetToModelHelper<GetGroupsUser> group = new DataSetToModelHelper<GetGroupsUser>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                model.groupuser = group.FillModel(dt);
            }
            else
            {
                model.groupuser = null;
            }
            return model;


        }

    }
    public class GetGroups
    {
        public List<GetGroupsUser> groupuser { get; set; }
        public List<GetGroupsRelation> relations { get; set; }
    }
    public class GetGroupsRelation
    {
        public string forumid { get; set; }
        public string forumname { get; set; }


    }
    public class GetGroupsUser
    {
        public int groupid { get; set; }
        public string grouptitle { get; set; }
    }
    public class Trum
    {
        /// <summary>
        /// 输入的内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 转发的类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 用户的id
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 需要转发表的id
        /// </summary>
        public string raletionid { get; set; }
        /// <summary>
        /// 党组织id
        /// </summary>
        public List<group> groupid { get; set; }
        /// <summary>
        /// 党小组论坛的ID 集合
        /// </summary>

        public string forumsid { get; set; }

        /// <summary>
        /// @人员列表
        /// </summary>
        public List<AtPersonnelFrombody> at_personnels { get; set; }
    }
    public class forum
    {
        public string forumid { get; set; }
    }
    public class group
    {
        public int id { get; set; }
    }
}

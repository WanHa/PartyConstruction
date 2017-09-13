using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static DTcms.DAL.Cloudlist;

namespace DTcms.DAL
{
    public class PryCloud
    {
        private UserGroupHelper usergroup = new UserGroupHelper();
        private QiNiuHelper qiniu = new QiNiuHelper();
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Submit(CloudModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        int groupid = usergroup.GetUserMinimumGroupId(model.userid);
                        str.Append("select dt_users.user_name as name,dt_users.mobile as tel,dt_user_groups.title as branch from dt_users");
                        str.Append(" left join dt_user_groups on dt_user_groups.id =  '" + groupid + @"'");
                        str.Append(" LEFT join P_UserDemand on P_UserDemand.P_CreateUser = dt_users.id");
                        str.Append(" where dt_users.id = '" + model.userid + @"'");
                        DataSet ds = DbHelperSQL.Query(str.ToString());
                        if (model.type == 0)//图片
                        {
                            StringBuilder sql = new StringBuilder();
                            Int64 size = qiniu.GetQiNiuFileSize(model.name);
                            sql.Append("insert into P_PartyCloud(");
                            sql.Append("P_Id,P_UserId,P_Type,P_CreateTime,P_CreateUser,P_Status,P_Size)");
                            sql.Append("values (");
                            sql.Append("@P_Id,@P_UserId,@P_Type,@P_CreateTime,@P_CreateUser,@P_Status,@P_Size)");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Type", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Size", SqlDbType.BigInt)
                            };
                            string twoid = Guid.NewGuid().ToString();
                            parameters[0].Value = twoid;
                            parameters[1].Value = model.userid;
                            parameters[2].Value = model.type;
                            parameters[3].Value = DateTime.Now;
                            parameters[4].Value = model.userid;
                            parameters[5].Value = 0;
                            parameters[6].Value = size;
                            object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters); //带事务    
                                    StringBuilder st = new StringBuilder();
                                    string url = qiniu.GetQiNiuFileUrl(model.name);
                                    st.Append("insert into P_Image(");
                                    st.Append("p_id,p_imageid,p_imageUrl,P_PictureName,P_CreateTime,P_CreateUser,P_Status,P_ImageType)");
                                    st.Append(" values (");
                                    st.Append("@p_id,@p_imageid,@p_imageUrl,@P_PictureName,@P_CreateTime,@P_CreateUser,@P_Status,@P_ImageType)");
                                    SqlParameter[] param = {
                                        new SqlParameter("@p_id", SqlDbType.NVarChar,36),
                                        new SqlParameter("@p_imageid", SqlDbType.NVarChar,36),
                                        new SqlParameter("@p_imageUrl", SqlDbType.NVarChar,200),
                                        new SqlParameter("@P_PictureName", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@P_CreateUser", SqlDbType.Int,50),
                                        new SqlParameter("@P_Status", SqlDbType.Int),
                                        new SqlParameter("@P_ImageType", SqlDbType.NVarChar,50),
                                    };
                                    param[0].Value = Guid.NewGuid().ToString();
                                    param[1].Value = twoid;
                                    param[2].Value = url;
                                    param[3].Value = model.name;
                                    param[4].Value = DateTime.Now;
                                    param[5].Value = model.userid;
                                    param[6].Value = 0;
                                    param[7].Value = (int)ImageTypeEnum.党建云;
                                    object obj2 = DbHelperSQL.GetSingle(conn, trans, st.ToString(), param); //带事务
                            trans.Commit();
                        }
                        if (model.type == 1)//视频
                        {
                            StringBuilder sql = new StringBuilder();
                            Int64 size = qiniu.GetQiNiuFileSize(model.name);
                            sql.Append("insert into P_PartyCloud(");
                            sql.Append("P_Id,P_UserId,P_Type,P_CreateTime,P_CreateUser,P_Status,P_Size)");
                            sql.Append("values (");
                            sql.Append("@P_Id,@P_UserId,@P_Type,@P_CreateTime,@P_CreateUser,@P_Status,@P_Size)");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Type", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Size", SqlDbType.BigInt)
                            };
                            string twoid = Guid.NewGuid().ToString();
                            parameters[0].Value = twoid;
                            parameters[1].Value = model.userid;
                            parameters[2].Value = model.type;
                            parameters[3].Value = DateTime.Now;
                            parameters[4].Value = model.userid;
                            parameters[5].Value = 0;
                            parameters[6].Value = size;
                            object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters); //带事务    
                                    StringBuilder st = new StringBuilder();
                                    model.fileurl = qiniu.GetQiNiuFileUrl(model.name);
                                    string pic = qiniu.GetQiNiuVideoPicUrl(model.fileurl);
                                    st.Append("insert into P_Video(");
                                    st.Append("P_Id,P_ParentId,P_VideoName,P_VideoPic,P_Url,P_CreateTime,P_CreateUser,P_Status,P_Source)");
                                    st.Append(" values (");
                                    st.Append("@P_Id,@P_ParentId,@P_VideoName,@P_VideoPic,@P_Url,@P_CreateTime,@P_CreateUser,@P_Status,@P_Source)");
                                    SqlParameter[] param = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_ParentId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_VideoName", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_VideoPic", SqlDbType.NVarChar,200),
                                        new SqlParameter("@P_Url", SqlDbType.NVarChar,400),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_Status", SqlDbType.Int,4),
                                        new SqlParameter("@P_Source", SqlDbType.Int)};
                                    param[0].Value = Guid.NewGuid().ToString();
                                    param[1].Value = twoid;
                                    param[2].Value = model.name;
                                    param[3].Value = pic;
                                    param[4].Value = model.fileurl;
                                    param[5].Value = DateTime.Now;
                                    param[6].Value = model.userid;
                                    param[7].Value = 0;
                                    param[8].Value = (int)VideoSourceEnum.党建云;
                            object obj3 = DbHelperSQL.GetSingle(conn, trans, st.ToString(), param); //带事务
                            trans.Commit();
                        }
                        if (model.type == 2)//文档
                        {
                            StringBuilder sql = new StringBuilder();
                            Int64 size = qiniu.GetQiNiuFileSize(model.name);
                            model.fileurl = qiniu.GetQiNiuFileUrl(model.name);
                            sql.Append("insert into P_PartyCloud(");
                            sql.Append("P_Id,P_UserId,P_Type,P_CreateTime,P_CreateUser,P_Status,P_Size)");
                            sql.Append("values (");
                            sql.Append("@P_Id,@P_UserId,@P_Type,@P_CreateTime,@P_CreateUser,@P_Status,@P_Size)");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar),
                                new SqlParameter("@P_Type", SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Size", SqlDbType.BigInt)
                            };
                            string twoid = Guid.NewGuid().ToString();
                            parameters[0].Value = twoid;
                            parameters[1].Value = model.userid;
                            parameters[2].Value = model.type;
                            parameters[3].Value = DateTime.Now;
                            parameters[4].Value = model.userid;
                            parameters[5].Value = 0;
                            parameters[6].Value = size;
                            object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters); //带事务    
                                    StringBuilder Strsql = new StringBuilder();
                                    Strsql.Append("insert into P_Document(");
                                    Strsql.Append("P_Id,P_RelationId,P_Title,P_DocUrl,P_CreateTime,P_CreateUser,P_Status)");
                                    Strsql.Append(" values (");
                                    Strsql.Append("@P_Id,@P_RelationId,@P_Title,@P_DocUrl,@P_CreateTime,@P_CreateUser,@P_Status)");
                                    SqlParameter[] param = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_RelationId", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_Title", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_DocUrl", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_Status", SqlDbType.Int,4),
                                    };
                                    param[0].Value = Guid.NewGuid().ToString();
                                    param[1].Value = twoid;
                                    param[2].Value = model.name;
                                    param[3].Value = model.fileurl;
                                    param[4].Value = DateTime.Now;
                                    param[5].Value = model.userid;
                                    param[6].Value = 0;
                                    object obj2 = DbHelperSQL.GetSingle(conn, trans, Strsql.ToString(), param); //带事务
                            trans.Commit();
                        }
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
        /// 党建云列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Cloudlist> GetCloudList(BranchList list)
        {
            List<Cloudlist> result = new List<Cloudlist>();
            StringBuilder querySql = new StringBuilder();
            querySql.Append("select * from (");
            switch (list.type)
            {
                case 0: // 图片
                    querySql.Append("select P_PartyCloud.P_Id as fileid,P_Image.P_ImageUrl as url,dt_users.user_name as name,P_PartyCloud.P_UserId as userid,P_PictureName as title,");
                    querySql.Append("CONVERT(varchar(100),P_Image.P_CreateTime, 23) as createtime,P_PartyCloud.P_Type as filetype,P_PartyCloud.P_Size as filesize from P_PartyCloud");
                    querySql.Append(" left join P_Image on P_Image.P_ImageId= P_PartyCloud.P_Id");
                    querySql.Append(" LEFT JOIN dt_users on dt_users.id =P_PartyCloud.P_UserId");
                    querySql.Append(" where P_Type =0 and dt_users.user_name is not null ");
                    if (!String.IsNullOrEmpty(list.where)) {
                    querySql.Append(" and P_Image.P_PictureName like  '%" + list.where + @"%' ");
                    }
                    break;
                case 1: // 视频
                    querySql.Append("select P_Video.P_Id as fileid,P_Video.P_Url as url,P_Video.P_VideoPic as pic,P_Video.P_VideoName as title,dt_users.user_name as name,P_PartyCloud.P_UserId as userid,");
                    querySql.Append("CONVERT(varchar(100),P_Video.P_CreateTime, 23) as createtime,P_PartyCloud.P_Type as filetype,P_PartyCloud.P_Size as filesize from P_PartyCloud ");
                    querySql.Append(" left join P_Video on P_Video.P_ParentId= P_PartyCloud.P_Id ");
                    querySql.Append(" LEFT JOIN dt_users on dt_users.id =P_PartyCloud.P_UserId");
                    querySql.Append(" where P_Type = 1 and dt_users.user_name is not null ");
                    if (!String.IsNullOrEmpty(list.where)) {
                        querySql.Append(" and P_Video.P_VideoName like  '%" + list.where + @"%' ");
                    }
                    break;
                case 2: // 文档
                    querySql.Append("select P_Document.P_Id as fileid,P_Document.P_DocUrl as url,P_Document.P_Title as title,dt_users.user_name as name,P_PartyCloud.P_UserId as userid,");
                    querySql.Append("CONVERT(varchar(100),P_Document.P_CreateTime, 23) as createtime,P_PartyCloud.P_Type as filetype,P_PartyCloud.P_Size as filesize from P_PartyCloud");
                    querySql.Append(" left join P_Document on P_Document.P_RelationId= P_PartyCloud.P_Id  ");
                    querySql.Append(" LEFT JOIN dt_users on dt_users.id =P_PartyCloud.P_UserId ");
                    querySql.Append(" where P_Type = 2 and dt_users.user_name is not null ");
                    if (!String.IsNullOrEmpty(list.where)) {
                        querySql.Append(" and P_Document.P_Title like  '%" + list.where + @"%' ");
                    }
                    break;
                default: // 全部
                    querySql.Append("select * from (select case when P_PartyCloud.P_Type = 0 then P_Image.P_ImageUrl ");
                    querySql.Append(" when P_PartyCloud.P_Type = 1 then P_Video.P_Url ");
                    querySql.Append(" when P_PartyCloud.P_Type = 2 then P_Document.P_DocUrl end as url,");
                    querySql.Append(" case when P_PartyCloud.P_Type = 0 then P_Image.P_PictureName");
                    querySql.Append(" when P_PartyCloud.P_Type = 1 then P_Video.P_VideoName");
                    querySql.Append(" when P_PartyCloud.P_Type = 2 then P_Document.P_title end as title,");
                    querySql.Append(" case when P_PartyCloud.P_Type = 1 then P_Video.P_VideoPic end as pic,");
                    querySql.Append(" P_PartyCloud.P_Type as filetype,P_PartyCloud.P_Size as filesize,");
                    querySql.Append("P_PartyCloud.P_Id as fileid,P_PartyCloud.P_UserId as userid,");
                    querySql.Append("(select user_name from dt_users where id =P_PartyCloud.P_UserId )as name,");
                    querySql.Append("CONVERT(varchar(100),P_PartyCloud.P_CreateTime, 23) as createtime ");
                    querySql.Append(" from P_PartyCloud ");
                    querySql.Append(" LEFT JOIN P_Image on P_Image.P_ImageId = P_PartyCloud.P_Id");
                    querySql.Append(" LEFT JOIN P_Video on P_Video.P_ParentId = P_PartyCloud.P_Id");
                    querySql.Append(" LEFT JOIN P_Document on P_Document.P_RelationId = P_PartyCloud.P_Id) as t");
                    querySql.Append(" where title like '%" + list.where + @"%'");
                    break;
            }
            querySql.Append(" ) as table1");
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(list.rows, list.page, querySql.ToString(), "fileid"));
            DataSetToModelHelper<Cloudlist> helper = new DataSetToModelHelper<Cloudlist>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                result = helper.FillModel(ds);
                foreach (Cloudlist item in result)
                {
                    StringBuilder groupSql = new StringBuilder();
                    groupSql.Append("select  dt_user_groups.id as groupid,dt_user_groups.title as branch from F_Split( ");
                    groupSql.Append("(select dt_users.group_id from dt_users where dt_users.id = " + list.userid + @"),',') as t ");
                    groupSql.Append(" left join dt_user_groups on dt_user_groups.id = t.value ");
                    groupSql.Append(" where t.value is not null and dt_user_groups.id is not null");
                    DataSet groupDs = DbHelperSQL.Query(groupSql.ToString());
                    DataSetToModelHelper<Group> groupHelper = new DataSetToModelHelper<Group>();
                    List<Group> groups = groupHelper.FillModel(groupDs);
                    item.groups = (groups == null ? new List<Group>() : groups);
                }
            }
            return result;
        }
        /// <summary>
        /// 分享党组织接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean GetBranch(BranchList model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string shareId = Guid.NewGuid().ToString();
                        if(model.type == 1)//视频
                        {
                            StringBuilder strsql = new StringBuilder();
                            strsql.Append("Insert into P_PartyCloudShare(");
                            strsql.Append("P_Id,P_BranchId,P_CreaterId,P_CreateTime,P_Status,P_Type,P_PartyCloudId)");
                            strsql.Append(" values (");
                            strsql.Append("@P_Id,@P_BranchId,@P_CreaterId,@P_CreateTime,@P_Status,@P_Type,@P_PartyCloudId)");
                            SqlParameter[] param = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,400),
                                        new SqlParameter("@P_BranchId", SqlDbType.NVarChar,400),
                                        new SqlParameter("@P_CreaterId", SqlDbType.Int),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@P_Status", SqlDbType.Int),
                                        new SqlParameter("@P_Type", SqlDbType.Int),
                                        new SqlParameter("@P_PartyCloudId", SqlDbType.NVarChar,100),
                            };
                            param[0].Value = shareId;
                            param[1].Value = model.groupid;
                            param[2].Value = model.userid;
                            param[3].Value = DateTime.Now;
                            param[4].Value = 0;
                            param[5].Value = 1;
                            param[6].Value = model.id;
                            object obj2 = DbHelperSQL.GetSingle(conn, trans, strsql.ToString(), param); //带事务
                        }
                        if(model.type == 0)//图片
                        {
                            StringBuilder strsql = new StringBuilder();
                            strsql.Append("Insert into P_PartyCloudShare(");
                            strsql.Append("P_Id,P_BranchId,P_CreaterId,P_CreateTime,P_Status,P_Type,P_PartyCloudId)");
                            strsql.Append(" values (");
                            strsql.Append("@P_Id,@P_BranchId,@P_CreaterId,@P_CreateTime,@P_Status,@P_Type,@P_PartyCloudId)");
                            SqlParameter[] param = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_BranchId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreaterId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@P_Status", SqlDbType.Int,4),
                                        new SqlParameter("@P_Type", SqlDbType.Int,4),
                                        new SqlParameter("@P_PartyCloudId", SqlDbType.NVarChar,100),
                            };
                            param[0].Value = shareId;
                            param[1].Value = model.groupid;
                            param[2].Value = model.userid;
                            param[3].Value = DateTime.Now;
                            param[4].Value = 0;
                            param[5].Value = 0;
                            param[6].Value = model.id;
                            object obj2 = DbHelperSQL.GetSingle(conn, trans, strsql.ToString(), param); //带事务
                        }
                        if(model.type == 2)//文档
                        {
                            StringBuilder strsql = new StringBuilder();
                            strsql.Append("Insert into P_PartyCloudShare(");
                            strsql.Append("P_Id,P_BranchId,P_CreaterId,P_CreateTime,P_Status,P_Type,P_PartyCloudId)");
                            strsql.Append(" values (");
                            strsql.Append("@P_Id,@P_BranchId,@P_CreaterId,@P_CreateTime,@P_Status,@P_Type,@P_PartyCloudId)");
                            SqlParameter[] param = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_BranchId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreaterId", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@P_Status", SqlDbType.Int,4),
                                        new SqlParameter("@P_Type", SqlDbType.Int,4),
                                        new SqlParameter("@P_PartyCloudId", SqlDbType.NVarChar,100),
                            };
                            param[0].Value = shareId;
                            param[1].Value = model.groupid;
                            param[2].Value = model.userid;
                            param[3].Value = DateTime.Now;
                            param[4].Value = 0;
                            param[5].Value = 2;
                            param[6].Value = model.id;
                            object obj2 = DbHelperSQL.GetSingle(conn, trans, strsql.ToString(), param); //带事务
                        }

                        StringBuilder branchInsertSql = new StringBuilder();
                        branchInsertSql.Append("insert into P_BranchPublish");
                        branchInsertSql.Append(" (P_id,P_BranchId,P_UserId,P_CreateTime,P_Status,P_Source,P_SourceId)");
                        branchInsertSql.Append(" values(@P_id,@P_BranchId,@P_UserId,@P_CreateTime,@P_Status,@P_Source,@P_SourceId)");

                        SqlParameter[] branchPar = {
                            new SqlParameter("@P_id",SqlDbType.NVarChar, 50),
                            new SqlParameter("@P_BranchId",SqlDbType.NVarChar, 50),
                            new SqlParameter("@P_UserId",SqlDbType.NVarChar, 50),
                            new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_Status",SqlDbType.Int, 4),
                            new SqlParameter("@P_Source",SqlDbType.Int, 4),
                            new SqlParameter("@P_SourceId",SqlDbType.NVarChar, 50)
                        };
                        branchPar[0].Value = Guid.NewGuid().ToString();
                        branchPar[1].Value = model.groupid;
                        branchPar[2].Value = model.userid;
                        branchPar[3].Value = DateTime.Now;
                        branchPar[4].Value = 0;
                        branchPar[5].Value = 2; // 分享
                        branchPar[6].Value = shareId;
                        DbHelperSQL.GetSingle(conn, trans, branchInsertSql.ToString(), branchPar); //带事务
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
        /// 点击分享获取党组织的接口
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public GetGroup ClickShare(int userid)
        {
            GetGroup model = new GetGroup();
            StringBuilder strsq = new StringBuilder();
            strsq.Append("select  dt_user_groups.id as groupid,dt_user_groups.title as grouptitle from F_Split( ");
            strsq.Append("(select dt_users.group_id from dt_users where dt_users.id = " + userid + @"),',') as t ");
            strsq.Append(" left join dt_user_groups on dt_user_groups.id = t.value ");
            strsq.Append(" where t.value is not null and dt_user_groups.id is not null");
            DataSet dt = DbHelperSQL.Query(strsq.ToString());
            DataSetToModelHelper<GetGroupsUsers> group = new DataSetToModelHelper<GetGroupsUsers>();
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
        /// <summary>
        /// 点击党建云获取数量
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Count> GetSum()
        {
            Count model = new Count();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_Type as types,count(P_Type)as sumcount from P_PartyCloud ");
            strsql.Append(" where P_UserId in (select id from dt_users) group by P_Type");
            DataSet data = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<Count> sum = new DataSetToModelHelper<Count>();
            List<Count> result = new List<Count>();
            if (data.Tables[0].Rows.Count != 0)
            {
                result = sum.FillModel(data);
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
    public class Group
    {
        public int groupid { get; set; }
        public string branch { get; set; }
    }
    public class Count
    {
        public int types { get; set; }
        public int sumcount { get; set; }
    }
    public class GetGroup
    {
        public List<GetGroupsUsers> groupuser { get; set; }
    }
    public class BranchList
    {
        public int page { get; set; }
        public int rows { get; set; }
        public int groupid { get; set; }
        public int type { get; set; }
        public int userid { get; set; }
        public string where { get; set; }
        public string id { get; set; }
    }
    public class GetGroupsUsers
    {
        public int groupid { get; set; }
        public string grouptitle { get; set; }
    }
    public class Cloudlist
    {
        public Int64 filesize { get; set; }
        public string fileid { get; set; }
        public int userid { get; set; }
        public string name { get; set; }
        public List<Group>groups  { get; set; }
        public string createtime { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string pic { get; set; }
        public int filetype { get; set; }
    }
    public class CloudModel
    {
        public int gid { get; set; }
        public int userid { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string fileurl { get; set; }
        public string pic { get; set; }
        public List<Image> url { get; set; }
        public List<Video> vurl { get; set; }
        public List<Document> docu { get; set; }
    }
    public class Document
    {
        public string title { get; set; }
        public string document { get; set; }
    }
    public class Video
    {
        public string vname { get; set; }
        public string videourl { get; set; }
    }
    public class Image
    {
        public string iname { get; set; }
        public string imgurl { get; set; }
    }
}

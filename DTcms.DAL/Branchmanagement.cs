using DTcms.Common;
using DTcms.DBUtility;
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
    public class Branchmanagement
    {
        private QiNiuHelper qiniu = new QiNiuHelper();
        /// <summary>
        /// 获取党组织列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<Banch> GetBranchList(GroupManagement group)
        {
            List<Banch> model = new List<Banch>();
            StringBuilder sql = new StringBuilder();
            if (group.type == 0)
            {
                sql.Append("select  dt_user_groups.title as groupname,dt_user_groups.id as groupid,dt_user_groups.manager_id as managerid,  isnull(P_Image.P_ImageUrl,'') as imageurl  from F_Split(");
                sql.Append("(select dt_users.group_id from dt_users where dt_users.id = '" + group.userid + @"'),',') as t ");
                sql.Append(" left join dt_user_groups on dt_user_groups.id = t.value ");
                sql.Append(" left join P_Image on P_Image.P_ImageId = dt_user_groups.id and P_Image.P_ImageType=" + (int)ImageTypeEnum.支部封面+ " and P_Image.P_Status = " + 0); //新增封面
                sql.Append(" where t.value != ''");
                DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(group.rows, group.page, sql.ToString(), "dt_user_groups.id"));
                DataSetToModelHelper<Banch> gp = new DataSetToModelHelper<Banch>();
                if (dt.Tables[0].Rows.Count != 0)
                {
                    model = gp.FillModel(dt);
                }
                else
                {
                    model = null;
                }
            }
            if (group.type == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id as groupid,title as groupname,dt_user_groups.manager_id as managerid," +
                                         " isnull(P_Image.P_ImageUrl,'') as imageurl, " +                                     //修改
                                         " case when id in (select t.value from F_Split(");
                strSql.Append(" (select dt_users.group_id from dt_users where dt_users.id = '" + group.userid + @"'),',') as t");
                strSql.Append(" left join dt_user_groups on dt_user_groups.id = t.value where t.value != '')");
                strSql.Append(" then 1 else 0 end as status from dt_user_groups " +
                                          " LEFT JOIN P_Image on P_Image.P_ImageId = convert(nvarchar,dt_user_groups.id) and P_Image.P_ImageType=" + (int)ImageTypeEnum.支部封面 + " and P_Image.P_Status = " + 0 + //修改 
                                          " where is_lock=0 and pid is null");
                DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(group.rows, group.page, strSql.ToString(), "dt_user_groups.id"));
                DataSetToModelHelper<Banch> grp = new DataSetToModelHelper<Banch>();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    model = grp.FillModel(ds);
                }
                else
                {
                    model = null;
                    return model;
                }
                foreach (Banch item in model)
                {
                    GetBranchList(item,group.userid);
                }

            }
            return model;
        }
        /// <summary>
        /// 个人介绍接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public Details GetDetails(int userid, int groupid)
        {
            Details model = new Details();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select dt_users.user_name as name,dt_users.mobile as tel,case when dt_users.sex = 0 then '男' else '女' end as sex,dt_users.nation as nation,P_Image.P_ImageUrl as ava,");
            strsql.Append("CONVERT(varchar(100), dt_users.join_party_time, 23) as jointime,u_company_type.name as work,dt_user_groups.title as branch");
            strsql.Append(" from dt_users LEFT JOIN dt_user_groups on dt_user_groups.id = '" + groupid + @"'");
            strsql.Append(" LEFT JOIN P_Image on P_ImageId = dt_users.id and P_ImageType='20011'");
            strsql.Append(" left join u_company_type on u_company_type.id = dt_users.now_company_id");
            strsql.Append(" where dt_users.id = '" + userid + @"'");
            DataSet da = DbHelperSQL.Query(strsql.ToString());
            if (da.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<Details> mo = new DataSetToModelHelper<Details>();
                model = mo.FillToModel(da.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 发布文字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Addchar(Release model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string p_id = Guid.NewGuid().ToString();
                        StringBuilder strsql = new StringBuilder();
                        strsql.Append("insert into P_BranchPublish(");
                        strsql.Append("P_Id,P_BranchId,P_UserId,P_Content,P_CreateTime,P_Status,P_Type,P_Source)");
                        strsql.Append("values (");
                        strsql.Append("@P_Id,@P_BranchId,@P_UserId,@P_Content,@P_CreateTime,@P_Status,@P_Type,@P_Source)");
                        SqlParameter[] parameters = {
                            new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_BranchId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UserId", SqlDbType.Int),
                            new SqlParameter("@P_Content", SqlDbType.NText),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_Status", SqlDbType.Int),
                            new SqlParameter("@P_Type", SqlDbType.Int),
                            new SqlParameter("@P_Source", SqlDbType.Int)};

                        parameters[0].Value = p_id;
                        parameters[1].Value = model.groupid;
                        parameters[2].Value = model.userid;
                        parameters[3].Value = model.content;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = 0;
                        parameters[6].Value = 0;
                        parameters[7].Value = 0;//来源支部发布
                        object obj = DbHelperSQL.ExecuteSql(conn, trans, strsql.ToString(), parameters);
                        AddAtData(p_id, model.at_personnels, trans, conn, model.userid.ToString());
                        trans.Commit();
                        if (model.at_personnels != null && model.at_personnels.Count > 0)
                        {
                            List<int> per = new List<int>();
                            foreach (var item in model.at_personnels)
                            {
                                per.Add(item.user_id);
                            }

                            PushMessageHelper.PushMessages(p_id, "您收到一条@信息.", per, model.userid, (int)PushTypeEnum.AT);
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
        /// 支部发布信息时,添加@人员信息
        /// </summary>
        /// <param name="mainId"></param>
        /// <param name="personnels"></param>
        /// <param name="trans"></param>
        /// <param name="conn"></param>
        /// <param name="createUser"></param>
        private void AddAtData(string mainId, List<AtPersonnelFrombody> personnels, SqlTransaction trans, SqlConnection conn, string createUser)
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
                    par[3].Value = (int)AtTypeEnum.支部发布;
                    par[4].Value = DateTime.Now;
                    par[5].Value = createUser;
                    par[6].Value = 0;
                    DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), par);
                }
            }

        }

        /// <summary>
        /// 发布文字图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean AddImage(Release model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("insert into P_BranchPublish(");
                        sql.Append("P_Id,P_BranchId,P_UserId,P_Content,P_CreateTime,P_Status,P_Type,P_Source)");
                        sql.Append("values (");
                        sql.Append("@P_Id,@P_BranchId,@P_UserId,@P_Content,@P_CreateTime,@P_Status,@P_Type,@P_Source)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_BranchId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.Int,4),
                                new SqlParameter("@P_Content", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Type", SqlDbType.Int,4),
                                new SqlParameter("@P_Source", SqlDbType.Int,4),
                            };
                        string twoid = Guid.NewGuid().ToString();
                        parameters[0].Value = twoid;
                        parameters[1].Value = model.groupid;
                        parameters[2].Value = model.userid;
                        parameters[3].Value = model.content;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = 0;
                        parameters[6].Value = 1;//图片文字
                        parameters[7].Value = 0;//来源支部发布
                        object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters); //带事务    
                        foreach (var item in model.im)
                        {
                            StringBuilder st = new StringBuilder();
                            string imgurl = qiniu.GetQiNiuFileUrl(item.iname);
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
                                        new SqlParameter("@P_ImageType", SqlDbType.NVarChar,50)};
                            param[0].Value = Guid.NewGuid().ToString();
                            param[1].Value = twoid;
                            param[2].Value = imgurl;
                            param[3].Value = item.iname;
                            param[4].Value = DateTime.Now;
                            param[5].Value = model.userid;
                            param[6].Value = 0;
                            param[7].Value = (int)ImageTypeEnum.支部管理分享;
                            object obj2 = DbHelperSQL.GetSingle(conn, trans, st.ToString(), param); //带事务
                        }
                        AddAtData(twoid, model.at_personnels, trans, conn, model.userid.ToString());
                        trans.Commit();
                        if (model.at_personnels != null && model.at_personnels.Count > 0)
                        {
                            List<int> per = new List<int>();
                            foreach (var item in model.at_personnels)
                            {
                                per.Add(item.user_id);
                            }

                            PushMessageHelper.PushMessages(twoid, "您收到一条@信息.", per, model.userid, (int)PushTypeEnum.AT);
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
        /// 发布视频文字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean AddVideo(Release model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("insert into P_BranchPublish(");
                        sql.Append("P_Id,P_BranchId,P_UserId,P_Content,P_CreateTime,P_Status,P_Type,P_Source)");
                        sql.Append("values (");
                        sql.Append("@P_Id,@P_BranchId,@P_UserId,@P_Content,@P_CreateTime,@P_Status,@P_Type,@P_Source)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_BranchId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UserId", SqlDbType.Int,4),
                                new SqlParameter("@P_Content", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_Type", SqlDbType.Int,4),
                                new SqlParameter("@P_Source", SqlDbType.Int,4),
                            };
                        string twoid = Guid.NewGuid().ToString();
                        parameters[0].Value = twoid;
                        parameters[1].Value = model.groupid;
                        parameters[2].Value = model.userid;
                        parameters[3].Value = model.content;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = 0;
                        parameters[6].Value = 2;//视频文字
                        parameters[7].Value = 0;//来源支部发布
                        object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters); //带事务

                        StringBuilder st = new StringBuilder();
                        model.videourl = qiniu.GetQiNiuFileUrl(model.vname);
                        string vpicture = qiniu.GetQiNiuVideoPicUrl(model.videourl);
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
                                        new SqlParameter("@P_Source", SqlDbType.Int, 4)};
                        param[0].Value = Guid.NewGuid().ToString();
                        param[1].Value = twoid;
                        param[2].Value = model.vname;
                        param[3].Value = vpicture;
                        param[4].Value = model.videourl;
                        param[5].Value = DateTime.Now;
                        param[6].Value = model.userid;
                        param[7].Value = 0;
                        param[8].Value = (int)VideoSourceEnum.支部;
                        object obj3 = DbHelperSQL.GetSingle(conn, trans, st.ToString(), param); //带事务
                        AddAtData(twoid, model.at_personnels, trans, conn, model.userid.ToString());
                        trans.Commit();
                        if (model.at_personnels != null && model.at_personnels.Count > 0)
                        {
                            List<int> per = new List<int>();
                            foreach (var item in model.at_personnels)
                            {
                                per.Add(item.user_id);
                            }

                            PushMessageHelper.PushMessages(twoid, "您收到一条@信息.", per, model.userid, (int)PushTypeEnum.AT);
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
        /// 获取组员列表
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<PersonList> GetMembersList(string groupid, int rows, int page)
        {
            List<PersonList> model = new List<PersonList>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id as userid,user_name as username,role as role,mobile from (select dt_users.*,");
            strSql.Append("(select case when dt_user_groups.id is not null then 1 else 0 end from dt_user_groups");
            strSql.Append(" where dt_user_groups.id = '" + groupid + @"' and dt_user_groups.manager_id = dt_users.id) as role from dt_users");
            strSql.Append(" where dt_users.group_id like '%" + groupid + @"%') table1 ");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strSql.ToString(), "table1.role desc"));
            DataSetToModelHelper<PersonList> result = new DataSetToModelHelper<PersonList>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                model = result.FillModel(dt);
                foreach (var item in model)
                {
                    StringBuilder sr = new StringBuilder();
                    sr.Append("select P_ImageUrl as avatar from P_Image where P_ImageId = '" + item.userid + @"' and P_ImageType='20011'");
                    DataSet data = DbHelperSQL.Query(sr.ToString());
                    DataSetToModelHelper<PersonList> mo = new DataSetToModelHelper<PersonList>();
                    PersonList aa = new PersonList();
                    if (data.Tables[0].Rows.Count != 0)
                    {
                        aa = mo.FillToModel(data.Tables[0].Rows[0]);
                        item.avatar = aa.avatar;
                    }
                    else
                    {
                        aa = null;
                    }
                }
            }
            else
            {
                model = null;
                return model;
            }
            //foreach (var item in model)
            //{
            //    if (item.userid == item.mid)
            //    {
            //        item.role = 0;//书记
            //    }
            //    if (item.userid != item.mid)
            //    {
            //        item.role = 1;//党员
            //    }
            //}
            return model;
        }
        /// <summary>
        /// 获取党组织简介接口
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public BranchLists GetBranch(string groupid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select title as bname,manager as name,(select count(id) from dt_users where group_id like ',%" + groupid + @"%,') as sum, ");
            strSql.Append(" intre as content from dt_user_groups where id = '" + groupid + @"'");
            DataSet info = DbHelperSQL.Query(strSql.ToString());
            DataSetToModelHelper<BranchLists> helper = new DataSetToModelHelper<BranchLists>();
            BranchLists model = new BranchLists();
            if (info.Tables[0].Rows.Count != 0)
            {
                model = helper.FillToModel(info.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 递归获取党组织
        /// </summary>
        /// <param name="item"></param>
        public void GetBranchList(Banch item,int userid)
        {
            List<Banch> mo = new List<Banch>(); 
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id as groupid,title as groupname,dt_user_groups.manager_id as managerid," +
                                     " isnull(P_Image.P_ImageUrl,'') as imageurl, " +                                     //修改
                                     " case when id in (select t.value from F_Split(");
            strSql.Append("(select dt_users.group_id from dt_users where dt_users.id = '" + userid + @"'),',') as t");
            strSql.Append(" left join dt_user_groups on dt_user_groups.id = t.value where t.value != '')");
            strSql.Append(" then 1 else 0 end as status from dt_user_groups " +
                                      " LEFT JOIN P_Image on P_Image.P_ImageId = convert(nvarchar,dt_user_groups.id) and P_Image.P_ImageType=" + (int)ImageTypeEnum.支部封面 + " and P_Image.P_Status = " + 0 + //修改 
                                      " where pid = '" + item.groupid + "' order by dt_user_groups.id");
            //DataSet data = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strSql.ToString(), "dt_user_groups.id"));
            DataSet data = DbHelperSQL.Query(strSql.ToString());
            DataSetToModelHelper<Banch> grp = new DataSetToModelHelper<Banch>();
            if (data.Tables[0].Rows.Count != 0)
            {
                mo = grp.FillModel(data);
                if (mo != null && mo.Count > 0)
                {
                    foreach (var child in mo)
                    {
                        GetBranchList(child,userid);
                    }
                }
                item.childlist = mo;
            }
            else
            {
                mo = null;
            }
        }
        /// <summary>
        /// 搜索党支部接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<BLists> SeachBranch(Where model)
        {
            List<BLists> mo = new List<BLists>();
            StringBuilder sql = new StringBuilder();
                sql.Append("select dt_user_groups.title as branchname,dt_user_groups.id as branchid,dt_user_groups.manager_id as mid,P_Image.P_ImageUrl as imgurl,");
                sql.Append(" case when id in (select t.value from F_Split((select dt_users.group_id from dt_users where dt_users.id = '"+model.userid+"'),',') as t ");
                sql.Append(" left join dt_user_groups on dt_user_groups.id = t.value where t.value != '') then 1 else 0 end as status ");
                sql.Append(" from dt_user_groups ");
                sql.Append(" left JOIN P_Image on P_Image.P_ImageId=dt_user_groups.id and P_Image.P_ImageType='20015' and P_Image.P_Status = 0");
                sql.Append(" where title like '%" + model.search + @"%'");
                DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql( model.rows, model.page, sql.ToString(), "dt_user_groups.create_time"));
                DataSetToModelHelper<BLists> gp = new DataSetToModelHelper<BLists>();
                if (dt.Tables[0].Rows.Count != 0)
                {
                    mo = gp.FillModel(dt);
                }
                else
                {
                    mo = null;
                }
            return mo;
        }
    }
    public class GroupManagement
    {
        public int userid { get; set; }
        public int type { get; set; }
        public int rows { get; set; }
        public int page { get; set; }
    }
    public class Banch
    {
        public int groupid { get; set; }
        public string groupname { get; set; }
        public int managerid { get; set; }
        public int status { get; set; }
        public List<Banch> childlist = new List<Banch>();
        public string imageurl { get; set; }
    }
    public class Details
    {
        public string ava { get; set; }
        public string branch { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string nation { get; set; }
        public string tel { get; set; }
        public string jointime { get; set; }
        public string work { get; set; }
    }
    public class Release
    {
        public int groupid { get; set; }
        public string content { get; set; }
        public int userid { get; set; }
        public int types { get; set; }
        public List<Images> im { get; set; }
        public string videourl { get; set; }
        public string vname { get; set; }
        /// <summary>
        /// @人员列表
        /// </summary>
        public List<AtPersonnelFrombody> at_personnels { get; set; }
    }
    public class Images
    {
        public string iname { get; set; }
    }
    public class PersonList
    {

        public int userid { get; set; }
        public string username { get; set; }
        //public int mid { get; set; }
        public int role { get; set; }
        public string avatar { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string mobile { get; set; }
    }
    public class BranchLists
    {
        public string bname { get; set; }
        public string name { get; set; }
        public int sum { get; set; }
        public string content { get; set; }
    }
    public class BLists
    {
        public int mid { get; set; }
        public string imgurl { get; set; }
        public int status { get; set; }
        public int branchid { get; set; }
        public string branchname { get; set; }
    }
    public class Where
    {
        public int userid { get; set; }
        public string search { get; set; }
        public int rows { get; set; }
        public int page { get; set; }
    }
}

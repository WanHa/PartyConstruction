using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DTcms.DAL
{
    public class Ptytrain
    {
        private UserGroupHelper usergroup = new UserGroupHelper();
        private QiNiuHelper qiniu = new QiNiuHelper();
        /// <summary>
        /// 党员诉求列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="asstatus"></param>
        /// <returns></returns>
        public List<member> GetPartytrainList(int userid, int rows, int page, int asstatus) 
        {
            List<member> model = new List<member>();
            StringBuilder stsql = new StringBuilder();
            int groupid = usergroup.GetUserMinimumGroupId(userid);
            //判断--处理中/已办结
            if (asstatus == 0)//处理中
                {
                stsql.Append("select * from (");
                stsql.Append("select P_CreateUser as createid,P_Content as content,P_Id as id,P_CheckStatus as checkstatus,CONVERT(varchar(100),P_CreateTime, 23) as createtime,P_Urgent as urgentstatus,P_CreateTime,");
                stsql.Append("(select dt_user_groups.title from dt_user_groups  left JOIN dt_users on dt_user_groups.id ='" + groupid + @"' where dt_users.id = '" + userid + @"') as branch,");
                stsql.Append("(select mobile from dt_users where id=(select manager_id from dt_user_groups  LEFT JOIN dt_users on dt_user_groups.id = '" + groupid + @"' where dt_users.id = '" + userid + @"')) as tel,");
                stsql.Append("(select manager from dt_user_groups where id='"+ groupid + "') as verifier,");
                stsql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb");
                stsql.Append(" from P_UserDemand where P_CreateUser= '" + userid + @"') as b");
                stsql.Append(" where (b.checkstatus =-1 and b.bbbb = 0 ) or (b.checkstatus=1 and b.bbbb =2)");
                DataSet dd = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, stsql.ToString(), "b.P_CreateTime desc"));
                DataSetToModelHelper<member> list = new DataSetToModelHelper<member>();
                if(dd.Tables[0].Rows.Count!=0)
                {
                    model = list.FillModel(dd);
                }
                else
                {
                    model = null;
                    return model;
                }
                foreach (var item in model)
                {
                    StringBuilder strsq = new StringBuilder();
                    strsq.Append("select P_Image.P_ImageUrl as image from P_Image");
                    strsq.Append(" where P_Image.P_ImageId ='" + item.id + @"'");
                    DataSet dat = DbHelperSQL.Query(strsq.ToString());
                    if (dat.Tables[0].Rows.Count!=0)
                    {
                        DataSetToModelHelper<photo> photo = new DataSetToModelHelper<photo>();
                        item.Photolist = photo.FillModel(dat);
                    }
                    else
                    {
                        item.Photolist = null;
                    }
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("select P_UserDemandSublist.P_Content as rcontent,CONVERT(varchar(100),P_UserDemandSublist.P_CreateTime, 23) as rcreatetime");
                    strsql.Append(" from P_UserDemandSublist where P_UserDemandSublist.P_UDId ='" + item.id + @"'");
                    strsql.Append(" ORDER BY P_UserDemandSublist.P_CreateTime Asc");
                    DataSet da = DbHelperSQL.Query(strsql.ToString());
                    if (da.Tables[0].Rows.Count!= 0)
                    {
                        DataSetToModelHelper<con> con = new DataSetToModelHelper<con>();
                        item.Conlist = con.FillModel(da);
                    }
                    else
                    {
                        item.Conlist = null;
                    }
                }
            }
            if (asstatus == 1)//已办结
                {
                    stsql.Append("select * from (");
                    stsql.Append("select P_CreateUser as createid,P_Content as content,P_Id as id,P_CheckStatus as checkstatus,CONVERT(varchar(100),P_CreateTime, 23) as createtime,P_CreateTime,P_Urgent as urgentstatus,");
                    stsql.Append("(select dt_user_groups.title from dt_user_groups  left JOIN dt_users on dt_user_groups.id ='" + groupid + @"' where dt_users.id = '" + userid + @"') as branch,");
                    stsql.Append("(select mobile from dt_users where id=(select manager_id from dt_user_groups  LEFT JOIN dt_users on dt_user_groups.id = '" + groupid + @"' where dt_users.id = '" + userid + @"')) as tel,");
                    stsql.Append("(select manager from dt_user_groups where id='" + groupid + "') as verifier,");
                    stsql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb");
                    stsql.Append(" from P_UserDemand where P_CreateUser= '" + userid + @"') as b");
                    stsql.Append(" where (b.checkstatus =-1 and b.bbbb =1)or (b.checkstatus=1 and b.bbbb =3)or (b.checkstatus =0 and b.bbbb=1)");
                    DataSet dd = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, stsql.ToString(), "b.P_CreateTime desc"));
                    DataSetToModelHelper<member> list = new DataSetToModelHelper<member>();
                    if(dd.Tables[0].Rows.Count!=0)
                    {
                        model = list.FillModel(dd);
                    }
                    else
                    {
                        model = null;
                        return model;
                    }
                    foreach (var item in model)
                    {
                        StringBuilder strsq = new StringBuilder();
                        strsq.Append("select P_Image.P_ImageUrl as image from P_Image");
                        strsq.Append(" where P_Image.P_ImageId ='" + item.id + @"'");
                        DataSet dat = DbHelperSQL.Query(strsq.ToString());
                        if (dat != null)
                        {
                            DataSetToModelHelper<photo> photo = new DataSetToModelHelper<photo>();
                            item.Photolist = photo.FillModel(dat);
                        }
                        else
                        {
                            item.Photolist = null;
                        }
                    StringBuilder ss = new StringBuilder();
                    ss.Append("select P_UserDemandSublist.P_Content as rcontent,CONVERT(varchar(100),P_UserDemandSublist.P_CreateTime, 23) as rcreatetime");
                    ss.Append(" from P_UserDemandSublist where P_UserDemandSublist.P_UDId ='" + item.id + @"'");
                    //ss.Append(" where P_UserDemandSublist.P_CreateUser ='" + item.createid + @"'");
                    ss.Append(" ORDER BY P_UserDemandSublist.P_CreateTime Asc");
                    DataSet da = DbHelperSQL.Query(ss.ToString());
                    if (da != null)
                    {
                        DataSetToModelHelper<con> con = new DataSetToModelHelper<con>();
                        item.Conlist = con.FillModel(da);
                    }
                }
            }   
                return model;
        }
        /// <summary>
        /// 审核人列表接口
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="asstatus"></param>
        /// <returns></returns>
        public List<Trainlist> GetLeadList(int userid,int rows, int page, int asstatus)
        {
            List<Trainlist> model = new List<Trainlist>();
            StringBuilder stsql = new StringBuilder();
            //int groupid = usergroup.GetUserMinimumGroupId(userid);
            if (asstatus == 0)//待处理
            {
                stsql.Append("select * from (");
                stsql.Append("select P_UserDemand.P_Id as id,dt_users.user_name as name,CONVERT(varchar(100), P_UserDemand.P_CreateTime, 23) as createtime,");
                stsql.Append("dt_users.mobile as tel,P_UserDemand.P_CreateUser as createid,P_UserDemand.P_CheckStatus as checkstatus,");
                stsql.Append("P_UserDemand.P_Content as content,dt_user_groups.title as branch,P_UserDemand.P_Urgent as urgentstatus,P_UserDemand.P_Reveiver,");
                stsql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb");
                stsql.Append(" from P_UserDemand LEFT JOIN dt_users on P_UserDemand.P_CreateUser = dt_users.id");
                stsql.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id =(select TOP 1 t.value from F_Split( ");
                stsql.Append(" (select dt_users.group_id from dt_users where dt_users.id = P_UserDemand.P_CreateUser),',') as t ");
                stsql.Append(" left join dt_user_groups on dt_user_groups.id = t.value");
                stsql.Append(" where t.value != '' order by dt_user_groups.grade DESC)) as b");
                stsql.Append(" where ((b.checkstatus = -1 and b.bbbb = 0) or (b.checkstatus = 1 and b.bbbb = 2)) and b.P_Reveiver = '"+userid+"'");
                DataSet dd = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, stsql.ToString(), "b.createtime desc"));
                DataSetToModelHelper<Trainlist> list = new DataSetToModelHelper<Trainlist>();
                if(dd.Tables[0].Rows.Count!=0)
                {
                    model = list.FillModel(dd.Tables[0]);
                }
                else
                {
                    model = null;
                    return model;
                }
                foreach (var item in model)
                {
                    StringBuilder strsq = new StringBuilder();
                    strsq.Append("select P_Image.P_ImageUrl as image from P_Image");
                    strsq.Append(" where P_Image.P_ImageId ='" + item.id + @"'");
                    DataSet dat = DbHelperSQL.Query(strsq.ToString());
                    if (dat.Tables[0].Rows.Count!=0)
                    {
                        DataSetToModelHelper<photo> photo = new DataSetToModelHelper<photo>();
                        item.Photolist = photo.FillModel(dat);
                    }
                    else
                    {
                        item.Photolist = null;
                    }
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("select P_UserDemandSublist.P_Content as rcontent,CONVERT(varchar(100),P_UserDemandSublist.P_CreateTime, 23) as rcreatetime");
                    strsql.Append(" from P_UserDemandSublist where P_UserDemandSublist.P_UDId ='" + item.id + @"'");
                    strsql.Append(" ORDER BY P_UserDemandSublist.P_CreateTime Asc");
                    DataSet da = DbHelperSQL.Query(strsql.ToString());
                    if (da.Tables[0].Rows.Count != 0)
                    {
                        DataSetToModelHelper<con> con = new DataSetToModelHelper<con>();
                        item.Conlist = con.FillModel(da);
                    }
                    else
                    {
                        item.Conlist = null;
                    }
                }
            }
            if (asstatus == 1)//已办结
                {
                    stsql.Append("select * from (");
                    stsql.Append("select P_UserDemand.P_Id as id,dt_users.user_name as name,CONVERT(varchar(100), P_UserDemand.P_CreateTime, 23) as createtime,P_CreateTime,");
                    stsql.Append("dt_users.mobile as tel,P_UserDemand.P_CreateUser as createid,P_UserDemand.P_CheckStatus as checkstatus,");
                    stsql.Append("P_UserDemand.P_Content as content,dt_user_groups.title as branch,P_UserDemand.P_Urgent as urgentstatus,P_UserDemand.P_Reveiver,");
                    stsql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb");
                    stsql.Append(" from P_UserDemand LEFT JOIN dt_users on P_UserDemand.P_CreateUser = dt_users.id");
                    stsql.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id =(select TOP 1 t.value from F_Split( ");
                    stsql.Append(" (select dt_users.group_id from dt_users where dt_users.id = P_UserDemand.P_CreateUser),',') as t ");
                    stsql.Append(" left join dt_user_groups on dt_user_groups.id = t.value");
                    stsql.Append(" where t.value != '' order by dt_user_groups.grade DESC)) as b");
                    stsql.Append(" where ((b.checkstatus = 0 and b.bbbb =1) or (b.checkstatus = -1 and b.bbbb = 1) or (b.checkstatus = 1 and b.bbbb = 3))and b.P_Reveiver = '" + userid + "'");
                    DataSet dd = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, stsql.ToString(), "b.P_CreateTime desc"));
                    DataSetToModelHelper<Trainlist> list = new DataSetToModelHelper<Trainlist>();
                    if(dd.Tables[0].Rows.Count!=0)
                    {
                        model = list.FillModel(dd.Tables[0]);
                    }
                    else
                    {
                        model = null;
                        return model;
                    }
                    foreach (var item in model)
                    {
                        StringBuilder strsq = new StringBuilder();
                        strsq.Append("select P_Image.P_ImageUrl as image from P_Image");
                        strsq.Append(" where P_Image.P_ImageId ='" + item.id + @"'");
                        DataSet dat = DbHelperSQL.Query(strsq.ToString());
                        if (dat.Tables[0].Rows.Count != 0)
                        {
                            DataSetToModelHelper<photo> photo = new DataSetToModelHelper<photo>();
                            item.Photolist = photo.FillModel(dat);
                        }
                        else
                        {
                            item.Photolist = null;
                        }
                        StringBuilder ss = new StringBuilder();
                        ss.Append("select P_UserDemandSublist.P_Content as rcontent,CONVERT(varchar(100),P_UserDemandSublist.P_CreateTime, 23) as rcreatetime");
                        ss.Append(" from P_UserDemandSublist where P_UserDemandSublist.P_UDId ='" + item.id + @"'");
                        ss.Append(" ORDER BY P_UserDemandSublist.P_CreateTime Asc");
                        DataSet da = DbHelperSQL.Query(ss.ToString());
                        if (da.Tables[0].Rows.Count!= 0)
                        {
                            DataSetToModelHelper<con> con = new DataSetToModelHelper<con>();
                            item.Conlist = con.FillModel(da);
                        }
                    }
                }
            return model;
        }
        /// <summary>
        /// 党员有意见接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Add(PartytrainModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //获取最小党组织id
                        int groupid = usergroup.GetUserMinimumGroupId(model.userid);
                        //根据党组织获取manager id
                        StringBuilder str = new StringBuilder();
                        str.Append("select manager_id from dt_user_groups where id='"+groupid+"'");
                        //DataSet dd = DbHelperSQL.Query(str.ToString());
                        //string manager = dd.Tables[0].Rows[0]["manager_id"].ToString();
                        string manager = Convert.ToString(DbHelperSQL.GetSingle(str.ToString()));

                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_UserDemand(");
                        strSql.Append("P_ID,P_CheckStatus,P_Content,P_CreateTime,P_CreateUser,P_Status,P_PublicityStatus,P_Urgent,P_Reveiver)");
                        strSql.Append(" values (");
                        strSql.Append("@P_ID,@P_CheckStatus,@P_Content,@P_CreateTime,@P_CreateUser,@P_Status,@P_PublicityStatus,@P_Urgent,@P_Reveiver)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_CheckStatus", SqlDbType.Int,4),
                                new SqlParameter("@P_Content", SqlDbType.NText),                               
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_PublicityStatus", SqlDbType.Int,4),
                                new SqlParameter("@P_Urgent", SqlDbType.Int),
                                new SqlParameter("@P_Reveiver", SqlDbType.NVarChar)
                               };
                        string twoid = Guid.NewGuid().ToString();
                        parameters[0].Value = twoid;
                        parameters[1].Value = -1;
                        parameters[2].Value = model.content;
                        parameters[3].Value = DateTime.Now;
                        parameters[4].Value = model.userid;
                        parameters[5].Value = 0;
                        parameters[6].Value = model.openstate;
                        parameters[7].Value = 1;
                        parameters[8].Value = manager;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务                       
                        List<OptionModel2> oo = model.options;
                        if (oo != null)
                        {
                            foreach (var item in oo)
                            {
                                if (item.imgurl != null)
                                {
                                    StringBuilder st = new StringBuilder();
                                    string picture = qiniu.GetQiNiuFileUrl(item.imgurl);
                                    st.Append("insert into P_Image(");
                                    st.Append("p_id,p_imageid,p_imageUrl,P_CreateTime,P_CreateUser,P_Status,P_ImageType)");
                                    st.Append(" values (");
                                    st.Append("@p_id,@p_imageid,@p_imageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_ImageType)");
                                    SqlParameter[] param = {
                                        new SqlParameter("@p_id", SqlDbType.NVarChar,36),
                                        new SqlParameter("@p_imageid", SqlDbType.NVarChar,36),
                                        new SqlParameter("@p_imageUrl", SqlDbType.NVarChar,200),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@P_CreateUser", SqlDbType.Int,50),
                                        new SqlParameter("@P_Status", SqlDbType.Int),
                                        new SqlParameter("@P_ImageType", SqlDbType.NVarChar,50)};
                                    param[0].Value = Guid.NewGuid().ToString();
                                    param[1].Value = twoid;
                                    param[2].Value = picture;
                                    param[3].Value = DateTime.Now;
                                    param[4].Value = model.userid;
                                    param[5].Value = 0;
                                    param[6].Value = (int)ImageTypeEnum.党代表直通车;
                                    object obj2 = DbHelperSQL.GetSingle(conn, trans, st.ToString(), param); //带事务
                                }
                            }
                        }
                        trans.Commit();
                        if (!String.IsNullOrEmpty(manager)) {
                            List<int> per = new List<int>();
                            per.Add(Convert.ToInt32(manager));
                            PushMessageHelper.PushMessages(twoid, "您收到一条用户诉求信息。", 
                                per, Convert.ToInt32(model.userid), (int)PushTypeEnum.书记直通车);
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
        ///回复内容接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Reply(PartytrainModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //书记回复
                    if (model.type == 0)
                    {
                        try
                        {
                                StringBuilder strSql = new StringBuilder();
                                strSql.Append("insert into P_UserDemandSublist(");
                                strSql.Append("P_ID,P_UDId,P_Content,P_CreateTime,P_CreateUser,P_Status)");
                                strSql.Append(" values (");
                                strSql.Append("@P_ID,@P_UDId,@P_Content,@P_CreateTime,@P_CreateUser,@P_Status)");
                                SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UDId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NText),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.Int,4),
                                new SqlParameter("@P_Status", SqlDbType.Int,4)};

                                parameters[0].Value = Guid.NewGuid().ToString();
                                parameters[1].Value = model.mid;
                                parameters[2].Value = model.content;
                                parameters[3].Value = DateTime.Now;
                                parameters[4].Value = model.userid;
                                parameters[5].Value = 0;
                                object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                                trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                    //不满意回复
                    if (model.type == 1)
                    {
                        try
                        {
                            StringBuilder strsql = new StringBuilder();
                            strsql.Append("UPDATE P_UserDemand set P_CheckStatus = @P_CheckStatus ");
                            strsql.Append("where P_CreateUser = @P_CreateUser and P_Id = @P_Id");
                            SqlParameter[] parameter = {
                                new SqlParameter("@P_CheckStatus", SqlDbType.Int,10),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_ID",SqlDbType.NVarChar,100)};
                            parameter[0].Value = 1;
                            parameter[1].Value = model.userid;
                            parameter[2].Value = model.mid;
                            object obj1 = DbHelperSQL.GetSingle(conn, trans, strsql.ToString(), parameter); //带事务   

                                StringBuilder strSql = new StringBuilder();
                                strSql.Append("insert into P_UserDemandSublist(");
                                strSql.Append("P_ID,P_UDId,P_Content,P_CreateTime,P_CreateUser,P_Status)");
                                strSql.Append(" values (");
                                strSql.Append("@P_ID,@P_UDId,@P_Content,@P_CreateTime,@P_CreateUser,@P_Status)");
                                SqlParameter[] parameters = {
                                new SqlParameter("@P_ID", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UDId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NText),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.Int,4),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                };
                                parameters[0].Value = Guid.NewGuid().ToString();
                                parameters[1].Value = model.mid;
                                parameters[2].Value = model.content;
                                parameters[3].Value = DateTime.Now;
                                parameters[4].Value = model.userid;
                                parameters[5].Value = 0;
                                object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                                trans.Commit();
                        }
                        catch (Exception ex)
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
        /// 加急接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartyUrgent GetPartyUrgent(int userid,string id)
        {
            PartyUrgent model = new PartyUrgent();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("UPDATE P_UserDemand set P_Urgent = @P_Urgent");
                    strsql.Append(" where P_CreateUser = @P_CreateUser and P_ID = @P_ID");
                    SqlParameter[] parameters = {
                                new SqlParameter("@P_Urgent", SqlDbType.Int,10),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ID",SqlDbType.NVarChar,100)};
                    parameters[0].Value = 0;
                    parameters[1].Value = userid;
                    parameters[2].Value = id;
                    object obj = DbHelperSQL.GetSingle(conn, trans, strsql.ToString(), parameters); //带事务      
                    trans.Commit();
                }
            }
            return model;
        }
        /// <summary>
        /// 满意状态接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartySatisfaction GetPartySatisfaction(int userid, string id)
        {
            PartySatisfaction model = new PartySatisfaction();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("UPDATE P_UserDemand set P_CheckStatus = @P_CheckStatus");
                    strsql.Append(" where P_CreateUser = @P_CreateUser and P_ID = @P_ID");
                    SqlParameter[] parameters = {
                                new SqlParameter("@P_CheckStatus", SqlDbType.Int,10),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_ID",SqlDbType.NVarChar,100)};
                    parameters[0].Value = 0;
                    parameters[1].Value = userid;
                    parameters[2].Value = id;
                    object obj = DbHelperSQL.GetSingle(conn, trans, strsql.ToString(), parameters); //带事务   
                    trans.Commit();
                }
            }
            return model;
        }
        /// <summary>
        /// 书记显示未处理数量
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="asstatus"></param>
        /// <returns></returns>
        public SumCount GetPendingCount(int userid)
        {
            SumCount model = new SumCount();
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*)as sum from (select P_UserDemand.P_Id as id,");
            sql.Append("P_UserDemand.P_Reveiver,P_UserDemand.P_CheckStatus as checkstatus,");
            sql.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb ");
            sql.Append(" from P_UserDemand LEFT JOIN dt_users on P_UserDemand.P_CreateUser = dt_users.id ");
            sql.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id =");
            sql.Append(" (select TOP 1 t.value from F_Split(  (select dt_users.group_id from dt_users ");
            sql.Append(" where dt_users.id = P_UserDemand.P_CreateUser),',') as t  left join dt_user_groups on dt_user_groups.id = t.value ");
            sql.Append(" where t.value != '' order by dt_user_groups.grade DESC)) as b ");
            sql.Append(" where ((b.checkstatus = -1 and b.bbbb = 0) or (b.checkstatus = 1 and b.bbbb = 2)) and b.P_Reveiver ='"+userid+"'");
            DataSet ds = DbHelperSQL.Query(sql.ToString());
            if(ds.Tables[0].Rows.Count>0)
            {
                DataSetToModelHelper<SumCount> rela = new DataSetToModelHelper<SumCount>();
                model= rela.FillToModel(ds.Tables[0].Rows[0]);
            }
            return model;
        }
    }
}

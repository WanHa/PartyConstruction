using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
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
    public class PartyGroupForum
    {
        /// <summary>
        /// 新建党小组论坛
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean AddForum(PartyGroupForumModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder forum = new StringBuilder();
                        forum.Append("insert into P_PartyGroupForum(");
                        forum.Append("P_Id,P_CreaterId,P_Title,P_Intro,P_CreateTime,P_Status)");
                        forum.Append(" values(");
                        forum.Append("@P_Id,@P_CreaterId,@P_Title,@P_Intro,@P_CreateTime,@P_Status)");
                        forum.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_CreaterId",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Title",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_Intro",SqlDbType.NText),
                            new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_Status",SqlDbType.Int,4)
                        };
                        string forumid = Guid.NewGuid().ToString();
                        parameters[0].Value = forumid;
                        parameters[1].Value = model.creater;
                        parameters[2].Value = model.title;
                        parameters[3].Value = model.intro;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, forum.ToString(), parameters); //带事务

                        QiNiuHelper name = new QiNiuHelper();
                        string imagename = name.GetQiNiuFileUrl(model.imagename);
                        StringBuilder image = new StringBuilder();
                            image.Append("insert into P_Image(");
                            image.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status,P_ImageType,P_PictureName)");
                            image.Append(" values(");
                            image.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_ImageType,@P_PictureName)");
                            image.Append(";select @@IDENTITY");
                            SqlParameter[] parameters2 = {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,200),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Status", SqlDbType.Int,4),
                            new SqlParameter("@P_ImageType",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100)
                            };
                            string imgid = Guid.NewGuid().ToString();
                            parameters2[0].Value = imgid;
                            parameters2[1].Value = forumid;
                            parameters2[2].Value = imagename;
                            parameters2[3].Value = DateTime.Now;
                            parameters2[4].Value = model.creater;
                            parameters2[5].Value = 0;
                            parameters2[6].Value = (int)ImageTypeEnum.论坛封面;
                            parameters2[7].Value = model.imagename;
                        object obj2 = DbHelperSQL.GetSingle(conn, trans, image.ToString(), parameters2); //带事务

                            StringBuilder relation = new StringBuilder();
                            relation.Append("insert into P_PersonGroupRelation(");
                            relation.Append("P_Id,P_PartyGroupId,P_UserId,P_Type,P_CreateTime,P_Status,P_Approval) ");
                            relation.Append("values(");
                            relation.Append("@P_Id,@P_PartyGroupId,@P_UserId,@P_Type,@P_CreateTime,@P_Status,@P_Approval)");
                            relation.Append(";select @@IDENTITY");
                            SqlParameter[] parameters3 ={
                                new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_PartyGroupId",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_UserId",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Type",SqlDbType.Int,4),
                                new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                new SqlParameter("@P_Status",SqlDbType.Int,4),
                                new SqlParameter("@P_Approval",SqlDbType.Int,4)
                            };
                            string relationid = Guid.NewGuid().ToString();
                            parameters3[0].Value = relationid;
                            parameters3[1].Value = forumid;
                            parameters3[2].Value = model.creater;
                            parameters3[3].Value = 0;  //组长
                            parameters3[4].Value = DateTime.Now;
                            parameters3[5].Value = 0;
                            parameters3[6].Value = 0;
                            object obj3 = DbHelperSQL.GetSingle(conn, trans, relation.ToString(), parameters3); //带事务
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
        /// 获取我所在/所有的党小组列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<GroupList> GetGroupList(int userid, int type, int rows, int page)
        {
            List<GroupList> model = new List<GroupList>();
            //我所在的党小组论坛
            if (type == 0)
            {
                //StringBuilder strSql = new StringBuilder();
                //strSql.Append(@"select P_PartyGroupForum.P_Id as forumid,P_PartyGroupForum.P_CreaterId as createid,P_PartyGroupForum.P_Title as title,
                //                             P_Image.P_ImageUrl as imageurl 
                //                             from P_PartyGroupForum
                //                             LEFT JOIN P_PersonGroupRelation on P_PersonGroupRelation.P_PartyGroupId = P_PartyGroupForum.P_Id 
                //                             LEFT JOIN P_Image on P_Image.P_ImageId = P_PartyGroupForum.P_Id
                //                             where P_PersonGroupRelation.P_UserId ='" + userid+ "' and P_PersonGroupRelation.P_Approval = 0 and P_PartyGroupForum.P_Status=0");

                string sql = String.Format(@"select P_PartyGroupForum.P_Id as forumid,
                        P_PartyGroupForum.P_CreaterId as createid,
                        P_PartyGroupForum.P_Title as title,
                        a.P_ImageUrl as background,
                        P_PartyGroupForum.P_CreateTime from P_PartyGroupForum
                        left join P_PersonGroupRelation on P_PersonGroupRelation.P_PartyGroupId = P_PartyGroupForum.P_Id
                        LEFT JOIN P_Image a on a.P_ImageId =P_PartyGroupForum.P_Id and a.P_ImageType ={1}
                        where P_PersonGroupRelation.P_UserId = '{0}' and P_PersonGroupRelation.P_Approval = 0 and P_PersonGroupRelation.P_Status = 0
                        and P_PartyGroupForum.P_Status=0",userid,(int)ImageTypeEnum.论坛封面);
                DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_PartyGroupForum.P_CreateTime desc"));
                DataSetToModelHelper<GroupList> helper = new DataSetToModelHelper<GroupList>();
                model = helper.FillModel(ds);
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        StringBuilder cou = new StringBuilder();
                        cou.Append("select count(*) as count from P_PersonGroupRelation where P_PartyGroupId = '" + item.forumid + "' and P_Approval = 0 and P_Status = 0");
                        //DataSet cc = DbHelperSQL.Query(cou.ToString());
                        //DataSetToModelHelper<GroupList> helper1 = new DataSetToModelHelper<GroupList>();
                        //GroupList model1 = helper1.FillToModel(cc.Tables[0].Rows[0]);
                        int count = Convert.ToInt32(DbHelperSQL.GetSingle(cou.ToString()));
                        item.count = count;
                    }
                }
            }
            //所有党小组论坛
            if(type == 1)
            {
                StringBuilder str = new StringBuilder();
                str.Append(@"select P_PartyGroupForum.P_Id as forumid,
                        P_PartyGroupForum.P_CreaterId as createid,
                        P_PartyGroupForum.P_Title as title,
                        a.P_ImageUrl as background
                        from P_PartyGroupForum
                        LEFT JOIN P_Image a on a.P_ImageId =P_PartyGroupForum.P_Id and a.P_ImageType=20014
                        where P_PartyGroupForum.P_Status=0");
                DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, str.ToString(), "P_PartyGroupForum.P_CreateTime"));
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<GroupList> helper = new DataSetToModelHelper<GroupList>();
                    model = helper.FillModel(ds);
                    foreach (var item in model)
                    {
                        GroupList sta = new GroupList();

                        StringBuilder count = new StringBuilder();
                        count.Append("select count(*) as count from P_PersonGroupRelation where P_PartyGroupId = '" + item.forumid + "' and P_Approval = 0 and P_Status = 0");
                        //DataSet cou = DbHelperSQL.Query(count.ToString());
                        //DataSetToModelHelper<GroupList> hel = new DataSetToModelHelper<GroupList>();
                        //GroupList model2 = hel.FillToModel(cou.Tables[0].Rows[0]);
                        int count1 = Convert.ToInt32(DbHelperSQL.GetSingle(count.ToString()));
                        item.count = count1;

                        //StringBuilder join = new StringBuilder();
                        //join.Append(@"select * from P_PersonGroupRelation where P_PartyGroupId = '" + item.forumid + "' and P_UserId = '" + userid + "'");
                        string join = String.Format(@"select TOP 1 case when P_PersonGroupRelation.P_Status = 1 
                            then 2 else P_PersonGroupRelation.P_Approval end from P_PersonGroupRelation 
                            where P_PartyGroupId = '{0}'
                            and P_PersonGroupRelation.P_UserId = '{1}'
                            order by P_PersonGroupRelation.P_CreateTime DESC", item.forumid, userid);

                        //string joinStatus = DbHelperSQL.GetSingle(join).ToString();
                        if (DbHelperSQL.GetSingle(join) != null) {
                            item.status = Convert.ToInt32(DbHelperSQL.GetSingle(join));
                        }
                        else{
                            item.status = 2;
                        }

                        //DataSet enjoy = DbHelperSQL.Query(join.ToString());
                        //DataSetToModelHelper<P_PersonGroupRelation> helper2 = new DataSetToModelHelper<P_PersonGroupRelation>();
                        //if (enjoy.Tables[0].Rows.Count > 0)
                        //{
                        //    P_PersonGroupRelation approval = helper2.FillToModel(enjoy.Tables[0].Rows[0]);
                        //    if (approval.P_Approval == 0)
                        //    {
                        //        item.status = 0;//加入的

                        //    }
                        //    if (approval.P_Approval == 1)
                        //    {
                        //        item.status = 1;//申请的
                        //    }
                        //}
                        //else
                        //{
                        //    item.status = 2;
                        //}
                    }
                }
            }
            return model;
        }


        /// <summary>
        /// 获取论坛简介
        /// </summary>
        /// <param name="groupforumId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ForumInfo GetForumInfo(string groupforumId)
        {
            ForumInfo model = new ForumInfo();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_ImageUrl as img , dt_users.user_name as username , ");
            strSql.Append("P_PartyGroupForum.P_Title as title , P_PartyGroupForum.P_Intro as content ,");
            strSql.Append("(select count (*) from P_PersonGroupRelation where P_PartyGroupId = '" + groupforumId + "' and P_Status=0 and P_Approval = 0) as groupmembers  ");
            strSql.Append("from P_PartyGroupForum ");
            strSql.Append("LEFT JOIN P_Image on P_Image.P_ImageId = P_PartyGroupForum.P_Id ");
            strSql.Append("LEFT JOIN dt_users on dt_users.id = P_PartyGroupForum.P_CreaterId ");
            strSql.Append(" where P_PartyGroupForum.P_Id = '" + groupforumId + "'");
            DataSet info = DbHelperSQL.Query(strSql.ToString());
            if(info.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<ForumInfo> helper = new DataSetToModelHelper<ForumInfo>();
                model = helper.FillToModel(info.Tables[0].Rows[0]);
            }
            return model;
        }

        /// <summary>
        /// 党小组论坛申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean CommitApply(CommitApply model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("select count(*) from P_PersonGroupRelation where P_PartyGroupId = '"+ model.groupforumid + "' and P_UserId = '"+model.userid+"' and P_Status = 0 and P_Approval != 2");
                        int count =Convert.ToInt32(DbHelperSQL.GetSingle(str.ToString()));
                        if (count == 0)
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("insert into P_PersonGroupRelation(");
                            strSql.Append("P_Id,P_PartyGroupId,P_UserId,P_CreateTime,P_Status,P_Approval,P_ApplyExplain) ");
                            strSql.Append("values(");
                            strSql.Append("@P_Id,@P_PartyGroupId,@P_UserId,@P_CreateTime,@P_Status,@P_Approval,@P_ApplyExplain) ");
                            strSql.Append(";select @@IDENTITY");
                            SqlParameter[] parameters = {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_PartyGroupId",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_UserId",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_Status",SqlDbType.Int,4),
                            new SqlParameter("@P_Approval",SqlDbType.Int,4),
                            new SqlParameter("@P_ApplyExplain",SqlDbType.NText)
                        };
                            string applyid = Guid.NewGuid().ToString();
                            parameters[0].Value = applyid;
                            parameters[1].Value = model.groupforumid;
                            parameters[2].Value = model.userid;
                            parameters[3].Value = DateTime.Now;
                            parameters[4].Value = 0;
                            parameters[5].Value = 1;//待审核
                            parameters[6].Value = model.content;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                            trans.Commit();
                            string sqlPer = String.Format(@"select P_PartyGroupForum.P_CreaterId from P_PartyGroupForum
                                    where P_PartyGroupForum.P_Id = '{0}'", model.groupforumid);
                            int pushUserId = Convert.ToInt32(DbHelperSQL.GetSingle(sqlPer));
                            List<int> per = new List<int>();
                            per.Add(pushUserId);
                            PushMessageHelper.PushMessages("", "您有一条专题学习小组审核信息需要处理",
                                per, model.userid, (int)PushTypeEnum.申请论坛);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 党小组论坛申请列表
        /// </summary>
        /// <param name="groupforumId"></param>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<ApplyList> GetApplyList(string userid, int rows,int page)
        {

            string querySql = String.Format(@"select
                        P_PersonGroupRelation.P_UserId as applyuserid,
                        P_Image.P_ImageUrl as useravatar,
                        dt_users.user_name as username,
                        P_PartyGroupForum.P_Id as forumid,
                        P_PartyGroupForum.P_Title as forumname,
                        P_PersonGroupRelation.P_ApplyExplain as content,
                        convert(nvarchar(100),P_PersonGroupRelation.P_CreateTime,20) as time,
                        case when P_PersonGroupRelation.P_UserId = '{0}' then 1 else 0 end as type,
                        P_PersonGroupRelation.P_CreateTime
                        from P_PartyGroupForum
                        left join P_PersonGroupRelation on P_PersonGroupRelation.P_PartyGroupId = P_PartyGroupForum.P_Id
                        left join dt_users on convert(nvarchar,dt_users.id) = P_PersonGroupRelation.P_UserId
                        left join P_Image on P_Image.P_ImageId = P_PersonGroupRelation.P_UserId and P_Image.P_ImageType = {1}
                        where (P_PartyGroupForum.P_CreaterId = '{0}' and P_PersonGroupRelation.P_Approval = 1) or (
                        P_PersonGroupRelation.P_UserId = '{0}' and P_PersonGroupRelation.P_Approval = 1
                        and P_PartyGroupForum.P_CreaterId != '{0}'
                        ) and P_PartyGroupForum.P_Status = 0", userid, (int)ImageTypeEnum.头像);

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, querySql, " P_PersonGroupRelation.P_CreateTime desc"));

            DataSetToModelHelper<ApplyList> helper = new DataSetToModelHelper<ApplyList>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                return helper.FillModel(ds);
            }

            //StringBuilder str = new StringBuilder();
            //str.Append("SELECT  *  from P_PartyGroupForum where P_CreaterId = '" + userid + "'");
            //DataSet info = DbHelperSQL.Query(str.ToString());
            //DataSetToModelHelper<P_PartyGroupForum> dd = new DataSetToModelHelper<P_PartyGroupForum>();
            //List<P_PartyGroupForum> list = dd.FillModel(info);
            //if(list != null)
            //{
            //    //  获取所有申请我的
            //    foreach (var item in list)
            //    {
            //        StringBuilder sql = new StringBuilder();
            //        sql.Append(@"select P_Image.P_ImageUrl as useravatat,dt_users.user_name as username,P_PartyGroupForum.P_Title as forumname,P_PersonGroupRelation.P_PartyGroupId as forumid,
            //                                P_PersonGroupRelation.P_UserId as applyuserid,P_PersonGroupRelation.P_ApplyExplain as content,P_PersonGroupRelation.P_CreateTime
            //                                from P_PersonGroupRelation
            //                                LEFT JOIN P_Image ON P_Image.P_ImageId =  P_PersonGroupRelation.P_UserId and P_Image.P_ImageType=20011
            //                                LEFT JOIN dt_users ON dt_users.id = P_PersonGroupRelation.P_UserId
            //                                LEFT JOIN P_PartyGroupForum ON P_PartyGroupForum.P_Id = P_PersonGroupRelation.P_PartyGroupId
            //                                where P_PersonGroupRelation.P_PartyGroupId = '" + item.P_Id + "' and P_PersonGroupRelation.P_Status = 0 and P_PersonGroupRelation.P_Approval = 1");
            //        DataSet lists = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql.ToString(), "P_PersonGroupRelation.P_CreateTime"));
            //        DataSetToModelHelper<ApplyList> aa = new DataSetToModelHelper<ApplyList>();
            //        List<ApplyList> ss = aa.FillModel(lists);
            //        if(ss != null)
            //        {
            //            foreach (ApplyList cc in ss)
            //            {
            //                cc.type = 0;

            //                result.Add(cc);
            //            }
            //        }

            //    }
            //}
         
            //// 获取所有我申请的
            //StringBuilder time = new StringBuilder();
            //time.Append("select * from P_PersonGroupRelation where P_PersonGroupRelation.P_UserId = '" + userid + "' and P_PersonGroupRelation.P_Type = 1 and P_PersonGroupRelation.P_Approval = 0");
            //DataSet gettime = DbHelperSQL.Query(time.ToString());
            //DataSetToModelHelper<P_PersonGroupRelation> tt = new DataSetToModelHelper<P_PersonGroupRelation>();
            //List<P_PersonGroupRelation> timelist = tt.FillModel(gettime);
            //if (timelist != null)
            //{
            //    foreach (var item2 in timelist)
            //    {
            //        DateTime a = Convert.ToDateTime(item2.P_CreateTime);
            //        StringBuilder me = new StringBuilder();
            //        me.Append(@"select (Select Datename (month, '" + a + @"')+'-'+Datename(day,  '" + a + @"')) as time ,'您申请的'+P_PartyGroupForum.P_Title+'后台已通过' as content from P_PersonGroupRelation
            //                                 LEFT JOIN P_PartyGroupForum on P_PartyGroupForum.P_Id = P_PersonGroupRelation.P_PartyGroupId 
            //                                 where P_PersonGroupRelation.P_UserId = '" + userid + "' and P_PersonGroupRelation.P_Type = 0 and P_PersonGroupRelation.P_Id = '" + item2.P_Id + "'");
            //        DataSet man = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, me.ToString(), "P_PersonGroupRelation.P_CreateTime"));
            //        DataSetToModelHelper<ApplyList> tton = new DataSetToModelHelper<ApplyList>();
            //        List<ApplyList> kk = tton.FillModel(man);
            //        if(kk != null)
            //        {
            //            foreach (ApplyList pp in kk)
            //            {
            //                pp.type = 1;

            //                result.Add(pp);
            //            }
            //        }
            //    }
            //}
            return null;
        }

        /// <summary>
        /// 党小组论坛申请审批
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean CheckApply(CheckApply model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //申请通过
                        if(model.isverify == 0)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("update P_PersonGroupRelation set P_Approval = @P_Approval,P_Type = @P_Type where P_PartyGroupId = '" + model.groupforumid + "' and P_UserId ='" + model.userid + "' and P_Approval = 1");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Approval", SqlDbType.Int,4),
                                new SqlParameter("@P_Type",SqlDbType.Int,4)
                            };
                            parameters[0].Value = 0;//通过
                            parameters[1].Value = 1;//组员
                            object obj2 = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameters); //带事务
                        }
                        //申请拒绝
                        else
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(@"update P_PersonGroupRelation set P_Approval = @P_Approval where P_PartyGroupId = '" + model.groupforumid + "' and P_UserId ='" + model.userid + "' and P_Approval = 1");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Approval", SqlDbType.Int,4),
                            };
                            parameters[0].Value = 2;//拒绝
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务
                        }
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 退出党小组论坛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public int DelGroupForum(DelGroupForum model)
        //{
        //    using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
        //    {
        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                StringBuilder usertype = new StringBuilder();
        //                usertype.Append("select P_Type from P_PersonGroupRelation where P_UserId = '" + model.userid + "' and P_PartyGroupId = '" + model.groupforumid + "'");
        //                int dd = Convert.ToInt32(DbHelperSQL.GetSingle(usertype.ToString()));
        //                //组长删除党小组
        //                if (dd == 0)
        //                {
        //                    StringBuilder sel = new StringBuilder();
        //                    sel.Append("select count(*) from P_PersonGroupRelation where P_PartyGroupId = '" + model.groupforumid + "' and P_Type = 1 and P_Status = 0");
        //                    int ds = Convert.ToInt32(DbHelperSQL.GetSingle(sel.ToString()));
        //                    if (ds == 0)
        //                    {
        //                        StringBuilder del = new StringBuilder();
        //                        del.Append("update P_PersonGroupRelation set P_Status = @P_Status where  P_PartyGroupId = '" + model.groupforumid + "' and P_UserId = '" + model.userid + "'");
        //                        SqlParameter[] parameters = {
        //                            new SqlParameter("@P_Status",SqlDbType.Int,4),
        //                        };
        //                        parameters[0].Value = 1;
        //                        object obj = DbHelperSQL.GetSingle(conn, trans, del.ToString(), parameters);

        //                        StringBuilder delforum = new StringBuilder();
        //                        delforum.Append("update P_PartyGroupForum set P_Status = @P_Status where P_Id = '" + model.groupforumid + "'");
        //                        SqlParameter[] parameters2 = {
        //                            new SqlParameter("@P_Status",SqlDbType.Int,4),
        //                        };
        //                        parameters2[0].Value = 1;
        //                        object obj2 = DbHelperSQL.GetSingle(conn, trans, delforum.ToString(), parameters2);
        //                    }
        //                    else
        //                    {
        //                        return 2;//退出失败
        //                    }
        //                }
        //                //组员退出党小组
        //                if (dd == 1)
        //                {
        //                    StringBuilder quit = new StringBuilder();
        //                    quit.Append("update P_PersonGroupRelation set P_Status = @P_Status where  P_PartyGroupId = '" + model.groupforumid + "' and P_UserId = '" + model.userid + "'");
        //                    SqlParameter[] parameters = {
        //                            new SqlParameter("@P_Status",SqlDbType.Int,4),
        //                        };
        //                    parameters[0].Value = 1;
        //                    object obj = DbHelperSQL.GetSingle(conn, trans, quit.ToString(), parameters);
        //                }
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                return 0;
        //            }
        //        }
        //    }
        //    return 1;//退出成功
        //}

        //退出党小组论坛
        public Boolean DelGroupForum(DelGroupForum model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //退出党小组论坛
                        if (model.type == 0)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("update P_PersonGroupRelation set P_Status = @P_Status where  P_PartyGroupId = '" + model.groupforumid + "' and P_UserId = '" + model.userid + "' and P_Approval = 0");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Status",SqlDbType.Int,4)
                            };
                            parameters[0].Value = 1;
                            object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameters);
                        }
                        //解散党小组论坛
                        if(model.type == 1)
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("update P_PartyGroupForum set P_Status = @P_Status where P_Id = '"+model.groupforumid+ "' and P_CreaterId = '"+model.userid+"'");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Status",SqlDbType.Int,4)
                            };
                            parameters[0].Value = 1;
                            object obj1 = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        }
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 举报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Report(Report model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_Report(");
                        strSql.Append("P_Id,P_UserId,P_RelevancyId,P_ReportContent,P_CreateTime,P_Status,P_Type) ");
                        strSql.Append("values(");
                        strSql.Append("@P_Id,@P_UserId,@P_RelevancyId,@P_ReportContent,@P_CreateTime,@P_Status,@P_Type) ");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameter ={
                                    new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_UserId",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_RelevancyId",SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_ReportContent",SqlDbType.NVarChar,100),
                                    new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                   new SqlParameter("@P_Status",SqlDbType.Int,4),
                                   new SqlParameter("@P_Type",SqlDbType.Int,4)
                        };
                        string repid = Guid.NewGuid().ToString();
                        parameter[0].Value = repid;
                        parameter[1].Value = model.userid;
                        parameter[2].Value = model.relevancyid;
                        parameter[3].Value = model.content;
                        parameter[4].Value = DateTime.Now;
                        parameter[5].Value = 0;
                        parameter[6].Value = model.type;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(),parameter);


                        foreach (var item in model.imagename)
                        {
                            QiNiuHelper name = new QiNiuHelper();
                            string imgname = name.GetQiNiuFileUrl(item.image);
                            StringBuilder image = new StringBuilder();
                            image.Append("insert into P_Image(");
                            image.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status)");
                            image.Append(" values(");
                            image.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status)");
                            image.Append(";select @@IDENTITY");
                            SqlParameter[] parameters2 = {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Status", SqlDbType.Int,4),
                        };
                            string imgid = Guid.NewGuid().ToString();
                            parameters2[0].Value = imgid;
                            parameters2[1].Value = repid;
                            parameters2[2].Value = imgname;
                            parameters2[3].Value = DateTime.Now;
                            parameters2[4].Value = model.userid;
                            parameters2[5].Value = 0;
                            object obj2 = DbHelperSQL.GetSingle(conn, trans, image.ToString(), parameters2); //带事务
                        }
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                 }
            }
            return true;
        }
    }
}

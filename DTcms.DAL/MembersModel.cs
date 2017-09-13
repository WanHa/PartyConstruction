using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{
    public class MembersModel
    {
        private UserGroupHelper usergroup = new UserGroupHelper();
        /// <summary>
        /// 模范党员列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<MeModel> GetModelList(int userid, int rows, int page)
        {
            List<MeModel> model = new List<MeModel>();
            List<P_ModelPartyMember> mm = new List<P_ModelPartyMember>();
            StringBuilder str = new StringBuilder();
            str.Append(" select * from P_ModelPartyMember where P_ModelPartyMember.P_Status=0 ");
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, str.ToString(), "P_ModelPartyMember.P_CreateTime"));
            DataSetToModelHelper<P_ModelPartyMember> member = new DataSetToModelHelper<P_ModelPartyMember>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                mm = member.FillModel(ds);
                foreach (var item in mm)
                {
                    int groupuserid = usergroup.GetUserMinimumGroupId(Convert.ToInt32(item.P_UserId));
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("select P_ModelPartyMember.P_id as id,dt_users.user_name as username,groups.title as title,");
                    strsql.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_ModelPartyMember.P_UserId AS VARCHAR)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                    strsql.Append("(select case when count(dt_user_groups.id) > 0 then '书记' else '党员' end from dt_user_groups where dt_user_groups.manager = dt_users.user_name) as duties");
                    strsql.Append(" from P_ModelPartyMember ");
                    strsql.Append(" left join dt_users on dt_users.id = P_ModelPartyMember.P_UserId  ");
                    strsql.Append(" left join dt_user_groups groups on groups.id =" + groupuserid + "  ");
                    strsql.Append(" where P_ModelPartyMember.P_Status=0 ");
                    int recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strsql.ToString())));
                    DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_ModelPartyMember.P_CreateTime"));
                    if (dt != null)
                    {
                        DataSetToModelHelper<MeModel> model1 = new DataSetToModelHelper<MeModel>();
                        model = model1.FillModel(dt);
                    }
                    else
                    {
                        model = null;
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
        /// 模范党员详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailMeModel ModelXiangQing(string id,int userid)
        {
            DetailMeModel model = new DetailMeModel();
            StringBuilder str = new StringBuilder();
            str.Append(" select * from P_ModelPartyMember where P_ModelPartyMember.P_Id='"+id+@"' ");
            DataSet dds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<P_ModelPartyMember> memer = new DataSetToModelHelper<P_ModelPartyMember>();
            P_ModelPartyMember party = new P_ModelPartyMember();
            if (dds.Tables[0].Rows.Count != 0)
            {
                party = memer.FillToModel(dds.Tables[0].Rows[0]);
                int groupuserid = usergroup.GetUserMinimumGroupId(Convert.ToInt32(party.P_UserId));
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select dt_users.user_name as username,case when dt_users.sex = '0' then '男' else '女' end as sex,datediff(year,birthday,getdate()) as age,");
                strsql.Append(" (select P_ImageUrl from P_Image where P_ImageId=CAST(P_ModelPartyMember.P_UserId AS VARCHAR)  and P_ImageType='" + (int)ImageTypeEnum.头像 + @"' ) as avatar,");
                strsql.Append("P_ModelPartyMember.P_Description as description,groups.title as title,");
                strsql.Append("(select case when count(dt_user_groups.id) > 0 then '书记' else '党员' end from dt_user_groups ");
                strsql.Append("where dt_user_groups.manager = dt_users.user_name) as duties from P_ModelPartyMember ");
                strsql.Append("left join dt_users on dt_users.id = P_ModelPartyMember.P_UserId ");
                strsql.Append("left join dt_user_groups groups on groups.id = "+ groupuserid + @" where P_ModelPartyMember.P_Status=0 ");
                strsql.Append("and  P_ModelPartyMember.P_Id='" + party.P_Id + "' and P_ModelPartyMember.P_Status = 0");
                DataSet ds = DbHelperSQL.Query(strsql.ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<DetailMeModel> model1 = new DataSetToModelHelper<DetailMeModel>();
                    model = model1.FillToModel(ds.Tables[0].Rows[0]);
                }
                else
                {
                    model = null;
                }
            }
            else
            {
                model = null;
            }
           
            return model;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="salt"></param>
        /// <param name="password1"></param>
        /// <returns></returns>
        public Boolean GetDengLuEnroll(string mobile,string salt,string password1)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (Exists(mobile))
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("select ");
                            strSql.Append("mobile=@mobile,");
                            strSql.Append("password=@password ");
                            strSql.Append(" from dt_users ");
                            SqlParameter[] parameters = {
                                new SqlParameter("@mobile", SqlDbType.NVarChar,100),
                                new SqlParameter("@password", SqlDbType.NVarChar,100)};
                            var password2 = DESEncrypt.Decrypt(password1,salt);
                            parameters[0].Value = mobile;
                            parameters[1].Value = password2;
                            DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                            trans.Commit();
                            return true;
                        }
                        else
                        {
                            return false;
                        }

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
        /// 是否存在记录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool Exists(string mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_users");
            strSql.Append(" where mobile=" + mobile + @" ");
            return DbHelperSQL.Exists(strSql.ToString());
        }
    }
    public class MeModel
    {
        /// <summary>
        /// 模范党员id
        /// </summary>
        public string id { get; set; } 
        /// <summary>
        /// 模范党员名称
        /// </summary>
        public string username { get; set; } 
        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 所属组织
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 组织内的职务
        /// </summary>
        public string duties { get; set; }
    }
    public class DetailMeModel
    {
        public string username { get; set; }
        public string avatar { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string duties { get; set; }
    }
}

using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class Organizations
    {
        private UserGroupHelper usergroup = new UserGroupHelper();
        static int? pid;
        public int groupid;
        /// <summary>
        /// 插入组织信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int addGroup(Model.user_groups model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string infoid = addInfo(model);
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"insert into dt_user_groups(title,grade,upgrade_exp,amount,point,discount,is_default,is_upgrade,is_lock,pid,manager,position,create_time,sort,
                                        contact_address,phone_fax,superior_org,sub_org_id,intre,manager_id,status,org_code,
                                        location,territory_relation,elected_date,expiration_date,lead_info_id,rewards_punishment_id,company_info_id,official_male_count,official_female_count,ready_male_count,ready_female_count)
                                        values(@title,@grade,@upgrade_exp,@amount,@point,@discount,@is_default,@is_upgrade,@is_lock,@pid,@manager,@position,@create_time,@sort,
                                               @contact_address,@phone_fax,@superior_org,@sub_org_id,
                                               @intre,@manager_id,@status,@org_code,
                                               @location,@territory_relation,@elected_date,@expiration_date,
                                               @lead_info_id,@rewards_punishment_id,@company_info_id,@official_male_count,@official_female_count,@ready_male_count,@ready_female_count)");
                        strSql.Append(" "+" select @id=@@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@title", SqlDbType.NVarChar,100),
                                new SqlParameter("@grade", SqlDbType.Int,4),
                                new SqlParameter("@upgrade_exp", SqlDbType.Int,4),
                                new SqlParameter("@amount", SqlDbType.Decimal),
                                new SqlParameter("@point", SqlDbType.Int,4),
                                new SqlParameter("@discount", SqlDbType.Int,4),
                                new SqlParameter("@is_default", SqlDbType.Int,4),
                                new SqlParameter("@is_upgrade", SqlDbType.Int,4),
                                new SqlParameter("@is_lock", SqlDbType.Int,4),
                                new SqlParameter("@pid", SqlDbType.Int,4),
                                new SqlParameter("@manager", SqlDbType.NVarChar,50),
                                new SqlParameter("@position", SqlDbType.NVarChar,255),
                                new SqlParameter("@create_time", SqlDbType.DateTime),
                                new SqlParameter("@sort", SqlDbType.Int,4),
                                new SqlParameter("@contact_address", SqlDbType.NVarChar,100),
                                new SqlParameter("@phone_fax", SqlDbType.NVarChar,100),
                                new SqlParameter("@superior_org", SqlDbType.NVarChar,100),
                                new SqlParameter("@sub_org_id", SqlDbType.NVarChar),
                                new SqlParameter("@intre", SqlDbType.NText),
                                new SqlParameter("@manager_id",SqlDbType.Int,4),
                                new SqlParameter("@status", SqlDbType.Int,4),
                                new SqlParameter("@org_code",  SqlDbType.NVarChar, 20),
                                new SqlParameter("@location", SqlDbType.NVarChar),
                                new SqlParameter("@territory_relation", SqlDbType.NVarChar),
                                new SqlParameter("@elected_date", SqlDbType.DateTime),
                                new SqlParameter("@expiration_date", SqlDbType.DateTime),
                                new SqlParameter("@lead_info_id", SqlDbType.NVarChar),
                                new SqlParameter("@rewards_punishment_id", SqlDbType.NVarChar,50),
                                new SqlParameter("@company_info_id", SqlDbType.NVarChar,50),
                                new SqlParameter("@official_male_count", SqlDbType.Int),
                                new SqlParameter("@official_female_count", SqlDbType.Int),
                                new SqlParameter("@ready_male_count", SqlDbType.Int),
                                new SqlParameter("@ready_female_count", SqlDbType.Int),
                                new SqlParameter("@id", SqlDbType.Int)
                        };
                        parameters[0].Value = model.title;
                        parameters[1].Value = model.grade;
                        parameters[2].Value = model.upgrade_exp;
                        parameters[3].Value = model.amount;
                        parameters[4].Value = model.point;
                        parameters[5].Value = model.discount;
                        parameters[6].Value = model.is_default;
                        parameters[7].Value = model.is_upgrade;
                        parameters[8].Value = model.is_lock;
                        parameters[9].Value = model.pid;
                        parameters[10].Value = model.manager;
                        parameters[11].Value = model.position;
                        parameters[12].Value = model.create_time;
                        parameters[13].Value = model.sort;
                        parameters[14].Value = model.contact_address;
                        parameters[15].Value = model.phone_fax;
                        parameters[16].Value = model.superior_org;
                        parameters[17].Value = model.sub_org_id;
                        parameters[18].Value = model.intre;
                        parameters[19].Value = model.Manager_id;
                        parameters[20].Value = 0;
                        parameters[21].Value = model.org_code;
                        parameters[22].Value = model.location;
                        parameters[23].Value = model.territory_relation;
                        parameters[24].Value = model.elected_date;
                        parameters[25].Value = model.expiration_date;
                        parameters[26].Value = infoid;
                        parameters[27].Value = model.rewards_punishment_id;
                        parameters[28].Value = model.company_info_id;
                        parameters[29].Value = model.official_male_count;
                        parameters[30].Value = model.official_female_count;
                        parameters[31].Value = model.ready_male_count;
                        parameters[32].Value = model.ready_female_count;
                        parameters[33].Direction = ParameterDirection.Output;//返回值
                        DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                        trans.Commit();
                        return Convert.ToInt32(parameters[33].Value.ToString());//获取自增值
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }          
        }
        public bool addUser(int managerid,int mid)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("select group_id from dt_users where id='"+managerid+"'");
                        DataSet ds = DbHelperSQL.Query(str.ToString());
                        string group_id = Convert.ToString(DbHelperSQL.GetSingle(str.ToString()));
                        if(group_id!=null)
                        {
                            StringBuilder sqll = new StringBuilder();
                            sqll.Append("Update dt_users set");
                            sqll.Append(" group_id=@group_id");
                            sqll.Append(" where id=@id");
                            SqlParameter[] para ={
                          new SqlParameter("@group_id",SqlDbType.NVarChar,255),
                          new SqlParameter("@id",SqlDbType.Int),
                            };
                            para[0].Value = group_id + mid + ",";
                            para[1].Value = managerid;
                            int i =DbHelperSQL.ExecuteSql(conn, trans, sqll.ToString(), para); //带事务    
                        }
                        else
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.Append("Insert into dt_users(");
                            sql.Append("group_id");
                            sql.Append(" values (");
                            sql.Append("@group_id) where id =@id");
                            SqlParameter[] parameters2 = {
                                new SqlParameter("@group_id", SqlDbType.NVarChar,255),
                                new SqlParameter("@id", SqlDbType.Int),
                            };
                            parameters2[0].Value = ","+ mid + ",";
                            parameters2[1].Value = managerid;
                            int i =DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameters2); //带事务                               
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
        ///<summary>
        ///插入领导班子成员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string addInfo(Model.user_groups model)
        {
            StringBuilder str = new StringBuilder();
            str.Append("select id from dt_users where user_name ='" + model.name + "' ");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                groupid = usergroup.GetUserMinimumGroupId(Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString()));
            }
            else
            {
                groupid = 0;
            }
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("insert into u_lead_info (id,name,job,contact_way,remark,status,create_time,create_user,group_id) values(");
                        sql.Append("@id,@name,@job,@contact_way,@remark,@status,@create_time,@create_user,@group_id)");
                        SqlParameter[] parameters =
                        {
                        new SqlParameter("@id",SqlDbType.NVarChar),
                        new SqlParameter("@name",SqlDbType.NVarChar),
                        new SqlParameter("@job",SqlDbType.NVarChar),
                        new SqlParameter("@contact_way",SqlDbType.NText),
                        new SqlParameter("@remark",SqlDbType.NVarChar),
                        new SqlParameter("@status",SqlDbType.Int),
                        new SqlParameter("@create_time",SqlDbType.DateTime),
                        new SqlParameter("@create_user",SqlDbType.NVarChar,50),
                        new SqlParameter("@group_id",SqlDbType.NVarChar,50),
                    };
                        parameters[0].Value = model.lead_info_id;
                        parameters[1].Value = model.name;
                        parameters[2].Value = model.job;
                        parameters[3].Value = model.contact_way;
                        parameters[4].Value = model.remark;
                        parameters[5].Value = 0;
                        parameters[6].Value = DateTime.Now;
                        parameters[7].Value = model.userid;
                        parameters[8].Value = groupid;
                        object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return model.lead_info_id;
        }
        /// <summary>
        /// 插入奖惩情况表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool addRewardsAndPunishment(Model.P_RewardsAndPunishment model1)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //string rpid = Guid.NewGuid().ToString();
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"insert into P_RewardsAndPunishment(P_Id,P_Title,P_DateTime,P_Content,P_RatifyOrganiz,P_Status)
                                               values(@P_Id,@P_Title,@P_DateTime,@P_Content,@P_RatifyOrganiz,0)");
                        SqlParameter[] parameters = {
                                    new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                    new SqlParameter("@P_Title", SqlDbType.NVarChar,100),
                                    new SqlParameter("@P_DateTime", SqlDbType.DateTime),
                                    new SqlParameter("@P_Content", SqlDbType.NVarChar,100),
                                    new SqlParameter("@P_RatifyOrganiz", SqlDbType.NVarChar,100),
                                    new SqlParameter("@P_Status", SqlDbType.Int),
                        };
                        parameters[0].Value = model1.P_Id;
                        parameters[1].Value = model1.P_Title;
                        parameters[2].Value = model1.P_DateTime;
                        parameters[3].Value = model1.P_Content;
                        parameters[4].Value = model1.P_RatifyOrganiz;
                        parameters[5].Value = 0;
                        //pid = rpid;
                        object obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                        trans.Commit();
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
        /// <summary>
        /// 插入下辖组织信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string setSubordinateGroupInfoId(string id, int exists, int count, string info)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"insert into P_SubordinateGroupInfo(P_Id,P_Exists,P_Count,P_info,P_Status) values(@id,@exists,@count,@info,0)");
                        SqlParameter[] parameters = {
                                    new SqlParameter("@id", SqlDbType.NVarChar,100),
                                    new SqlParameter("@exists", SqlDbType.Int),
                                    new SqlParameter("@count", SqlDbType.Int),
                                    new SqlParameter("@info", SqlDbType.NVarChar,100)
                        };
                        parameters[0].Value = id;
                        parameters[1].Value = exists;
                        parameters[2].Value = count;
                        parameters[3].Value = info;
                        object obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return id;
        }
        /// <summary>
        /// 插入单位信息
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public string UnitInfo(string id, string comname, string value, int count, int service)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"insert into u_company_type(id,name,employee_count,status,com_nature,service_organiz)
                                                    values(@id,@name,@employee_count,@status,@com_nature,@service_organiz)");
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@id",SqlDbType.NVarChar,50),
                            new SqlParameter("@name",SqlDbType.NVarChar,50),
                            new SqlParameter("@employee_count",SqlDbType.Int),
                            new SqlParameter("@status",SqlDbType.Int),
                            new SqlParameter("@com_nature",SqlDbType.NVarChar,50),
                            new SqlParameter("@service_organiz",SqlDbType.Int,4),
                            };
                        parameters[0].Value = id;
                        parameters[1].Value = comname;
                        parameters[2].Value = count;
                        parameters[3].Value = 0;
                        parameters[4].Value = value;
                        parameters[5].Value = service;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return id;
        }
        /// <summary>
        /// 查询父级id
        /// </summary>
        /// <param name="partyname"></param>
        /// <returns></returns>
        public int? SearchParty(string partyname)
        {
            StringBuilder str = new StringBuilder();
            str.Append("select id from dt_user_groups where title = '" + partyname + "'");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                pid = Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString());
            }
            else
            {
                pid = null;
            }
            return pid;
        }

        #region 获取领导班子信息
        public Model.u_lead_info leadinfo(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 id,name,job,contact_way,remark");
            strSql.Append(" from u_lead_info");
            strSql.Append(" where id in (select lead_info_id from dt_user_groups where id=@id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LeadDataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public Model.u_lead_info LeadDataRowToModel(DataRow row)
        {
            Model.u_lead_info model = new Model.u_lead_info();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["name"] != null && row["name"].ToString() != "")
                {
                    model.name = row["name"].ToString();
                }
                if (row["job"] != null && row["job"].ToString() != "")
                {
                    model.job = row["job"].ToString();
                }
                if (row["contact_way"] != null && row["contact_way"].ToString() != "")
                {
                    model.contact_way = row["contact_way"].ToString();
                }
                if (row["remark"] != null && row["remark"].ToString() != "")
                {
                    model.remark = row["remark"].ToString();
                }
            }
            return model;
        }
        #endregion

        #region 获取单位信息
        public Model.u_company_type unitinfo(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 id,name,employee_count,com_nature,service_organiz ");
            strSql.Append(" from u_company_type");
            strSql.Append(" where id in (select company_info_id from dt_user_groups where id=@id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return UnitDataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public Model.u_company_type UnitDataRowToModel(DataRow row)
        {
            Model.u_company_type model = new Model.u_company_type();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["name"] != null && row["name"].ToString() != "")
                {
                    model.name = row["name"].ToString();
                }
                if (row["employee_count"] != null && row["employee_count"].ToString() != "")
                {
                    model.employee_count = int.Parse(row["employee_count"].ToString());
                }
                if (row["com_nature"] != null && row["com_nature"].ToString() != "")
                {
                    model.com_nature = row["com_nature"].ToString();
                }
                if (row["service_organiz"] != null && row["service_organiz"].ToString() != "")
                {
                    model.service_organiz = int.Parse(row["service_organiz"].ToString());
                }
            }
            return model;
        }
        #endregion

        /// <summary>
        /// 获取奖惩信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.P_RewardsAndPunishment getRewardsAndPunishment(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 P_Id,P_Title,P_DateTime,P_Content,P_RatifyOrganiz,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status  ");
            strSql.Append(" from P_RewardsAndPunishment");
            strSql.Append(" where P_id in (select rewards_punishment_id from dt_user_groups where id=@id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return rewardsDataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取奖惩信息模型
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.P_RewardsAndPunishment rewardsDataRowToModel(DataRow row)
        {
            Model.P_RewardsAndPunishment model = new Model.P_RewardsAndPunishment();
            if (row != null)
            {
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_Title"] != null)
                {
                    model.P_Title = row["P_Title"].ToString();
                }
                if (row["P_DateTime"] != null && row["P_DateTime"].ToString() != "")
                {
                    model.P_DateTime = DateTime.Parse(row["P_DateTime"].ToString());
                }
                if (row["P_Content"] != null)
                {
                    model.P_Content = row["P_Content"].ToString();
                }
                if (row["P_RatifyOrganiz"] != null)
                {
                    model.P_RatifyOrganiz = row["P_RatifyOrganiz"].ToString();
                }
                if (row["P_CreateTime"] != null && row["P_CreateTime"].ToString() != "")
                {
                    model.P_CreateTime = DateTime.Parse(row["P_CreateTime"].ToString());
                }
                if (row["P_CreateUser"] != null)
                {
                    model.P_CreateUser = row["P_CreateUser"].ToString();
                }
                if (row["P_UpdateTime"] != null && row["P_UpdateTime"].ToString() != "")
                {
                    model.P_UpdateTime = DateTime.Parse(row["P_UpdateTime"].ToString());
                }
                if (row["P_UpdateUser"] != null)
                {
                    model.P_UpdateUser = row["P_UpdateUser"].ToString();
                }
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = int.Parse(row["P_Status"].ToString());
                }
            }
            return model;
        }
        /// <summary>
        /// 获取组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.user_groups getGroups(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 id,title,grade,upgrade_exp,amount,point,discount,is_default,is_upgrade,is_lock,pid,manager,position,create_time,
                                            sort,contact_address,phone_fax,superior_org,sub_org_id,intre,manager_id,
                                            org_code,location,territory_relation,elected_date,expiration_date,lead_info_id,rewards_punishment_id,company_info_id,
                                            official_male_count,official_female_count,ready_male_count,ready_female_count");
            strSql.Append(" from dt_user_groups");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return groupDataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取组织信息
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.user_groups groupDataRowToModel(DataRow row)
        {
            Model.user_groups model = new Model.user_groups();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["title"] != null)
                {
                    model.title = row["title"].ToString();
                }
                if (row["grade"] != null && row["grade"].ToString() != "")
                {
                    model.grade = int.Parse(row["grade"].ToString());
                }
                if (row["upgrade_exp"] != null && row["upgrade_exp"].ToString() != "")
                {
                    model.upgrade_exp = int.Parse(row["upgrade_exp"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = Decimal.Parse(row["amount"].ToString());
                }
                if (row["point"] != null && row["point"].ToString() != "")
                {
                    model.point = int.Parse(row["point"].ToString());
                }
                if (row["discount"] != null && row["discount"].ToString() != "")
                {
                    model.discount = int.Parse(row["discount"].ToString());
                }
                if (row["is_default"] != null && row["is_default"].ToString() != "")
                {
                    model.is_default = int.Parse(row["is_default"].ToString());
                }
                if (row["is_upgrade"] != null && row["is_upgrade"].ToString() != "")
                {
                    model.is_upgrade = int.Parse(row["is_upgrade"].ToString());
                }
                if (row["is_lock"] != null && row["is_lock"].ToString() != "")
                {
                    model.is_lock = int.Parse(row["is_lock"].ToString());
                }
                if (row["pid"] != null && row["pid"].ToString() != "")
                {
                    model.pid = int.Parse(row["pid"].ToString());
                }
                if (row["manager"] != null)
                {
                    model.manager = row["manager"].ToString();
                }
                if (row["position"] != null)
                {
                    model.position = row["position"].ToString();
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
                if (row["sort"] != null && row["sort"].ToString() != "")
                {
                    model.sort = int.Parse(row["sort"].ToString());
                }
                if (row["contact_address"] != null)
                {
                    model.contact_address = row["contact_address"].ToString();
                }
                if (row["phone_fax"] != null && row["phone_fax"].ToString() != "")
                {
                    model.phone_fax = row["phone_fax"].ToString();
                }
                if (row["superior_org"] != null)
                {
                    model.superior_org = row["superior_org"].ToString();
                }
                if (row["sub_org_id"] != null && row["sub_org_id"].ToString() != "")
                {
                    model.sub_org_id = row["sub_org_id"].ToString();
                }
                if (row["intre"] != null)
                {
                    model.intre = row["intre"].ToString();
                }
                if (row["manager_id"] != null && row["manager_id"].ToString() != "")
                {
                    model.Manager_id = int.Parse(row["manager_id"].ToString());
                }
                //if (row["status"] != null && row["status"].ToString() != "")
                //{
                //    model.status = int.Parse(row["status"].ToString());
                //}
                if (row["org_code"] != null)
                {
                    model.org_code = row["org_code"].ToString();
                }
                if (row["location"] != null)
                {
                    model.location = row["location"].ToString();
                }
                if (row["territory_relation"] != null)
                {
                    model.territory_relation = row["territory_relation"].ToString();
                }
                if (row["elected_date"] != null && row["elected_date"].ToString()!="")
                {
                    model.elected_date = Convert.ToDateTime(row["elected_date"].ToString());
                }
                if (row["expiration_date"] != null && row["expiration_date"].ToString() != "")
                {
                    model.expiration_date = Convert.ToDateTime(row["expiration_date"].ToString());
                }
                if (row["lead_info_id"] != null && row["lead_info_id"].ToString() != "")
                {
                    model.lead_info_id = row["lead_info_id"].ToString();
                }
                if (row["rewards_punishment_id"] != null && row["rewards_punishment_id"].ToString() != "")
                {
                    model.rewards_punishment_id = row["rewards_punishment_id"].ToString();
                }
                if (row["company_info_id"] != null && row["company_info_id"].ToString() != "")
                {
                    model.company_info_id = row["company_info_id"].ToString();
                }
                if (row["official_male_count"] != null && row["official_male_count"].ToString() != "")
                {
                    model.official_male_count = int.Parse(row["official_male_count"].ToString());
                }
                if (row["official_female_count"] != null && row["official_female_count"].ToString() != "")
                {
                    model.official_female_count = int.Parse(row["official_female_count"].ToString());
                }
                if (row["ready_male_count"] != null && row["ready_male_count"].ToString() != "")
                {
                    model.ready_male_count = int.Parse(row["ready_male_count"].ToString());
                }
                if (row["ready_female_count"] != null && row["ready_female_count"].ToString() != "")
                {
                    model.ready_female_count = int.Parse(row["ready_female_count"].ToString());
                }
            }
            return model;
        }

        #region 获取下辖组织情况
        public string getExists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select P_Exists from P_SubordinateGroupInfo
                            where P_id in (select sub_org_id from dt_user_groups where id=@id)");
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.NVarChar,50),
            };
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return "0";
            }
            else
            {
                return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0].Rows[0][0].ToString() == null ? "0" : DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0].Rows[0][0].ToString();
            }
        }

        public string getCount(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select P_Count from P_SubordinateGroupInfo
                            where P_id in (select sub_org_id from dt_user_groups where id=@id)");
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.NVarChar,50),
            };
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return "0";
            }
            else
            {
                return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0].Rows[0][0].ToString() == null ? "0" : DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0].Rows[0][0].ToString();
            }
        }

        public string getInfo(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select P_Info from P_SubordinateGroupInfo
                            where P_id in (select sub_org_id from dt_user_groups where id=@id)");
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.NVarChar,50),
            };
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return "0";
            }
            else
            {
                return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0].Rows[0][0].ToString() == null ? "" : DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0].Rows[0][0].ToString();
            }
        }
        #endregion

        /// <summary>
        /// 更改下辖党组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exists"></param>
        /// <param name="count"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool subordinate(string id, int exists, int count, string info)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("Update P_SubordinateGroupInfo set P_Exists =@exists,P_Count=@count,P_Info=@info where P_Id =@id ");
                        SqlParameter[] parameters = {
                                    new SqlParameter("@exists", SqlDbType.Int),
                                    new SqlParameter("@count", SqlDbType.Int),
                                    new SqlParameter("@info", SqlDbType.NVarChar,50),
                                    new SqlParameter("@id", SqlDbType.NVarChar,50),
                        };
                        parameters[0].Value = exists;
                        parameters[1].Value = count;
                        parameters[2].Value = info;
                        parameters[3].Value = id;
                        object obj = DbHelperSQL.ExecuteSql(str.ToString(), parameters);
                        trans.Commit();
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
        /// <summary>
        /// 更改单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comname"></param>
        /// <param name="sort"></param>
        /// <param name="count"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public bool changeunit(string id, string comname, string sort, int count, int service)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("Update u_company_type set name=@name,employee_count=@employee_count,status=@status,com_nature=@com_nature,service_organiz=@service_organiz");
                        sql.Append(" where id = @id");
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@name",SqlDbType.NVarChar,50),
                            new SqlParameter("@employee_count",SqlDbType.Int),
                            new SqlParameter("@status",SqlDbType.Int),
                            new SqlParameter("@com_nature",SqlDbType.NVarChar,50),
                            new SqlParameter("@service_organiz",SqlDbType.Int,4),
                            new SqlParameter("@id",SqlDbType.NVarChar,50),
                            };
                        parameters[0].Value = comname;
                        parameters[1].Value = count;
                        parameters[2].Value = 0;
                        parameters[3].Value = sort;
                        parameters[4].Value = service;
                        parameters[5].Value = id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters);
                        trans.Commit();
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

        /// <summary>
        /// 修改奖赏信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean changeRewardsAndPunishment(Model.P_RewardsAndPunishment model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"update P_RewardsAndPunishment set P_Title = @P_Title,P_DateTime = @P_DateTime, P_RatifyOrganiz=@P_RatifyOrganiz,P_Content=@P_Content
                                                where P_Id = @P_Id");
                        SqlParameter[] parameters = {
                            new SqlParameter("@P_Title", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_DateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_RatifyOrganiz", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_Content", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                        };
                        parameters[0].Value = model.P_Title;
                        parameters[1].Value = model.P_DateTime;
                        parameters[2].Value = model.P_RatifyOrganiz;
                        parameters[3].Value = model.P_Content;
                        parameters[4].Value = model.P_Id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
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
        /// <summary>
        /// 更改领导班子成员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean changeInfo(Model.u_lead_info model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("update u_lead_info set name=@name,job=@job,contact_way=@contact_way,remark=@remark where id = @lead_info_id ");
                        SqlParameter[] parameters = {
                            new SqlParameter("@name",SqlDbType.NVarChar,50),
                            new SqlParameter("@job",SqlDbType.NVarChar,50),
                            new SqlParameter("@contact_way",SqlDbType.NVarChar,50),
                            new SqlParameter("@remark",SqlDbType.NText,1000),
                            new SqlParameter("@lead_info_id",SqlDbType.NVarChar,50)
                        };
                        parameters[0].Value = model.name;
                        parameters[1].Value = model.job;
                        parameters[2].Value = model.contact_way;
                        parameters[3].Value = model.remark;
                        parameters[4].Value = model.id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 新建领导班子成员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean AddInfo(Model.u_lead_info model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("update u_lead_info set name=@name,job=@job,contact_way=@contact_way,remark=@remark where id = @lead_info_id ");
                        SqlParameter[] parameters = {
                            new SqlParameter("@name",SqlDbType.NVarChar,50),
                            new SqlParameter("@job",SqlDbType.NVarChar,50),
                            new SqlParameter("@contact_way",SqlDbType.NVarChar,50),
                            new SqlParameter("@remark",SqlDbType.NText,1000),
                            new SqlParameter("@lead_info_id",SqlDbType.NVarChar,50)
                        };
                        parameters[0].Value = model.name;
                        parameters[1].Value = model.job;
                        parameters[2].Value = model.contact_way;
                        parameters[3].Value = model.remark;
                        parameters[4].Value = model.id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 更改党组织信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateUsersGroup(Model.user_groups model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"update dt_user_groups set title=@title,pid=@pid,manager=@manager,position=@position,create_time=@create_time,sort=@sort,
                            contact_address=@contact_address,phone_fax=@phone_fax,superior_org=@superior_org,sub_org_id=@sub_org_id,
						    intre = @intre, manager_id = @manager_id, org_code = @org_code,location = @location,territory_relation=@territory_relation,
                            elected_date=@elected_date,expiration_date=@expiration_date,
                            lead_info_id= @lead_info_id,rewards_punishment_id=@rewards_punishment_id,company_info_id =@company_info_id,
                            official_male_count=@official_male_count,official_female_count=@official_female_count,ready_male_count=@ready_male_count,
                            ready_female_count=@ready_female_count
							where id = @id");
                        SqlParameter[] parameters = {
                            new SqlParameter("@title", SqlDbType.NVarChar),
                            new SqlParameter("@pid", SqlDbType.Int),
                            new SqlParameter("@manager", SqlDbType.NVarChar),
                            new SqlParameter("@position", SqlDbType.NVarChar),
                            new SqlParameter("@create_time", SqlDbType.DateTime),
                            new SqlParameter("@sort", SqlDbType.Int),
                            new SqlParameter("@contact_address", SqlDbType.NVarChar),
                            new SqlParameter("@phone_fax", SqlDbType.NVarChar),
                            new SqlParameter("@superior_org", SqlDbType.NVarChar),
                            new SqlParameter("@sub_org_id", SqlDbType.NVarChar),
                            new SqlParameter("@intre", SqlDbType.NText),
                            new SqlParameter("@manager_id", SqlDbType.Int),
                            new SqlParameter("@org_code", SqlDbType.NVarChar),
                            new SqlParameter("@location", SqlDbType.NVarChar),
                            new SqlParameter("@territory_relation", SqlDbType.NVarChar),
                            new SqlParameter("@elected_date", SqlDbType.DateTime),
                            new SqlParameter("@expiration_date", SqlDbType.DateTime),
                            new SqlParameter("@lead_info_id", SqlDbType.NVarChar),
                            new SqlParameter("@rewards_punishment_id", SqlDbType.NVarChar),
                            new SqlParameter("@company_info_id", SqlDbType.NVarChar),
                            new SqlParameter("@official_male_count", SqlDbType.Int),
                            new SqlParameter("@official_female_count", SqlDbType.Int),
                            new SqlParameter("@ready_male_count", SqlDbType.Int),
                            new SqlParameter("@ready_female_count", SqlDbType.Int),
                            new SqlParameter("@id", SqlDbType.Int),
                        };
                            parameters[0].Value = model.title;
                            parameters[1].Value = model.pid;
                            parameters[2].Value = model.manager;
                            parameters[3].Value = model.position;
                            parameters[4].Value = model.create_time;
                            parameters[5].Value = model.sort;
                            parameters[6].Value = model.contact_address;
                            parameters[7].Value = model.phone_fax;
                            parameters[8].Value = model.superior_org;
                            parameters[9].Value = model.sub_org_id;
                            parameters[10].Value = model.intre;
                            parameters[11].Value = model.Manager_id;
                            parameters[12].Value = model.org_code;                            
                            parameters[13].Value = model.location;
                            parameters[14].Value = model.territory_relation;
                            parameters[15].Value = model.elected_date;
                            parameters[16].Value = model.expiration_date;
                            parameters[17].Value = model.lead_info_id;
                            parameters[18].Value = model.rewards_punishment_id;
                            parameters[19].Value = model.company_info_id;
                            parameters[20].Value = model.official_male_count;
                            parameters[21].Value = model.official_female_count;
                            parameters[22].Value = model.ready_male_count;
                            parameters[23].Value = model.ready_female_count;
                            parameters[24].Value = model.id;
                        object obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
    }
}

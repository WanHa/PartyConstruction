using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTcms.DBUtility;
using DTcms.Common;
using DTcms.Model;

namespace DTcms.DAL
{
    /// <summary>
    /// 数据访问类:用户
    /// </summary>
    public partial class users
    {
        private string databaseprefix; //数据库表名前缀
        public users(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region 基本方法================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "users");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        public bool Exists(string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "users");
            strSql.Append(" where user_name=@user_name ");
            SqlParameter[] parameters = {
                    new SqlParameter("@user_name", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 检查同一IP注册间隔(小时)内是否存在
        /// </summary>
        public bool Exists(string reg_ip, int regctrl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "users");
            strSql.Append(" where reg_ip=@reg_ip and DATEDIFF(hh,reg_time,getdate())<@regctrl ");
            SqlParameter[] parameters = {
                    new SqlParameter("@reg_ip", SqlDbType.NVarChar,30),
                    new SqlParameter("@regctrl", SqlDbType.Int,4)};
            parameters[0].Value = reg_ip;
            parameters[1].Value = regctrl;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into " + databaseprefix + "users(");
            strSql.Append("group_id,user_name,salt,password,mobile,email,nick_name,sex,birthday,telphone,area,address,qq,msn,amount,point,exp,status,reg_time,reg_ip,id_card,nation,marital_status,new_class_id,administration_rank_id,military_rank_id,police_rank_id,children_info,only_child_award,passport_info_id,");
            strSql.Append("entitled_group_id,income_source_id,native_place,live_place,graduate_school,graduate_time,education_info_id,degree_info,join_party_time,party_membership,first_branch,party_job,now_organiz,group_type_id,community_info,group_live,reward_punishment_id,former_company_id,now_company_id,");
            strSql.Append("is_badly_off,is_organiz_identity,financial_situation_id,healthy_condition_id,badly_off_reason_id,badly_off_describe,enjoy_help_id,float_commie_id,wechat) ");
            strSql.Append(" values (");
            strSql.Append("@group_id,@user_name,@salt,@password,@mobile,@email,@nick_name,@sex,@birthday,@telphone,@area,@address,@qq,@msn,@amount,@point,@exp,@status,@reg_time,@reg_ip,@id_card,@nation,@marital_status,@new_class_id,@administration_rank_id,@military_rank_id,@police_rank_id,@children_info,@only_child_award,@passport_info_id,");
            strSql.Append("@entitled_group_id,@income_source_id,@native_place,@live_place,@graduate_school,@graduate_time,@education_info_id,@degree_info,@join_party_time,@party_membership,@first_branch,@party_job,@now_organiz,@group_type_id,@community_info,@group_live,@reward_punishment_id,@former_company_id,@now_company_id,");
            strSql.Append("@is_badly_off,@is_organiz_identity,@financial_situation_id,@healthy_condition_id,@badly_off_reason_id,@badly_off_describe,@enjoy_help_id,@float_commie_id,@wechat) ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@group_id", SqlDbType.NVarChar,255),
                    new SqlParameter("@user_name", SqlDbType.NVarChar,100),
                    new SqlParameter("@salt", SqlDbType.NVarChar,20),
                    new SqlParameter("@password", SqlDbType.NVarChar,100),
                    new SqlParameter("@mobile", SqlDbType.NVarChar,20),
                    new SqlParameter("@email", SqlDbType.NVarChar,50),
                    new SqlParameter("@nick_name", SqlDbType.NVarChar,100),
                    new SqlParameter("@sex", SqlDbType.NVarChar,20),
                    new SqlParameter("@birthday", SqlDbType.DateTime),
                    new SqlParameter("@telphone", SqlDbType.NVarChar,50),
                    new SqlParameter("@area", SqlDbType.NVarChar,255),
                    new SqlParameter("@address", SqlDbType.NVarChar,255),
                    new SqlParameter("@qq", SqlDbType.NVarChar,20),
                    new SqlParameter("@msn", SqlDbType.NVarChar,100),
                    new SqlParameter("@amount", SqlDbType.Decimal,5),
                    new SqlParameter("@point", SqlDbType.Int,4),
                    new SqlParameter("@exp", SqlDbType.Int,4),
                    new SqlParameter("@status", SqlDbType.TinyInt,1),
                    new SqlParameter("@reg_time", SqlDbType.DateTime),
                    new SqlParameter("@reg_ip", SqlDbType.NVarChar,20),
                    new SqlParameter("@id_card", SqlDbType.NVarChar, 20),
                    new SqlParameter("@nation", SqlDbType.NVarChar, 50),
                    new SqlParameter("@marital_status", SqlDbType.Int,4),
                    new SqlParameter("@new_class_id", SqlDbType.NVarChar, 50),
                    new SqlParameter("@administration_rank_id", SqlDbType.NVarChar, 50),
                    new SqlParameter("@military_rank_id", SqlDbType.NVarChar, 50),
                    new SqlParameter("@police_rank_id", SqlDbType.NVarChar, 50),
                    new SqlParameter("@children_info", SqlDbType.Int,4),
                    new SqlParameter("@only_child_award", SqlDbType.Int, 4),
                    new SqlParameter("@passport_info_id", SqlDbType.NVarChar, 50),
                    new SqlParameter("@entitled_group_id", SqlDbType.NVarChar, 50),
                    new SqlParameter("@income_source_id", SqlDbType.NVarChar, 100),
                    new SqlParameter("@native_place", SqlDbType.NVarChar, 100),
                    new SqlParameter("@live_place", SqlDbType.NVarChar,100),
                    new SqlParameter("@graduate_school", SqlDbType.NVarChar,100),
                    new SqlParameter("@graduate_time", SqlDbType.DateTime),
                    new SqlParameter("@education_info_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@degree_info", SqlDbType.Int,4),
                    new SqlParameter("@join_party_time", SqlDbType.DateTime),
                    new SqlParameter("@party_membership", SqlDbType.NVarChar,50),
                    new SqlParameter("@first_branch", SqlDbType.NVarChar,50),
                    new SqlParameter("@party_job", SqlDbType.NVarChar,50),
                    new SqlParameter("@now_organiz", SqlDbType.NVarChar,50),
                    new SqlParameter("@group_type_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@community_info", SqlDbType.NVarChar,100),
                    new SqlParameter("@group_live", SqlDbType.NVarChar,100),
                    new SqlParameter("@reward_punishment_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@former_company_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@now_company_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@is_badly_off", SqlDbType.Int,4),
                    new SqlParameter("@is_organiz_identity", SqlDbType.Int,4),
                    new SqlParameter("@financial_situation_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@healthy_condition_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@badly_off_reason_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@badly_off_describe", SqlDbType.NText,1000),
                    new SqlParameter("@enjoy_help_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@float_commie_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@wechat", SqlDbType.NVarChar,50)};
            parameters[0].Value = ","+model.group_id+",";
            parameters[1].Value = model.user_name;
            parameters[2].Value = model.salt;
            parameters[3].Value = model.password;
            parameters[4].Value = model.mobile;
            parameters[5].Value = model.email;
            parameters[6].Value = model.nick_name;
            parameters[7].Value = model.sex;
            parameters[8].Value = model.birthday;
            parameters[9].Value = model.telphone;
            parameters[10].Value = model.area;
            parameters[11].Value = model.address;
            parameters[12].Value = model.qq;
            parameters[13].Value = model.msn;
            parameters[14].Value = model.amount;
            parameters[15].Value = model.point;
            parameters[16].Value = model.exp;
            //parameters[17].Value = model.status;
            parameters[17].Value = 1;
            parameters[18].Value = model.reg_time;
            parameters[19].Value = model.reg_ip;
            parameters[20].Value = model.id_card;
            parameters[21].Value = model.nation;
            parameters[22].Value = model.marital_status;
            parameters[23].Value = model.new_class_id;
            parameters[24].Value = model.administration_rank_id;
            parameters[25].Value = model.military_rank_id;
            parameters[26].Value = model.police_rank_id;
            parameters[27].Value = model.children_info;
            parameters[28].Value = model.only_child_award;
            parameters[29].Value = model.passport_info_id;
            parameters[30].Value = model.education_info_id;
            parameters[31].Value = model.income_source_id;
            parameters[32].Value = model.native_place;
            parameters[33].Value = model.live_place;
            parameters[34].Value = model.graduate_school;
            parameters[35].Value = model.graduate_time;
            parameters[36].Value = model.education_info_id;
            parameters[37].Value = model.degree_info;
            parameters[38].Value = model.join_party_time;
            parameters[39].Value = model.party_membership;
            parameters[40].Value = model.first_branch;
            parameters[41].Value = model.party_job;
            parameters[42].Value = model.now_organiz;
            parameters[43].Value = model.group_type_id;
            parameters[44].Value = model.community_info;
            parameters[45].Value = model.group_live;
            parameters[46].Value = model.reward_punishment_id;
            parameters[47].Value = model.former_company_id;
            parameters[48].Value = model.now_company_id;
            parameters[49].Value = model.is_badly_off;
            parameters[50].Value = model.is_organiz_identity;
            parameters[51].Value = model.financial_situation_id;
            parameters[52].Value = model.healthy_condition_id;
            parameters[53].Value = model.badly_off_reason_id;
            parameters[54].Value = model.badly_off_describe;
            parameters[55].Value = model.enjoy_help_id;
            parameters[56].Value = model.float_commie_id;
            parameters[57].Value = model.wechat;
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);

            QiNiuHelper helper = new QiNiuHelper();
            string imageUrl = String.IsNullOrEmpty(model.avatar) ? "" : helper.GetQiNiuFileUrl(model.avatar);
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
            parameters2[1].Value = obj;
            parameters2[2].Value = imageUrl;
            parameters2[3].Value = DateTime.Now;
            parameters2[4].Value = obj;
            parameters2[5].Value = 0;
            parameters2[6].Value = (int)ImageTypeEnum.头像;
            parameters2[7].Value = model.avatar;
            object obj2 = DbHelperSQL.GetSingle(image.ToString(), parameters2);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }         
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "users set ");
            strSql.Append("group_id=@group_id,");
            strSql.Append("user_name=@user_name,");
            strSql.Append("salt=@salt,");
            strSql.Append("password=@password,");
            strSql.Append("mobile=@mobile,");
            strSql.Append("email=@email,");
            strSql.Append("nick_name=@nick_name,");
            strSql.Append("sex=@sex,");
            strSql.Append("birthday=@birthday,");
            strSql.Append("telphone=@telphone,");
            strSql.Append("area=@area,");
            strSql.Append("address=@address,");
            strSql.Append("qq=@qq,");
            strSql.Append("msn=@msn,");
            strSql.Append("amount=@amount,");
            strSql.Append("point=@point,");
            strSql.Append("exp=@exp,");
            //strSql.Append("status=@status,");
            strSql.Append("reg_time=@reg_time,");
            strSql.Append("reg_ip=@reg_ip,");
            strSql.Append("client_id=@client_id,");
            strSql.Append("login_time=@login_time,");
            strSql.Append("role_id=@role_id,");
            strSql.Append("id_card=@id_card,");
            strSql.Append("nation=@nation,");
            strSql.Append("marital_status=@marital_status,");
            strSql.Append("new_class_id=@new_class_id,");
            strSql.Append("administration_rank_id=@administration_rank_id,");
            strSql.Append("military_rank_id=@military_rank_id,");
            strSql.Append("police_rank_id=@police_rank_id,");
            strSql.Append("children_info=@children_info,");
            strSql.Append("only_child_award=@only_child_award,");
            strSql.Append("passport_info_id=@passport_info_id,");
            strSql.Append("entitled_group_id=@entitled_group_id,");
            strSql.Append("income_source_id=@income_source_id,");
            strSql.Append("native_place=@native_place,");
            strSql.Append("live_place=@live_place,");
            strSql.Append("graduate_school=@graduate_school,");
            strSql.Append("graduate_time=@graduate_time,");
            strSql.Append("education_info_id=@education_info_id,");
            strSql.Append("degree_info=@degree_info,");
            strSql.Append("join_party_time=@join_party_time,");
            strSql.Append("party_membership=@party_membership,");
            strSql.Append("first_branch=@first_branch,");
            strSql.Append("party_job=@party_job,");
            strSql.Append("now_organiz=@now_organiz,");
            strSql.Append("group_type_id=@group_type_id,");
            strSql.Append("community_info=@community_info,");
            strSql.Append("group_live=@group_live,");
            strSql.Append("reward_punishment_id=@reward_punishment_id,");
            strSql.Append("former_company_id=@former_company_id,");
            strSql.Append("now_company_id=@now_company_id,");
            strSql.Append("is_badly_off=@is_badly_off,");
            strSql.Append("is_organiz_identity=@is_organiz_identity,");
            strSql.Append("financial_situation_id=@financial_situation_id,");
            strSql.Append("healthy_condition_id=@healthy_condition_id,");
            strSql.Append("badly_off_reason_id=@badly_off_reason_id,");
            strSql.Append("badly_off_describe=@badly_off_describe,");
            strSql.Append("enjoy_help_id=@enjoy_help_id,");
            strSql.Append("float_commie_id=@float_commie_id,");
            strSql.Append("wechat=@wechat");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@group_id", SqlDbType.NVarChar,255),
                    new SqlParameter("@user_name", SqlDbType.NVarChar,100),
                    new SqlParameter("@salt", SqlDbType.NVarChar,20),
                    new SqlParameter("@password", SqlDbType.NVarChar,100),
                    new SqlParameter("@mobile", SqlDbType.NVarChar,20),
                    new SqlParameter("@email", SqlDbType.NVarChar,50),
                    new SqlParameter("@nick_name", SqlDbType.NVarChar,100),
                    new SqlParameter("@sex", SqlDbType.NVarChar,20),
                    new SqlParameter("@birthday", SqlDbType.DateTime),
                    new SqlParameter("@telphone", SqlDbType.NVarChar,50),
                    new SqlParameter("@area", SqlDbType.NVarChar,255),
                    new SqlParameter("@address", SqlDbType.NVarChar,255),
                    new SqlParameter("@qq", SqlDbType.NVarChar,20),
                    new SqlParameter("@msn", SqlDbType.NVarChar,100),
                    new SqlParameter("@amount", SqlDbType.Decimal,5),
                    new SqlParameter("@point", SqlDbType.Int,4),
                    new SqlParameter("@exp", SqlDbType.Int,4),
                    //new SqlParameter("@status", SqlDbType.TinyInt,1),
                    new SqlParameter("@reg_time", SqlDbType.DateTime),
                    new SqlParameter("@reg_ip", SqlDbType.NVarChar,20),
                    new SqlParameter("@client_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@login_time", SqlDbType.DateTime),
                    new SqlParameter("@role_id", SqlDbType.Int,4),
                    new SqlParameter("@id_card", SqlDbType.NVarChar,20),
                    new SqlParameter("@nation", SqlDbType.NVarChar,50),
                    new SqlParameter("@marital_status", SqlDbType.Int,4),
                    new SqlParameter("@new_class_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@administration_rank_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@military_rank_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@police_rank_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@children_info", SqlDbType.Int,4),
                    new SqlParameter("@only_child_award", SqlDbType.Int,4),
                    new SqlParameter("@passport_info_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@entitled_group_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@income_source_id", SqlDbType.NVarChar,100),
                    new SqlParameter("@native_place", SqlDbType.NVarChar,100),
                    new SqlParameter("@live_place", SqlDbType.NVarChar,100),
                    new SqlParameter("@graduate_school", SqlDbType.NVarChar,100),
                    new SqlParameter("@graduate_time", SqlDbType.DateTime),
                    new SqlParameter("@education_info_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@degree_info", SqlDbType.Int,4),
                    new SqlParameter("@join_party_time", SqlDbType.DateTime),
                    new SqlParameter("@party_membership", SqlDbType.NVarChar,50),
                    new SqlParameter("@first_branch", SqlDbType.NVarChar,50),
                    new SqlParameter("@party_job", SqlDbType.NVarChar,50),
                    new SqlParameter("@now_organiz", SqlDbType.NVarChar,50),
                    new SqlParameter("@group_type_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@community_info", SqlDbType.NVarChar,100),
                    new SqlParameter("@group_live", SqlDbType.NVarChar,100),
                    new SqlParameter("@reward_punishment_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@former_company_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@now_company_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@is_badly_off", SqlDbType.Int,4),
                    new SqlParameter("@is_organiz_identity", SqlDbType.Int,4),
                    new SqlParameter("@financial_situation_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@healthy_condition_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@badly_off_reason_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@badly_off_describe", SqlDbType.NText,500),
                    new SqlParameter("@enjoy_help_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@float_commie_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@wechat", SqlDbType.NVarChar,50),
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = "," + model.group_id + ",";
            parameters[1].Value = model.user_name;
            parameters[2].Value = model.salt;
            parameters[3].Value = model.password;
            parameters[4].Value = model.mobile;
            parameters[5].Value = model.email;
            parameters[6].Value = model.nick_name;
            parameters[7].Value = model.sex;
            parameters[8].Value = model.birthday;
            parameters[9].Value = model.telphone;
            parameters[10].Value = model.area;
            parameters[11].Value = model.address;
            parameters[12].Value = model.qq;
            parameters[13].Value = model.msn;
            parameters[14].Value = model.amount;
            parameters[15].Value = model.point;
            parameters[16].Value = model.exp;
            //parameters[17].Value = 1;
            //parameters[17].Value = model.status;
            parameters[17].Value = model.reg_time;
            parameters[18].Value = model.reg_ip;
            parameters[19].Value = model.client_id;
            parameters[20].Value = model.login_time;
            parameters[21].Value = model.role_id;
            parameters[22].Value = model.id_card;
            parameters[23].Value = model.nation;
            parameters[24].Value = model.marital_status;
            parameters[25].Value = model.new_class_id;
            parameters[26].Value = model.administration_rank_id;
            parameters[27].Value = model.military_rank_id;
            parameters[28].Value = model.police_rank_id;
            parameters[29].Value = model.children_info;
            parameters[30].Value = model.only_child_award;
            parameters[31].Value = model.passport_info_id;
            parameters[32].Value = model.entitled_group_id;
            parameters[33].Value = model.income_source_id;
            parameters[34].Value = model.native_place;
            parameters[35].Value = model.live_place;
            parameters[36].Value = model.graduate_school;
            parameters[37].Value = model.graduate_time;
            parameters[38].Value = model.education_info_id;
            parameters[39].Value = model.degree_info;
            parameters[40].Value = model.join_party_time;
            parameters[41].Value = model.party_membership;
            parameters[42].Value = model.first_branch;
            parameters[43].Value = model.party_job;
            parameters[44].Value = model.now_organiz;
            parameters[45].Value = model.group_type_id;
            parameters[46].Value = model.community_info;
            parameters[47].Value = model.group_live;
            parameters[48].Value = model.reward_punishment_id;
            parameters[49].Value = model.former_company_id;
            parameters[50].Value = model.now_company_id;
            parameters[51].Value = model.is_badly_off;
            parameters[52].Value = model.is_organiz_identity;
            parameters[53].Value = model.financial_situation_id;
            parameters[54].Value = model.healthy_condition_id;
            parameters[55].Value = model.badly_off_reason_id;
            parameters[56].Value = model.badly_off_describe;
            parameters[57].Value = model.enjoy_help_id;
            parameters[58].Value = model.float_commie_id;
            parameters[59].Value = model.wechat;
            parameters[60].Value = model.id;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

            QiNiuHelper qiNiuHelper = new QiNiuHelper();
            string imageUrl = String.IsNullOrEmpty(model.avatar) ? "" : qiNiuHelper.GetQiNiuFileUrl(model.avatar);
            StringBuilder image = new StringBuilder();
            image.Append("update P_Image set ");
            image.Append("P_ImageUrl=@P_ImageUrl,");
            image.Append("P_PictureName=@P_PictureName,");
            image.Append("P_UpdateTime=@P_UpdateTime ");
            image.Append("where P_ImageId=@P_ImageId ");
            image.Append("and P_ImageType=@P_ImageType");
            SqlParameter[] parameters2 = {
                            new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,200),
                            new SqlParameter("@P_PictureName", SqlDbType.NVarChar,100),
                            new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_ImageId", SqlDbType.NVarChar,36),
                            new SqlParameter("@P_ImageType", SqlDbType.NVarChar,100)
                            };
            parameters2[0].Value = imageUrl;
            parameters2[1].Value = model.avatar;
            parameters2[2].Value = DateTime.Now;
            parameters2[3].Value = model.id;
            parameters2[4].Value = (int)ImageTypeEnum.头像;
            object obj2 = DbHelperSQL.GetSingle(image.ToString(), parameters2);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            //获取用户旧数据
            Model.users model = GetModel(id);
            if (model == null)
            {
                return false;
            }

            List<CommandInfo> sqllist = new List<CommandInfo>();
            //删除积分记录
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from " + databaseprefix + "user_point_log ");
            strSql1.Append(" where user_id=@id");
            SqlParameter[] parameters1 = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters1[0].Value = id;
            CommandInfo cmd = new CommandInfo(strSql1.ToString(), parameters1);
            sqllist.Add(cmd);

            //删除金额记录
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from " + databaseprefix + "user_amount_log ");
            strSql2.Append(" where user_id=@id");
            SqlParameter[] parameters2 = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters2[0].Value = id;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            //删除附件购买记录
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("delete from " + databaseprefix + "user_attach_log");
            strSql3.Append(" where user_id=@id");
            SqlParameter[] parameters3 = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters3[0].Value = id;
            cmd = new CommandInfo(strSql3.ToString(), parameters3);
            sqllist.Add(cmd);

            //删除短消息
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("delete from " + databaseprefix + "user_message ");
            strSql4.Append(" where post_user_name=@post_user_name or accept_user_name=@accept_user_name");
            SqlParameter[] parameters4 = {
                    new SqlParameter("@post_user_name", SqlDbType.NVarChar,100),
                    new SqlParameter("@accept_user_name", SqlDbType.NVarChar,100)};
            parameters4[0].Value = model.user_name;
            parameters4[1].Value = model.user_name;
            cmd = new CommandInfo(strSql4.ToString(), parameters4);
            sqllist.Add(cmd);

            //删除申请码
            StringBuilder strSql5 = new StringBuilder();
            strSql5.Append("delete from " + databaseprefix + "user_code ");
            strSql5.Append(" where user_id=@id");
            SqlParameter[] parameters5 = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters5[0].Value = id;
            cmd = new CommandInfo(strSql5.ToString(), parameters5);
            sqllist.Add(cmd);

            //删除登录日志
            StringBuilder strSql6 = new StringBuilder();
            strSql6.Append("delete from " + databaseprefix + "user_login_log ");
            strSql6.Append(" where user_id=@id");
            SqlParameter[] parameters6 = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters6[0].Value = id;
            cmd = new CommandInfo(strSql6.ToString(), parameters6);
            sqllist.Add(cmd);

            //删除OAuth授权用户信息
            StringBuilder strSql8 = new StringBuilder();
            strSql8.Append("delete from " + databaseprefix + "user_oauth ");
            strSql8.Append(" where user_id=@id");
            SqlParameter[] parameters8 = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters8[0].Value = id;
            cmd = new CommandInfo(strSql8.ToString(), parameters8);
            sqllist.Add(cmd);

            //删除用户充值表
            StringBuilder strSql9 = new StringBuilder();
            strSql9.Append("delete from " + databaseprefix + "user_recharge ");
            strSql9.Append(" where user_id=@id");
            SqlParameter[] parameters9 = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters9[0].Value = id;
            cmd = new CommandInfo(strSql9.ToString(), parameters9);
            sqllist.Add(cmd);

            //删除用户主表
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + databaseprefix + "users ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.users GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 id,user_name,salt,password,mobile,email,nick_name,sex,birthday,telphone,area,address,qq,msn,amount,point,exp,status,reg_time,reg_ip,client_id,login_time,role_id,
                            id_card, nation, marital_status,new_class_id,administration_rank_id,military_rank_id,police_rank_id,children_info,only_child_award,passport_info_id,entitled_group_id,income_source_id,native_place,
                            live_place,graduate_school,graduate_time,education_info_id,degree_info,join_party_time,party_membership,first_branch,party_job,now_organiz,group_type_id,community_info,group_live,reward_punishment_id,
                            former_company_id,now_company_id,is_badly_off,is_organiz_identity,financial_situation_id,healthy_condition_id,badly_off_reason_id,badly_off_describe,enjoy_help_id,float_commie_id,wechat,
                            float_commie,com_name,com_type_id");
            strSql.Append(" from " + databaseprefix + "users");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)
            };
            parameters[0].Value = id;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Model.users userData = DataRowToModel(ds.Tables[0].Rows[0]);
                userData.group_id = new UserGroupHelper().GetUserMinimumGroupId(userData.id).ToString();
                return userData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户名密码返回一个实体
        /// </summary>
        public Model.users GetModel(string user_name, string password, int emaillogin, int mobilelogin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,user_name,salt,password,mobile,email,avatar,nick_name,sex,birthday,telphone,area,address,qq,msn,amount,point,exp,status,reg_time,reg_ip");
            strSql.Append(" from " + databaseprefix + "users");
            strSql.Append(" where (user_name=@user_name");
            if (emaillogin == 1)
            {
                strSql.Append(" or email=@user_name");
            }
            if (mobilelogin == 1)
            {
                strSql.Append(" or mobile=@user_name");
            }
            strSql.Append(") and password=@password and status<3");

            SqlParameter[] parameters = {
                        new SqlParameter("@user_name", SqlDbType.NVarChar,100),
                        new SqlParameter("@password", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;
            parameters[1].Value = password;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Model.users userData = DataRowToModel(ds.Tables[0].Rows[0]);
                userData.group_id = new UserGroupHelper().GetUserMinimumGroupId(userData.id).ToString();
                return userData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户名返回一个实体
        /// </summary>
        public Model.users GetModel(string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,user_name,salt,password,mobile,email,avatar,nick_name,sex,birthday,telphone,area,address,qq,msn,amount,point,exp,status,reg_time,reg_ip");
            strSql.Append(" from " + databaseprefix + "users");
            strSql.Append(" where user_name=@user_name and status<3");
            SqlParameter[] parameters = {
                    new SqlParameter("@user_name", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Model.users userData = DataRowToModel(ds.Tables[0].Rows[0]);
                userData.group_id = new UserGroupHelper().GetUserMinimumGroupId(userData.id).ToString();
                return userData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,user_name,salt,password,mobile,email,avatar,nick_name,sex,birthday,telphone,area,address,qq,msn,amount,point,exp,status,reg_time,reg_ip,");
            strSql.Append("(select TOP 1 t.value from F_Split((select dt_users.group_id from dt_users where dt_users.id = table1.id),',') as t left join dt_user_groups on dt_user_groups.id = t.value where t.value != '' order by dt_user_groups.grade DESC) as group_id");
            strSql.Append(" FROM " + databaseprefix + "users as table1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + "id,user_name,salt,password,mobile,email,(select P_ImageUrl from P_Image where P_Status = 0 and P_ImageType = 20011 and P_ImageId = table1.id) as avatar,nick_name,sex,birthday,telphone,area,address,qq,msn,amount,point,exp,status,reg_time,reg_ip,"
                + "(select TOP 1 t.value from F_Split((select dt_users.group_id from dt_users where dt_users.id = table1.id),',') as t left join dt_user_groups on dt_user_groups.id = t.value where t.value != '' order by dt_user_groups.grade DESC) as group_id"
                + " FROM " + databaseprefix + "users as table1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        #endregion

        #region 扩展方法================================
        /// <summary>
        /// 检查Email是否存在
        /// </summary>
        public bool ExistsEmail(string email)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "users");
            strSql.Append(" where email=@email ");
            SqlParameter[] parameters = {
                    new SqlParameter("@email", SqlDbType.NVarChar,100)};
            parameters[0].Value = email;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 检查手机号码是否存在
        /// </summary>
        public bool ExistsMobile(string mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "users");
            strSql.Append(" where mobile=@mobile ");
            SqlParameter[] parameters = {
                    new SqlParameter("@mobile", SqlDbType.NVarChar,20)};
            parameters[0].Value = mobile;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据用户名取得Salt
        /// </summary>
        public string GetSalt(string user_name)
        {
            //尝试用户名取得Salt
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 salt from " + databaseprefix + "users");
            strSql.Append(" where user_name=@user_name");
            SqlParameter[] parameters = {
                    new SqlParameter("@user_name", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;
            string salt = Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString(), parameters));
            if (!string.IsNullOrEmpty(salt))
            {
                return salt;
            }
            //尝试用手机号取得Salt
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("select top 1 salt from " + databaseprefix + "users");
            strSql1.Append(" where mobile=@mobile");
            SqlParameter[] parameters1 = {
                    new SqlParameter("@mobile", SqlDbType.NVarChar,20)};
            parameters1[0].Value = user_name;
            salt = Convert.ToString(DbHelperSQL.GetSingle(strSql1.ToString(), parameters1));
            if (!string.IsNullOrEmpty(salt))
            {
                return salt;
            }
            //尝试用邮箱取得Salt
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("select top 1 salt from " + databaseprefix + "users");
            strSql2.Append(" where email=@email");
            SqlParameter[] parameters2 = {
                    new SqlParameter("@email", SqlDbType.NVarChar,50)};
            parameters2[0].Value = user_name;
            salt = Convert.ToString(DbHelperSQL.GetSingle(strSql2.ToString(), parameters2));
            if (!string.IsNullOrEmpty(salt))
            {
                return salt;
            }
            return string.Empty;
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public int UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "users set " + strValue);
            strSql.Append(" where id=" + id);
            return DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个Users对象实体
        /// </summary>
        public Model.users DataRowToModel(DataRow row)
        {
            Model.users model = new Model.users();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                //if (row["group_id"] != null && row["group_id"].ToString() != "")
                //{
                //	model.group_id = row["group_id"].ToString();
                //}
                if (row["user_name"] != null)
                {
                    model.user_name = row["user_name"].ToString();
                }
                if (row["salt"] != null)
                {
                    model.salt = row["salt"].ToString();
                }
                if (row["password"] != null)
                {
                    model.password = row["password"].ToString();
                }
                if (row["mobile"] != null)
                {
                    model.mobile = row["mobile"].ToString();
                }
                if (row["email"] != null)
                {
                    model.email = row["email"].ToString();
                }
                //if (row["avatar"] != null)
                //{
                //    model.avatar = row["avatar"].ToString();
                //}
                if (row["nick_name"] != null)
                {
                    model.nick_name = row["nick_name"].ToString();
                }
                if (row["sex"] != null)
                {
                    model.sex = row["sex"].ToString();
                }
                if (row["birthday"] != null && row["birthday"].ToString() != "")
                {
                    model.birthday = DateTime.Parse(row["birthday"].ToString());
                }
                if (row["telphone"] != null)
                {
                    model.telphone = row["telphone"].ToString();
                }
                if (row["area"] != null)
                {
                    model.area = row["area"].ToString();
                }
                if (row["address"] != null)
                {
                    model.address = row["address"].ToString();
                }
                if (row["qq"] != null)
                {
                    model.qq = row["qq"].ToString();
                }
                if (row["msn"] != null)
                {
                    model.msn = row["msn"].ToString();
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row["point"] != null && row["point"].ToString() != "")
                {
                    model.point = int.Parse(row["point"].ToString());
                }
                if (row["exp"] != null && row["exp"].ToString() != "")
                {
                    model.exp = int.Parse(row["exp"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
                if (row["reg_time"] != null && row["reg_time"].ToString() != "")
                {
                    model.reg_time = DateTime.Parse(row["reg_time"].ToString());
                }
                if (row["reg_ip"] != null)
                {
                    model.reg_ip = row["reg_ip"].ToString();
                }
                if (row["client_id"] != null && row["client_id"].ToString() != "")
                {
                    model.client_id = row["client_id"].ToString();
                }
                if (row["login_time"] != null && row["login_time"].ToString() != "")
                {
                    model.login_time = DateTime.Parse(row["login_time"].ToString());
                }
                if (row["role_id"] != null && row["role_id"].ToString() != "")
                {
                    model.role_id = int.Parse(row["role_id"].ToString());
                }
                if (row["id_card"] != null)
                {
                    model.id_card = row["id_card"].ToString();
                }
                if (row["nation"] != null)
                {
                    model.nation = row["nation"].ToString();
                }
                if (row["marital_status"] != null && row["marital_status"].ToString() != "")
                {
                    model.marital_status = int.Parse(row["marital_status"].ToString());
                }
                if (row["new_class_id"] != null && row["new_class_id"].ToString() != "")
                {
                    model.new_class_id = row["new_class_id"].ToString();
                }
                if (row["administration_rank_id"] != null && row["administration_rank_id"].ToString() != "")
                {
                    model.administration_rank_id = row["administration_rank_id"].ToString();
                }
                if (row["military_rank_id"] != null && row["military_rank_id"].ToString() != "")
                {
                    model.military_rank_id = row["military_rank_id"].ToString();
                }
                if (row["police_rank_id"] != null && row["police_rank_id"].ToString() != "")
                {
                    model.police_rank_id = row["police_rank_id"].ToString();
                }
                if (row["children_info"] != null && row["children_info"].ToString() != "")
                {
                    model.children_info = int.Parse(row["children_info"].ToString());
                }
                if (row["only_child_award"] != null && row["only_child_award"].ToString() != "")
                {
                    model.only_child_award = int.Parse(row["only_child_award"].ToString());
                }
                if (row["passport_info_id"] != null && row["passport_info_id"].ToString() != "")
                {
                    model.passport_info_id = row["passport_info_id"].ToString();
                }
                if (row["entitled_group_id"] != null && row["entitled_group_id"].ToString() != "")
                {
                    model.entitled_group_id = row["entitled_group_id"].ToString();
                }
                if (row["income_source_id"] != null && row["income_source_id"].ToString() != "")
                {
                    model.income_source_id = row["income_source_id"].ToString();
                }
                if (row["native_place"] != null && row["native_place"].ToString() != "")
                {
                    model.native_place = row["native_place"].ToString();
                }
                if (row["live_place"] != null && row["live_place"].ToString() != "")
                {
                    model.live_place = row["live_place"].ToString();
                }
                if (row["graduate_school"] != null && row["graduate_school"].ToString() != "")
                {
                    model.graduate_school = row["graduate_school"].ToString();
                }
                if (row["graduate_time"] != null && row["graduate_time"].ToString() != "")
                {
                    model.graduate_time = DateTime.Parse(row["graduate_time"].ToString());
                }
                if (row["education_info_id"] != null && row["education_info_id"].ToString() != "")
                {
                    model.education_info_id = row["education_info_id"].ToString();
                }
                if (row["degree_info"] != null && row["degree_info"].ToString() != "")
                {
                    model.degree_info = int.Parse(row["degree_info"].ToString());
                }
                if (row["join_party_time"] != null && row["join_party_time"].ToString() != "")
                {
                    model.join_party_time = DateTime.Parse(row["join_party_time"].ToString());
                }
                if (row["party_membership"] != null && row["party_membership"].ToString() != "")
                {
                    model.party_membership = row["party_membership"].ToString();
                }
                if (row["first_branch"] != null && row["first_branch"].ToString() != "")
                {
                    model.first_branch = row["first_branch"].ToString();
                }
                if (row["party_job"] != null && row["party_job"].ToString() != "")
                {
                    model.party_job = row["party_job"].ToString();
                }
                if (row["now_organiz"] != null && row["now_organiz"].ToString() != "")
                {
                    model.now_organiz = row["now_organiz"].ToString();
                }
                if (row["group_type_id"] != null && row["group_type_id"].ToString() != "")
                {
                    model.group_type_id = row["group_type_id"].ToString();
                }
                if (row["community_info"] != null && row["community_info"].ToString() != "")
                {
                    model.community_info = row["community_info"].ToString();
                }
                if (row["group_live"] != null && row["group_live"].ToString() != "")
                {
                    model.group_live = row["group_live"].ToString();
                }
                if (row["reward_punishment_id"] != null && row["reward_punishment_id"].ToString() != "")
                {
                    model.reward_punishment_id = row["reward_punishment_id"].ToString();
                }
                if (row["former_company_id"] != null && row["former_company_id"].ToString() != "")
                {
                    model.former_company_id = row["former_company_id"].ToString();
                }
                if (row["now_company_id"] != null && row["now_company_id"].ToString() != "")
                {
                    model.now_company_id = row["now_company_id"].ToString();
                }
                if (row["is_badly_off"] != null && row["is_badly_off"].ToString() != "")
                {
                    model.is_badly_off = int.Parse(row["is_badly_off"].ToString());
                }
                if (row["is_organiz_identity"] != null && row["is_organiz_identity"].ToString() != "")
                {
                    model.is_organiz_identity = int.Parse(row["is_organiz_identity"].ToString());
                }
                if (row["wechat"] != null && row["wechat"].ToString() != "")
                {
                    model.wechat = row["wechat"].ToString();
                }
                if (row["financial_situation_id"] != null && row["financial_situation_id"].ToString() != "")
                {
                    model.financial_situation_id = row["financial_situation_id"].ToString();
                }
                if (row["healthy_condition_id"] != null && row["healthy_condition_id"].ToString() != "")
                {
                    model.healthy_condition_id = row["healthy_condition_id"].ToString();
                }
                if (row["badly_off_reason_id"] != null && row["badly_off_reason_id"].ToString() != "")
                {
                    model.badly_off_reason_id = row["badly_off_reason_id"].ToString();
                }
                if (row["badly_off_describe"] != null && row["badly_off_describe"].ToString() != "")
                {
                    model.badly_off_describe = row["badly_off_describe"].ToString();
                }
                if (row["enjoy_help_id"] != null && row["enjoy_help_id"].ToString() != "")
                {
                    model.enjoy_help_id = row["enjoy_help_id"].ToString();
                }
                if (row["float_commie_id"] != null && row["float_commie_id"].ToString() != "")
                {
                    model.float_commie_id = row["float_commie_id"].ToString();
                }
                if (row["com_name"] != null)
                {
                    model.com_name = row["com_name"].ToString();
                }
                if (row["com_type_id"] != null && row["com_type_id"].ToString() != "")
                {
                    model.com_type_id = row["com_type_id"].ToString();
                }
                if (row["party_job"] != null)
                {
                    model.party_job = row["party_job"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获取流动党员信息模型
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.u_float_commie floatDataRowToModel(DataRow row)
        {
            Model.u_float_commie model = new Model.u_float_commie();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["flow_type"] != null && row["flow_type"].ToString() != "")
                {
                    model.flow_type = row["flow_type"].ToString();
                }
                if (row["linkman"] != null && row["linkman"].ToString() != "")
                {
                    model.linkman = row["linkman"].ToString();
                }
                if (row["flow_reason"] != null && row["flow_reason"].ToString() != "")
                {
                    model.flow_reason = row["flow_reason"].ToString();
                }
                if (row["contact"] != null && row["contact"].ToString() != "")
                {
                    model.contact = row["contact"].ToString();
                }
                if (row["id_number"] != null && row["id_number"].ToString() != "")
                {
                    model.id_number = row["id_number"].ToString();
                }
                if (row["group_linkman"] != null && row["group_linkman"].ToString() != "")
                {
                    model.group_linkman = row["group_linkman"].ToString();
                }
                if (row["discharge_place"] != null && row["discharge_place"].ToString() != "")
                {
                    model.discharge_place = row["discharge_place"].ToString();
                }
                if (row["group_contact"] != null && row["group_contact"].ToString() != "")
                {
                    model.group_contact = row["group_contact"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获取享受帮扶情况模型
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.u_enjoy_help enjoyDataRowToModel(DataRow row)
        {
            Model.u_enjoy_help model = new Model.u_enjoy_help();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["start_time"] != null && row["start_time"].ToString() != "")
                {
                    model.start_time = Convert.ToDateTime(row["start_time"].ToString());
                }
                if (row["end_time"] != null && row["end_time"].ToString() != "")
                {
                    model.end_time = Convert.ToDateTime(row["end_time"].ToString());
                }
                if (row["help_way_id"] != null && row["help_way_id"].ToString() != "")
                {
                    model.help_way_id = row["help_way_id"].ToString();
                }
                if (row["remark"] != null && row["remark"].ToString() != "")
                {
                    model.remark = row["remark"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获取单位信息模型
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.u_company_type companyDataRowToModel(DataRow row)
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
                if (row["relation_com"] != null && row["relation_com"].ToString() != "")
                {
                    model.relation_com = row["relation_com"].ToString();
                }
                if (row["com_type_id"] != null && row["com_type_id"].ToString() != "")
                {
                    model.com_type_id = row["com_type_id"].ToString();
                }
                if (row["post_type_id"] != null && row["post_type_id"].ToString() != "")
                {
                    model.post_type_id = row["post_type_id"].ToString();
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


        /// <summary>
        /// 获取奖惩信息模型
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.u_reward_punishment rewardsDataRowToModel(DataRow row)
        {
            Model.u_reward_punishment model = new Model.u_reward_punishment();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["title"] != null)
                {
                    model.title = row["title"].ToString();
                }
                if (row["reason"] != null && row["reason"].ToString() != "")
                {
                    model.reason = row["reason"].ToString();
                }
                if (row["approval_authority"] != null)
                {
                    model.approval_authority = row["approval_authority"].ToString();
                }
                if (row["office_level"] != null)
                {
                    model.office_level = row["office_level"].ToString();
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
            }
            return model;
        }


        /// <summary>
        /// 获取头像模型
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Model.P_Image imageDataRowToModel(DataRow row)
        {
            Model.P_Image model = new Model.P_Image();
            if(row != null)
            {
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if(row["P_ImageUrl"] != null && row["P_ImageUrl"].ToString() != "")
                {
                    model.P_ImageUrl = row["P_ImageUrl"].ToString(); 
                }
                if (row["P_PictureName"] != null && row["P_PictureName"].ToString() != "")
                {
                    model.P_PictureName = row["P_PictureName"].ToString();
                }
                if (row["P_ImageType"] != null && row["P_ImageType"].ToString() != "")
                {
                    model.P_ImageType = row["P_ImageType"].ToString();
                }
            }
            return model;
        }
        #endregion


        #region 添加党员奖惩
        public bool addreward(u_reward_punishment model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("insert into u_reward_punishment(");
                        str.Append("id,title,reason,approval_authority,office_level,status) ");
                        str.Append("values(");
                        str.Append("@id,@title,@reason,@approval_authority,@office_level,@status) ");
                        str.Append(";select @@IDENTITY");
                        SqlParameter[] parameter = {
                            new SqlParameter("@id",SqlDbType.NVarChar,50),
                            new SqlParameter("@title",SqlDbType.NVarChar,50),
                            new SqlParameter("@reason",SqlDbType.NText,500),
                            new SqlParameter("@approval_authority",SqlDbType.NVarChar,100),
                            new SqlParameter("@office_level",SqlDbType.NVarChar,50),
                            new SqlParameter("@status",SqlDbType.Int,4)
                        };
                        parameter[0].Value = model.id;
                        parameter[1].Value = model.title;
                        parameter[2].Value = model.reason;
                        parameter[3].Value = model.approval_authority;
                        parameter[4].Value = model.office_level;
                        parameter[5].Value = 0;
                        object obj = DbHelperSQL.Query(conn, trans, str.ToString(), parameter);
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
        # endregion 
        
        #region 添加现工作单位信息
        public bool addcompany(u_company_type model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("insert into u_company_type(");
                        str.Append("id,name,employee_count,relation_com,com_type_id,post_type_id,status,type) ");
                        str.Append("values(");
                        str.Append("@id,@name,@employee_count,@relation_com,@com_type_id,@post_type_id,@status,@type) ");
                        str.Append(";select @@IDENTITY");
                        SqlParameter[] parameter = {
                            new SqlParameter("@id",SqlDbType.NVarChar,50),
                            new SqlParameter("@name",SqlDbType.NVarChar,50),
                            new SqlParameter("@employee_count",SqlDbType.Int,4),
                            new SqlParameter("@relation_com",SqlDbType.NVarChar,100),
                            new SqlParameter("@com_type_id",SqlDbType.NVarChar,50),
                            new SqlParameter("@post_type_id",SqlDbType.NVarChar,50),
                            new SqlParameter("@status",SqlDbType.Int,4),
                            new SqlParameter("@type",SqlDbType.Int,4)
                        };
                        parameter[0].Value = model.id;
                        parameter[1].Value = model.name;
                        parameter[2].Value = model.employee_count;
                        parameter[3].Value = model.relation_com;
                        parameter[4].Value = model.com_type_id;
                        parameter[5].Value = model.post_type_id;
                        parameter[6].Value = 0;
                        parameter[7].Value = 1;
                        object obj = DbHelperSQL.Query(conn, trans, str.ToString(), parameter);
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
        #endregion
        #region 添加原工作单位信息
        public bool addOrigCompany(u_company_type model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("insert into u_company_type(");
                        str.Append("id,name,employee_count,relation_com,com_type_id,post_type_id,status,type) ");
                        str.Append("values(");
                        str.Append("@id,@name,@employee_count,@relation_com,@com_type_id,@post_type_id,@status,@type) ");
                        str.Append(";select @@IDENTITY");
                        SqlParameter[] parameter = {
                            new SqlParameter("@id",SqlDbType.NVarChar,50),
                            new SqlParameter("@name",SqlDbType.NVarChar,50),
                            new SqlParameter("@employee_count",SqlDbType.Int,4),
                            new SqlParameter("@relation_com",SqlDbType.NVarChar,100),
                            new SqlParameter("@com_type_id",SqlDbType.NVarChar,50),
                            new SqlParameter("@post_type_id",SqlDbType.NVarChar,50),
                            new SqlParameter("@status",SqlDbType.Int,4),
                            new SqlParameter("@type",SqlDbType.Int,4)
                        };
                        parameter[0].Value = model.id;
                        parameter[1].Value = model.name;
                        parameter[2].Value = model.employee_count;
                        parameter[3].Value = model.relation_com;
                        parameter[4].Value = model.com_type_id;
                        parameter[5].Value = model.post_type_id;
                        parameter[6].Value = 0;
                        parameter[7].Value = 0;
                        object obj = DbHelperSQL.Query(conn, trans, str.ToString(), parameter);
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
        #endregion
        #region 添加帮扶详情
        public bool addEnjoyHelp(u_enjoy_help model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("insert into u_enjoy_help(");
                        str.Append("id,start_time,end_time,help_way_id,remark,status) ");
                        str.Append("values(");
                        str.Append("@id,@start_time,@end_time,@help_way_id,@remark,@status)");
                        str.Append(";select @@IDENTITY");
                        SqlParameter[] parameter = {
                            new SqlParameter("@id",SqlDbType.NVarChar,50),
                            new SqlParameter("@start_time",SqlDbType.DateTime),
                            new SqlParameter("@end_time",SqlDbType.DateTime),
                            new SqlParameter("@help_way_id",SqlDbType.NVarChar,50),
                            new SqlParameter("@remark",SqlDbType.NText),
                            new SqlParameter("@status",SqlDbType.Int,4)
                        };
                        parameter[0].Value = model.id;
                        parameter[1].Value = model.start_time;
                        parameter[2].Value = model.end_time;
                        parameter[3].Value = model.help_way_id;
                        parameter[4].Value = model.remark;
                        parameter[5].Value = 0;
                        object obj = DbHelperSQL.Query(conn, trans, str.ToString(), parameter);
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
        #endregion

        #region 添加流动党员信息
        public bool addFloatCommie(u_float_commie model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("insert into u_float_commie(");
                        str.Append("id,flow_type,linkman,flow_reason,contact,id_number,group_linkman,discharge_place,group_contact,status) ");
                        str.Append("values(");
                        str.Append("@id,@flow_type,@linkman,@flow_reason,@contact,@id_number,@group_linkman,@discharge_place,@group_contact,@status) ");
                        str.Append(";select @@IDENTITY");
                        SqlParameter[] parameter = {
                            new SqlParameter("@id",SqlDbType.NVarChar,50),
                            new SqlParameter("@flow_type",SqlDbType.NVarChar,50),
                            new SqlParameter("@linkman",SqlDbType.NVarChar,50),
                            new SqlParameter("@flow_reason",SqlDbType.NText,500),
                            new SqlParameter("@contact",SqlDbType.NVarChar,100),
                            new SqlParameter("@id_number",SqlDbType.NVarChar,50),
                            new SqlParameter("@group_linkman",SqlDbType.NVarChar,50),
                            new SqlParameter("@discharge_place",SqlDbType.NVarChar,100),
                            new SqlParameter("@group_contact",SqlDbType.NVarChar,100),
                            new SqlParameter("@status",SqlDbType.Int,4)
                        };
                        parameter[0].Value = model.id;
                        parameter[1].Value = model.flow_type;
                        parameter[2].Value = model.linkman;
                        parameter[3].Value = model.flow_reason;
                        parameter[4].Value = model.contact;
                        parameter[5].Value = model.id_number;
                        parameter[6].Value = model.group_linkman;
                        parameter[7].Value = model.discharge_place;
                        parameter[8].Value = model.group_contact;
                        parameter[9].Value = 0;
                        object obj = DbHelperSQL.Query(conn, trans, str.ToString(), parameter);
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
        #endregion

        #region ==========
        /// <summary>
        /// 修改奖惩信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatereward(u_reward_punishment model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("update u_reward_punishment set title=@title,reason=@reason,approval_authority=@approval_authority,office_level=@office_level  where id=@reward_punishment_id");
                        SqlParameter[] parameter = {
                        new SqlParameter("@title",SqlDbType.NVarChar,50),
                        new SqlParameter("@reason",SqlDbType.NText,500),
                        new SqlParameter("@approval_authority",SqlDbType.NVarChar,100),
                        new SqlParameter("@office_level",SqlDbType.NVarChar,50),
                        new SqlParameter("@reward_punishment_id",SqlDbType.NVarChar,50)
                    };
                        parameter[0].Value = model.title;
                        parameter[1].Value = model.reason;
                        parameter[2].Value = model.approval_authority;
                        parameter[3].Value = model.office_level;
                        parameter[4].Value = model.id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameter);
                        trans.Commit();
                    }
                    catch(Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 修改现工作单位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatecompany(u_company_type model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("update u_company_type set name=@name,employee_count=@employee_count,relation_com=@relation_com,");
                        str.Append("com_type_id=@com_type_id,post_type_id=@post_type_id where id=@now_company_id");
                        SqlParameter[] parameter = {
                        new SqlParameter("@name",SqlDbType.NVarChar,50),
                        new SqlParameter("@employee_count",SqlDbType.Int,4),
                        new SqlParameter("@relation_com",SqlDbType.NVarChar,100),
                        new SqlParameter("@com_type_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@post_type_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@now_company_id",SqlDbType.NVarChar,50)
                    };
                        parameter[0].Value = model.name;
                        parameter[1].Value = model.employee_count;
                        parameter[2].Value = model.relation_com;
                        parameter[3].Value = model.com_type_id;
                        parameter[4].Value = model.post_type_id;
                        parameter[5].Value = model.id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameter);
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
        /// 更改原工作单位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateOrigCompany(u_company_type model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("update u_company_type set name=@name,employee_count=@employee_count,relation_com=@relation_com,");
                        str.Append("com_type_id=@com_type_id,post_type_id=@post_type_id where id=@former_company_id");
                        SqlParameter[] parameter = {
                        new SqlParameter("@name",SqlDbType.NVarChar,50),
                        new SqlParameter("@employee_count",SqlDbType.Int,4),
                        new SqlParameter("@relation_com",SqlDbType.NVarChar,100),
                        new SqlParameter("@com_type_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@post_type_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@former_company_id",SqlDbType.NVarChar,50)
                    };
                        parameter[0].Value = model.name;
                        parameter[1].Value = model.employee_count;
                        parameter[2].Value = model.relation_com;
                        parameter[3].Value = model.com_type_id;
                        parameter[4].Value = model.post_type_id;
                        parameter[5].Value = model.id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameter);
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
        /// 更改享受帮扶详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateEnjoyHelp(u_enjoy_help model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("update u_enjoy_help set start_time=@start_time,end_time=@end_time,help_way_id=@help_way_id,remark=@remark where id=@enjoy_help_id");
                        SqlParameter[] parameter = {
                        new SqlParameter("@start_time",SqlDbType.DateTime),
                        new SqlParameter("@end_time",SqlDbType.DateTime),
                        new SqlParameter("@help_way_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@remark",SqlDbType.NText,500),
                        new SqlParameter("@enjoy_help_id",SqlDbType.NVarChar,50)
                    };
                        parameter[0].Value = model.start_time;
                        parameter[1].Value = model.end_time;
                        parameter[2].Value = model.help_way_id;
                        parameter[3].Value = model.remark;
                        parameter[4].Value = model.id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameter);
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
        /// 修改流动党员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateFloatCommie(u_float_commie model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("update u_float_commie set flow_type=@flow_type,linkman=@linkman,flow_reason=@flow_reason,contact=@contact,");
                        str.Append("id_number=@id_number,group_linkman=@group_linkman,discharge_place=@discharge_place,group_contact=@group_contact where id=@float_commie_id");
                        SqlParameter[] parameter = {
                        new SqlParameter("@flow_type",SqlDbType.NVarChar,50),
                        new SqlParameter("@linkman",SqlDbType.NVarChar,50),
                        new SqlParameter("@flow_reason",SqlDbType.NText,500),
                        new SqlParameter("@contact",SqlDbType.NVarChar,100),
                        new SqlParameter("@id_number",SqlDbType.NVarChar,50),
                        new SqlParameter("@group_linkman",SqlDbType.NVarChar,50),
                        new SqlParameter("@discharge_place",SqlDbType.NVarChar,100),
                        new SqlParameter("@group_contact",SqlDbType.NVarChar,100),
                        new SqlParameter("@float_commie_id",SqlDbType.NVarChar,50)
                    };
                        parameter[0].Value = model.flow_type;
                        parameter[1].Value = model.linkman;
                        parameter[2].Value = model.flow_reason;
                        parameter[3].Value = model.contact;
                        parameter[4].Value = model.id_number;
                        parameter[5].Value = model.group_linkman;
                        parameter[6].Value = model.discharge_place;
                        parameter[7].Value = model.group_contact;
                        parameter[8].Value = model.id;
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameter);
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
        #endregion

        #region 获取分表信息（修改显示）================
        //获取头像信息
        public Model.P_Image GetImageModel(int id)
        {
            StringBuilder str = new StringBuilder();
            str.Append("select P_Image.P_Id,P_Image.P_ImageUrl,P_Image.P_PictureName,P_Image.P_ImageType from P_Image ");
            str.Append("left join dt_users on dt_users.id = P_Image.P_ImageId ");
            str.Append("where P_Image.P_ImageId = '" + id + "' and P_Image.P_Status = 0 and P_Image.P_ImageType = 20011");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return imageDataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        //党员奖惩信息
        public Model.u_reward_punishment getrewardpunishment(int id)
        {
            string rewardid = "";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select reward_punishment_id from dt_users where id = '" + id + "'");
            DataSet info = DbHelperSQL.Query(strSql.ToString());
            if (info.Tables[0].Rows.Count > 0)
            {
                rewardid = info.Tables[0].Rows[0][0].ToString();
            }

            StringBuilder str = new StringBuilder();
            str.Append("select u_reward_punishment.id,u_reward_punishment.title,u_reward_punishment.reason,u_reward_punishment.approval_authority,");
            str.Append("u_reward_punishment.office_level,u_reward_punishment.status from u_reward_punishment ");
            str.Append("left join dt_users on dt_users.reward_punishment_id = u_reward_punishment.id ");
            str.Append("where u_reward_punishment.id = '" + rewardid + "' and u_reward_punishment.status = 0");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return rewardsDataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        //原单位信息
        public Model.u_company_type getFormerCompany(int id)
        {
            string comid = "";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select former_company_id from dt_users where id = '" + id + "'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                comid = ds.Tables[0].Rows[0][0].ToString();
            }

            StringBuilder str = new StringBuilder();
            str.Append("select u_company_type.id,u_company_type.name,u_company_type.employee_count,u_company_type.relation_com,");
            str.Append("u_company_type.com_type_id,u_company_type.post_type_id,u_company_type.com_nature,u_company_type.service_organiz ");
            str.Append("from u_company_type ");
            str.Append("left join dt_users on dt_users.former_company_id = u_company_type.id ");
            str.Append("where u_company_type.id = '" + comid + "' and u_company_type.status = 0  and u_company_type.type = 0");
            DataSet info = DbHelperSQL.Query(str.ToString());
            if (info.Tables[0].Rows.Count > 0)
            {
                return companyDataRowToModel(info.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        //获取现单位信息
        public Model.u_company_type getNowCompany(int id)
        {
            string comid = "";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select now_company_id from dt_users where id = '" + id + "'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                comid = ds.Tables[0].Rows[0][0].ToString();
            }

            StringBuilder str = new StringBuilder();
            str.Append("select u_company_type.id,u_company_type.name,u_company_type.employee_count,u_company_type.relation_com,");
            str.Append("u_company_type.com_type_id,u_company_type.post_type_id,u_company_type.com_nature,u_company_type.service_organiz ");
            str.Append("from u_company_type ");
            str.Append("left join dt_users on dt_users.now_company_id = u_company_type.id ");
            str.Append("where u_company_type.id = '" + comid + "' and u_company_type.status = 0 and u_company_type.type = 1");
            DataSet info = DbHelperSQL.Query(str.ToString());
            if (info.Tables[0].Rows.Count > 0)
            {
                return companyDataRowToModel(info.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        //获取享受帮扶信息
        public Model.u_enjoy_help getEnjoyHelp(int id)
        {
            string enjoyid = "";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select enjoy_help_id from dt_users where id = '" + id + "'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                enjoyid = ds.Tables[0].Rows[0][0].ToString();
            }

            StringBuilder str = new StringBuilder();
            str.Append("select u_enjoy_help.id,u_enjoy_help.start_time,u_enjoy_help.end_time,u_enjoy_help.help_way_id,u_enjoy_help.remark ");
            str.Append("from u_enjoy_help ");
            str.Append("left join dt_users on dt_users.enjoy_help_id = u_enjoy_help.id ");
            str.Append("where u_enjoy_help.id = '" + enjoyid + "' and u_enjoy_help.status = 0");
            DataSet info = DbHelperSQL.Query(str.ToString());
            if (info.Tables[0].Rows.Count > 0)
            {
                return enjoyDataRowToModel(info.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        //获取流动党员信息
        public Model.u_float_commie getFloatCommie(int id)
        {
            string floatid = "";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select float_commie_id from dt_users where id = '" + id + "'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                floatid = ds.Tables[0].Rows[0][0].ToString();
            }

            StringBuilder str = new StringBuilder();
            str.Append("select u_float_commie.id,u_float_commie.flow_type,u_float_commie.linkman,u_float_commie.flow_reason,");
            str.Append("u_float_commie.contact,u_float_commie.id_number,u_float_commie.group_linkman,u_float_commie.discharge_place,u_float_commie.group_contact ");
            str.Append("from u_float_commie ");
            str.Append("left join dt_users on dt_users.float_commie_id = u_float_commie.id ");
            str.Append("where u_float_commie.id = '" + floatid + "' and u_float_commie.status = 0");
            DataSet info = DbHelperSQL.Query(str.ToString());
            if (info.Tables[0].Rows.Count > 0)
            {
                return floatDataRowToModel(info.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
using donet.io.rong.methods;
using donet.io.rong.models;
using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class UserLogic
    {

        /// <summary>
        /// 是否存在账号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool AccountExists(string account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_users");
            strSql.Append(" where mobile='" + account + @"' and status != 1"); // status = 1 表示账号待验证无法登录使用
            return DbHelperSQL.Exists(strSql.ToString());
        }

        private Model.users GetAccountInfo(string account) {

            string sql = String.Format(@"select * from dt_users where dt_users.mobile = '{0}' and dt_users.status != 1", account);

            DataSet ds = DbHelperSQL.Query(sql);

            DataSetToModelHelper<Model.users> helper = new DataSetToModelHelper<Model.users>();

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
            return helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// 获取账号信息数据
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private UserLoginModel GetAccountLoginInfo(string account) {

            UserLoginModel result;

            string sqlStr = String.Format(@"select id as user_id, 
                        user_name, 
                        id_card,status as account_status, 
                        mobile,
                        P_Image.P_ImageUrl as avatar,
                        dt_users.token as im_token
                        from dt_users
                        left join P_Image on P_ImageId = dt_users.id and P_ImageType = {0} 
                        where dt_users.mobile = '{1}'", (int)ImageTypeEnum.头像, account);
            // 账号人员信息
            DataSet ds = DbHelperSQL.Query(sqlStr.ToString());

            DataSetToModelHelper<UserLoginModel> helper = new DataSetToModelHelper<UserLoginModel>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                 result = helper.FillToModel(dr);
                // 获取人员组织信息
                GetAccountGroupInfo(result);
            }
            else {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetAccountGroupInfo(UserLoginModel accountInfo) {

            string groupSql = String.Format(@"select TOP 1 dt_user_groups.id,dt_user_groups.title from F_Split(
                        (select dt_users.group_id from dt_users where dt_users.id = {0}),',') as t
                        left join dt_user_groups on dt_user_groups.id = t.value
                        where t.value != ''
                        order by dt_user_groups.grade DESC", accountInfo.user_id);
            // 用户最小党组织信息
            DataSet ds = DbHelperSQL.Query(groupSql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                accountInfo.gourp_id = ds.Tables[0].Rows[0]["id"].ToString();
                accountInfo.group_name = ds.Tables[0].Rows[0]["title"].ToString();
            }

            string isManagerSql = String.Format(@"select count(1) from dt_users
                        left join dt_user_groups on dt_user_groups.manager_id = dt_users.id
                        where dt_users.id = {0} and dt_user_groups.id is not NULL", accountInfo.user_id);
            // 是否是管理员
            Boolean isManager = DbHelperSQL.Exists(isManagerSql);
            // 判断是否是管理员
            if (isManager)
            {   //  是管理员，判断所属组织个数
                string groupCountSql = String.Format(@"select count(1) from F_Split(
                        (select dt_users.group_id from dt_users where dt_users.id = {0}),',') as t
                        where t.value != ''", accountInfo.user_id);
                // 用户所属组织个数
                int groupCount = Convert.ToInt32(DbHelperSQL.GetSingle(groupCountSql));
                // 判断所属组织个数 大于1 表示用户是书记也是党员,否则为 书记
                if (groupCount > 1)
                {
                    accountInfo.code = 2;  // 书记和党员
                }
                else {
                    accountInfo.code = 1;  // 书记
                }
            }
            else { // 不是书记就是党员
                accountInfo.code = 0;   // 党员
            }

        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账号（电话号）</param>
        /// <param name="password">密码</param>
        /// <param name="clientId">个推clientid</param>
        /// <returns></returns>
        public Tuple<int, UserLoginModel> Login(string account, string password, string  clientId) {

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString)) {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction()) {
                    try
                    {
                        // 获取账号是否存在
                        Model.users info = GetAccountInfo(account);
                        if (info != null)
                        {
                            // 登录密码转码
                            var encryptionPassword = DESEncrypt.Encrypt(password, info.salt);
                            if (info.status == 2) {
                                return Tuple.Create<int, UserLoginModel>((int)LoginReturnEnum.账号正在审核中, null);
                            }
                            if (encryptionPassword.Equals(info.password))
                            {
                                // 获取登录账号信息
                                UserLoginModel userLogin = GetAccountLoginInfo(account);
                                // 修改个推clientid
                                Boolean isUpdate = UpdateGeTuiClientId(account, clientId);
                                if (userLogin == null || !isUpdate)
                                {
                                    return Tuple.Create<int, UserLoginModel>((int)LoginReturnEnum.登录失败, null);
                                }
                                return Tuple.Create<int, UserLoginModel>((int)LoginReturnEnum.登录成功, userLogin);
                            }
                            return Tuple.Create<int, UserLoginModel>((int)LoginReturnEnum.密码不正确, null);

                        }
                        else {
                            return Tuple.Create<int, UserLoginModel>((int)LoginReturnEnum.账号不存在, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return Tuple.Create<int, UserLoginModel>((int)LoginReturnEnum.登录失败, null);
                    }
                }
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="account">账号（电话号）</param>
        /// <returns></returns>
        public Boolean Logout(string account, string clientid) {
            Boolean result = true;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString)) {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction()) {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("update dt_users set dt_users.client_id = '' where dt_users.mobile =@mobile and client_id = @client_id");
                        SqlParameter[] parameter = {
                            new SqlParameter("@mobile", SqlDbType.NVarChar),
                            new SqlParameter("@client_id", SqlDbType.NVarChar)
                        };
                        parameter[0].Value = account;
                        parameter[1].Value = clientid;
                        DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameter);
                        trans.Commit();

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 修改用户clientid
        /// </summary>
        /// <param name="account">账号（电话号）</param>
        /// <param name="clientId">个推clientid</param>
        /// <returns></returns>
        private Boolean UpdateGeTuiClientId(string account, string clientId) {

            Boolean result = true;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder querySql = new StringBuilder();
                        querySql.Append("select * from dt_users where dt_users.mobile = @mobile");
                        SqlParameter[] queryPar = {
                            new SqlParameter("@mobile",SqlDbType.NVarChar,50)
                        };
                        queryPar[0].Value = account;
                        DataSet ds = DbHelperSQL.Query(querySql.ToString(), queryPar);
                        string oldClientId = ds.Tables[0].Rows[0]["client_id"].ToString();
                        int userId = Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString());
                        if (String.IsNullOrEmpty(oldClientId) || !oldClientId.Equals(clientId))
                        {

                            if (!oldClientId.Equals(clientId)) {

                                //Dictionary<string, object> data = new Dictionary<string, object>();
                                //data.Add("msgtype", 1);
                                //data.Add("msg","账号已在其他设备上登录。");
                                //data.Add("msgid","");
                                //data.Add("userid",1);
                                //data.Add("time", DateTime.Now.ToString("yyyy-mm-dd HH:MM:SS"));
                                GeTuiPushModel pushData = new GeTuiPushModel()
                                {
                                    push_type = (int)PushTypeEnum.登出,
                                    push_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    push_title = "您的账号已在其他设备上登录。",
                                    user_id = userId,
                                    message_id = ""
                                };

                                DTcms.Common.PushMessage.PushMessageToSingle(oldClientId, JsonHelper.ObjectToJSON(pushData));
                            }

                            StringBuilder updateSql = new StringBuilder();
                            updateSql.Append("update dt_users set dt_users.client_id = @client_id,dt_users.login_time = @login_time");
                            updateSql.Append(" where dt_users.mobile = @mobile");
                            SqlParameter[] updatePar = {
                                new SqlParameter("@client_id",SqlDbType.NVarChar),
                                new SqlParameter("@login_time", SqlDbType.DateTime),
                                new SqlParameter("@mobile", SqlDbType.NVarChar)
                            };
                            updatePar[0].Value = clientId;
                            updatePar[1].Value = DateTime.Now;
                            updatePar[2].Value = account;
                            DbHelperSQL.ExecuteSql(conn, trans, updateSql.ToString(), updatePar);
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                    
                }
            }

            return result;
        }

        /// <summary>
        /// 账号注册加密接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="salt"></param>
        /// <param name="password1"></param>
        /// <returns></returns>
        public UserLoginModel Register(string account, string password, string card, int groupId, string clientId)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (IsRegister(account, card, groupId))
                        {
                            //获取账户信息
                            UserLoginModel userInfo = GetAccountLoginInfo(account);

                            //融云获取Token
                            string token = "";
                            string RongAppKey = ConfigHelper.GetAppSettings("RongAppKey");
                            string RongAppSecret = ConfigHelper.GetAppSettings("RongAppSecret");
                            User user = new User(RongAppKey, RongAppSecret);
                            if (!string.IsNullOrEmpty(userInfo.user_id.ToString()))
                            {
                                TokenReslut tr = user.getToken(userInfo.user_id.ToString(), userInfo.user_name, userInfo.avatar);
                                token = tr.getToken();
                            }

                            //将Token插入数据表
                            string salt = Utils.GetCheckCode(6);
                            string encryptionPsw = DESEncrypt.Encrypt(password, salt);
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("update dt_users set ");
                            strSql.Append("salt=@salt,");
                            strSql.Append("password=@password,");
                            strSql.Append("status=@status,");
                            strSql.Append("client_id=@client_id,");
                            strSql.Append("login_time=@login_time,");
                            strSql.Append("token=@token");
                            strSql.Append(" where mobile=@mobile");
                            SqlParameter[] parameters = {
                                new SqlParameter("@salt", SqlDbType.NVarChar,100),
                                new SqlParameter("@password", SqlDbType.NVarChar,100),
                                new SqlParameter("@status", SqlDbType.Int,50),
                                new SqlParameter("@client_id", SqlDbType.NVarChar,100),
                                new SqlParameter("@login_time", SqlDbType.DateTime,100),
                                new SqlParameter("@token", SqlDbType.VarChar,300),
                                new SqlParameter("@mobile", SqlDbType.NVarChar,100)};
                            parameters[0].Value = salt;
                            parameters[1].Value = encryptionPsw;
                            parameters[2].Value = 2;    // 0 正常 1 待验证 2 待审核
                            parameters[3].Value = clientId;
                            parameters[4].Value = DateTime.Now;
                            parameters[5].Value = token;
                            parameters[6].Value = account;
                            DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                            trans.Commit();
                            return GetAccountLoginInfo(account);
                        }
                        else
                        {
                            return null ;
                        }

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
		/// 判断是否可以注册账号
		/// </summary>
		public bool IsRegister(string mobile, string card, int groupId)
        {

            string sql = String.Format(@"select count(1) from dt_users 
                        where mobile = '{0}' and group_id like '%,{1},%' and id_card = '{2}'",mobile, groupId, card);
          
            return DbHelperSQL.Exists(sql);
        }

        /// <summary>
        /// 获取党组织列表
        /// </summary>
        /// <returns></returns>
        public List<PartyOrganizationModel> GetPartyOrganizationList(string mobile) {

            StringBuilder sql = new StringBuilder();
            sql.Append("select convert(int,t.value) as gourp_id,dt_user_groups.title as gourp_name from F_Split");
            sql.Append("((select dt_users.group_id from dt_users where dt_users.mobile = @mobile and dt_users.status = 1),',') as t");
            sql.Append(" left join dt_user_groups on dt_user_groups.id = t.value");
            sql.Append(" where t.value != ''");

            SqlParameter[] parameter = {
                new SqlParameter("@mobile",SqlDbType.NVarChar)
            };
            parameter[0].Value = mobile;
            DataSet ds = DbHelperSQL.Query(sql.ToString(), parameter);

            DataSetToModelHelper<PartyOrganizationModel> helper = new DataSetToModelHelper<PartyOrganizationModel>();

            List<PartyOrganizationModel> result = helper.FillModel(ds);

            return result;
        }

        public int GetAccountStatus(string account) {

            StringBuilder sql = new StringBuilder();

            sql.Append("select dt_users.status from dt_users where dt_users.mobile = @mobile");

            SqlParameter[] parameter = {
                new SqlParameter("@mobile",SqlDbType.NVarChar, 100)
            };
            parameter[0].Value = account;

            string status = Convert.ToString(DbHelperSQL.GetSingle(sql.ToString(), parameter));

            if (!String.IsNullOrEmpty(status))
            {
                return Convert.ToInt32(status);
            }


            return -1;
        }

        public TokenResult getToken(string id)
        {
            TokenResult result;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT dt_users.user_name AS name," +
                                            " ISNULL(dt_users.token,'') AS token ," +
                                            " ISNULL(P_Image.P_ImageUrl,'') AS portraitUri " +
                                            " FROM dt_users " +
                                            " LEFT JOIN P_Image ON convert(varchar,dt_users.id)=P_Image.P_ImageId" + " AND P_Image.P_ImageType=" + (int)ImageTypeEnum.头像 + " AND P_Image.P_Status=0"+
                                            " WHERE dt_users.id=@id" );

            SqlParameter[] parameter = {
                new SqlParameter("@id",SqlDbType.Int)
            };

            parameter[0].Value = Convert.ToInt32(id);

            DataSet ds = DbHelperSQL.Query(sql.ToString(), parameter);

            DataSetToModelHelper<TokenResult> helper = new DataSetToModelHelper<TokenResult>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                result = helper.FillToModel(dr);
            }
            else
            {
                result = null;
            }

            return result;
        }
    }
}

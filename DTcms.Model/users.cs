using System;
namespace DTcms.Model
{
    /// <summary>
    /// 会员主表
    /// </summary>
    [Serializable]
    public partial class users
    {
        public users()
        { }
        #region Model
        private int _id;
        private string _group_id;
        private string _user_name;
        private string _salt;
        private string _password;
        private string _mobile = string.Empty;
        private string _email = string.Empty;
        private string _avatar = string.Empty;
        private string _nick_name = string.Empty;
        private string _sex = string.Empty;
        private DateTime? _birthday;
        private string _telphone;
        private string _area = string.Empty;
        private string _address = string.Empty;
        private string _qq = string.Empty;
        private string _msn = string.Empty;
        private decimal _amount = 0M;
        private int _point = 0;
        private int _exp = 0;
        private int _status = 0;
        private DateTime _reg_time = DateTime.Now;
        private string _reg_ip;
        private string _client_id;
        private DateTime? _login_time;
        private int? _role_id;
        private string _id_card;
        private string _nation;
        private int? _marital_status;
        private string _new_class_id;
        private string _administration_rank_id;
        private string _military_rank_id;
        private string _police_rank_id;
        private int? _children_info;
        private int? _only_child_award;
        private string _passport_info_id;
        private string _entitled_group_id;
        private string _income_source_id;
        private string _native_place;
        private string _live_place;
        private string _graduate_school;
        private DateTime? _graduate_time;
        private string _education_info_id;
        private int? _degree_info;
        private DateTime? _join_party_time;
        private string _party_membership;
        private string _first_branch;
        private string _party_job;
        private string _now_organiz;
        private string _group_type_id;
        private string _community_info;
        private string _group_live;
        private string _reward_punishment_id;
        private string _former_company_id;
        private string _now_company_id;
        private int? _is_badly_off;
        private int? _is_organiz_identity;
        private string _financial_situation_id;
        private string _healthy_condition_id;
        private string _badly_off_reason_id;
        private string _badly_off_describe;
        private string _enjoy_help_id;
        private string _float_commie_id;
        private string _wechat;
        private int? _float_commie;
        private string _com_name;
        private string _com_type_id;
        private string _token;



        /// <summary>
        /// 自增ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户组ID
        /// </summary>
        //public int group_id
        //{
        //    set { _group_id = value; }
        //    get { return _group_id; }
        //}
        public string group_id {
            set { _group_id = value; }
            get { return _group_id; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name
        {
            set { _user_name = value; }
            get { return _user_name; }
        }
        /// <summary>
        /// 6位随机字符串,加密用到
        /// </summary>
        public string salt
        {
            set { _salt = value; }
            get { return _salt; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string avatar
        {
            set { _avatar = value; }
            get { return _avatar; }
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string nick_name
        {
            set { _nick_name = value; }
            get { return _nick_name; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? birthday
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string telphone
        {
            set { _telphone = value; }
            get { return _telphone; }
        }
        /// <summary>
        /// 所属地区
        /// </summary>
        public string area
        {
            set { _area = value; }
            get { return _area; }
        }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// QQ号码
        /// </summary>
        public string qq
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// MSN账号
        /// </summary>
        public string msn
        {
            set { _msn = value; }
            get { return _msn; }
        }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 积分
        /// </summary>
        public int point
        {
            set { _point = value; }
            get { return _point; }
        }
        /// <summary>
        /// 经验值
        /// </summary>
        public int exp
        {
            set { _exp = value; }
            get { return _exp; }
        }
        /// <summary>
        /// 账户状态,0正常,1待验证,2待审核,3锁定
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime reg_time
        {
            set { _reg_time = value; }
            get { return _reg_time; }
        }
        /// <summary>
        /// 注册IP
        /// </summary>
        public string reg_ip
        {
            set { _reg_ip = value; }
            get { return _reg_ip; }
        }

        public string client_id { get => _client_id; set => _client_id = value; }
        public DateTime? login_time { get => _login_time; set => _login_time = value; }
        public int? role_id { get => _role_id; set => _role_id = value; }
        public string id_card { get => _id_card; set => _id_card = value; }
        public string nation { get => _nation; set => _nation = value; }
        public int? marital_status { get => _marital_status; set => _marital_status = value; }
        public string new_class_id { get => _new_class_id; set => _new_class_id = value; }
        public string administration_rank_id { get => _administration_rank_id; set => _administration_rank_id = value; }
        public string military_rank_id { get => _military_rank_id; set => _military_rank_id = value; }
        public string police_rank_id { get => _police_rank_id; set => _police_rank_id = value; }
        public int? children_info { get => _children_info; set => _children_info = value; }
        public int? only_child_award { get => _only_child_award; set => _only_child_award = value; }
        public string passport_info_id { get => _passport_info_id; set => _passport_info_id = value; }
        public string entitled_group_id { get => _entitled_group_id; set => _entitled_group_id = value; }
        public string income_source_id { get => _income_source_id; set => _income_source_id = value; }
        public string native_place { get => _native_place; set => _native_place = value; }
        public string live_place { get => _live_place; set => _live_place = value; }
        public string graduate_school { get => _graduate_school; set => _graduate_school = value; }
        public DateTime? graduate_time { get => _graduate_time; set => _graduate_time = value; }
        public string education_info_id { get => _education_info_id; set => _education_info_id = value; }
        public int? degree_info { get => _degree_info; set => _degree_info = value; }
        public DateTime? join_party_time { get => _join_party_time; set => _join_party_time = value; }
        public string party_membership { get => _party_membership; set => _party_membership = value; }
        public string first_branch { get => _first_branch; set => _first_branch = value; }
        public string party_job { get => _party_job; set => _party_job = value; }
        public string now_organiz { get => _now_organiz; set => _now_organiz = value; }
        public string group_type_id { get => _group_type_id; set => _group_type_id = value; }
        public string community_info { get => _community_info; set => _community_info = value; }
        public string group_live { get => _group_live; set => _group_live = value; }
        public string reward_punishment_id { get => _reward_punishment_id; set => _reward_punishment_id = value; }
        public string former_company_id { get => _former_company_id; set => _former_company_id = value; }
        public string now_company_id { get => _now_company_id; set => _now_company_id = value; }
        public int? is_badly_off { get => _is_badly_off; set => _is_badly_off = value; }
        public int? is_organiz_identity { get => _is_organiz_identity; set => _is_organiz_identity = value; }
        public string financial_situation_id { get => _financial_situation_id; set => _financial_situation_id = value; }
        public string healthy_condition_id { get => _healthy_condition_id; set => _healthy_condition_id = value; }
        public string badly_off_reason_id { get => _badly_off_reason_id; set => _badly_off_reason_id = value; }
        public string badly_off_describe { get => _badly_off_describe; set => _badly_off_describe = value; }
        public string enjoy_help_id { get => _enjoy_help_id; set => _enjoy_help_id = value; }
        public string float_commie_id { get => _float_commie_id; set => _float_commie_id = value; }
        public string wechat { get => _wechat; set => _wechat = value; }
        public int? float_commie { get => _float_commie; set => _float_commie = value; }
        public string com_name { get => _com_name; set => _com_name = value; }
        public string com_type_id { get => _com_type_id; set => _com_type_id = value; }
        public string token { get => _token; set => _token = value; }





        #endregion

    }
}
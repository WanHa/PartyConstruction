using System;
namespace DTcms.Model
{
    /// <summary>
    /// 会员组别
    /// </summary>
    [Serializable]
    public partial class user_groups
    {
        public user_groups()
        { }
        #region Model
        private int _id;
        private string _title = "";
        private int _grade = 0;
        private int _upgrade_exp = 0;
        private decimal _amount = 0M;
        private int _point = 0;
        private int _discount = 100;
        private int _is_default = 0;
        private int _is_upgrade = 1;
        private int _is_lock = 0;
        private int? _pid;
        private string _manager;
        private string _position;
        private int _manager_id;
        private int? _status;
        private string _location;
        private string _org_code;
        private DateTime? _create_time;
        private int? _sort;
        private string _contact_address;
        private string _superior_org;
        private string _sub_org_id;
        private string _intre;
        private string _phone_fax;
        private string _territory_relation;
        private DateTime? _elected_date;
        private DateTime? _expiration_date;
        private string _lead_info_id;
        private string _rewards_punishment_id;
        private string _company_info_id;
        private string _phone;
        private string _secretary_name;
        private string _contact_person;
        private string _contact_person_tel;
        private int _official_male_count;
        private int _official_female_count;
        private int _ready_male_count;
        private int _ready_female_count;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 组别名称
        /// </summary>
        public string title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 会员等级值
        /// </summary>
        public int grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// 升级经验值
        /// </summary>
        public int upgrade_exp
        {
            set { _upgrade_exp = value; }
            get { return _upgrade_exp; }
        }
        /// <summary>
        /// 默认预存款
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 默认积分
        /// </summary>
        public int point
        {
            set { _point = value; }
            get { return _point; }
        }
        /// <summary>
        /// 购物折扣
        /// </summary>
        public int discount
        {
            set { _discount = value; }
            get { return _discount; }
        }
        /// <summary>
        /// 是否注册用户组
        /// </summary>
        public int is_default
        {
            set { _is_default = value; }
            get { return _is_default; }
        }
        /// <summary>
        /// 是否自动升级
        /// </summary>
        public int is_upgrade
        {
            set { _is_upgrade = value; }
            get { return _is_upgrade; }
        }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public int is_lock
        {
            set { _is_lock = value; }
            get { return _is_lock; }
        }
        /// <summary>
        /// 父级
        /// </summary>
        public int? pid
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 党委书记
        /// </summary>
        public string manager
        {
            set { _manager = value; }
            get { return _manager; }
        }
        /// <summary>
        /// 位置
        /// </summary>
        public string position
        {
            set { _position = value; }
            get { return _position; }
        }

        public int Manager_id { get => _manager_id; set => _manager_id = value; }
        public int? status { get => _status; set => _status = value; }
        public string location { get => _location; set => _location = value; }
        public string org_code { get => _org_code; set => _org_code = value; }
        public DateTime? create_time { get => _create_time; set => _create_time = value; }
        public int? sort { get => _sort; set => _sort = value; }
        public string contact_address { get => _contact_address; set => _contact_address = value; }
        public string superior_org { get => _superior_org; set => _superior_org = value; }
        public string sub_org_id { get => _sub_org_id; set => _sub_org_id = value; }
        public string intre { get => _intre; set => _intre = value; }
        public string phone_fax { get => _phone_fax; set => _phone_fax = value; }
        public string territory_relation { get => _territory_relation; set => _territory_relation = value; }
        public DateTime? elected_date { get => _elected_date; set => _elected_date = value; }
        public DateTime? expiration_date { get => _expiration_date; set => _expiration_date = value; }
        public string lead_info_id { get => _lead_info_id; set => _lead_info_id = value; }
        public string rewards_punishment_id { get => _rewards_punishment_id; set => _rewards_punishment_id = value; }
        public string company_info_id { get => _company_info_id; set => _company_info_id = value; }
        public string phone { get => _phone; set => _phone = value; }
        public string secretary_name { get => _secretary_name; set => _secretary_name = value; }
        public string contact_person { get => _contact_person; set => _contact_person = value; }
        public string contact_person_tel { get => _contact_person_tel; set => _contact_person_tel = value; }
        public int official_male_count { get => _official_male_count; set => _official_male_count = value; }
        public int official_female_count { get => _official_female_count; set => _official_female_count = value; }
        public int ready_male_count { get => _ready_male_count; set => _ready_male_count = value; }
        public int ready_female_count { get => _ready_female_count; set => _ready_female_count = value; }
        public string P_CompanyName { get; set; }
        public int? P_PeopleCount { get; set; }
        public string P_CompanyTypeid { get; set; }
        public string P_CreateVoluntaryTeam { get; set; }

        //领导班子信息
        public int userid { get; set; }
        public string name { get; set; }
        public string job { get; set; }
        public string contact_way { get; set; }
        public string remark { get; set; }

        #endregion Model
    }
}
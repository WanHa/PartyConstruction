using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.homepage
{
    public class MapGroupInfoModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public int group_id { get; set; }

        private string _group_name = "无";
        /// <summary>
        /// 组织名称
        /// </summary>
        public string group_name {
            get { return _group_name; }
            set { _group_name = value; }
        }

        private string _contact_address = "无";
        /// <summary>
        /// 联系地址
        /// </summary>
        public string contact_address {
            get { return _contact_address; }
            set { _contact_address = value; }
        }

        private string _secretary = "无";
        /// <summary>
        /// 组织书记
        /// </summary>
        public string secretary {
            get { return _secretary; }
            set { _secretary = value; }
        }

        /// <summary>
        /// 人数
        /// </summary>
        public int personnel_count { get; set; }

        private List<GroupUserModel> _group_users = new List<GroupUserModel>();
        /// <summary>
        /// 组织下的人员信息列表
        /// </summary>
        public List<GroupUserModel> group_users {
            get { return _group_users; }
            set { _group_users = value; }
        }
    }

    /// <summary>
    /// 组织人员信息
    /// </summary>
    public class GroupUserModel {
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }
        
        /// <summary>
        /// 用户名称
        /// </summary>
        public string user_name { get; set; }
    }
}

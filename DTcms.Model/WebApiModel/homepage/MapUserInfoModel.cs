using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.homepage
{
    public class MapUserInfoModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string user_name { get; set; }

        private string _group_name;
        /// <summary>
        /// 组织名称
        /// </summary>
        public string group_name {
            get { return String.IsNullOrEmpty(_group_name) ? "无" : _group_name; }
            set { _group_name = value; }
        }

        private string _secretary;
        /// <summary>
        /// 书记
        /// </summary>
        public string secretary {
            get { return String.IsNullOrEmpty(_secretary) ? "无" : _secretary; }
            set { _secretary = value; }
        }

        private string _superior_organization;
        /// <summary>
        /// 上级组织
        /// </summary>
        public string superior_organization {
            get { return String.IsNullOrEmpty(_superior_organization) ? "无" : _superior_organization; }
            set { _superior_organization = value; }
        }

        /// <summary>
        /// 活动信息列表
        /// </summary>
        public List<MapActivityModel> activitys { get; set; }

    }
}

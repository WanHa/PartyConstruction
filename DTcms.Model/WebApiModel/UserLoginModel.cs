using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class UserLoginModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string id_card { get; set; }

        private int _account_status;

        /// <summary>
        /// 账号状态 1-->可用,0--不可用(数据库中0-->可用, 1-->待注册,2-->待审核)
        /// </summary>
        public int account_status {
            get { return _account_status == 0 ? 1 : 0; }
            set { _account_status = value; }
        }

        /// <summary>
        /// 党组织ID
        /// </summary>
        public string gourp_id { get; set; }

        /// <summary>
        /// 党组织名称
        /// </summary>
        public string group_name { get; set; }

        /// <summary>
        /// 用户身份 0-->党员 1--> 书记 2--> 党员和书记
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string mobile { get; set; }

        private string _avatar;
        /// <summary>
        /// 头像
        /// </summary>
        public string avatar {
            get { return _avatar == null ? "" : _avatar; }
            set { _avatar = value; }
        }

        /// <summary>
        /// 融云IM token
        /// </summary>
        public string im_token { get; set; }
    }

}

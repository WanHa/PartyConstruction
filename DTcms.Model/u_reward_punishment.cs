using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class u_reward_punishment
    {
        public u_reward_punishment()
        { }
        private string _id;
        private string _title;
        private string _reason;
        private string _approval_authority;
        private string _office_level;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private int _status;

        public string id { get => _id; set => _id = value; }
        public string title { get => _title; set => _title = value; }
        public string reason { get => _reason; set => _reason = value; }
        public string approval_authority { get => _approval_authority; set => _approval_authority = value; }
        public string office_level { get => _office_level; set => _office_level = value; }
        public DateTime? create_time { get => _create_time; set => _create_time = value; }
        public string create_user { get => _create_user; set => _create_user = value; }
        public DateTime? update_time { get => _update_time; set => _update_time = value; }
        public string update_user { get => _update_user; set => _update_user = value; }
        public int status { get => _status; set => _status = value; }
    }
}

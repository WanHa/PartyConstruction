using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class u_lead_info
    {
        public u_lead_info()
        { }
        private string _id;
        private string _group_id;
        private string _name;
        private string _job;
        private string _contact_way;
        private string _remark;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private int _status;

        public string id { get => _id; set => _id = value; }
        public string group_id { get => _group_id; set => _group_id = value; }
        public string name { get => _name; set => _name = value; }
        public string job { get => _job; set => _job = value; }
        public string contact_way { get => _contact_way; set => _contact_way = value; }
        public string remark { get => _remark; set => _remark = value; }
        public DateTime? create_time { get => _create_time; set => _create_time = value; }
        public string create_user { get => _create_user; set => _create_user = value; }
        public DateTime? update_time { get => _update_time; set => _update_time = value; }
        public string update_user { get => _update_user; set => _update_user = value; }
        public int status { get => _status; set => _status = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class u_enjoy_help
    {
        public u_enjoy_help()
        { }
        private string _id;
        private DateTime? _start_time;
        private DateTime? _end_time;
        private string _help_way_id;
        private string _remark;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private int _status;

        public string id { get => _id; set => _id = value; }
        public DateTime? start_time { get => _start_time; set => _start_time = value; }
        public DateTime? end_time { get => _end_time; set => _end_time = value; }
        public string help_way_id { get => _help_way_id; set => _help_way_id = value; }
        public string remark { get => _remark; set => _remark = value; }
        public DateTime? create_time { get => _create_time; set => _create_time = value; }
        public string create_user { get => _create_user; set => _create_user = value; }
        public DateTime? update_time { get => _update_time; set => _update_time = value; }
        public string update_user { get => _update_user; set => _update_user = value; }
        public int status { get => _status; set => _status = value; }
    }
}

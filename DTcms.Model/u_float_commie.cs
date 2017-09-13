using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class u_float_commie
    {
        public u_float_commie()
        { }
        private string _id;
        private string _flow_type;
        private string _linkman;
        private string _flow_reason;
        private string _contact;
        private string _id_number;
        private string _group_linkman;
        private string _discharge_place;
        private string _group_contact;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private int _status;

        public string id { get => _id; set => _id = value; }
        public string flow_type { get => _flow_type; set => _flow_type = value; }
        public string linkman { get => _linkman; set => _linkman = value; }
        public string flow_reason { get => _flow_reason; set => _flow_reason = value; }
        public string contact { get => _contact; set => _contact = value; }
        public string id_number { get => _id_number; set => _id_number = value; }
        public string group_linkman { get => _group_linkman; set => _group_linkman = value; }
        public string discharge_place { get => _discharge_place; set => _discharge_place = value; }
        public string group_contact { get => _group_contact; set => _group_contact = value; }
        public DateTime? create_time { get => _create_time; set => _create_time = value; }
        public string create_user { get => _create_user; set => _create_user = value; }
        public DateTime? update_time { get => _update_time; set => _update_time = value; }
        public string update_user { get => _update_user; set => _update_user = value; }
        public int status { get => _status; set => _status = value; }
    }
}

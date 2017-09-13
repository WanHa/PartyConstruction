using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class u_new_class
    {
        public u_new_class()
        { }
        private string _id;
        private string _type;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private int? _status;

        public string id { get => _id; set => _id = value; }
        public DateTime? create_time { get => _create_time; set => _create_time = value; }
        public string create_user { get => _create_user; set => _create_user = value; }
        public DateTime? update_time { get => _update_time; set => _update_time = value; }
        public string update_user { get => _update_user; set => _update_user = value; }
        public int? status { get => _status; set => _status = value; }
        public string type { get => _type; set => _type = value; }
    }
}

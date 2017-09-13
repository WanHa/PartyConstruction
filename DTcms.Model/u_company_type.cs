using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class u_company_type
    {
        public u_company_type()
        { }
        private string _id;
        private string _name;
        private int? _employee_count;
        private string _relation_com;
        private string _com_type_id;
        private string _post_type_id;
        private DateTime? _create_time;
        private string _create_user;
        private DateTime? _update_time;
        private string _update_user;
        private int _status;
        private string _com_nature;
        private int _service_organiz;
        private int _type;

        public string id { get => _id; set => _id = value; }
        public string name { get => _name; set => _name = value; }
        public int? employee_count { get => _employee_count; set => _employee_count = value; }
        public string relation_com { get => _relation_com; set => _relation_com = value; }
        public string com_type_id { get => _com_type_id; set => _com_type_id = value; }
        public string post_type_id { get => _post_type_id; set => _post_type_id = value; }
        public DateTime? create_time { get => _create_time; set => _create_time = value; }
        public string create_user { get => _create_user; set => _create_user = value; }
        public DateTime? update_time { get => _update_time; set => _update_time = value; }
        public string update_user { get => _update_user; set => _update_user = value; }
        public int status { get => _status; set => _status = value; }
        public string com_nature { get => _com_nature; set => _com_nature = value; }
        public int service_organiz { get => _service_organiz; set => _service_organiz = value; }
        public int type { get => _type; set => _type = value; }
    }
}

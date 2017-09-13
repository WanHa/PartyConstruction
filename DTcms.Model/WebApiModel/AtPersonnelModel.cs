using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class AtPersonnelModel
    {
        public int user_id { get; set; }

        public string user_name { get; set; }

        private string _avatar;
        public string avatar {
            get { return _avatar == null ? "" : _avatar; }
            set { _avatar = value; }
        }
    }
}

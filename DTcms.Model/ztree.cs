using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class ztree
    {
        public string id { get; set; }

        public int channel_id { get; set; }

        public int category_id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public string img_url { get; set; }

        public string createuser { get; set; }

        public List<group> groupid { get; set; }

        public List<string> userid { get; set; }
    }

    public class group
    {
        public string gid { get; set; }
    }

    public class user
    {
        public string uid { get; set; }
    }
}

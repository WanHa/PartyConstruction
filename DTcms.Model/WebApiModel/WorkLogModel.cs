using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class WorkLogModel
    {
        public string title { get; set; }
        public string typeid { get; set; }
        public string content { get; set; }
        public int userid { get; set; }
        public List<Imageurl> imageurl { get; set; }

    }
    public class Imageurl
    {
        public string picname { get; set; }
    }
}

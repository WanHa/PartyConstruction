using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model.WebApiModel
{
    public class DetailPaperModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string createtime { get; set; }
        public int collect { get; set; }
        public int collectcount { get; set; }
        public int trankcount { get; set; }
        public string username { get; set; }
        public string groupname { get; set; }
        public string imgurl { get; set; }
    }
}

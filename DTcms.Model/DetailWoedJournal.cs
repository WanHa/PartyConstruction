using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class DetailWoedJournal
    {
        public string title { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public string username { get; set; }
        public string senduser { get; set; }
        public DateTime createtime { get; set; }
        public string replycontent { get; set; }
    }
}

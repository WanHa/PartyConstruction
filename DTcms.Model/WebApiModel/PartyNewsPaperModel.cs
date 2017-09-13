using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTcms.Model.WebApiModel
{
    public class PartyNewsPaperModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime createtime { get; set; }
    }
}
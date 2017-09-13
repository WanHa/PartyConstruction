using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class PartyCommentModel
    {
        public int userid { get; set; }
        public string username { get; set; }
        public int articleid { get; set; }
        public string content { get; set; }
    }
}

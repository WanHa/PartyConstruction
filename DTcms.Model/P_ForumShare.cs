using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_ForumShare
    {
        public P_ForumShare()
        { }
        private string _P_Id;
        private string _P_GroupForumId;
        private int _P_UserId;
        private string _P_Content;
        private DateTime? _P_CreateTime;
        private int? _P_Status;
        private int? _P_Type;
        private int? _P_Source;
        private string _P_SourceId;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_GroupForumId { get => _P_GroupForumId; set => _P_GroupForumId = value; }
        public int P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public int? P_Status { get => _P_Status; set => _P_Status = value; }
        public int? P_Type { get => _P_Type; set => _P_Type = value; }
        public int? P_Source { get => _P_Source; set => _P_Source = value; }
        public string P_SourceId { get => _P_SourceId; set => _P_SourceId = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_BranchPublish
    {
        private string _P_Id;
        private string _P_BranchId;
        private string _P_UserId;
        private string _P_Content;
        private DateTime? _P_CreateTime;
        private int _P_Status;
        private int? _P_Type;
        private string _P_SourceId;
        private int? _P_Source;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_BranchId { get => _P_BranchId; set => _P_BranchId = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int? P_Type { get => _P_Type; set => _P_Type = value; }
        public string P_SourceId { get => _P_SourceId; set => _P_SourceId = value; }
        public int? P_Source { get => _P_Source; set => _P_Source = value; }
    }
}

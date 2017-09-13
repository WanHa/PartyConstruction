using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_PartyCloudShare
    {
        public P_PartyCloudShare()
        { }
        private string _P_Id;
        private string _P_PartyCloudId;
        private string _P_CreaterId;
        private DateTime? _P_CreateTime;
        private int? _P_Type;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_PartyCloudId { get => _P_PartyCloudId; set => _P_PartyCloudId = value; }
        public string P_CreaterId { get => _P_CreaterId; set => _P_CreaterId = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public int? P_Type { get => _P_Type; set => _P_Type = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_PersonGroupRelation
    {
        public P_PersonGroupRelation()
        { }
        private string _P_Id;
        private string _P_PartyGroupId;
        private string _P_UserId;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int _P_Type;
        private int _P_Approval;
        private string _P_ApplyExplain;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_PartyGroupId { get => _P_PartyGroupId; set => _P_PartyGroupId = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int P_Type { get => _P_Type; set => _P_Type = value; }
        public int P_Approval { get => _P_Approval; set => _P_Approval = value; }
        public string P_ApplyExplain { get => _P_ApplyExplain; set => _P_ApplyExplain = value; }
    }
}

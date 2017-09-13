using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_PartyCommitteeNotify
    {
        public P_PartyCommitteeNotify()
        { }
        private string _P_Id;
        private string _P_Relation;
        private string _P_GroupId;
        private string _P_ToUser;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_Relation { get => _P_Relation; set => _P_Relation = value; }
        public string P_GroupId { get => _P_GroupId; set => _P_GroupId = value; }
        public string P_ToUser { get => _P_ToUser; set => _P_ToUser = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

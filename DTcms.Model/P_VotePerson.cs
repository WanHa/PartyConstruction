using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_VotePerson
    {
        public P_VotePerson()
        { }
        private string _P_Id;
        private string _P_ActivityId;
        private string _P_ByVoteUserId;
        private string _P_VoteUserID;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int _P_NameStatus;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_ActivityId { get => _P_ActivityId; set => _P_ActivityId = value; }
        public string P_ByVoteUserId { get => _P_ByVoteUserId; set => _P_ByVoteUserId = value; }
        public string P_VoteUserID { get => _P_VoteUserID; set => _P_VoteUserID = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int P_NameStatus { get => _P_NameStatus; set => _P_NameStatus = value; }
    }
}

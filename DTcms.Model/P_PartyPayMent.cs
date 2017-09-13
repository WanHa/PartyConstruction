using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class P_PartyPayMent
    {
        public P_PartyPayMent() { }
        #region Model
        private string _P_Id;
        private string _P_Title;
        private int? _P_PayMentState;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int? _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_Title { get => _P_Title; set => _P_Title = value; }
        public int? P_PayMentState { get => _P_PayMentState; set => _P_PayMentState = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int? P_Status { get => _P_Status; set => _P_Status = value; }
        #endregion
    }
}

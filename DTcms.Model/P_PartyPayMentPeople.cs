using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class P_PartyPayMentPeople
    {
        public P_PartyPayMentPeople()
        { }
        #region Model
        private string _P_ID;
        private string _P_UserID;
        private string _P_Tel;
        private decimal? _P_Money;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private string _P_PayMentId;
        private string _PayStatus;
        private int? paysta;

        public string P_ID { get => _P_ID; set => _P_ID = value; }
        public string P_UserID { get => _P_UserID; set => _P_UserID = value; }
        public string P_Tel { get => _P_Tel; set => _P_Tel = value; }
        public decimal? P_Money { get => _P_Money; set => _P_Money = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string P_PayMentId { get => _P_PayMentId; set => _P_PayMentId = value; }
        public string PayStatus { get => _PayStatus; set => _PayStatus = value; }
        public int? Paysta { get => paysta; set => paysta = value; }

        public int? a { get; set; }
        #endregion Model
    }
}

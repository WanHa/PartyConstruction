using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_PartyPayMentRecord
    {
        public P_PartyPayMentRecord()
        { }
        #region model
        private string _P_Id;
        private string _P_OutTradeNo;
        private string _P_Content;
        private decimal? _P_PaySum;
        private DateTime? _P_PayTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int? _P_PayType;
        private string _P_TradeNo;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_OutTradeNo { get => _P_OutTradeNo; set => _P_OutTradeNo = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public decimal? P_PaySum { get => _P_PaySum; set => _P_PaySum = value; }
        public DateTime? P_PayTime { get => _P_PayTime; set => _P_PayTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int? P_PayType { get => _P_PayType; set => _P_PayType = value; }
        public string P_TradeNo { get => _P_TradeNo; set => _P_TradeNo = value; }
        #endregion
    }
}

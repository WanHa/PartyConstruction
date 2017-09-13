using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_UserDemand
    {
        public P_UserDemand()
        {  }
        private string _P_Id;
        private string _P_Content;
        private int? _P_CheckStatus;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private string _P_ImageId;
        private int _P_PublicityStatus;
        private int _P_Urgent;
        private int _Sum;
        private string _StatusName;
        private string _Username;
        private string _ReplyStatus;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public int? P_CheckStatus { get => _P_CheckStatus; set => _P_CheckStatus = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string P_ImageId { get => _P_ImageId; set => _P_ImageId = value; }
        public int P_PublicityStatus { get => _P_PublicityStatus; set => _P_PublicityStatus = value; }
        public int P_Urgent { get => _P_Urgent; set => _P_Urgent = value; }
        public int Sum { get => _Sum; set => _Sum = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string Username { get => _Username; set => _Username = value; }
        public string ReplyStatus { get => _ReplyStatus; set => _ReplyStatus = value; }
    }
}

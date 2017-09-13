using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_ReviewActivity
    {
        public P_ReviewActivity()
        { }
        private string _P_Id;
        private string _P_Title;
        private string _P_Content;
        private int? _P_RAStatus;
        private string _P_UserId;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private DateTime? _P_EndTime;
        private int _P_VoteResult;
        private int _P_OptionType;
        private string _AsStatus; //扩展字段


        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_Title { get => _P_Title; set => _P_Title = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public int? P_RAStatus { get => _P_RAStatus; set => _P_RAStatus = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public DateTime? P_EndTime { get => _P_EndTime; set => _P_EndTime = value; }
        public string AsStatus { get => _AsStatus; set => _AsStatus = value; }
        public int P_VoteResult { get => _P_VoteResult; set => _P_VoteResult = value; }
        public int P_OptionType { get => _P_OptionType; set => _P_OptionType = value; }
    }
}

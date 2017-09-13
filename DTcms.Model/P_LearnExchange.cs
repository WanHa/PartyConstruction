using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_LearnExchange
    {
        public P_LearnExchange()
        { }
        private string _P_Id;
        private string _P_UserId;
        private string _P_Title;
        private string _P_Content;
        private string _P_ImageId;
        private int? _P_AuditState;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
		private int _P_Status;
        private string name;
        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public string P_Title { get => _P_Title; set => _P_Title = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public string P_ImageId { get => _P_ImageId; set => _P_ImageId = value; }
        public int? P_AuditState { get => _P_AuditState; set => _P_AuditState = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
		public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string Name { get => name; set => name = value; }
    }
}

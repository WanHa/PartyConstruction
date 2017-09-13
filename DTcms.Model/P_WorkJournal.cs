using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_WorkJournal
    {
        public P_WorkJournal()
        { }
        private string _P_Id;
        private string _P_Title;
        private string _P_TypeId;
        private string _P_Content;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private string _P_SendUser;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_Title { get => _P_Title; set => _P_Title = value; }
        public string P_TypeId { get => _P_TypeId; set => _P_TypeId = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string P_SendUser { get => _P_SendUser; set => _P_SendUser = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_AnswerRecord
    {
        public P_AnswerRecord()
        { }
        private string _P_Id;
        private string _P_RecordId;
        private string _P_TestQuestionId;
        private string _P_UserAnswerId;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_RecordId { get => _P_RecordId; set => _P_RecordId = value; }
        public string P_TestQuestionId { get => _P_TestQuestionId; set => _P_TestQuestionId = value; }
        public string P_UserAnswerId { get => _P_UserAnswerId; set => _P_UserAnswerId = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

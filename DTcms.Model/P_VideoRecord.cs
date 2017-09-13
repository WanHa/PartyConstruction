using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_VideoRecord
    {
        public P_VideoRecord()
        { }
        private string _P_Id;
        private string _P_UserId;
        private string _P_VideoId;
        private string _P_QuestionBankId;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int _P_MaxPlaybackTime;
        private int _P_LastPlaybackTime;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public string P_VideoId { get => _P_VideoId; set => _P_VideoId = value; }
        public string P_QuestionBankId { get => _P_QuestionBankId; set => _P_QuestionBankId = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int P_MaxPlaybackTime { get => _P_MaxPlaybackTime; set => _P_MaxPlaybackTime = value; }
        public int P_LastPlaybackTime { get => _P_LastPlaybackTime; set => _P_LastPlaybackTime = value; }
    }
}

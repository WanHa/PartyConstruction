using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class P_TestPaper
    {
        public P_TestPaper()
        { }
        #region Model
        private string _P_Id;
        private string _P_QuestionBankId;
        private string _P_TestPaperName;
        private string _P_Description;
        private int _P_AnswerTime;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int _P_IsRepeat;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_QuestionBankId { get => _P_QuestionBankId; set => _P_QuestionBankId = value; }
        public string P_TestPaperName { get => _P_TestPaperName; set => _P_TestPaperName = value; }
        public string P_Description { get => _P_Description; set => _P_Description = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int P_AnswerTime { get => _P_AnswerTime; set => _P_AnswerTime = value; }
        public int P_IsRepeat { get => _P_IsRepeat; set => _P_IsRepeat = value; }

        #endregion Model
    }
}

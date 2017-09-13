using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class P_TestList
    {
        public P_TestList()
        { }
        #region Model
        private string _P_Id;
        private string _P_TestQuestionId;
        private string _P_Choices;
        private int _P_Correct;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private string _P_Sequence;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_TestQuestionId { get => _P_TestQuestionId; set => _P_TestQuestionId = value; }
        public string P_Choices { get => _P_Choices; set => _P_Choices = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string P_Sequence { get => _P_Sequence; set => _P_Sequence = value; }
        public int P_Correct { get => _P_Correct; set => _P_Correct = value; }

        #endregion Model
    }
}

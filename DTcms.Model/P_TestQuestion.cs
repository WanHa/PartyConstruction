using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class P_TestQuestion
    {
        public P_TestQuestion()
        { }
        #region Model
        private string _P_Id;
        private string _P_TestPaperId;
        private string _P_QuestionStem;
        private string _P_Type;
        private int _P_Score;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private List<P_TestList> _questions_list;

        /// <summary>
        /// 扩展字段字典
        /// </summary>
        private Dictionary<string, string> _fields;
        public Dictionary<string, string> fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        /// <summary>
        /// 扩展字段TestList
        /// </summary>
        private List<P_TestList> _TestList;
        public List<P_TestList> TestList
        {
            set { _TestList = value; }
            get { return _TestList; }
        }

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_TestPaperId { get => _P_TestPaperId; set => _P_TestPaperId = value; }
        public string P_QuestionStem { get => _P_QuestionStem; set => _P_QuestionStem = value; }
        public string P_Type { get => _P_Type; set => _P_Type = value; }
        public int P_Score { get => _P_Score; set => _P_Score = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public List<P_TestList> questions_list { get => _questions_list; set => _questions_list = value; }
        #endregion Model
    }
}

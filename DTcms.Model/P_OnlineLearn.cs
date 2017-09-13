using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    /// <summary>
    /// 实体类P_OnlineLearn。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class P_OnlineLearn
    {

        public P_OnlineLearn() { }
        #region Model
        private string _P_Id;
        private string _P_LearnUrl;
        private DateTime _P_CreateTime;
        private string _P_CreateUser;
        private DateTime _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_LearnUrl { get => _P_LearnUrl; set => _P_LearnUrl = value; }
        public DateTime P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }




        #endregion


    }
}

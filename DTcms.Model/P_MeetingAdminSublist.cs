using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_MeetingAdminSublist
    {
        public P_MeetingAdminSublist()
        { }
        #region Model
        private string _P_ID;
        private string _P_UserId;
        private string _P_MeeID;
        private int? _P_SigninInfo;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_ID { get => _P_ID; set => _P_ID = value; }
        public string P_Title { get => P_UserId; set => P_UserId = value; }
        public string P_MeeID { get => _P_MeeID; set => _P_MeeID = value; }
        public int? P_SigninInfo { get => _P_SigninInfo; set => _P_SigninInfo = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        #endregion

    }
}

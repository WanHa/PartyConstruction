using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_MeetingAdmin
    {
        public P_MeetingAdmin()
        { }
        #region Model
        private string _P_Id;
        private string _P_Title;
        private string _P_MeeContent;
        private DateTime? _P_StartTime;
        private DateTime? _P_EndTime;
        private string _P_MeePlace;
        private int? _P_MAStatus;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int _P_PeopleAmount;
        private List<P_MeetingAdminSublist> _admin_sublist;
        private string _StatusName;
        private int _CountUser;
        private List<P_MeetingAdminSublist> _UserId;
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
        private List<P_MeetingAdminSublist> _MeetingAdminSublist;
        public List<P_MeetingAdminSublist> MeetingAdminSublist
        {
            set { _MeetingAdminSublist = value; }
            get { return _MeetingAdminSublist; }
        }

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_Title { get => _P_Title; set => _P_Title = value; }
        public string P_MeeContent { get => _P_MeeContent; set => _P_MeeContent = value; }
        public DateTime? P_StartTime { get => _P_StartTime; set => _P_StartTime = value; }
        public DateTime? P_EndTime { get => _P_EndTime; set => _P_EndTime = value; }
        public string P_MeePlace { get => _P_MeePlace; set => _P_MeePlace = value; }
        public int? P_MAStatus { get => _P_MAStatus; set => _P_MAStatus = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int P_PeopleAmount { get => _P_PeopleAmount; set => _P_PeopleAmount = value; }
        public List<P_MeetingAdminSublist> admin_sublist { get => _admin_sublist; set => _admin_sublist = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public int CountUser { get => _CountUser; set => _CountUser = value; }
        public List<P_MeetingAdminSublist> UserId { get => _UserId; set => _UserId = value; }

        #endregion
    }
}

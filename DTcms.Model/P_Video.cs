using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class P_Video
    {
        public P_Video() { }
        #region Model
        private string _P_Id;
        private string _P_ParentId;
        private string _P_VideoName;
        private string _P_VideoPic;
        private string _P_Url;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int? _P_Number;
        private int _P_VideoLength;
        private int? _P_Source;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_ParentId { get => _P_ParentId; set => _P_ParentId = value; }
        public string P_VideoName { get => _P_VideoName; set => _P_VideoName = value; }
        public string P_VideoPic { get => _P_VideoPic; set => _P_VideoPic = value; }
        public string P_Url { get => _P_Url; set => _P_Url = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int? P_Number { get => _P_Number; set => _P_Number = value; }
        public int P_VideoLength { get => _P_VideoLength; set => _P_VideoLength = value; }
        public int? P_Source { get => _P_Source; set => _P_Source = value; }
        #endregion Model
    }
}

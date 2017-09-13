using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_ActivityStyle
    {
        public P_ActivityStyle()
        { }
        private string _P_Id;
        private string _P_ActivityName;
        private string _P_ActivitySite;
        private string _P_Sponsor;
        private string _P_Particular;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_ActivityName { get => _P_ActivityName; set => _P_ActivityName = value; }
        public string P_ActivitySite { get => _P_ActivitySite; set => _P_ActivitySite = value; }
        public string P_Sponsor { get => _P_Sponsor; set => _P_Sponsor = value; }
        public string P_Particular { get => _P_Particular; set => _P_Particular = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string img_url { get; set; }
        public DateTime P_ActivityStartTime { get; set; }
        public DateTime P_ActivityEndTime { get; set; }
    }
}

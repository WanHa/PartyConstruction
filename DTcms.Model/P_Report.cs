using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_Report
    {
        public P_Report()
        { }
        private string _P_Id;
        private string _P_UserId;
        private string _P_RelevancyId;
        private string _P_ReportContent;
        private DateTime? _P_CreateTime;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public string P_RelevancyId { get => _P_RelevancyId; set => _P_RelevancyId = value; }
        public string P_ReportContent { get => _P_ReportContent; set => _P_ReportContent = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

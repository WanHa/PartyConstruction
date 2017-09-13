using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_VisitRecord
    {
        public P_VisitRecord()
        { }
        private string _P_Id;
        private string _P_FileId;
        private string _P_Visitor;
        private DateTime? _P_VisitTime;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_FileId { get => _P_FileId; set => _P_FileId = value; }
        public string P_Visitor { get => _P_Visitor; set => _P_Visitor = value; }
        public DateTime? P_VisitTime { get => _P_VisitTime; set => _P_VisitTime = value; }
    }
}

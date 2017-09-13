using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_DownloadRecord
    {
        public P_DownloadRecord()
        { }
        private string _P_Id;
        private string _P_FileId;
        private DateTime? _P_DownloadTime;
        private string _P_DownloadUser;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_FileId { get => _P_FileId; set => _P_FileId = value; }
        public DateTime? P_DownloadTime { get => _P_DownloadTime; set => _P_DownloadTime = value; }
        public string P_DownloadUser { get => _P_DownloadUser; set => _P_DownloadUser = value; }
    }
}

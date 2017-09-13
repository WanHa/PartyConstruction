using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_QiNiuInfo
    {
        public P_QiNiuInfo() { }

        private string _P_Id;
        private string _P_AK;
        private string _P_SK;
        private string _P_RootUrl;
        private string _P_Scope;
        private string _P_CreateUser;
        private DateTime? _P_CreateTime;
        private string _P_UpdateUser;
        private DateTime? _P_UpdateTime;
        private int? _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_AK { get => _P_AK; set => _P_AK = value; }
        public string P_SK { get => _P_SK; set => _P_SK = value; }
        public string P_RootUrl { get => _P_RootUrl; set => _P_RootUrl = value; }
        public string P_Scope { get => _P_Scope; set => _P_Scope = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public int? P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_SubordinateGroupInfo
    {
        public P_SubordinateGroupInfo()
        { }
        private string _P_Id;
        private int? _P_Exists;
        private int? _P_Count;
        private string _P_Info;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public int? P_Exists { get => _P_Exists; set => _P_Exists = value; }
        public int? P_Count { get => _P_Count; set => _P_Count = value; }
        public string P_Info { get => _P_Info; set => _P_Info = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

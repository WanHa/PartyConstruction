using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public class P_Transmit
    {
        public P_Transmit()
        {

        }
        private string _P_Id;
        private int? _P_UserId;
        private string _P_RelationId;
        private string _P_Type;
        private string _P_Content;
        private string _P_OrganizeId;
        private int? _P_TransmitTime;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int? _P_Category;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public int? P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public string P_RelationId { get => _P_RelationId; set => _P_RelationId = value; }
        public string P_Type { get => _P_Type; set => _P_Type = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public string P_OrganizeId { get => _P_OrganizeId; set => _P_OrganizeId = value; }
        public int? P_TransmitTime { get => _P_TransmitTime; set => _P_TransmitTime = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int? P_Category { get => _P_Category; set => _P_Category = value; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_Apply
    {
        public P_Apply()
        { }
        private string _P_Id;
        private string _P_RelationId;
        private string _P_ApplyUserId;
        private string _P_ApplyContent;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private int _P_Source;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_ApplyUserId { get => _P_ApplyUserId; set => _P_ApplyUserId = value; }
        public string P_ApplyContent { get => _P_ApplyContent; set => _P_ApplyContent = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public int P_Source { get => _P_Source; set => _P_Source = value; }
        public string P_RelationId { get => _P_RelationId; set => _P_RelationId = value; }
    }
}

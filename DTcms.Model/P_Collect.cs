﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_Collect
    {
        public P_Collect()
        { }
        private string _P_Id;
        private int? _P_UserId;
        private string _P_Relation;
        private string _P_Type;
        private int? _P_CollectTime;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public int? P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public string P_Relation { get => _P_Relation; set => _P_Relation = value; }
        public string P_Type { get => _P_Type; set => _P_Type = value; }
        public int? P_CollectTime { get => _P_CollectTime; set => _P_CollectTime = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_PartyGallery
    {
        public P_PartyGallery()
        { }
        private string _P_Id;
        private string _P_RoleName;
        private int? _P_Rank;
        private int? _P_RoleStatus;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_RoleName { get => _P_RoleName; set => _P_RoleName = value; }
        public int? P_Rank { get => _P_Rank; set => _P_Rank = value; }
        public int? P_RoleStatus { get => _P_RoleStatus; set => _P_RoleStatus = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

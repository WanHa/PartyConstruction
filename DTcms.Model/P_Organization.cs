using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class P_Organization
    {
        public P_Organization()
        { }
        #region Model
        private string _Id;
        private string _PId;
        private string _Name;
        private string _Creator;
        private DateTime? _CreateTime;
        private string _UpdateUser;
        private DateTime? _UpdateTime;
        private int _Status;
        private string _Manager;

        public string Id { get => _Id; set => _Id = value; }
        public string PId { get => _PId; set => _PId = value; }
        public string Name { get => _Name; set => _Name = value; }
        public DateTime? CreateTime { get => _CreateTime; set => _CreateTime = value; }
        public string Creator { get => _Creator; set => _Creator = value; }
        public DateTime? UpdateTime { get => _UpdateTime; set => _UpdateTime = value; }
        public string UpdateUser { get => _UpdateUser; set => _UpdateUser = value; }
        public int Status { get => _Status; set => _Status = value; }
        public string Manager { get => _Manager; set => _Manager = value; }


        #endregion Model
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_PartyGroupForum
    {
        public P_PartyGroupForum()
        { }
        private string _P_Id;
        private string _P_CreaterId;
        private string _P_Title;
        private string _P_Intro;
        private string _P_ImageId;
        private DateTime? _P_CreateTime;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_CreaterId { get => _P_CreaterId; set => _P_CreaterId = value; }
        public string P_Title { get => _P_Title; set => _P_Title = value; }
        public string P_Intro { get => _P_Intro; set => _P_Intro = value; }
        public string P_ImageId { get => _P_ImageId; set => _P_ImageId = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

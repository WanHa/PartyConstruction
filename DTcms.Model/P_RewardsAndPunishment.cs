using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_RewardsAndPunishment
    {
        public P_RewardsAndPunishment()
        { }
        private string _P_Id;
        private string _P_Title;
        private DateTime? _P_DateTime;
        private string _P_Content;
        private string _P_RatifyOrganiz;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_Title { get => _P_Title; set => _P_Title = value; }
        public DateTime? P_DateTime { get => _P_DateTime; set => _P_DateTime = value; }
        public string P_Content { get => _P_Content; set => _P_Content = value; }
        public string P_RatifyOrganiz { get => _P_RatifyOrganiz; set => _P_RatifyOrganiz = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
    }
}

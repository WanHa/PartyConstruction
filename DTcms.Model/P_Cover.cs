using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_Cover
    {
        private string _P_Id;
        private string _P_UserId;
        private DateTime? _Create_time;
        private string _Create_user;
        private DateTime? _Update_time;
        private string _Update_user;
        private int _Status;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_UserId { get => _P_UserId; set => _P_UserId = value; }
        public DateTime? Create_time { get => _Create_time; set => _Create_time = value; }
        public string Create_user { get => _Create_user; set => _Create_user = value; }
        public DateTime? Update_time { get => _Update_time; set => _Update_time = value; }
        public string Update_user { get => _Update_user; set => _Update_user = value; }
        public int Status { get => _Status; set => _Status = value; }
    }
}

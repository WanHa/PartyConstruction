using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class P_Image
    {
        public P_Image()
        {

        }
        private string _P_Id;
        private string _P_ImageId;
        private string _P_ImageUrl;
        private DateTime? _P_CreateTime;
        private string _P_CreateUser;
        private DateTime? _P_UpdateTime;
        private string _P_UpdateUser;
        private int _P_Status;
        private string _P_PictureName;
        private string _P_ImageType;

        public string P_Id { get => _P_Id; set => _P_Id = value; }
        public string P_ImageId { get => _P_ImageId; set => _P_ImageId = value; }
        public string P_ImageUrl { get => _P_ImageUrl; set => _P_ImageUrl = value; }
        public DateTime? P_CreateTime { get => _P_CreateTime; set => _P_CreateTime = value; }
        public string P_CreateUser { get => _P_CreateUser; set => _P_CreateUser = value; }
        public DateTime? P_UpdateTime { get => _P_UpdateTime; set => _P_UpdateTime = value; }
        public string P_UpdateUser { get => _P_UpdateUser; set => _P_UpdateUser = value; }
        public int P_Status { get => _P_Status; set => _P_Status = value; }
        public string P_PictureName { get => _P_PictureName; set => _P_PictureName = value; }
        public string P_ImageType { get => _P_ImageType; set => _P_ImageType = value; }
    }
}

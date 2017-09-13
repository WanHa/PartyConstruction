using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class UploadPicture
    {
        private DAL.UploadPicture dal = new DAL.UploadPicture();

        public string AddImage(UploadPictureModel model)
        {
            return dal.AddImage(model);
        }
    }
}

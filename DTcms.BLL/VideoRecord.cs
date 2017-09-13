using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.PhoneVideo;
namespace DTcms.BLL
{
    public class VideoRecord
    {
        PhoneVideo phone = new PhoneVideo();

        public List<videomodel> VideoList(int userid, int rows, int page)
        {
            return phone.GetVideoList(userid, rows, page);

        }

        public timemodel BllVideoTime(int userid)
        {

            return phone.Getmaxtime(userid);

        }
        /// <summary>
        /// 个人中心头像，背景图以及名字
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public personalmodel BllPersonCenter(int userid)
        {

            return phone.GetPersonalInfo(userid);

        }

    }
}

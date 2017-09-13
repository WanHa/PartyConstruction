using DTcms.DAL;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
   public class MyCollecter
    {
        private MyCollect dal = new MyCollect();
        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<MyCollectModel> GetMyCollect(int userid, int rows, int page)
        {
            return dal.GetMycollects(userid,rows,page);
        }
        /// <summary>
        /// 用户更换头像
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="imgname"></param>
        /// <returns></returns>
        public Boolean UpAvatar(int userid, string imgname)
        {
            return dal.UpAvatar(userid,imgname);
        }
        /// <summary>
        /// 修改用户的密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public Boolean UpPassWord(int userid, string pass)
        {
            return dal.UpPassWord(userid,pass);
        }
        public List<ContentModel> GetPersonalCenter(int userid, int rows, int page)
        {
            return dal.GetPersonalCenter(userid,rows,page);
        }
        public bool GetDeleteContent(List<DeleteCenter> fromBody)
        {
            return dal.GetDeleteContent(fromBody);
        }

    }
}

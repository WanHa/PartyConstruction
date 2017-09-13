using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.Left_home;

namespace DTcms.BLL
{
    public class home
    {
        private Left_home left = new Left_home();
        /// <summary>
        /// 获取年龄
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public List<Ages> GetAge(string groupid)
        {
            return left.GetAge(groupid);
        }
        /// <summary>
        /// 获取性别
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public Gender GetSex(string groupid)
        {
            return left.GetSex(groupid);
        }
        public List<Learntime> GetGrouptime(string groupid)
        {
            return left.GetGrouptime(groupid);
        }
        public List<Economic> GetMoney(string groupid)
        {
            return left.GetMoney(groupid);
        }
    }
}

using DTcms.Common;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public partial class signlnStatistics
    {
        private readonly DAL.signlnStatistics dal;
        public signlnStatistics()
        {
            dal = new DAL.signlnStatistics();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 手动签到
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateType(string id)
        {
            return dal.UpdateType(id);
        }

        public bool PhoneSingIn(string meetId, string userId)
        {
            return dal.PhoneSingIn(meetId, userId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.P_MeetingAdminSublist GetModel(string id)
        {
            return dal.GetModel(id);
        }
        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        /// <summary>
        /// 检查生成目录名与指定路径下的一级目录是否同名
        /// </summary>
        /// <param name="dirPath">指定的路径</param>
        /// <param name="build_path">生成目录名</param>
        /// <returns>bool</returns>
        private bool DirPathExists(string dirPath, string build_path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Utils.GetMapPath(dirPath));
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                if (build_path.ToLower() == dir.Name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }






    }
}

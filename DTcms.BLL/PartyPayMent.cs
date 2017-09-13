using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTcms.Common;
using System.Data;
using System.IO;

namespace DTcms.BLL
{
    public partial class P_PartyPayMent
    {
        private readonly DAL.P_PartyPayMent dal;

        public P_PartyPayMent()
        {
            dal = new DAL.P_PartyPayMent();
        }
        #region 基本方法===============================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            return dal.Exists(id);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Model.P_PartyPayMent model, List<Model.P_PartyPayMentPeople> peopleList)
        {
            return dal.Add(model, peopleList);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.P_PartyPayMent model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string userid)
        {
            return dal.Delete(id, userid);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.P_PartyPayMent GetModel(string id)
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

        #endregion


        #region 私有方法===============================
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
        #endregion
    }
}

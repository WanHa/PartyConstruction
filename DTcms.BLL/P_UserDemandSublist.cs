using DTcms.Common;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public partial class P_UserDemandSublist
    {
        private readonly DAL.P_UserDemandSublist dal;

        public P_UserDemandSublist()
        {
            dal = new DAL.P_UserDemandSublist();
        }

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
        public string Add(Model.P_UserDemandSublist model,string id)
        {
            return dal.Add(model,id);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.P_UserDemandSublist model)
        {
            return dal.Update(model);
        }

        public List<Model.P_UserDemandSublist> Getlist(string id)
        {
            return dal.Getlist(id);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string UserId)
        {
            return dal.Delete(id, UserId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Userdemand GetToModel(string id)
        {
            return dal.GetToModel(id);
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
        public List<Ims> GetUrl(string id)
        {
            return dal.GetUrl(id);
        }
    }
}

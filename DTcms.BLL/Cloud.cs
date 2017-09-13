using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class Cloud
    {
        private PryCloud ptycloud = new PryCloud();
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public Boolean Submit(CloudModel model)
        {
            return ptycloud.Submit(model);
        }
        /// <summary>
        /// 党建云列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<Cloudlist> GetCloudList(BranchList list)
        {
            return ptycloud.GetCloudList(list);
        }
        /// <summary>
        /// 分享党组织接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Boolean GetBranch(BranchList model)
        {
            return ptycloud.GetBranch(model);
        }
        /// <summary>
        /// 点击分享获取党组织的接口
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public GetGroup ClickShare(int userid)
        {
            return ptycloud.ClickShare(userid);
        }
        /// <summary>
        /// 点击党建云获取数量
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Count> GetSum()
        {
            return ptycloud.GetSum();
        }
        /// <summary>
        /// 检索查询数据列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //public List<Cloudlist> GetTypeList(BranchList list)
        //{
        //    return ptycloud.GetTypeList(list);
        //}
    }
}

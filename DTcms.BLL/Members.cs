using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public class Members
    {
        private MembersModel model = new MembersModel();
        /// <summary>
        /// 模范党员列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<MeModel> GetModelList(int userid, int rows, int page)
        {
            return model.GetModelList(userid, rows, page); 
        }

        /// <summary>
        /// 模范党员详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailMeModel ModelXiangQing(string id, int userid)
        {
            return model.ModelXiangQing(id,userid);
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="salt"></param>
        /// <param name="password1"></param>
        /// <returns></returns>
        public Boolean GetDengLuEnroll(string mobile,string salt,string password1)
        {
            return model.GetDengLuEnroll(mobile,salt,password1);
        }
    }
}

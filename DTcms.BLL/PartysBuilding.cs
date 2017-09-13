using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public class PartysBuilding
    {
        private PartyBuilding building = new PartyBuilding();
        /// <summary>
        /// 获取cms列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<BuildingModel> GetPartyNewsList(BuildModel buil)
        {
            return building.GetPartyNewsList(buil);
        }
        /// <summary>
        /// 获取cms详情接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailPaperModel SelPartyNewsPeper(int userid, int id)
        {
            return building.SelPartyNewsPeper(userid, id);
        }
        /// <summary>
        /// 收藏方法
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Boolean GetCollect(int userid, string id, string type)
        {
            return building.GetCollect(userid, id, type);
        }
        /// <summary>
        /// 删除收藏接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DelectCollect(int userid, string id,string type)
        {
            return building.DelectCollect(userid, id,type);
        }
        /// <summary>
        /// 账号注册加密接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="salt"></param>
        /// <param name="password1"></param>
        /// <returns></returns>
        public Boolean GetRegisterEnroll(string mobile,string salt, string password1)
        {
            return building.GetRegisterEnroll(mobile, salt, password1);
        }
    }
}

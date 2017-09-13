using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public partial class PartyMap
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.PartyMap pm;

        public PartyMap()
        {
            pm = new DAL.PartyMap();
        }

        public string MapGetPositionList()
        {
            return pm.MapGetPositionList();
        }

		/// <summary>
		/// 获取党组织信息
		/// 传入党组织名称，根据传入的值查询全部或者关键字查询
		/// </summary>
		/// <param name="party"></param>
		/// <returns></returns>
		public List<PartyMassageModel> getPartyMassage(string party, int rows, int page)
		{
			return pm.getPartyMassage(party, rows, page);
		}
		/// <summary>
		/// 获取党组织详细信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PartyMassageDetailsModel getPartyMassageDetails(string id)
		{
			return pm.getPartyMassageDetails(id);
		}
		/// <summary>
		/// 获取职务、姓名列表
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public List<PartyList> getPartyMember(string groupId,int rows,int page)
		{
			return pm.getPartyMember(groupId,rows,page);
		}

		/// <summary>
		/// 传入经纬度与距离返回所传经纬度相应距离的位置
		/// </summary>
		/// <param name="lat">纬度</param>
		/// <param name="lng">精度</param>
		/// <param name="dist">距离</param>
		/// <returns></returns>
		public List<BranchL> getLocation(double lat, double lng, string party)
		{
			return pm.getLocation(lat, lng,party);
		}
	}
}

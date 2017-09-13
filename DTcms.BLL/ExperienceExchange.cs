using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using static DTcms.DAL.ExperienceExchange;

namespace DTcms.BLL
{
	public partial class ExperienceExchange
	{

		private readonly DAL.ExperienceExchange dal;

		public ExperienceExchange()
		{
			dal = new DAL.ExperienceExchange();
		}
		public List<Model.P_LearnExchange> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
		{
			return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
		}

        public LearnExchangeModel SelLearnExchange(string id)
        {
            return dal.SelLearnExchange(id);
        }

        public List<LearnExchangeImage> SelLearnExchangeImg(string id)
        {
            return dal.SelLearnExchangeImg(id);
        }

        public int SelStatus(string id)
        {
            return dal.SelStatus(id);
        }
        /// <summary>
        /// 通过操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Pass(string id)
		{
			return dal.Pass(id);
		}
		/// <summary>
		/// 拒绝操作
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Refuse(string id)
		{
			return dal.Refuse(id);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.P_LearnExchange GetModel(string id)
		{
			return dal.GetModel(id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string id)
		{
			return dal.Delete(id);
		} 
	}
}

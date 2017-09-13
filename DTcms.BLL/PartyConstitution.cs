using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.Model.WebApiModel.PartyConstitutionModel;

namespace DTcms.BLL
{
    public class PartyConstitution
    {
        private DAL.PartyConstitution dal = new DAL.PartyConstitution();

        /// <summary>
        /// 获取党规党章下的类别列表
        /// </summary>
        /// <returns></returns>
        public List<Constitution> GetPartyConstitution()
        {
            return dal.GetPartyConstitution();
        }
    }
}

using DTcms.DAL;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.P_DailyLoWord;

namespace DTcms.BLL
{
    public class P_DailyLogWord
    {
        private P_DailyLoWord word = new P_DailyLoWord();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return word.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }
        public DetailWoedJournal SelPartyNewsPeper(string id)
        {
            return word.SelPartyNewsPeper(id);
        }

        public List<DailyLogImage> SelDailyLogImage(string id)
        {
            return word.SelDailyLogImage(id);
        }

        public bool Delete(string id, string userid)
        {
            return word.Delete(id,userid);
        }
    }
}

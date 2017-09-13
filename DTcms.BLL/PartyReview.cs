using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class PartyReview
    {
        private PartyReviewActivtity dal = new PartyReviewActivtity();
        public List<usercount> GetReview(string id)
        {
            return dal.GetReview(id);
        }
    }
}

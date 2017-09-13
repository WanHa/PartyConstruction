using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
   public  class PartyReviewActivtity
    {
        public List<usercount> GetReview(string id)
        {
            List<usercount> user = new List<usercount>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select P_VoteOption.P_Option as username,");
            strSql.Append(" (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ByVoteUserId=P_VoteOption.P_Id ) as cont ");
            strSql.Append(" from P_VoteOption ");           
            strSql.Append(" where P_VoteOption.P_ActivityId='" + id + @"'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataSetToModelHelper<usercount> model = new DataSetToModelHelper<usercount>();
            if (ds != null)
            {
                user= model.FillModel(ds.Tables[0]);
            }
            else
            {
                user = null;
            }
            return user;
        }
    }
    public class usercount
    {
        public string username { get; set; }
        public int cont { get; set; }
    }
}

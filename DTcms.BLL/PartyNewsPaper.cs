using DTcms.DAL;
using DTcms.Model;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public class PartyNewsPaper
    {
        private PartyNewsPaperBiz paper = new PartyNewsPaperBiz();

        /// <summary>
        /// 获取转发时,@人员列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<AtPersonnelModel> GetAtPersonnel(string userId, int page, int rows) {
            return paper.GetAtPersonnel(userId, page, rows);
        }

        public Boolean SubTranck(Trum transmit)
        {
            return paper.SubTranck(transmit);
        }
        public GetGroups GetGroupsRelation(int userid)
        {
            return paper.GetGroupsRelation(userid);
        }
    }
}

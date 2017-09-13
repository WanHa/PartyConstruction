using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.PartyBranchManagement;
using static DTcms.DAL.WeChat;

namespace DTcms.BLL
{
    public class WeChatCon
    {
        WeChat WC =new WeChat();
        public PayConfigModel GetWeChat()
        {
            return WC.GetWeChat();
        }



      

    }





}

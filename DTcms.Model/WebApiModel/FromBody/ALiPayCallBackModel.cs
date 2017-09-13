using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    public class ALiPayCallBackModel
    {
        public string notify_time { get; set; }

        public string notify_type { get; set; }

        public string notify_id { get; set; }

        public string app_id { get; set; }

        public string trade_no { get; set; }

        public string out_trade_no { get; set; }

        public string out_biz_no { get; set; }

        public string buyer_id { get; set; }

        public string buyer_logon_id { get; set; }

        public decimal receipt_amount { get; set; }
    }
}

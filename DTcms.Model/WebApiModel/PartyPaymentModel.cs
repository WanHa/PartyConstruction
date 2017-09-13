using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    /// <summary>
    /// API党费缴纳model
    /// </summary>
    public class Detail
    {
        public string username { get; set; }
        public string mobile { get; set; }
        public string card{ get; set; }
        public decimal money { get; set; }
        public string moneyid { get; set; }
    }
    public class PartyPaymentModel
    {
        public string username { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string content { get; set; }
        
        public string p_id { get; set; }       
    }
    public class PartyPaymentRecordModel
    {
        public string username { get; set; }
        public string avatar { get; set; }
        public string content { get; set; }
        //public DateTime paytime { get; set; }
        public decimal paysum { get; set; }
        private string _paytime;
        public string paytime
        {
            get
            {
                return DateTime.Parse(_paytime == null ? "" : _paytime).ToString("HH:MM");
            }
            set
            {
                _paytime = value;
            }
        }
    }
}

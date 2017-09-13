using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class ZTreeModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 组织上级ID
        /// </summary>
        public int pId { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 是否打开
        /// </summary>
        public bool open { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean @checked { get; set; }
        //public string url { get; set; }

        //public string icon { get; set; }

        //public string title { get; set; }
    }
}

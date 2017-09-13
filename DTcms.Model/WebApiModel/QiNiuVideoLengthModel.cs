using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
   public class QiNiuVideoLengthModel
    {
        public VideolendthModel format { get; set; }
    }

    public class VideolendthModel {

        public double duration { get; set; }
    }
}

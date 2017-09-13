using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class Ptrain
    {
        private ptrain partytrain = new ptrain();
        public DemandCount GetDetails(int year)
        {
            return partytrain.GetDetails(year);
        }
        public DemandCount GetPending(int year)
        {
            return partytrain.GetPending(year);
        }
        public DemandCount GetFinishDemand(int year)
        {
            return partytrain.GetFinishDemand(year);
        }
    }
}

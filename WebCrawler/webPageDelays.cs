using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class webPageDelays
    {
        public int hashValue;
        public int lastVisited;
        public int delayValue;
        public webPageDelays(int hashvalue, int delayValue)
        {
            hashValue = hashvalue;
            this.delayValue = delayValue;
        }
    }
}

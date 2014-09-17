using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class incidenceVector
    {
        public string term;
        public List<int> pageIDs = new List<int>();

        public incidenceVector (string term)
        {
            this.term = term;
        }

        public int pageCount
        {
            get { return pageIDs.Count(); }
        }
    }
}

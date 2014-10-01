using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class incidenceVector
    {
        public double idf;
        public string term;
        public List<Posting> pageIDs = new List<Posting>();

        public incidenceVector (string term)
        {
            this.term = term;
        }

        public int pageCount
        {
            get { return pageIDs.Count(); }
        }
    }

    class Posting
    {
        public double tfStar;
        public int pageID;
        public int pageCount;

        public Posting(int pageID)
        {
            this.pageID = pageID;
            pageCount = 0;
        }

        public double getTfIdf(double idf)
        {
            return tfStar * idf;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class incidenceVector
    {
        public float idf;
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
        public float tfStar;
        public int pageID;
        public int pageCount;

        public Posting(int pageID)
        {
            this.pageID = pageID;
            pageCount = 0;
        }

        public float getTfIdf(float idf)
        {
            return tfStar * idf;
        }
    }
}

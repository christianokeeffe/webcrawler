using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Ranker
{
    class IDF
    {
        public void getIDF(List<string> documents, List<incidenceVector> terms)
        {
            double pagecount = documents.Count();
            double df;
            foreach (incidenceVector page in terms)
            {
                df = page.pageIDs.Count();
                page.idf = (float)Math.Log10(pagecount / df);
            }
        }
    }
}

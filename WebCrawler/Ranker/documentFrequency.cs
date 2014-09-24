using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Ranker
{
    class documentFrequency
    {
        public void calculateTFAndIDF(List<string> documents, List<incidenceVector> listOfTerms) {
            double pageCount = documents.Count();
            foreach (incidenceVector term in listOfTerms) {
                term.idf = (float)Math.Log10(pageCount / term.pageIDs.Count());
                foreach (Posting post in term.pageIDs) {
                    if (post.pageCount > 0) {
                        post.tfStar = (1 + (float)Math.Log10(post.pageCount));
                    } else {
                        post.tfStar = 0;
                    }
                }
            }
        }
    }
}

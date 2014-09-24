using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Ranker
{
    class tfStar
    {
        public void calculateTFStar(List<incidenceVector> listOfTerms) {
            foreach (incidenceVector term in listOfTerms) {
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

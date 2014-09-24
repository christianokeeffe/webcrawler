using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Ranker
{
    class normalize
    {

        public List<List<termValue>> getNormalizeVectors(List<incidenceVector> inputList, List<string> searchList, List<string> pageIDlist)
        {
            List<List<termValue>> docTermIFIDF = new List<List<termValue>>();

            for(int i = 0; i < pageIDlist.Count; i++)
            {
                List<termValue> temp = new List<termValue>();
                docTermIFIDF.Add(temp);
            }

            IEnumerable<incidenceVector> selectedTerms = inputList.Where(vec => searchList.Contains(vec.term));

            foreach (incidenceVector termVector in selectedTerms)
            {
                for (int i = 0; i < termVector.pageIDs.Count; i++)
                {
                    termValue temp = new termValue();
                    temp.tfidf = termVector.pageIDs[i].getTfIdf(termVector.idf);
                    temp.term = termVector.term;
                    docTermIFIDF[termVector.pageIDs[i].pageID].Add(temp);
                }
            }

            for(int i = 0; i < docTermIFIDF.Count; i++)
            {
                double sum = 0.0;
                for(int j = 0; j < docTermIFIDF[i].Count; j++)
                {
                    sum += docTermIFIDF[i][j].tfidf * docTermIFIDF[i][j].tfidf;
                }
                double sqr = Math.Sqrt(sum);
                for (int j = 0; j < docTermIFIDF[i].Count; j++)
                {
                    docTermIFIDF[i][j].normtfidf = docTermIFIDF[i][j].tfidf / sqr;
                }
            }

            return docTermIFIDF;
        }
    }
}

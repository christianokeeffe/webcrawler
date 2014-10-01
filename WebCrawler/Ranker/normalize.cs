using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Ranker
{
    class normalize
    {

        public List<List<termValue>> getNormalizeVectors(List<incidenceVector> inputList, string[] searchList, List<string> pageIDlist)
        {
            List<List<termValue>> docTermIFIDF = new List<List<termValue>>();

            for(int i = 0; i < pageIDlist.Count; i++)
            {
                List<termValue> temp = new List<termValue>();
                for (int j = 0; j < searchList.Length; j++ )
                {
                    termValue tmpterm = new termValue();
                    tmpterm.term = searchList[j];
                    tmpterm.tf = 0.0;
                    temp.Add(tmpterm);
                }
                docTermIFIDF.Add(temp);
            }

            IEnumerable<incidenceVector> selectedTerms = inputList.Where(vec => searchList.Contains(vec.term));

            foreach (incidenceVector termVector in selectedTerms)
            {
                int j = 0;
                for (int i = 0; i < termVector.pageIDs.Count; i++)
                {
                    docTermIFIDF[termVector.pageIDs[i].pageID][j].tf = termVector.pageIDs[i].tfStar;
                }
                j++;
            }

            for (int i = 0; i < docTermIFIDF.Count; i++ )
            {
                bool[] isPresent = new bool[searchList.Length];
                docTermIFIDF[i].Sort(delegate(termValue tp1, termValue tp2) { return tp1.term.CompareTo(tp2.term); });

                for(int j = 0; j < docTermIFIDF[i].Count; j++)
                {
                    int q = 0;
                    while(docTermIFIDF[i][j].term != searchList[q])
                    {
                        q++;
                    }
                    if(q < docTermIFIDF[i].Count)
                    {
                        isPresent[q] = true;
                    }
                    else
                    {
                        isPresent[q] = false;
                    }
                }

                for(int j = 0; j < isPresent.Length; j++)
                {
                    if(!isPresent[j])
                    {
                        termValue temp = new termValue();
                        temp.term = searchList[j];
                        temp.tf = 0.0;
                        docTermIFIDF[i].Add(temp);
                    }
                }
            }

                /*for (int i = 0; i < docTermIFIDF.Count; i++)
                {
                    double sum = 0.0;
                    for (int j = 0; j < docTermIFIDF[i].Count; j++)
                    {
                        sum += docTermIFIDF[i][j].tf * docTermIFIDF[i][j].tf;
                    }
                    double sqr = Math.Sqrt(sum);
                    for (int j = 0; j < docTermIFIDF[i].Count; j++)
                    {
                        if (sqr == 0.0)
                        {
                            docTermIFIDF[i][j].normtf = 0.0;
                        }
                        else
                        {
                            docTermIFIDF[i][j].normtf = docTermIFIDF[i][j].tf / sqr;
                        }
                    }
                    //docTermIFIDF[i].Sort(delegate(termValue tp1, termValue tp2) { return tp1.term.CompareTo(tp2.term); });
                }*/
            for (int j = 0; j < docTermIFIDF[0].Count; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < docTermIFIDF.Count; i++)
                {
                    sum += docTermIFIDF[i][j].tf * docTermIFIDF[i][j].tf;
                }
                double sqr = Math.Sqrt(sum);
                for (int i = 0; i < docTermIFIDF.Count; i++)
                {
                    if (sqr == 0.0)
                    {
                        docTermIFIDF[i][j].normtf = 0.0;
                    }
                    else
                    {
                        docTermIFIDF[i][j].normtf = docTermIFIDF[i][j].tf / sqr;
                    }
                }
                //docTermIFIDF[i].Sort(delegate(termValue tp1, termValue tp2) { return tp1.term.CompareTo(tp2.term); });
            }

            return docTermIFIDF;
        }
    }
}

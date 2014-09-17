using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class indexer
    {
        public Tuple<List<string>,List<incidenceVector>> getIndexTable(List<KeyValuePair<string, string>> inputList)
        {
            List<termPagePair> termPageList = new List<termPagePair>();
            List<string> pageIDList = new List<string>();
            stemmer stem = new stemmer();
            for (int i = 0; i < inputList.Count; i++ )
            {
                List<string> tokens = stem.stem(tokenizer.splitString(inputList[i].Value));
                for(int j = 0; j < tokens.Count; j++)
                {
                    termPageList.Add(new termPagePair(tokens[j], i));
                }
                pageIDList.Add(inputList[i].Key);
            }
            
            termPageList = sortPairs(termPageList);
            return new Tuple<List<string>,List<incidenceVector>>(pageIDList, pairsToVectorTable(termPageList));
        }

        private List<termPagePair> sortPairs(List<termPagePair> listToSort)
        {
            listToSort.Sort(
                delegate(termPagePair tp1, termPagePair tp2) 
                { 
                    int compval = tp1.term.CompareTo(tp2.term);
                    if(compval == 0)
                    {
                        return tp1.pageID.CompareTo(tp2.pageID);
                    }
                    else
                    {
                        return compval;
                    }
                });
            return listToSort;
        }

        private List<incidenceVector> pairsToVectorTable(List<termPagePair> inputList)
        {
            string currentTerm = "";
            int currentPageID = -1;
            List<incidenceVector> returnList = new List<incidenceVector>();
            for(int i = 0; i < inputList.Count; i++)
            {
                if(currentTerm != inputList[i].term)
                {
                    returnList.Add(new incidenceVector(inputList[i].term));
                    currentTerm = inputList[i].term;
                    currentPageID = inputList[i].pageID;
                    returnList[returnList.Count - 1].pageIDs.Add(new Posting(currentPageID));
                }
                else if (currentPageID != inputList[i].pageID)
                {
                    currentPageID = inputList[i].pageID;
                    returnList[returnList.Count - 1].pageIDs.Add(new Posting(currentPageID));
                }

                returnList[returnList.Count - 1].pageIDs[returnList[returnList.Count - 1].pageIDs.Count - 1].pageCount++;
            }
            return returnList;
        }
    }
}

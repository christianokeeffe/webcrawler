﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class indexer
    {
        public List<incidenceVector> getIndexTable(List<KeyValuePair<string, string>> inputList)
        {
            
            /*termPageList = sortPairs(termPageList);
            return pairsToVectorTable(termPageList);*/
            return null;
        }

        private List<termPagePair> sortPairs(List<termPagePair> listToSort)
        {
            listToSort.Sort(
                delegate(termPagePair tp1, termPagePair tp2) 
                { 
                    int compval = tp1.term.CompareTo(tp1.term);
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
            List<incidenceVector> returnList = new List<incidenceVector>();
            for(int i = 0; i < inputList.Count; i++)
            {
                if(currentTerm != inputList[i].term)
                {
                    returnList.Add(new incidenceVector(inputList[i].term));
                }
                returnList[returnList.Count - 1].pageIDs.Add(inputList[i].pageID);
            }
            return returnList;
        }
    }
}
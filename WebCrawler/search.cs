using SearchEngine.Ranker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class search
    {
        public List<KeyValuePair<int, double>> searchFromString(string q, Tuple<List<string>, List<incidenceVector>> indexList)
        {
            string[] qarr = tokenizer.splitString(q).OrderBy(n => n).ToArray();
            stemmer stem = new stemmer();
            qarr = stem.stem(qarr).ToArray();
            double[] termIDFs = new double[qarr.Length];
            double[] normTermIDFs = new double[qarr.Length];
            normalize norm = new normalize();
            List<List<termValue>> docTermIFIDF =  norm.getNormalizeVectors(indexList.Item2, qarr, indexList.Item1);
            
            double sum = 0.0;
            for(int i = 0; i < qarr.Length; i++)
            {
                IEnumerable<incidenceVector> temp = indexList.Item2.Where(vec => qarr[i]==vec.term);
                int pagecount = 0;
                if (temp.Count() > 0)
                {
                    pagecount = temp.First().pageCount;
                }
                if (pagecount > 0)
                {
                    termIDFs[i] = Math.Log10((double)indexList.Item1.Count / (double)pagecount);
                    sum += termIDFs[i] * termIDFs[i];
                }
            }
            sum = Math.Sqrt(sum);
            if (sum > 0)
            {
                for (int i = 0; i < qarr.Length; i++)
                {
                    normTermIDFs[i] = termIDFs[i] / sum;
                }
            }

            List<KeyValuePair<int,double>> pageValueList = new List<KeyValuePair<int,double>>();
            for(int i = 0; i < docTermIFIDF.Count;i++)
            {
                double tempVal = 0.0;
                for(int j = 0; j < qarr.Length; j++)
                {
                    tempVal += normTermIDFs[j] * docTermIFIDF[i][j].normtf;
                }
                pageValueList.Add(new KeyValuePair<int, double>(i, tempVal));
            }

            pageValueList.Sort(
                    delegate(KeyValuePair<int, double> tp1, KeyValuePair<int, double> tp2)
                    {
                        return tp2.Value.CompareTo(tp1.Value);
                    });
            return pageValueList.Where(it => it.Value > 0).ToList();
        }
    }
}

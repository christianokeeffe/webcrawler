using Iveonik.Stemmers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class stemmer
    {
        public List<string> stem (List<string> inputString)
        {
            List<string> returnString = new List<string>();
            IStemmer eng = new EnglishStemmer();
            foreach (string str in inputString)
            {
                string stemmed = eng.Stem(str);
                if (!returnString.Contains(stemmed))
                {
                    returnString.Add(stemmed);
                }
            }

            return returnString;
        }
    }
}

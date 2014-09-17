using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchEngine
{
    class nearMatch
    {
        private static bool isNearMatch(string s1, string s2, int shingling_length, double threshold)
        {
            double result = jac(shingling_hash(s1, shingling_length), shingling_hash(s2, shingling_length));
            return result >= threshold;
        }

        private static List<int> shingling_hash(string s, int shingling)
        {
            List<int> hash_S = new List<int>();
            string temp = Regex.Replace(s, @"[^A-Za-z\s]", string.Empty);
            List<String> s_strings = temp.ToLower().Split(null).ToList<string>();
            for (int i = 0; i < s_strings.Count - shingling + 1; i++)
            {
                string sum = "";
                for (int j = 0; j < shingling; j++)
                {
                    sum += s_strings[i + j];
                }
                hash_S.Add(sum.GetHashCode());
            }
            return hash_S;
        }

        private static double jac(List<int> s1, List<int> s2)
        {
            double uniuncount = s1.Union(s2).ToList().Count;
            double intersectcount = s1.Intersect(s2).ToList().Count;
            return intersectcount / uniuncount;
        }
    }
}

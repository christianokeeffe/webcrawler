using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class webpage
    {
        public Uri baseUrl;
        public List<robotRestriction> restrictions;
        public int hashValue;
        public int lastVisited;
        public int delayValue;

        public webpage()
        {
        }

        public webpage(Uri inputUrl, List<robotRestriction> inputList, int inputHash, int visited, int delay)
        {
            baseUrl = inputUrl;
            restrictions = inputList;
            hashValue = inputHash;
            lastVisited = visited;
            delayValue = delay;
        }
    }
}

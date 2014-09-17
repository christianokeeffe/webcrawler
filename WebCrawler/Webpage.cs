using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class webPage
    {
        public Uri baseUrl;
        public List<robotRestriction> restrictions;
        public int hashValue;
        public int lastVisited;
        public int delayValue;

        public webPage()
        {
            lastVisited = 0;
            delayValue = 2;
        }

        public webPage(Uri inputUrl)
        {
            baseUrl = inputUrl;
            hashValue = inputUrl.GetHashCode();
            lastVisited = 0;
            delayValue = 2;
        }
    }
}

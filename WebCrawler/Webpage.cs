using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
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

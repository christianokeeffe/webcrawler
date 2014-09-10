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

        public webpage()
        {
        }

        public webpage(Uri inputUrl, List<robotRestriction> inputList)
        {
            baseUrl = inputUrl;
            restrictions = inputList;
        }
    }
}

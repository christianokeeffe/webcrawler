using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class robotRestriction
    {
        public string type;
        public Uri url;
        public robotRestriction()
        {

        }

        public robotRestriction(string inputType, Uri inputUrl)
        {
            type = inputType;
            url = inputUrl;
        }
    }
}

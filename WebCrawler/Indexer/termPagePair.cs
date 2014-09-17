using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class termPagePair
    {
        public string term = "";
        public int pageID = -1;

        public termPagePair(string term, int pageID)
        {
            this.term = term;
            this.pageID = pageID;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class robotRestriction
    {
        public string type;
        public string url;
        public robotRestriction()
        {

        }

        public robotRestriction(string inputType, string inputUrl)
        {
            type = inputType;
            url = inputUrl;
        }
    }
}

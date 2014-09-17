using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            WebCrawler webCrawler = new WebCrawler();
            Queue<Uri> listOfPages = new Queue<Uri>();
            listOfPages.Enqueue(new Uri("http://www.reddit.com"));
            List<KeyValuePair<string, string>> temp = webCrawler.crawlWebSites(listOfPages, "OKEEFFE");
            indexer index = new indexer();
            Tuple<List<string>, List<incidenceVector>> list = index.getIndexTable(temp);
            List<incidenceVector> templist = new List<incidenceVector>();
            for (int i = 0; i < list.Item2.Count; i++ )
            {
                if(list.Item2[i].pageIDs.Count != 1)
                {
                    templist.Add(list.Item2[i]);
                }
            }
                Console.ReadKey();
        }
    }
}

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
            listOfPages.Enqueue(new Uri("http://9gag.com"));
            listOfPages.Enqueue(new Uri("http://cnn.com"));
            listOfPages.Enqueue(new Uri("http://bbc.com"));
            listOfPages.Enqueue(new Uri("http://reddit.com"));
            listOfPages.Enqueue(new Uri("http://onemorelevel.com"));
            List<KeyValuePair<string, string>> temp = webCrawler.crawlWebSites(listOfPages, "OKEEFFE");
            indexer index = new indexer();
            Tuple<List<string>, List<incidenceVector>> list = index.getIndexTable(temp);
            bool exit = false;
            search searchInstance = new search();
            while(!exit)
            {
                string q = Console.ReadLine();
                if(q == "exit")
                {
                    exit = true;
                }
                else
                {
                    List<KeyValuePair<int,double>> result = searchInstance.searchFromString(q,list);
                    if (result.Count > 0)
                    {
                        Console.WriteLine("Best result: " + list.Item1[result[0].Key]);
                    }
                    else
                    {
                        Console.WriteLine("No matching result");
                    }
                }
            }
        }
    }
}

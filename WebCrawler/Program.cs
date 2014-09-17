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
            listOfPages.Enqueue(new Uri("http://www.december.com/html/demo/hello.html"));
            List<KeyValuePair<string, string>> temp = webCrawler.crawlWebSites(listOfPages, "OKEEFFE");
            
            Console.ReadKey();
        }
    }
}

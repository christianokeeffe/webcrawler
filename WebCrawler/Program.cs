using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri webpage = new Uri("http://tv2.dk");
            getLinksFromUrl(webpage);
            Console.ReadKey();
        }


        private static void crawlWebSites(Queue<Uri> listOfPages, string botName)
        {
            List<robotRestriction> restrictions = new List<robotRestriction>();
            List<string> pageContents = new List<string>();
            restrictionsCheck restrictionsChecker = new restrictionsCheck();
            nearMatch matchCheck = new nearMatch();
            while (pageContents.Count < 1000 && listOfPages.Count != 0)
            {
                Uri URL = listOfPages.Dequeue();
                string siteContent = crawlSite(botName, URL, restrictions, restrictionsChecker);
                if(siteContent == null)
                {
                    listOfPages.Enqueue(URL);
                }
                else
                {
                    
                    pageContents.Add(siteContent);
                }
            }
        }

        private static string crawlSite(string botName, Uri webPage, List<robotRestriction> restrictions, restrictionsCheck restrictionsChecker)
        {
            //restrictions = restrictionsChecker.checkAndGetRobotFile(webPage, botName, restrictions);
            //check delay
            //check permission
            //crawl
            return null;
        }
        /*
        private static string getPageContent(Uri webpage)
        {

        }*/


        private static List<string> getLinksFromUrl(Uri webpage)
        {
            List<string> Links = new List<string>();
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load((webpage.ToString()));
            foreach(HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]").Distinct())
            {
                string href = link.Attributes["href"].Value;
                if (!Links.Contains(href))
                {
                    Links.Add(href);
                }
            }
            return Links;
        }


        private int convertUriToHash(Uri webPage)
        {
            string domainName = webPage.AbsoluteUri.Replace(webPage.AbsolutePath, "");
            int hashCode = domainName.GetHashCode();
            return hashCode;
        }
    }
}

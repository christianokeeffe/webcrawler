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
            string test = getPageContent(webpage);
            getLinksFromString(test);
            Console.ReadKey();
        }


        private static void crawlWebSites(Queue<Uri> listOfPages, string botName)
        {
            List<robotRestriction> restrictions = new List<robotRestriction>();
            List<string> pageContents = new List<string>();
            restrictionsCheck restrictionsChecker = new restrictionsCheck();
            nearMatch matchCheck = new nearMatch();
            List<webPage> webpages = new List<webPage>();
            while (pageContents.Count < 1000 && listOfPages.Count != 0)
            {
                Uri URL = listOfPages.Dequeue();
                string siteContent = crawlSite(botName, URL, restrictions, restrictionsChecker, webpages);
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

        private static string crawlSite(string botName, Uri webPageUrl, List<robotRestriction> restrictions, restrictionsCheck restrictionsChecker, List<webPage> webpages)
        {
            webPage tempPage = null;
            webpages = restrictionsChecker.checkAndGetRobotFile(webPageUrl, botName, webpages, tempPage);

            if (restrictionsChecker.canVisit(tempPage))
            {
                //restrictionsChecker.
                //crawl
            }
            else
            {
                return null;
            }
        }
        
        private static string getPageContent(Uri webpage)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load((webpage.ToString()));
            string htmlString = doc.DocumentNode.InnerHtml;
            return htmlString;
        }


        private static List<string> getLinksFromString(string htmlString)
        {
            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlString);
            List<string> Links = new List<string>();
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


    }
}

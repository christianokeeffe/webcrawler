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
            Queue<Uri> listOfPages = new Queue<Uri>();
            listOfPages.Enqueue(new Uri("http://www.aau.dk/"));
            crawlWebSites(listOfPages, "OKEEFFE");
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
                string siteContent = "";
                try
                {
                    siteContent = crawlSite(botName, URL, restrictionsChecker, webpages, listOfPages);
                }
                catch (Exception e) { }
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

        private static string crawlSite(string botName, Uri webPageUrl, restrictionsCheck restrictionsChecker, List<webPage> webpages, Queue<Uri> toVisit)
        {
            webPage tempPage = new webPage();
            webpages = restrictionsChecker.checkAndGetRobotFile(webPageUrl, botName, webpages, tempPage);

            if (restrictionsChecker.canVisit(tempPage))
            {
                if(restrictionsChecker.isAllowed(tempPage.restrictions, webPageUrl))
                {
                    string returnValue = getPageContent(webPageUrl);
                    Console.WriteLine(webPageUrl);
                    List<string> stringsToAdd = getLinksFromString(returnValue);
                    foreach(string s in stringsToAdd)
                    {
                        string temp = s;
                        if(!s.StartsWith("http"))
                        {
                            temp = tempPage.baseUrl + s;
                        }
                        toVisit.Enqueue(new Uri(temp));
                    }
                    return returnValue;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
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

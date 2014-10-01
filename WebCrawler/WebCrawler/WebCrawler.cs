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
    class WebCrawler
    {
        public List<KeyValuePair<string, string>> crawlWebSites(Queue<Uri> listOfPages, string botName)
        {
            List<robotRestriction> restrictions = new List<robotRestriction>();
            List<KeyValuePair<string, string>> pageContents = new List<KeyValuePair<string, string>>();
            restrictionsCheck restrictionsChecker = new restrictionsCheck();
            nearMatch matchCheck = new nearMatch();
            List<webPage> webpages = new List<webPage>();
            List<string> usedSites = new List<string>();
            int startTime = restrictionsChecker.time();
            int badUri = 0;
            while (pageContents.Count < 200 && listOfPages.Count != 0)
            {
                Uri URL = listOfPages.Dequeue();
                if (usedSites.IndexOf(URL.ToString()) == -1)
                {
                    string siteContent = "";
                    try
                    {
                        siteContent = crawlSite(botName, URL, restrictionsChecker, webpages, listOfPages);
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (NullReferenceException) { }
                    catch (WebException) { }
                    catch (IOException) { }
                    catch (ArgumentException) { }
                    catch (UriFormatException) { badUri++; }
                    if (siteContent == null)
                    {
                        listOfPages.Enqueue(URL);
                    }
                    else
                    {
                        usedSites.Add(URL.ToString());
                    }

                    if (siteContent != "" && siteContent != null)
                    {
                        pageContents.Add(new KeyValuePair<string, string>(URL.ToString(),siteContent));
                        Console.WriteLine(pageContents.Count + " " + URL.ToString());
                    }
                }
                else
                {

                }
            }
            int endTime = restrictionsChecker.time();
            //Console.WriteLine("Did " + pageContents.Count + " pages in " + (startTime - endTime) + "seconds!");
            //Console.WriteLine("That is " + (pageContents.Count / (endTime - startTime)) + " pages per second");
            Console.WriteLine("I had " + badUri + " uri errors:(");
            return pageContents;
        }

        private static string crawlSite(string botName, Uri webPageUrl, restrictionsCheck restrictionsChecker, List<webPage> webpages, Queue<Uri> toVisit)
        {

            webPage tempPage = webpages[restrictionsChecker.checkAndGetRobotFile(webPageUrl, botName, webpages)];

            if (restrictionsChecker.canVisit(tempPage))
            {
                if (restrictionsChecker.isAllowed(tempPage.restrictions, webPageUrl))
                {
                    string returnValue = getPageContent(webPageUrl);
                    tempPage.lastVisited = restrictionsChecker.time();
                    List<string> stringsToAdd = getLinksFromString(returnValue);
                    foreach (string s in stringsToAdd)
                    {
                        string temp = s;
                        if (!s.StartsWith("http"))
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
            HtmlNodeCollection linkList = doc.DocumentNode.SelectNodes("//a[@href]");
            if (linkList != null)
            {
                foreach (HtmlNode link in linkList.Distinct())
                {
                    string href = link.Attributes["href"].Value;
                    if (!Links.Contains(href))
                    {
                        Links.Add(href);
                    }
                }
            }
            return Links;
        }

    }
}

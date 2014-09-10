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

        private static List<robotRestriction> checkAndGetRobotFile(Uri webPage, string botName, List<robotRestriction> restrictions)
        {
            return null;
        }

        private static void crawlWebSites(Queue<Uri> listOfPages, string botName)
        {
            List<robotRestriction> restrictions = new List<robotRestriction>();
            List<string> pageContents = new List<string>();
            while(pageContents.Count < 1000 && listOfPages.Count != 0)
            {
                Uri URL = listOfPages.Dequeue();
                crawlSite(botName, URL, restrictions);
            }
        }

        private static string crawlSite(string botName, Uri webPage, List<robotRestriction> restrictions)
        {
            restrictions = checkAndGetRobotFile(webPage, botName, restrictions);
            //check delay
            //check permission
            //crawl
            return null;
        }

        private static List<robotRestriction> getRobotsRestrictions(Uri webPage, string botName, List<webPageDelays> webDelays)
        {
            string domainName = webPage.AbsoluteUri.Replace(webPage.AbsolutePath, "");
            string robotFile = domainName +"/robots.txt";
            int domainHash = domainName.GetHashCode();
            List<robotRestriction> robList = new List<robotRestriction>();
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(robotFile);
            StreamReader reader = new StreamReader(stream);
            string str = "";
            string agent = "*";
            bool hasSpecificRules = false;
            bool isRelevant = true;
            botName = botName.ToLower();

            while (reader.Peek() >= 0)
            {
                str = reader.ReadLine().ToLower();

                if (str.StartsWith("user-agent:"))
                {
                    str = str.Substring("user-agent:".Length);
                    str = str.Trim();
                    agent = str;
                    if (str == botName)
                    {
                        if (!hasSpecificRules)
                        {
                            robList.Clear();
                            hasSpecificRules = true;
                            isRelevant = true;
                        }
                    } else if (str == "*")
                    {
                        if(!hasSpecificRules)
                        {
                            isRelevant = true;
                        }
                        else
                        {
                            isRelevant = false;
                        }
                    } else {
                        isRelevant = false;
                    }
                }
                if(isRelevant) 
                {
                    if (str.StartsWith("allow"))
                    {
                        str = str.Substring("allow:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("allow", new Uri(str)));
                    }
                    if (str.StartsWith("allowed"))
                    {
                        str = str.Substring("allowed:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("allow", new Uri(str)));
                    }
                    if (str.StartsWith("disallow"))
                    {
                        str = str.Substring("disallow:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("disallow", new Uri(str)));
                    }
                    if (str.StartsWith("disallow"))
                    {
                        str = str.Substring("disallow:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("disallow", new Uri(str)));
                    }
                    if (str.StartsWith("crawl-delay"))
                    {
                        str = str.Substring("crawl-delay:".Length);
                        str = str.Trim();
                        int i = 0;
                        while (i < webDelays.Count && webDelays[i].hashValue != domainHash)
                        {
                            i++;
                        }
                        if(i < webDelays.Count)
                        {
                            webDelays[i].delayValue = int.Parse(str) ;
                        }
                        else
                        {
                            webDelays.Add(new webPageDelays(domainHash, int.Parse(str)));
                        }
                        robList.Add(new robotRestriction("delay", new Uri(str)));
                    }
                }
            }

            return robList;
        }

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

        private static bool isAllowed(List<robotRestriction> restrictions, Uri webpage)
        {
            foreach (robotRestriction restrict in restrictions)
            {
                if(restrict.type == "disallow")
                {
                    if (webpage.ToString().Contains(restrict.url.ToString()))
                    {
                        return false;
                    }
                }
                else if(restrict.type == "allow")
                {
                    if (webpage.ToString().Contains(restrict.url.ToString()))
                    {
                        return true;
                    }
                }
            }
            return true;
        }

        private int convertUriToHash(Uri webPage, string append)
        {
            string domainName = webPage.AbsoluteUri.Replace(webPage.AbsolutePath, "");
            if (append != "")
            {

            }
            int hashCode = domainName.GetHashCode();
            return hashCode;
        }

        private bool sendDelay(Uri webPage, List<webPageDelays> webDelays)
        {
            if(webDelays.Count > 0) {
                foreach(webPageDelays test in webDelays) {

                }

            }
            return true;
        }
    }
}

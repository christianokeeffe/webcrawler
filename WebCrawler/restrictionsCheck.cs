using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class restrictionsCheck
    {

        public static bool isAllowed(List<robotRestriction> restrictions, Uri webpage)
        {
            foreach (robotRestriction restrict in restrictions)
            {
                if (restrict.type == "disallow")
                {
                    if (webpage.ToString().Contains(restrict.url.ToString()))
                    {
                        return false;
                    }
                }
                else if (restrict.type == "allow")
                {
                    if (webpage.ToString().Contains(restrict.url.ToString()))
                    {
                        return true;
                    }
                }
            }
            return true;
        }


        public static List<webPage> checkAndGetRobotFile(Uri inputPage, string botName, List<webPage> allWebpages)
        {
            bool visited = false;
            int i = 0;
            while (visited == false && i < allWebpages.Count)
            {
                if (allWebpages[i].baseUrl == new Uri(inputPage.AbsoluteUri.Replace(inputPage.AbsolutePath, "")))
                {
                    visited = true;
                }
            }
            if (!visited)
            {
//                webpage tempPage = new webpage(new Uri(inputPage.AbsoluteUri.Replace(inputPage.AbsolutePath, "")), getRobotsRestrictions(new Uri(inputPage.AbsoluteUri.Replace(inputPage.AbsolutePath, "")), botName));
//                allWebpages.Add(tempPage);
            }
            return allWebpages;
        }

        public static List<robotRestriction> getRobotsRestrictions(Uri webPage, string botName, List<webPageDelays> webDelays)
        {
            string domainName = webPage.AbsoluteUri.Replace(webPage.AbsolutePath, "");
            string robotFile = domainName + "/robots.txt";
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
                    }
                    else if (str == "*")
                    {
                        if (!hasSpecificRules)
                        {
                            isRelevant = true;
                        }
                        else
                        {
                            isRelevant = false;
                        }
                    }
                    else
                    {
                        isRelevant = false;
                    }
                }
                if (isRelevant)
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
                        if (i < webDelays.Count)
                        {
                            webDelays[i].delayValue = int.Parse(str);
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
    }
}

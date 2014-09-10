﻿﻿using System;
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


        public List<webPage> checkAndGetRobotFile(Uri inputPage, string botName, List<webPage> allWebpages)
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
                webPage tempPage = new webPage(new Uri(inputPage.AbsoluteUri.Replace(inputPage.AbsolutePath, "")));
                tempPage = getRobotsRestrictions(botName, tempPage);
                allWebpages.Add(tempPage);
            }
            return allWebpages;
        }

        public static webPage getRobotsRestrictions(string botName, webPage thisWebpage)
        {
            string robotFile = thisWebpage.baseUrl + "/robots.txt";
            List<robotRestriction> robList = new List<robotRestriction>();
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(robotFile);
            StreamReader reader = new StreamReader(stream);
            string str = "";
            string agent = "*";
            bool hasSpecificRules = false;
            bool isRelevant = true;
            botName = botName.ToLower();
            thisWebpage.delayValue = 2;

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
                        thisWebpage.delayValue = int.Parse(str);
                    }
                }
            }

            thisWebpage.restrictions = robList;

            return thisWebpage;
        }
    }
}
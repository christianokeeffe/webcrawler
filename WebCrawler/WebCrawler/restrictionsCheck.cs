﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class restrictionsCheck
    {

        public bool isAllowed(List<robotRestriction> restrictions, Uri webpage)
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


        public int checkAndGetRobotFile(Uri inputPage, string botName, List<webPage> allWebpages)
        {
            bool visited = false;
            int i = 0;
            while (visited == false && i < allWebpages.Count)
            {
                string url = inputPage.Scheme + "://" + inputPage.Host;
                if (allWebpages[i].baseUrl == new Uri(url))
                {
                    visited = true;
                    return i;
                }
                i++;
            }
            if (!visited)
            {
                string url = inputPage.Scheme + "://" + inputPage.Host;
                webPage tempPage = new webPage();
                tempPage.baseUrl = new Uri(url);
                tempPage = getRobotsRestrictions(botName, tempPage);
                allWebpages.Add(tempPage);
                
            }
            return allWebpages.Count - 1;
        }

        public static webPage getRobotsRestrictions(string botName, webPage thisWebpage)
        {
            string robotFile = thisWebpage.baseUrl.ToString().TrimEnd('/') + "/robots.txt";
            List<robotRestriction> robList = new List<robotRestriction>();
            WebClient client = new WebClient();
            try
            {
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
                            robList.Add(new robotRestriction("allow", str));
                        }
                        if (str.StartsWith("allowed"))
                        {
                            str = str.Substring("allowed:".Length);
                            str = str.Trim();
                            robList.Add(new robotRestriction("allow", str));
                        }
                        if (str.StartsWith("disallow"))
                        {
                            str = str.Substring("disallow:".Length);
                            str = str.Trim();
                            robList.Add(new robotRestriction("disallow", str));
                        }
                        if (str.StartsWith("disallow"))
                        {
                            str = str.Substring("disallow:".Length);
                            str = str.Trim();
                            robList.Add(new robotRestriction("disallow", str));
                        }
                        if (str.StartsWith("crawl-delay"))
                        {
                            str = str.Substring("crawl-delay:".Length);
                            str = str.Trim();
                            thisWebpage.delayValue = (int)double.Parse(str);
                            if(thisWebpage.delayValue == 0)
                            {
                                thisWebpage.delayValue = 1;
                                thisWebpage.delayValue = 1;
                                thisWebpage.delayValue = 1;
                                thisWebpage.delayValue = 1;
                                thisWebpage.delayValue = 1;
                            }
                        }
                    }
                }
            }
            catch (System.Net.WebException) { }

            thisWebpage.restrictions = robList;


            return thisWebpage;
        }
        public int time()
      {
            int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

      public bool canVisit(webPage webDelays)
        {
            if (time() - webDelays.lastVisited >= webDelays.delayValue)
            {
                webDelays.lastVisited = time();
                return true;
            }
            return false;
        }
    }
}
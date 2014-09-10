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

            Console.ReadKey();
        }

        private static List<robotRestriction> getRobotsRestrictions(Uri webPage, string botName)
        {
            string robotFile = webPage.AbsoluteUri.Replace(webPage.AbsolutePath, "") + "/robots.txt";
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
                        robList.Add(new robotRestriction("allow",str));
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
                        robList.Add(new robotRestriction("delay", str));
                    }
                }
            }

            return robList;
        }
    }
}

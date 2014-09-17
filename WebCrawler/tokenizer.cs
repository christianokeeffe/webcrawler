using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    class tokenizer
    {
        internal static string[] splitString(string input){
            Regex rRemScript = new Regex(@"<script [^>]*>[\s\S]*?</script>");
            string noScript = rRemScript.Replace(input, "");

            string stripped = Regex.Replace(noScript, @"<(.|\n)*?>", " ");
            string htmlRemoved = RemoveUnwantedTags(stripped);
            string lowerCased = Regex.Replace(htmlRemoved, @"[^A-Za-z\s]", " ").ToLower();
            lowerCased = lowerCased.Replace('\r',' ').Replace('\n',' ').Replace('\t',' ');

            string[] tokens = lowerCased.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }

        internal static string RemoveUnwantedTags(string data)
        {
            var document = new HtmlDocument();
            document.LoadHtml(data);
            var nodes = new Queue<HtmlNode>(document.DocumentNode.SelectNodes("./*|./text()"));

            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                if (node.Name != "strong" && node.Name != "em" && node.Name != "u" && node.Name != "#text")
                {

                    var childNodes = node.SelectNodes("./*|./text()");

                    if (childNodes != null)
                    {
                        foreach (var child in childNodes)
                        {
                            nodes.Enqueue(child);
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);

                }
            }

            return document.DocumentNode.InnerHtml;
        }
    }
}

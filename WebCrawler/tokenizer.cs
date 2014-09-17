using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchEngine
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

            string[] stopWordsRemoved = noStopWords(tokens);

            return stopWordsRemoved;
        }

        internal static string[] noStopWords(string[] tokens)
        {
            string[] stopWords = {"a", "about", "above", "after", "again", "against", "all", "am", "an", "and", "any", "are", "arent", "as", "at", "be", "because", "been", "before",
            "being", "below", "between", "both", "but", "by", "cant", "cannot", "could", "couldnt", "did", "didnt", "do", "does", "doesnt", "doing", "dont", "down", "during", "each",
            "few", "for", "from", "further", "had", "hadnt", "has", "hasnt", "have", "havent", "having", "he", "hed", "hell", "hes", "her", "here", "heres", "hers", "herself", "him",
            "himself", "his", "how", "hows", "i", "im", "ive", "if", "in", "into", "is", "isnt", "it", "its", "itself", "lets", "me", "more", "most", "mustnt", "my", "myself", "no",
            "nor", "not", "of", "off", "on", "once", "only", "or", "other", "ought", "our", "ours", "ourselves", "out", "over", "own", "same", "shant", "she", "shes", "should", "shouldnt",
            "so", "some", "such", "than", "that", "thats", "the", "their", "theirs", "them", "themselves", "then", "there", "theres", "these", "they", "theyd", "theyll", "theyre", "theyve",
            "this", "those", "through", "to", "too", "under", "until", "up", "very", "was", "wasnt", "we", "weve", "were", "werent", "what", "whats", "when", "whens", "where", "wheres",
            "which", "while", "who", "whos", "whom", "why", "whys", "with", "wont", "would", "wouldnt", "you", "youd", "youll", "youre", "youve", "your", "yours", "yourself", "yourselves"};
            foreach (string word in stopWords)
                tokens = tokens.Where(w => w != word).ToArray();
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

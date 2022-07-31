using HtmlAgilityPack;
using SocialNetwork.Parser.Services.Interfaces;

namespace SocialNetwork.Parser.Services.Implementations
{
	public class ChukotkaArticle:IArticle
	{
        public string Name { get; set; }

        public void FillParams(HtmlNode Node)
        {
            Name = Node.InnerText;
        }
    }
}


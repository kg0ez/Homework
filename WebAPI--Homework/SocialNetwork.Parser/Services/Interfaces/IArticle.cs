using HtmlAgilityPack;

namespace SocialNetwork.Parser.Services.Interfaces
{
	public interface IArticle
	{
		public string Name { get; set; }

		public abstract void FillParams(HtmlNode Node);
	}
}


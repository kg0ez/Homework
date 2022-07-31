using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SocialNetwork.Parser.Services.Implementations;
using SocialNetwork.Parser.Services.Interfaces;

namespace SocialNetwork.Parser
{
    class Program
    {
        public static List<IArticle> articlesList = new List<IArticle>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        private static async Task ChukotkaLoad()
        {
            string site = "https://old.prochukotku.ru";
            WebRequest NewsListRequest = WebRequest.Create(site + "/news/main/");
            using WebResponse NewsListResponse = await NewsListRequest.GetResponseAsync();
            using Stream stream = NewsListResponse.GetResponseStream();
            HtmlDocument document = new HtmlDocument();
            document.Load(stream);

            var NewsBlockNodes = document.DocumentNode.SelectNodes(".//div[contains(@class, 'item nuclear')]");
            for (int i = 0; i < 10; i++)
            {
                IArticle article = new ChukotkaArticle();
                var NewsNodes = NewsBlockNodes[i].SelectNodes(".//a");
                article.FillParams(NewsNodes[0]);
                articlesList.Add(article);
            }
        }
        public static List<IArticle> Posts()
        {
            ChukotkaLoad();
            return articlesList;
        }
    }
}

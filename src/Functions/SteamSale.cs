using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;

namespace Discord_Bot
{
    public static class SteamSaleCheck {

        public static void Index()
        {
            string url = "https://store.steampowered.com/search/?specials=1&os=win";
            var response = CallUrl(url).Result;
            var gamesOnSale = ParseHtml(response);

            WriteToCsv(gamesOnSale);
        }

        private static List<string> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            List<string> soups = new List<string>();

            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div[@class='col search_name ellipsis']"))
            {
                soups.Add(node.InnerText);
            }

            return soups;
        }

        private static void WriteToCsv(List<string> soups)
        {
            string csvPath = $"{AppDomain.CurrentDomain.BaseDirectory}SteamSale.csv";
            StringBuilder sb = new StringBuilder();
            foreach (var soup in soups)
            {
                sb.AppendLine(soup);
            }
            System.IO.File.WriteAllText(csvPath, sb.ToString());
        }
        private static async Task<string> CallUrl(string urlString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(urlString);
            return response;
        }

    }
}
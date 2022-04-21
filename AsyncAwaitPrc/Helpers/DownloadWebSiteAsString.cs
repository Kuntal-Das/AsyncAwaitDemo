using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncAwaitPrc.Helpers
{
    public class DownloadWebSiteAsString
    {
        static readonly HttpClient client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        internal static string DownloadWebsite(string site)
        {
            string websiteData;
            using (WebClient client = new WebClient())
            {
                //websiteData = await Task.Run(() => client.DownloadString(site));
                websiteData = client.DownloadString(site);
            }
            return $"{site} downloaded: {websiteData.Length} characters long";
        }

        internal static async Task<string> DownloadWebsiteAsync(string site)
        {
            HttpResponseMessage response = await client.GetAsync(site);
            response.EnsureSuccessStatusCode();
            string websiteData = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            return $"{site} downloaded: {websiteData.Length} characters long";
        }
    }
}

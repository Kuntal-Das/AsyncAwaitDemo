using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAwaitPrc.Helpers
{
    public class DownloadWebSiteAsString
    {
        internal static string DownloadWebsite(string site)
        {
            string websiteData;
            using (WebClient client = new WebClient())
            {
                websiteData = client.DownloadString(site);
            }

            return $"{site} downloaded: {websiteData.Length} characters long";
        }

        internal static async Task<string> DownloadWebsiteAsync(string site)
        {
            string websiteData;
            using (WebClient client = new WebClient())
            {
                //websiteData = await Task.Run(() => client.DownloadString(site));
                websiteData = await client.DownloadStringTaskAsync(site);
            }

            return $"{site} downloaded: {websiteData.Length} characters long";
        }
    }
}

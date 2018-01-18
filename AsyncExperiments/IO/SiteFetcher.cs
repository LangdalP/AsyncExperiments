using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace AsyncExperiments.IO
{
    public static class SiteFetcher
    {
        public static IEnumerable<string> DownloadPages(IEnumerable<string> urls)
        {
            return urls.Select(url => DownloadPage(url));
        }

        public static async Task<IEnumerable<string>> DownloadPagesNaiveAsync(IEnumerable<string> urls)
        {
            // The select loop calls DownloadPageNaiveAsync which creates completed tasks
            var fetchTasks = urls.Select(url => DownloadPageNaiveAsync(url)).ToList();
            // When we get here, all tasks are already done
            var pages = await Task.WhenAll(fetchTasks);
            return pages;
        }

        public static async Task<IEnumerable<string>> DownloadPagesOnMultipleThreadsAsync(IEnumerable<string> urls)
        {
            // The select loop calls Task.Run for every element, which runs the given lamba on a thread in the thread pool
            List<Task<string>> fetchTasks = urls.Select(url => Task.Run(() => DownloadPage(url))).ToList();
            var pages = await Task.WhenAll(fetchTasks);
            return pages;
        }

        public static async Task<IEnumerable<string>> DownloadPagesProperAsync(IEnumerable<string> urls)
        {
            // The select loop calls simply creates Tasks that might or might not start right away
            // These tasks, being truly async., will yield control back to the caller once they reach their 'await' constructs
            // and resume execution some time after their respective requests complete
            List<Task<string>> fetchTasks = urls.Select(DownloadPageProperAsync).ToList();
            var pages = await Task.WhenAll(fetchTasks);
            return pages;
        }

        private static Task<string> DownloadPageNaiveAsync(string url)
        {
            return Task.FromResult(DownloadPage(url));
        }

        private static async Task<string> DownloadPageProperAsync(string url)
        {
            var webClient = new WebClient();
            return await webClient.DownloadStringTaskAsync(url);
        }

        private static string DownloadPage(string url)
        {
            var webClient = new WebClient();
            return webClient.DownloadString(url);
        }
    }
}
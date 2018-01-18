using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AsyncExperiments.IO;
using AsyncExperiments.Models;

namespace AsyncExperiments.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string[] Urls =
        {
            "https://www.vg.no",
            "https://www.db.no",
            "https://www.nrk.no",
            "https://www.aftenposten.no",
            "https://www.adressa.no",
            "https://www.smp.no",
            "https://www.bekk.no"
        };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SyncFetch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var pages = SiteFetcher.DownloadPages(Urls).ToList();

            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            var elapsedString = $"{ts.Seconds:00}.{ts.Milliseconds:000} s";
            return View("FetchResults", new FetchResultsViewModel("Sync. fetch", Urls.Length, elapsedString));
        }

        public async Task<ActionResult> NaiveAsyncFetch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var pages = (await SiteFetcher.DownloadPagesNaiveAsync(Urls)).ToList();

            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            var elapsedString = $"{ts.Seconds:00}.{ts.Milliseconds:000} s";
            return View("FetchResults", new FetchResultsViewModel("Naive Async. fetch", Urls.Length, elapsedString));
        }

        public async Task<ActionResult> ParallelFetch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var pages = (await SiteFetcher.DownloadPagesOnMultipleThreadsAsync(Urls)).ToList();

            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            var elapsedString = $"{ts.Seconds:00}.{ts.Milliseconds:000} s";
            return View("FetchResults", new FetchResultsViewModel("Parallel fetch", Urls.Length, elapsedString));
        }

        public async Task<ActionResult> AsyncFetch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var pages = (await SiteFetcher.DownloadPagesProperAsync(Urls)).ToList();

            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            var elapsedString = $"{ts.Seconds:00}.{ts.Milliseconds:000} s";
            return View("FetchResults", new FetchResultsViewModel("Proper Async. fetch", Urls.Length, elapsedString));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
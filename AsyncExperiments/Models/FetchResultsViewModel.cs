using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsyncExperiments.Models
{
    public class FetchResultsViewModel
    {
        public string Title { get; set; }
        public int NumPagesFetched { get; set; }
        public string TimeElapsed { get; set; }

        public FetchResultsViewModel(string title, int numPagesFetched, string timeElapsed)
        {
            Title = title;
            NumPagesFetched = numPagesFetched;
            TimeElapsed = timeElapsed;
        }
    }
}
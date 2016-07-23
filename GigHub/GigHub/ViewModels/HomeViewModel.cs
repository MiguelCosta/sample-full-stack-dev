using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.ViewModels
{
    public class HomeViewModel
    {
        public bool ShowActions { get; set; }

        public IEnumerable<Gig> UpcomingGigs { get; set; }
    }
}

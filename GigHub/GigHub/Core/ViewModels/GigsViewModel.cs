using GigHub.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Core.ViewModels
{
    public class GigsViewModel
    {
        public ILookup<int, Attendance> Attendances { get; internal set; }

        public IEnumerable<Gig> Gigs { get; set; }

        public string Heading { get; set; }

        public string SearchTerm { get; set; }

        public bool ShowActions { get; set; }
    }
}

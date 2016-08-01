using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class GigDetailsViewModel
    {

        public Gig Gig { get; set; }

        public bool IsGoing { get; set; }

        public bool IsFollowing { get; set; }
    }
}
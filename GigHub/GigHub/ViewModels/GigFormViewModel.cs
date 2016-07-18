using GigHub.Models;
using System;
using System.Collections.Generic;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public string Date { get; set; }

        public DateTime DateTime
        {
            get
            {
                return DateTime.Parse($"{Date} {Time}");
            }
        }

        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Time { get; set; }

        public string Venue { get; set; }
    }
}

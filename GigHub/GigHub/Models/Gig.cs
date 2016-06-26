using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Gig
    {
        [Required]
        public ApplicationUser Artist { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public Genre Genre { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }
    }
}

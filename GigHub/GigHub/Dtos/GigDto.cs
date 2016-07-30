using GigHub.Models;
using System;
using System.Collections.Generic;

namespace GigHub.Dtos
{
    public class GigDto
    {
        public UserDto Artist { get; set; }

        public DateTime DateTime { get; set; }

        public GenreDto Genre { get; set; }

        public int Id { get; set; }

        public bool IsCanceled { get;  set; }

        public string Venue { get; set; }

    }
}